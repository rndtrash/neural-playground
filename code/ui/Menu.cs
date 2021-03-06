using Sandbox;
using Sandbox.UI;
using System.Linq;

namespace NP
{
	[UseTemplate]
    public partial class Menu : Panel
	{
		public bool IsFold => !HasClass( "unfold" );

		public Menu()
		{
			Local.Hud.AddEventListener( "populated", () => {
				var menuIcon = AddChild<Button>( "button navbaricon" );
				menuIcon.Text = "(Un)fold Menu";
				menuIcon.AddEventListener( "onclick", () => { Log.Info( $"{IsFold}" ); if ( IsFold ) Unfold(); else Fold(); } );
				var img = menuIcon.AddChild<Image>();
				img.SetTexture( "/ui/np.icon.menu.png" );

				AddIcon( "/home", "Home", "/ui/np.icon.home.png" );
				AddIcon( "/monitoring", "Alive creatures", "/ui/np.icon.alive.png" );
				AddIcon( "/creatures", "Design creatures", "/ui/np.icon.edit.png" );
			} );
		}

		public void AddIcon( string href, string name, string image )
		{
			var icon = AddChild<NavigatorButton>( "button navbaricon" );
			icon.Text = name;
			icon.HRef = href;
			var img = icon.AddChild<Image>();
			img.SetTexture( image );
			icon.AddChild<Panel>( "marker" );
		}

		public void Fold()
		{
			if ( IsFold )
				return;
			SetClass( "unfold", false );
		}

		public void Unfold()
		{
			if ( !IsFold )
				return;
			SetClass( "unfold", true );
		}
	}
}
