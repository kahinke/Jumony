using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ivony.Html.Web
{
  public class HtmlBindingContext
  {

    /// <summary>
    /// 进行绑定的范畴
    /// </summary>
    public IHtmlContainer BindingScope { get; private set; }

    /// <summary>
    /// 进行绑定的范畴的数据容器
    /// </summary>
    public IDictionary ScopeData { get; private set; }

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
  }
}
