using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace Jumony.Demo.HelpCenter
{
  public class HelpCategory : HelpTopic
  {

    private string _virtualPath;

    public HelpCategory( string virtualPath )
    {
      if ( virtualPath == null )
        throw new ArgumentNullException( "virtualPath" );

      _virtualPath = VirtualPathUtility.AppendTrailingSlash( virtualPath );

      if ( !VirtualPathProvider.DirectoryExists( _virtualPath ) )
        throw new ArgumentException( "虚拟路径不是一个目录", "virtualPath" );
    }


    public override string VirtualPath
    {
      get { return _virtualPath; }
    }

    public override string DocumentPath
    {
      get { return VirtualPathUtility.Combine( VirtualPath, "index.html" ); }
    }

    public override HelpTopic[] ChildTopics
    {
      get
      {
        var directory = VirtualPathProvider.GetDirectory( VirtualPath );
        return directory.Children.OfType<VirtualFile>()
          .Select( f => VirtualPathUtility.ToAppRelative( f.VirtualPath ) )
          .Where( p => VirtualPathUtility.GetExtension( p ) == ".html" )
          .Where( p => VirtualPathUtility.GetFileName( p ) != "index.html" )
          .Union( directory.Children.OfType<VirtualDirectory>().Select( d => VirtualPathUtility.ToAppRelative( d.VirtualPath ) ).Where( d => VirtualPathProvider.FileExists( VirtualPathUtility.Combine( d, "index.html" ) ) ) )
          .Select( p => GetTopic( VirtualPathUtility.MakeRelative( helpEntriesVirtualPath, p ) ) ).ToArray();
      }
    }


  }
}
