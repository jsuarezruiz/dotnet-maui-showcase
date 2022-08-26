using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Events
{
    public interface IEventInfo
    {
        string GetTitle { get; }
        Color GetColor { get; }
        ImageSource GetIcon { get; }
        string Name { get; }




    }
}
