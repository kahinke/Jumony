using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ivony.Fluent;

namespace Ivony.Html.Web
{
  public class HtmlBindingContext
  {
    /// <summary>
    /// 创建 HtmlBindingContext 实例
    /// </summary>
    /// <param name="scope">进行绑定的范畴</param>
    /// <param name="binders">进行绑定的器</param>
    /// <param name="dataContext">当前数据上下文</param>
    public HtmlBindingContext( IHtmlElementBinder[] binders )
    {
      Binders = binders;
    }

    /// <summary>
    /// 进行绑定的范畴
    /// </summary>
    public IHtmlContainer BindingScope { get; private set; }

    /// <summary>
    /// 进行绑定的范畴的数据容器
    /// </summary>
    public IDictionary<string, object> Data { get; private set; }

    /// <summary>
    /// 当前的数据上下文
    /// </summary>
    public object DataContext { get; private set; }

    /// <summary>
    /// 当前数据上下文应用的范畴
    /// </summary>
    public IHtmlContainer DataContextScope { get; private set; }

    /// <summary>
    /// 当前正在绑定的元素
    /// </summary>
    public IHtmlElement CurrentElement { get; private set; }

    /// <summary>
    /// 元素绑定器
    /// </summary>
    public IHtmlElementBinder[] Binders { get; private set; }


    public void DataBind( IHtmlContainer scope, object dataContext, IDictionary<string, object> data )
    {

      BindingScope = scope;
      DataContext = DataContext;
      Data = data;

      foreach ( var element in scope.Elements() )
        BindElement( element );
    }


    private void BindElement( IHtmlElement element )
    {
      CurrentElement = element;

      var data = Binders.ForAll( b => b.BindElement( this ) ).ToArray().NotNull().FirstOrDefault();
      if ( data != null )
      {
        DataContext = data;
        DataContextScope = element;
      }

      foreach ( var child in element.Elements() )
        BindElement( element );

    }
  }
}
