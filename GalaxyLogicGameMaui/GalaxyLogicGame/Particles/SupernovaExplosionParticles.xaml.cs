using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;

namespace GalaxyLogicGame.Particles
{

    public partial class SupernovaExplosionParticles : AbsoluteLayout
    {
        private BoxView[] particles = new BoxView[20];
        private Color[] colors = { /*Color.Red, Color.LightBlue, Color.Purple, Color.Pink, Color.Aqua, Color.FromArgb("fff")*/ };
        public SupernovaExplosionParticles()
        {
            InitializeComponent();
        }
        public async Task Play(AbsoluteLayout layout, int duration, int delay)
        {
            layout.Children.Add(mainLayout);
            Random random = new Random();

            for (int i = 0; i < particles.Length; i++)
            {
                int size = random.Next(5, 15);
                BoxView particle = new BoxView
                {
                    BackgroundColor = colors[i % colors.Length],
                    Opacity = 0
                };
                particles[i] = particle;
                mainLayout.Children.Add(particle);
                AbsoluteLayout.SetLayoutBounds(particle, new Rect(0.5, 0.5, size, size));
                AbsoluteLayout.SetLayoutFlags(particle, AbsoluteLayoutFlags.PositionProportional);

                Position p = CirclePosition.CalculatePosition(CirclePosition.ConvertDegreToRadian(random.Next(360)), random.Next(30, 60));
                particle.FadeTo(1, (uint)duration);
                particle.TranslateTo(p.X, p.Y, (uint)duration, Easing.SinOut);
                particle.RotateTo(random.Next(-360, 360), (uint)duration, Easing.SinOut);
            }
            await Task.Delay(duration + delay);

            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].FadeTo(0, (uint)duration * 2);
            }

            await Task.Delay(duration * 2);


            layout.Children.Remove(this);
        }
    }
}