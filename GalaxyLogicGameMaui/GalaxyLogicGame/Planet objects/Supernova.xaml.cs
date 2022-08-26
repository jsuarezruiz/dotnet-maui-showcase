using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaxyLogicGame.ButtonBackgroundThings;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Planet_objects
{

    public partial class Supernova : PlanetBase
    {
        private double angularValue = 0;
        /// <type values>
        /// 0 = basic planet with some number determining its value
        /// 1 = merging planet
        /// 2 = exploding star
        /// 3 = shrinking giant
        public Supernova()
        {
            InitializeComponent();
            
        }
        //public Position Position { set { AbsoluteLayout.SetLayoutBounds(wholeLayout, new Rect(value.X, value.Y, 60, 60)); } get { return  } }
        //public Color BGColor { set { bg.BGColor = value; } get { return bg.BGColor; } }
        public override string Text {
            set {
                label.Text = value; 
                labelShadow.Text = value;
                if (int.Parse(value) == 0) { label.IsVisible = false; labelShadow.IsVisible = false; }
                else { binary.BinaryString = (int.Parse(value) % 2).ToString(); }
            }
            get { return label.Text; }
        }
        //public Color TextColor { set { label.TextColor = value; } get { return label.TextColor; } }
        //public double FontSize { set { label.FontSize = value; } get { return label.FontSize; } }
        public override int Type { get => 2; set { /* .. */ } }
        public double RotateStar {
            set {
                starBG.Rotation = value;
                if (starBG.Rotation >= 360) starBG.Rotation -= 360;
                angularValue = (angularValue + 0.025) % (Math.PI * 2);
                if (!Text.Equals("0")) starRing.Scale = Math.Sin(angularValue) * 0.15 + 1;
            }
            get { return starBG.Rotation; }
        }

        public double StarSize { get { return starRing.Scale; } set { starRing.Scale = value; } }
        public async Task FadeStarBase(int delay)
        {
            await Task.WhenAll(
                starBase.FadeTo(0, (uint)delay));
        }
        public override BinaryIndicator Binary => binary;
    }
}