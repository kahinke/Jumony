using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebTest
{
  public class TestResultFailure : TestResult
  {
    public TestResultFailure( TestInfo info, TestAssertFailedException exception ) : base( info ) { }
  }
}
