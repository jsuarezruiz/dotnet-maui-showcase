using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaxyLogicGame.Planet_objects;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;

namespace GalaxyLogicGame.Particles
{

    public partial class PolymorphParticle : AbsoluteLayout
    {
        private Particle[] particles = new Particle[2];
        private PlanetBase planet;
        private bool playing;
        private int xDirection;
        private int yDirection;
        private double planetAngularDistance = 0;
        //private double planetAngularDistance;


        public PolymorphParticle()
        {
            InitializeComponent();
        }
        public void Move()
        {
            planetAngularDistance = (planetAngularDistance + 0.025) % (Math.PI * 2);
            planet.Scale = Math.Sin(planetAngularDistance) * 0.15 + 1;

            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].AngularDistance = (particles[i].AngularDistance + 0.017) % (Math.PI * 2);

                Position p = CirclePosition.CalculatePosition(particles[i].Angle, Math.Sin(particles[i].AngularDistance) * 40);

                particles[i].TranslationX = p.X;
                particles[i].TranslationY = p.Y;
            }
        }

        public void Play()
        {
            Random random = new Random();
            for (int i = 0; i < particles.Length; i++)
            {


                particles[i] = new Particle
                {
                    BackgroundColor = Color.FromArgb("fff"),
                    Angle = random.NextDouble() * 2 * Math.PI,
                    AngularDistance = 0,//i * Math.PI/2,
                    Scale = 0.33,
                    Opacity = 0.5,
                    CornerRadius = 30
                };
                mainLayout.Children.Add(particles[i]);
                AbsoluteLayout.SetLayoutBounds(particles[i], new Rect(0.5, 0.5, 60, 60));
                AbsoluteLayout.SetLayoutFlags(particles[i], AbsoluteLayoutFlags.PositionProportional);
            }

            playing = true;
        }


        public async Task Stop(int duration)
        {
            playing = false;

            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].FadeTo(0, (uint)duration);
            }
            await Task.Delay(duration);
        }

        public bool IsPlaying { get => playing; }
        public PlanetBase PulsingPlanet { set { planet = value; } }
    }

    class Particle : Button
    {
        private double angle; // in radians
        private double angularDistance;

        public double Angle { get => angle; set { angle = value; } }
        public double AngularDistance { get => angularDistance; set { angularDistance = value; } }
    }
}