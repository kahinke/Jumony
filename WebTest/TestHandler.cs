using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WebTest
{
  public class TestHandler : IHttpHandler, IHttpAsyncHandler
  {
    public bool IsReusable
    {
      get { return false; }
    }

    public void ProcessRequest( HttpContext context )
    {
      throw new InvalidOperationException();
    }

    public IAsyncResult BeginProcessRequest( HttpContext context, AsyncCallback callback, object extraData )
    {


      var tcs = new TaskCompletionSource<object>();

      var task = ProcessRequestCore( context );
      task.ContinueWith( t =>
      {
        if ( t.IsFaulted )
          tcs.TrySetException( t.Exception.InnerException );
        else if ( t.IsCanceled )
          tcs.TrySetCanceled();
        else
          tcs.TrySetResult( null );

        if ( callback != null ) callback( tcs.Task );
      }, TaskScheduler.Default );

      return tcs.Task;
    }


    public void EndProcessRequest( IAsyncResult result )
    {
      var task = (Task) result;
      if ( task.Status == TaskStatus.Faulted )
        throw task.Exception;
    }

    protected async Task ProcessRequestCore( HttpContext context )
    {

      var types = TestManager.FindTestClasses();

      TestManager manager = new TestManager();


      foreach ( var testType in types )
      {
        GenerateReport( context.Response.Output, manager.RunTest( testType ) );
      }

    }

    private void GenerateReport( TextWriter writer, TestResult[] results )
    {

      foreach ( var result in results )
      {
        if ( result.IsSuccessed )
          writer.WriteLine( "<div class='result successed'>" );
        else
          writer.WriteLine( "<div class='result'>" );


        writer.WriteLine( "<span class='name'>{0}</span>", result.TestInfo.Name );
        writer.WriteLine( "<span class='message'>{0}</span>", result.Message );
        writer.WriteLine( "</div>" );
      }
    }
  }
}
