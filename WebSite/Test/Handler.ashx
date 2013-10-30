<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;
using WebTest;

public class Handler : IHttpHandler
{

  public void ProcessRequest( HttpContext context )
  {

    var manager = new TestManager();
    var results = manager.RunTest( new MyTest() );

  }

  public bool IsReusable
  {
    get
    {
      return false;
    }
  }

}