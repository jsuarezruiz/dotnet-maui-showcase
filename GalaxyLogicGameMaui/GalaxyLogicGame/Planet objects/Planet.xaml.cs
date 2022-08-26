using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaxyLogicGame.ButtonBackgroundThings;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;

namespace GalaxyLogicGame.Planet_objects
{

    public partial class Planet : PlanetBase
    {
        private IButtonBG bg;
        private int dreamNumber = 0;
        
        private int type;
        /// <type values>
        /// -1 = empty space
        /// 0 = basic planet with some number determining its value
        /// 1 = merging planet
        /// 2 = supernova
        /// 3 = shrinking giant
        /// 4 = debris
        /// 5 = blueberry
        /// 
        /// Blackhole is not a planet
        public Planet()
        {
            InitializeComponent();

            if (true)// (Device.RuntimePlatform == Device.Tizen)
            {
                bg = new ButtonBG { CornerRadius = 30, IsEnabled = false, };
            }
            else
            {
                bg = new BoxViewBG { CornerRadius = 30 };
            }
            mainLayout.Children.Add((View)bg);
            AbsoluteLayout.SetLayoutBounds((View)bg, new Rect(0.5, 0.5, 60, 60));
            AbsoluteLayout.SetLayoutFlags((View)bg, AbsoluteLayoutFlags.PositionProportional);
            // Lower
            mainLayout.Children.Remove((View)bg);
            mainLayout.Children.Insert(0, (View)bg);
        }
        //public Position Position { set { AbsoluteLayout.SetLayoutBounds(wholeLayout, new Rect(value.X, value.Y, 60, 60)); } get { return  } }
        public override Color BGColor { set { bg.BGColor = value; } get { return bg.BGColor; } }
        public override string Text {
            set {
                label.Text = value;

                if (this.type == 0) // setting up colors
                {
                    Color tempColor = Functions.GetColor(int.Parse(label.Text) - 1);
                    bg.BGColor = tempColor;
                    label.TextColor = Functions.DetermineWhiteOrBlack(tempColor);
                    binary.BinaryString = binary.BinaryString = (int.Parse(value) % 2).ToString();
                }
                else if (this.type == 3) binary.BinaryString = (int.Parse(value) % 2).ToString();
            }
            get { return label.Text; }
        }
        public Color TextColor { set { label.TextColor = value; } get { return label.TextColor; } }
        //public double FontSize { set { label.FontSize = value; } get { return label.FontSize; } }
        public override int Type {
            set {
                type = value;
                if (value == 1) binary.BinaryString = "X";
                else if (value == -1) { mainLayout.IsVisible = false; } // later add a void object
            }
            get { if (type == 3) return 0; return type; }
        }
        public override bool IsTypeThree => type == 3;

        public override BinaryIndicator Binary => binary;
        public override int DreamNumber {
            get {
                return dreamNumber;
            }

            set {
                dreamNumber = value;
                dreamNumberLayout.IsVisible = dreamNumber != 0;
                if (dreamNumberLayout.IsVisible) dreamNumberLabel.Text = dreamNumber.ToString();
            }
        }
    }
}