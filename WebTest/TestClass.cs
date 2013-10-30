using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebTest
{
  public abstract class TestClass
  {

    protected TestClass()
    {
      Assert = new TestAssert();
    }


    protected TestAssert Assert
    {
      get;
      private set;
    }


    public virtual void Initialize()
    {

    }


    public virtual void Cleanup()
    {

    }

  }
}
