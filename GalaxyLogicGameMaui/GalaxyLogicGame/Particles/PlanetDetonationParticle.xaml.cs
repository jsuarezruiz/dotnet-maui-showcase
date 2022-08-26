using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Particles
{

    public partial class PlanetDetonationParticle : AbsoluteLayout
    {
        private BoxView[] parts = new BoxView[4];
        public PlanetDetonationParticle()
        {
            InitializeComponent();
            parts[0] = box1;
            parts[1] = box2;
            parts[2] = box3;
            parts[3] = box4;

        }

        public async Task Play(Color color, AbsoluteLayout layout)
        {

            layout.Children.Add(this);
            Random random = new Random();
            for (int i = 0; i < parts.Length; i++)
            {
                int corner = 0; // (case 3)
                parts[i].BackgroundColor = color;
                switch (i)
                {
                    case 0:
                        corner = 180;
                        break;
                    case 1:
                        corner = 90;
                        break;
                    case 2:
                        corner = 270;
                        break;
                }
                
                Position p = CirclePosition.CalculatePosition(random.Next(0, 90) + corner, random.Next(80, 120));
                parts[i].TranslateTo(p.X, p.Y, 500, Easing.CubicIn);
                parts[i].RotateTo(random.Next(-360, 360), 500, Easing.CubicIn);
                parts[i].FadeTo(0, 500, Easing.SinIn);
            }
            await Task.Delay(2500);
        }
    }
}