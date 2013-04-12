using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Ivony.Fluent;

namespace Ivony.Html.Web
{
  public static class HtmlElementBinder
  {
    public static void BindData( IHtmlContainer scope, IHtmlElementBinder[] binders )
    {
      BindData( scope, binders, null );
    }

    public static void BindData( IHtmlContainer scope, IHtmlElementBinder[] binders, object dataContext )
    {
      BindData( scope, binders, dataContext, new Dictionary<string, object>() );
    }

    public static void BindData( IHtmlContainer scope, IHtmlElementBinder[] binders, object dataContext, IDictionary<string, object> data )
    {

      var context = new HtmlBindingContext( binders );
      context.DataBind( scope, dataContext, data );
    }




    private delegate object HandlerBindExecutor( object handler, IHtmlElement element );

    private class BinderInfo
    {
      public ICssSelector Selector { get; set; }
      public HandlerBindExecutor Executor { get; set; }
    }


    private static KeyedCache<Type, BinderInfo[]> _cache = new KeyedCache<Type, BinderInfo[]>();


    public static IHtmlElementBinder GetBinders( object handler )
    {
      var handlerType = handler.GetType();
      SelectorBasedElementBinder binder = new SelectorBasedElementBinder();

      var binderInfos = _cache.FetchOrCreateItem( handlerType, ()
        => handlerType.GetMethods( BindingFlags.Public | BindingFlags.Instance )
          .Select( m => CreateBinderInfo( m ) )
          .NotNull()
          .ToArray()
        );

      foreach ( var info in binderInfos )
        binder.AddBinder( info.Selector, context => info.Executor( handler, context.CurrentElement ) );


      return binder;
    }

    private static BinderInfo CreateBinderInfo( MethodInfo method )
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

        Expression<HandlerBindExecutor> result = Expression.Lambda<HandlerBindExecutor>( methodCallExpression, new ParameterExpression[] { handlerParamter, elementParameter } );

        return new BinderInfo() { Selector = selector, Executor = result.Compile() };
      }

      return null;
    }

  }
}
