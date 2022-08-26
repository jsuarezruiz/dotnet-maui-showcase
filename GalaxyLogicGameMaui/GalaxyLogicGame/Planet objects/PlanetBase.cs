using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Planet_objects
{
    public abstract class PlanetBase : AbsoluteLayout, IPlanet
    {
        public abstract string Text { get; set; }
        public abstract int Type { get; set; }
        public abstract BinaryIndicator Binary { get; }
        public virtual Color BGColor { get => Color.FromArgb("888"); set { } }
        public virtual bool IsTypeThree => false;
        public virtual int DreamNumber { get => 0; set { } }
    }
}
