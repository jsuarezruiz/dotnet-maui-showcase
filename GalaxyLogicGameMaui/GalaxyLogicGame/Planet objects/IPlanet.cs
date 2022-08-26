using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Planet_objects
{
    public interface IPlanet
    {
        string Text { get; set; }
        int Type { get; set; }
        BinaryIndicator Binary { get; }
    }
}
