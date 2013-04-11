using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Ivony.Fluent;

namespace Ivony.Html.Web
{



  /// <summary>
  /// 标注一个方法将用于处理指定筛选器筛选出的的元素
  /// </summary>
  [AttributeUsage( AttributeTargets.Method )]
  public class HandleElementAttribute : Attribute
  {
    public HandleElementAttribute( string selectorExpression )
    {
      Selector = selectorExpression;
    }

    /// <summary>
    /// 用于筛选要处理的元素的 CSS 选择器
    /// </summary>
    public string Selector
    {
      get;
      private set;
    }

  }
}

