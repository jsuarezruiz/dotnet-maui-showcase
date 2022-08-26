using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaxyLogicGame.Planet_objects;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Events.EventChallengesBoards
{

    public partial class BinaryBoard : AbsoluteLayout, IBoard
    {
        private const int offset = 62;
        public BinaryBoard()
        {
            InitializeComponent();
        }

        public void ChangeAppearance()
        {
            if (textLayout.Opacity == 0)
            {
                textLayout.FadeTo(1, 250);
                binaryLayout.FadeTo(0, 250);
            }
            else
            {
                textLayout.FadeTo(0, 250);
                binaryLayout.FadeTo(1, 250);
            }
        }

        public async Task Appear(IGameBG bg, string[] formation)
        {
            bg.Board = this;
            bg.LowerUILayout.Children.Add(this);

            // Lower
            bg.LowerUILayout.Children.Remove(this);
            bg.LowerUILayout.Children.Insert(0, this);


            for (int i = 0; i < formation.Length; i++)
            {
                BinaryIndicator binary = new BinaryIndicator
                {
                    TranslationX = -155 + offset * (i + 1),
                    TranslationY = 0,
                    Scale = 0.66,
                    IsVisible = true,
                    BinaryString = formation[i]
                };

                binaryLayout.Children.Add(binary);
            }


            await this.FadeTo(1, 500);

            

        }
        public int MovesLeft { set { leftLabel.Text = value.ToString() + " moves left"; } }

        public async Task Disappear(IGameBG bg)
        {
            await this.FadeTo(0, 250);
            bg.LowerUILayout.Children.Remove(this);
            bg.Board = null;
        }
    }
}