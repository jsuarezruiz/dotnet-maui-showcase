using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Particles
{

    public partial class SupernovaExplosion : AbsoluteLayout
    {
        public SupernovaExplosion()
        {
            InitializeComponent();
        }
        public async Task Play(AbsoluteLayout layout, double scale, int duration)
        {
            layout.Children.Add(mainLayout);


            lensFlare.ScaleTo(scale / 90, (uint)duration);
            for (int i = 3; i > 0; i--)
            {
                SupernovaExplosionParticles particles = new SupernovaExplosionParticles { Scale = ( scale / 30) / i };

                particles.Play(mainLayout, duration, 0);

                await Task.Delay(duration / 3);
            }
            lensFlare.FadeTo(0, (uint)duration);
        }
    }
}