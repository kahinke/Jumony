using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Ivony.Fluent;
using Ivony;
using Ivony.Html;
using Ivony.Html.ExpandedAPI;
using Ivony.Html.Web;
using System.Web.Hosting;
using System.Web.Caching;

namespace Jumony.Demo.HelpCenter
{

  /// <summary>
  /// 代表一个帮助主题
  /// </summary>
  public abstract class HelpTopic
  {

    private static string cachePrefix = "HelpTopicCache_";
    private static KeyedCache<string, HelpTopic> _cache = new KeyedCache<string, HelpTopic>();





    public static HelpTopic GetTopic( string virtualPath )
    {
      var cacheKey = cachePrefix + virtualPath;

      var topic = HttpRuntime.Cache.Get( cachePrefix + virtualPath ) as HelpTopic;
      if ( topic == null )
      {
        topic = CreateTopic( virtualPath );
        HttpRuntime.Cache.Insert( cacheKey, topic, new CacheDependency( HostingEnvironment.MapPath( topic.DocumentPath ) ) );
      }

      return topic;
    }


    protected const string helpEntriesVirtualPath = "~/HelpEntries/";

    protected static VirtualPathProvider VirtualPathProvider { get { return HostingEnvironment.VirtualPathProvider; } }



    private static HelpTopic CreateTopic( string virtualPath )
    {

      if ( virtualPath == null )
        throw new ArgumentNullException( "virtualPath" );

      virtualPath = VirtualPathUtility.Combine( helpEntriesVirtualPath, virtualPath );


      if ( VirtualPathProvider.DirectoryExists( virtualPath ) )
        return new HelpCategory( virtualPath );

      else if ( VirtualPathProvider.FileExists( virtualPath ) )
        return new HelpEntry( virtualPath );

      else
        return null;

    }





    public abstract string VirtualPath { get; }

    public abstract string DocumentPath { get; }

    public abstract HelpTopic[] ChildTopics { get; }

    public IHtmlDocument Document
    {
      get { return HtmlProviders.LoadDocument( DocumentPath ); }
    }

    public string Title
    {
      get{ return Document.FindFirst( "head" ).FindFirst( "title" ).InnerHtml(); }
    }

    public string Summary
    {
      get { return Document.FindFirstOrDefault( "body > p" ).IfNull( "", element => element.InnerText() ); }
    }


    public HelpTopic Parent
    {
      get
      {
        if ( VirtualPath.EqualsIgnoreCase( helpEntriesVirtualPath ) )
          return null;

        var virtualPath = VirtualPathUtility.GetDirectory( VirtualPathUtility.RemoveTrailingSlash( VirtualPath ) );
        if ( virtualPath.EqualsIgnoreCase( helpEntriesVirtualPath ) )
          return GetTopic( "." );

        return GetTopic( VirtualPathUtility.MakeRelative( helpEntriesVirtualPath, virtualPath ) );
      }
    }

  }
}