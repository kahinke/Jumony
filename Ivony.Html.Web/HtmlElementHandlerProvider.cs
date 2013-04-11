﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Ivony.Fluent;

namespace Ivony.Html.Web
{
  public class HtmlElementHandlerProvider
  {

    private class ElementHandlerInfo
    {
      public ICssSelector Selector { get; set; }
      public Executor Executor { get; set; }
    }

    private delegate void Executor( object handler, IHtmlElement element );

    private static KeyedCache<Type, ElementHandlerInfo[]> _cache = new KeyedCache<Type, ElementHandlerInfo[]>();

    private static ElementHandlerInfo CreateHandlerInfo( MethodInfo method )
    {
      var selectorAttribute = method.GetCustomAttributesData().FirstOrDefault( a => a.Constructor.DeclaringType == typeof( HandleElementAttribute ) );
      if ( selectorAttribute == null )
        return null;

      var selectorExpression = selectorAttribute.ConstructorArguments.First().Value as string;


      ICssSelector selector;
      try
      {
        selector = CssParser.ParseSelector( selectorExpression );
      }
      catch
      {
        return null;
      }


      var parameters = method.GetParameters();
      if ( parameters.Length == 1 && parameters[0].ParameterType == typeof( IHtmlElement ) )
      {

        ParameterExpression handlerParamter = Expression.Parameter( typeof( object ), "handler" );
        ParameterExpression elementParameter = Expression.Parameter( typeof( IHtmlElement ), "element" );

        UnaryExpression instance = Expression.Convert( handlerParamter, method.ReflectedType );
        MethodCallExpression methodCallExpression = Expression.Call( instance, method, elementParameter );

        Expression<Executor> result = Expression.Lambda<Executor>( methodCallExpression, new ParameterExpression[] { handlerParamter, elementParameter } );

        return new ElementHandlerInfo() { Selector = selector, Executor = result.Compile() };
      }

      return null;
    }


    public static IHtmlElementBinder[] GetElementHandlers( HtmlHandlerBase handler )
    {

      object _handler = handler;

      var wrapper = handler as IHandlerWrapper;
      if ( wrapper != null )
        _handler = wrapper.Handler;


      var type = _handler.GetType();
      var handlerInfos = _cache.FetchOrCreateItem( type, ()
        => type.GetMethods( BindingFlags.Public | BindingFlags.Instance )
          .Select( m => CreateHandlerInfo( m ) )
          .NotNull()
          .ToArray()
        );

      return handlerInfos.Select( info => new HtmlElementHandler( info.Selector, element => info.Executor( _handler, element ) ) ).ToArray();
    }



    /// <summary>
    /// 元素处理器，使用 CSS 选择器筛选并处理元素。
    /// </summary>
    private class HtmlElementHandler : IHtmlElementBinder
    {

      public HtmlElementHandler( ICssSelector selector, Action<IHtmlElement> processer )
      {
        Selector = selector;
        _processer = processer;
      }

      private Action<IHtmlElement> _processer;

      public ICssSelector Selector
      {
        get;
        private set;
      }

      public object ProcessElement( IHtmlElement element )
      {
        _processer( element );
        return null;
      }
    }


  }
}
