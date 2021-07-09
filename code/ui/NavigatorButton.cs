
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System.Linq;
using System.Threading.Tasks;

namespace Sandbox.UI
{
	[Library( "navlink" )]
	public class NavigatorButton : Button
	{
		public string HRef;

		public override void OnParentChanged()
		{
			base.OnParentChanged();
		}

		public override void SetProperty( string name, string value )
		{
			base.SetProperty( name, value );
			
			if ( name == "href")
			{
				HRef = value;
			}
		}

		protected override void OnMouseDown( MousePanelEvent e )
		{
			if ( e.Button == "mouseleft" )
			{
				NP.Hud.Instance.NPPanel.Navigator.CreateEvent( "navigate", HRef );
				e.StopPropagation();
			}
		}

		public override void Tick()
		{
			base.Tick();

			var active = NP.Hud.Instance.NPPanel.Navigator.CurrentUrlMatches( HRef );
			SetClass( "active", active );
		}
	}
}
