using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Pages_and_descriptions
{

    public partial class ChallengeDescriptionWithPowerup : AbsoluteLayout
    {
        private int difficultyLevel;
        private string titleText;
        private Color color;
        private string powerupDescription;
        private string icon;
        private string titleImage;
        //private string videoSource;
        private AbsoluteLayout layout;

        public ChallengeDescriptionWithPowerup()
        {
            InitializeComponent();
        }
        public string TitleImage { set { titleImage = value; } }
        //public string VideoSource { set { videoSource = value; } }
        public AbsoluteLayout PowerupDescriptionLayout { set { this.layout = value; } }
        public bool Completed { set { challengeDescription.Completed = value; } }
        public string Title { set { challengeDescription.Title = value; /*if (value.Length > 22) challengeDescription.Title.FontSize = 16;*/ } get { return challengeDescription.Title; } }
        public int DifficultyLevel {
            set {
                challengeDescription.DifficultyLevel = value;
            }
        }
        public string Description { set { challengeDescription.Description = value; } get { return challengeDescription.Description; } }
        public string PowerupTitle { set { powerupLabel.Text = "Complete to unlock " + value; titleText = value; } }
        public Color Color { set { color = value; } }

        public string PowerupDescription { set { powerupDescription = value; } }
        public string Icon { set { icon = value; } }


        private async void OnClicked(object sender, EventArgs e)
        {
            var page = new PowerupDescriptionPage
            {
                TitleImage = titleImage,
                TitleText = titleText,
                Icon = icon,
                Color = color,
                Description = powerupDescription,
            };
            if (layout != null) page.AddLayout(layout);
            await Navigation.PushAsync(page) ;
        }
    }
}