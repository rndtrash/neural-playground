using Sandbox.UI;
using System.Linq;

//
// You don't need to put things in a namespace, but it doesn't hurt.
//
namespace NP
{
	/// <summary>
	/// This is the HUD entity. It creates a RootPanel clientside, which can be accessed
	/// via RootPanel on this entity, or Local.Hud.
	/// </summary>
	[UseTemplate]
	public partial class Hud : Sandbox.HudEntity<RootPanel>
	{
		public static Hud Instance { get; protected set; }

		public NPPanel NPPanel { get; protected set; }

		public Hud()
		{
			if ( IsClient )
			{
				Instance = this;
				RootPanel.SetTemplate( "/Hud.html" ); // FIXME: fucking hell UseTemplate doesn't work here for some bizzare reason
				NPPanel = RootPanel.Children.OfType<NPPanel>().First();
				RootPanel.CreateEvent( "populated" );
			}
		}
	}

}
