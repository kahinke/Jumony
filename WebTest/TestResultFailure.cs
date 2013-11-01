using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebTest
{
  internal class TestResultFailure : TestResult
  {

    public TestAssertFailureException Failure
    {
      get;
      private set;
    }

    public TestResultFailure( TestInfo info, TestAssertFailureException exception )
      : base( info )
    {
      Failure = exception;
    }

    public override string Message
    {
      get
      {
        return string.Format( "测试断言失败： {0}", Failure.Message );
      }
    }

    public override bool IsSuccessed
    {
      get { return false; }
    }
  }
}
