using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Planet_objects
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Blueberry : PlanetBase
	{
		public Blueberry ()
		{
			InitializeComponent ();
		}
        public override string Text { get => "X"; set { } }
        public override int Type { get => 5; set { } }
        public override BinaryIndicator Binary => binary;

    }
}