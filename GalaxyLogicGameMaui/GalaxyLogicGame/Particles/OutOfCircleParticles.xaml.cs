using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;

namespace GalaxyLogicGame
{

    public partial class OutOfCircleParticles : AbsoluteLayout
    {
        private BoxView[] particles = new BoxView[3];
        public OutOfCircleParticles()
        {
            InitializeComponent();
        }

        public async Task Play(AbsoluteLayout layout, int delay, int duration)
        {
            Random random = new Random();

            await Task.Delay(delay);

            for (int i = 0; i < particles.Length; i++)
            {
                int size = random.Next(5, 15);
                BoxView particle = new BoxView
                {
                    BackgroundColor = Color.FromArgb("f00"),
                };
                particles[i] = particle;
                mainLayout.Children.Add(particle);
                AbsoluteLayout.SetLayoutBounds(particle, new Rect(0.5, 0.5, size, size));
                AbsoluteLayout.SetLayoutFlags(particle, AbsoluteLayoutFlags.PositionProportional);

                Position p = CirclePosition.CalculatePosition(CirclePosition.ConvertDegreToRadian(random.Next(360)), random.Next(30, 50));
                particle.TranslateTo(p.X, p.Y, (uint)duration, Easing.SinOut);
                particle.RotateTo(random.Next(-360, 360), (uint)duration, Easing.SinOut);
            }
            await Task.Delay(duration);

            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].FadeTo(0, (uint)duration * 2);
            }

            await Task.Delay(duration*2);


            layout.Children.Remove(this);
        }
    }
}