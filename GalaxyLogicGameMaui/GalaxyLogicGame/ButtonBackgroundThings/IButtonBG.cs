using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.ButtonBackgroundThings
{
    interface IButtonBG
    {
        Color BGColor { get; set; }
        bool Visibility { set; }
    }
}
