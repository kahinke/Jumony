using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebTest;
using Ivony.Web;

public class MyTest : TestClass
{

  public void Test1()
  {

    var service = new MyTest();

    WebServices.RegisterService( service, "~/" );

    Assert.AreEqual( service, WebServices.GetServices<MyTest>( "~/abc" ).FirstOrDefault() );
  }

  public void Test2()
  {
    Assert.AreEqual( 2, 2 );

  }

  public void Test3()
  {
    throw new Exception( "TMD" );
  }

}
