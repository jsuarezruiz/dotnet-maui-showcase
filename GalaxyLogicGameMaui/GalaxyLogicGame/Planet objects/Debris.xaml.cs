using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Planet_objects
{

    public partial class Debris : PlanetBase
    {
        private int time = 0;
        private int counter = 0;
        public Debris()
        {
            InitializeComponent();
        }
        public override string Text { get => "X"; set { } }
        public override int Type { get => 4; set { } }
        public override BinaryIndicator Binary => binary;
        public void RotateDebris()
        {
            time++;
            if (time == 50)
            {
                time = 0;

                switch (counter)
                {
                    case 0:
                        dustParticle.Play();
                        counter++;
                        break;
                    case 1:
                        dustParticle2.Play();
                        counter++;
                        break;
                    case 2:
                        counter = 0;
                        break;
                }
            }
            //debris.Rotation = (debris.Rotation + 0.1) % 360;
        }
    }
}