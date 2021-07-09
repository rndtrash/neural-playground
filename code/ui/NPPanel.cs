using Sandbox;
using Sandbox.UI;
using System.Linq;

namespace NP
{
	public partial class NPPanel : Panel
	{
		public NavigatorPanel Navigator { get; protected set; }
		public Menu Menu { get; protected set; }

		public NPPanel()
		{
			Navigator = AddChild<NavigatorPanel>();
			Navigator.Navigate( "/home" );

			Menu = AddChild<Menu>();
		}
	}
}
