using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml.Linq;
using Ivony.Html;
using Ivony.Html.Parser;
using Ivony.Html.Web;
using Ivony.Html.Forms;
using System.Web.Hosting;

namespace Jumony.Demo.HelpCenter
{
  public class FeedbackController : Controller
  {

    public ActionResult AddFeedback( string contact, string path, int helpful, int understandable, string content )
    {

      var document = HtmlProviders.LoadDocument( "~/Views/Help/feedback.html" );
      var form = document.FindFirst( "form" ).AsForm();
      form["contact"].TrySetValue( contact );
      form["path"].TrySetValue( path );
      form["helpful"].TrySetValue( helpful.ToString() );
      form["understandable"].TrySetValue( understandable.ToString() );
      form["content"].TrySetValue( content );

      using ( var stream =  System.IO.File.OpenWrite( HostingEnvironment.MapPath( string.Format( "~/Feedbacks/{0:yyyyMMdd_HHmmss}.html", DateTime.Now ) ) ) )
      {
        document.Render( stream, Encoding.UTF8 );
      }

      return RedirectToAction( "Entry", "Help", new { path = path } );
    }

  }
}
