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

    public partial class BlackholeParticles : AbsoluteLayout
    {
        private BoxView[] particles = new BoxView[3];
        public BlackholeParticles()
        {
            InitializeComponent();
        }

        public async Task Play(AbsoluteLayout layout, int duration)
        {
            Random random = new Random();

            for (int i = 0; i < particles.Length; i++)
            {
                int size = random.Next(3, 10);

                BoxView particle = new BoxView();
                particles[i] = particle;
                mainLayout.Children.Add(particle);
                AbsoluteLayout.SetLayoutBounds(particle, new Rect(0.5, 0.5, size, size));
                AbsoluteLayout.SetLayoutFlags(particle, AbsoluteLayoutFlags.PositionProportional);

                Position p = CirclePosition.CalculatePosition(CirclePosition.ConvertDegreToRadian(random.Next(360)), random.Next(30, 70));
                particle.Rotation = random.Next(-360, 360);

                if (particle.Rotation < -120) particle.BackgroundColor = Color.FromArgb("fff");
                else particle.BackgroundColor = Color.FromHex("7800b0");
                // more colors

                particle.TranslationX = p.X;
                particle.TranslationY = p.Y;
                particle.TranslateTo(0, 0, (uint)duration * 2, Easing.SinOut);
                particle.RotateTo(random.Next(-360, 360), (uint)duration, Easing.SinOut);
                particle.FadeTo(0, (uint)duration, Easing.SinIn);

            }
            await Task.Delay(duration);

            layout.Children.Remove(this);
        }
    }
}