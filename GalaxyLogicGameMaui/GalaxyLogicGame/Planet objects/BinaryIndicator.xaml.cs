using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Planet_objects
{

    public partial class BinaryIndicator : AbsoluteLayout
    {
        public BinaryIndicator()
        {
            InitializeComponent();
        }

        public string BinaryString { get => label.Text; set { label.Text = value; } }
    }
}