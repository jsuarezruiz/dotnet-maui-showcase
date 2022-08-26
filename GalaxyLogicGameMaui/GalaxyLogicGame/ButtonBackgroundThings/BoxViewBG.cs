using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.ButtonBackgroundThings
{
    class BoxViewBG : BoxView, IButtonBG
    {
        public Color BGColor { get { return this.BackgroundColor; } set { this.BackgroundColor = value; } }
        public bool Visibility { set { this.IsVisible = value; } }

    }
}
