using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTest
{
  public class TestManager
  {

    public TestReport RunTest( Type testClass )
    {

      var instance = Activator.CreateInstance( testClass ) as TestClass;


      var testMethods = 


      instance.Initialize();


      instance.Cleanup();
    }

  }
}
