using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ivony.Fluent;

namespace Ivony.Html.Web
{
  public class SelectorBasedElementBinder : IHtmlElementBinder
  {


    SortedDictionary<ICssSelector, Func<HtmlBindingContext,object>> _binders = new SortedDictionary<ICssSelector, Func<HtmlBindingContext, object>>();


    public void AddBinder( ICssSelector selector, Func<HtmlBindingContext, object> binder )
    {
      _binders.Add( selector, binder );
    }


    public object BindElement( HtmlBindingContext context )
    {
      var binder = _binders.Where( b => b.Key.IsEligible( context.CurrentElement ) ).Select( b => b.Value ).FirstOrDefault();
      if ( binder == null )
        return null;

      return binder( context );
    }
  }
}
