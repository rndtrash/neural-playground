using System;
using System.Collections.Generic;
using System.Linq;

namespace Sandbox.UI
{
	/// <summary>
	/// A panel that acts like a website. A single page is always visible
	/// but it will cache other views that you visit, and allow forward/backward navigation.
	/// </summary>
	[Library( "navigator" )]
	public class NavigatorPanel : Panel
	{
		public Panel CurrentPanel => Current?.InnerPanel;
		public string CurrentUrl => Current?.Url;

		class HistoryItem
		{
			public Panel Panel;
			public Panel InnerPanel;
			public string Url;
		}

		internal void RemoveUrls( Func<string, bool> p )
		{
			var removes = Cache.Where( x => p( x.Url ) ).ToArray();

			foreach( var remove in removes )
			{
				remove.Panel.Delete();
				Cache.Remove( remove );
			}
		}

		List<HistoryItem> Cache = new();

		HistoryItem Current;
		Stack<HistoryItem> Back = new();
		Stack<HistoryItem> Forward = new();

		public void Navigate( string url )
		{
			if ( url == CurrentUrl )
				return;

			var attr = NavigatorTargetAttribute.FindValidTarget( url ); 
			if ( attr == null )
			{
				NotFound( url );
				return;
			}

			Log.Info( $"{url}" );

			Forward.Clear();

			if ( Current != null )
			{
				Back.Push( Current );
				Current.Panel.AddClass( "hidden" );
				Current = null;
			}

			var cached = Cache.FirstOrDefault( x => x.Url == url );
			if ( cached != null )
			{
				cached.Panel.RemoveClass( "hidden" );
				Current = cached;
				Current.Panel.Parent = this;
			}
			else
			{
				var panel = Add.Panel( "navigator-canvas" );
				var inner = attr.Create<Panel>();
				inner.Parent = panel;
				inner.AddClass( "navigator-body" );

				Current = new HistoryItem { Panel = panel, InnerPanel = inner, Url = url };

				foreach ( var ( key, value ) in attr.ExtractProperties( url ) )
				{
					inner.SetProperty( key, value );
				}

				Cache.Add( Current );
			}
		}

		protected virtual void NotFound( string url )
		{
			Log.Warning( $"Url Not Found: {url}" );
		}

		internal bool CurrentUrlMatches( string url )
		{
			return CurrentUrl == url;
		}

		public override void SetProperty( string name, string value )
		{
			base.SetProperty( name, value );

			if ( name == "default" ) Navigate( value ); 
		}

		[PanelEvent] 
		public bool NavigateEvent( string url )
		{
			Log.Info( $"NavigateEvent ${url}" );
			Navigate( url );
			return false;
		}

		protected override void OnBack( PanelEvent e )
		{
			GoBack();
			e.StopPropagation();
		}

		protected override void OnForward( PanelEvent e )
		{
			GoForward();
			e.StopPropagation();
		}

		public virtual void GoBack()
		{
			if ( !Back.TryPop( out var result ) )
			{
				PlaySound( "ui.navigate.deny" );
				return;
			}

			if ( !Cache.Contains( result ) )
			{
				GoBack();
				return;
			}

			PlaySound( "ui.navigate.back" );

			if ( Current != null )
				Forward.Push( Current );

			Switch( result );
		}

		public virtual void GoForward()
		{
			if ( !Forward.TryPop( out var result ) )
			{
				PlaySound( "ui.navigate.deny" );
				return;
			}

			if ( !Cache.Contains( result ) )
			{
				GoForward();
				return;
			}

			PlaySound( "ui.navigate.forward" );

			if ( Current != null )
				Back.Push( Current );

			Switch( result );
		}

		void Switch( HistoryItem item )
		{
			if ( Current == item ) return;

			Current?.Panel.AddClass( "hidden" );
			Current = null;

			Current = item;
			Current?.Panel.RemoveClass( "hidden" );
		}
	}
}
