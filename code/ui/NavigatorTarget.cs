using System;
using System.Collections.Generic;
using System.Linq;

namespace Sandbox.UI
{
	/// <summary>
	/// Mark a Panel with this class for it to be navigatable
	/// </summary>
	public class NavigatorTargetAttribute : LibraryAttribute
	{
		public string Url { get; internal set; }

		string[] Parts;

		public NavigatorTargetAttribute( string url )
		{
			Url = url;
			Parts = url.ToString().Split( '/', StringSplitOptions.RemoveEmptyEntries );
		}

		bool TestPart( string part, string ours )
		{
			// this is a variable
			if ( ours.StartsWith( '{' ) && ours.EndsWith( '}' ) )
				return true;

			return part == ours;
		}

		public bool CanServeUrl( string url )
		{
			if ( string.IsNullOrEmpty( url ) ) return false;

			var a = url.ToString().Split( '/', StringSplitOptions.RemoveEmptyEntries );
			if ( a.Length != Parts.Length ) return false;

			for ( int i=0; i< Parts.Length; i++ )
			{
				if ( !TestPart( a[i], Parts[i] ) )
					return false;
			}

			return true; 
		}

		public static NavigatorTargetAttribute FindValidTarget( string url )
		{
			return Library.GetAttributes<NavigatorTargetAttribute>()
				.Where( x => x.CanServeUrl( url ) )
				.FirstOrDefault(); 
		}

		internal IEnumerable<(string key, string value)> ExtractProperties( string url )
		{
			var a = url.ToString().Split( '/', StringSplitOptions.RemoveEmptyEntries );
			if ( a.Length != Parts.Length ) yield break;

			for ( int i = 0; i < Parts.Length; i++ )
			{
				if ( !Parts[i].StartsWith( '{' ) ) continue;
				if ( !Parts[i].EndsWith( '}' ) ) continue;

				var key = Parts[i][1..^1];

				yield return (key, a[i]);
			}
		}
	}
}
