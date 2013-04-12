using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ivony.Html.Web
{
  public interface IHtmlElementBinder
  {

    public object BindElement( HtmlBindingContext context );

  }
}
