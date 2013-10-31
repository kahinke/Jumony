using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTest
{
  public class TestAssert
  {

    internal TestAssert()
    {

    }

    public void Error( string format, params object[] args )
    {
      throw new TestAssertFailureException( string.Format( CultureInfo.InvariantCulture, format, args ) );
    }


  }
}
