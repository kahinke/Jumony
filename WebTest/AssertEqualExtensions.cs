using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTest
{
  public static class AssertEqualExtensions
  {


    private const string _message = "断言失败，两个对象不相等";


    public static void AreEqual( this TestAssert assert, object obj1, object obj2 )
    {
      AreEqual( assert, obj1, obj2, _message );
    }


    public static void AreEqual( this TestAssert assert, object obj1, object obj2, string message )
    {

      if ( object.Equals( obj1, obj2 ) )
        return;

      throw new TestAssertFailureException( message );


    }

  }
}
