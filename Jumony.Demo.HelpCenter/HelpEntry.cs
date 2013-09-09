using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumony.Demo.HelpCenter
{
  public class HelpEntry : HelpTopic
  {

    private string _virtualPath;

    public HelpEntry( string virtualPath )
    {
      if ( virtualPath == null )
        throw new ArgumentNullException( "virtualPath" );

      if ( !VirtualPathProvider.FileExists( virtualPath ) )
        throw new ArgumentException( "虚拟路径不是一个文件", "virtualPath" );

      _virtualPath = virtualPath;
    }

    public override string VirtualPath
    {
      get { return _virtualPath; }
    }

    public override string DocumentPath
    {
      get { return VirtualPath; }
    }

    public override HelpTopic[] ChildTopics { get { return new HelpTopic[0]; } }
  }
}
