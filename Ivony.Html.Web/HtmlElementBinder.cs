using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
  }
}
