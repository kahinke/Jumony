using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebTest
{
  public class TestResultException : TestResult
  {


    public TestResultException( TestInfo info, Exception exception )
      : base( info )
    {
      Exception = exception;
    }


    public Exception Exception
    {
      get;
      private set;

    }


    public override string Message
    {
      get
      {
        return string.Format( "执行测试中发生类型为 {0} 的异常，异常信息为： {1}", Exception.GetType().FullName, Exception.Message );
      }
    }

    public override bool IsSuccessed
    {
      get
      {
        return false;
      }
    }
  }
}
