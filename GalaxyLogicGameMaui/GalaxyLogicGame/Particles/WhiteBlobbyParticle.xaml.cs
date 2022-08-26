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

    public partial class WhiteBlobbyParticle : AbsoluteLayout
    {
        private Button[] particles = new Button[1];
        private bool playing;
        private int xDirection;
        private int yDirection;
        private double angle; // in radians
        private double angularDistance;
        public WhiteBlobbyParticle()
        {
            InitializeComponent();
        }

        public void Move()
        {
            angularDistance = (angularDistance + 0.017) % (Math.PI * 2);

            Position p = CirclePosition.CalculatePosition(angle, Math.Sin(angularDistance) * 10);

            particles[0].TranslationX = p.X;
            particles[0].TranslationY = p.Y;
        }

        public void Play()
        {
            particles[0] = new Button
            {
                BackgroundColor = Color.FromArgb("fff"),
            };

            mainLayout.Children.Add(particles[0]);
            AbsoluteLayout.SetLayoutBounds(particles[0], new Rect(0.5, 0.5, 60, 60));
            AbsoluteLayout.SetLayoutFlags(particles[0], AbsoluteLayoutFlags.PositionProportional);

            Random random = new Random();

            angle = random.NextDouble() * 2 * Math.PI;
            angularDistance = 0;

            playing = true;
        }


        public async Task Stop(int duration)
        {
            playing = false;

            await particles[0].FadeTo(0, (uint)duration);
            mainLayout.Children.Remove(particles[0]);
        }

        public bool IsPlaying { get => playing; }
    }
}