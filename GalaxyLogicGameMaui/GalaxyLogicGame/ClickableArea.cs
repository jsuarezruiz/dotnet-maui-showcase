using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame
{
    class ClickableArea : Button
    {
        private int index;
        public ClickableArea()
        {
            Opacity = 0;
           
            
        }
        public int Index { get { return index; } set { index = value; } }
    }
}
