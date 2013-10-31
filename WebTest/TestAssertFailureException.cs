using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTest
{
  internal class TestAssertFailureException : Exception
  {

    internal TestAssertFailureException( string message )
      : base( message )
    {

    }

  }
}
