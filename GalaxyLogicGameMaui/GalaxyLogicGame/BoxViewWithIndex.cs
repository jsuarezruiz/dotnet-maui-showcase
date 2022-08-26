using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame
{
    class BoxViewWithIndex : BoxView
    {
        int index;

        //Position position;

        
        public BoxViewWithIndex(){

        }
        

        public int Index { get { return index; } set { index = value; } }
        //public Position Position { set { position = value; } get { return position; } }
    }
}
