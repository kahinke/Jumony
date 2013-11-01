using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTest
{
  public class TestResultSuccess : TestResult
  {

    public TestResultSuccess( TestInfo info, TimeSpan duration )
      : base( info )
    {

    }


    public TimeSpan Duration
    {
      get;
      private set;
    }



    public override string Message
    {
      get
      {
        return string.Format( "测试成功，耗时 {0} 毫秒", Duration.TotalMilliseconds );
      }
    }

    public override bool IsSuccessed
    {
      get { return true; }
    }
  }
}
