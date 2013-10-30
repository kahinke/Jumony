using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebTest
{
  public class TestManager
  {

    public TestResult[] RunTest( TestClass instance )
    {

      var results = new List<TestResult>();

      foreach ( var method in GetTestMethods( instance.GetType() ) )
      {

        instance.Initialize();

        results.Add( RunTest( instance, method ) );

        instance.Cleanup();


      }

      return results.ToArray();


    }

    public TestInfo GetTestInfo( MethodInfo method )
    {
      return new TestInfo()
      {
        Name = method.Name
      };
    }

    public TestResult RunTest( TestClass instance, MethodInfo method )
    {

      var info = GetTestInfo( method );
      var invoker = CreateInvoker( instance, instance.GetType(), method );
      try
      {
        var watch = Stopwatch.StartNew();
        invoker( instance );
        watch.Stop();

        return Success( info, watch.Elapsed );

      }

      catch ( TestAssertFailedException exception )
      {
        return Failure( info, exception );
      }

      catch ( Exception exception )
      {
        return Exception( info, exception );
      }
    }


    private Action<object> CreateInvoker( TestClass instance, Type instanceType, MethodInfo method )
    {
      var instanceVariable = Expression.Convert( Expression.Variable( typeof( object ), "instance" ), instanceType );

      var expression = Expression.Lambda<Action<object>>( Expression.Call( instanceVariable, method ), Expression.Parameter( typeof( object ), "instance" ) );

      return expression.Compile();
    }


    private TestResult Success( TestInfo info, TimeSpan duration )
    {
      return new TestResultSuccess( info, duration );
    }

    private TestResult Failure( TestInfo info, TestAssertFailedException exception )
    {
      return new TestResultFailure( info, exception );
    }

    private TestResult Exception( TestInfo info, Exception exception )
    {
      return new TestResultException( info, exception );
    }


    private static MethodInfo[] GetTestMethods( Type testClass )
    {
      return testClass.GetMethods( BindingFlags.Public | BindingFlags.Instance )
        .Where( m => !m.GetParameters().Any() )
        .ToArray();
    }

  }
}
