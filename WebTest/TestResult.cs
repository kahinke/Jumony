using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTest
{

  /// <summary>
  /// 测试结果
  /// </summary>
  public abstract class TestResult
  {

    protected TestResult( TestInfo info )
    {
      TestInfo = info;
    }


    public TestInfo TestInfo
    {
      get;
      private set;
    }


    public abstract string Message
    {
      get;
    }

    public abstract bool IsSuccessed
    {
      get;
    }


  }



}
