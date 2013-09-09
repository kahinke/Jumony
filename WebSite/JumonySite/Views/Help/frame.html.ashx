<%@ WebHandler Language="C#" Class="frame_html" %>

using System;
using System.Web;
using Ivony.Html;
using Ivony.Html.Web;
using Jumony.Demo.HelpCenter;

public class frame_html : ViewHandler<HelpTopic>
{

  protected override void ProcessScope()
  {
    if ( Model is HelpCategory )
    {
      FindFirst( "partial[view=feedback]" ).Remove();
    }
    else
    {
      FindFirst( "partial[view=childs]" ).Remove();
    }
      
  }
}