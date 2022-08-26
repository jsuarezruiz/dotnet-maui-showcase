using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Pages_and_descriptions
{

    public partial class DevelopmentInfoThumbnail : AbsoluteLayout
    {
        private string titleText;
        private string text1;
        private string text2;
        private string text3;
        private string mainImageSource;
        private string secondImageSource;
        private string specialCharacter = "";
        private string thirdImageSource;
        private string fourthImageSource;
        private string firstImageSource;
        private Layout firstLayout;
        private bool invertedColors = false;
        public DevelopmentInfoThumbnail()
        {
            InitializeComponent();
        }
        public bool IsTitleVisible { set { titleShadow.IsVisible = value; title.IsVisible = value; } }
        public bool InvertedColors { set { invertedColors = value; } }
        public string TitleText { set { titleText = value; title.Text = value; titleShadow.Text = value; } }
        public string MainImage { set { mainImage.Source = value; mainImageSource = value; } }
        public string FirstImage { set { firstImageSource = value; } }
        public string ThirdText { set { text3 = value; } }
        public string SecondImage { set { secondImageSource = value; } }
        public string ThirdImage { set { thirdImageSource = value; } }
        public string FourthImage { set { fourthImageSource = value; } }
        public string Text { set { text1 = value; } }
        public string SecondText { set { text2 = value; } }
        public string SpecialCharacter { set { specialCharacter = value; } }
        public Layout FirstLayout { set { firstLayout = value; } }

        private async void OnClicked(object sender, EventArgs e)
        {
            if (specialCharacter.Equals("exp"))
                await Navigation.PushAsync(new AboutDevelopmentPageWithExperimentalButton
                {
                    TitleText = titleText,
                    MainImage = mainImageSource,
                    SecondaryImage = secondImageSource,
                    Text = text1,
                    SecondText = text2
                });
            else
            {
                await Launch();

            }
        }
        public async Task Launch()
        {
            AboutDevelopmentPage page = new AboutDevelopmentPage
            {
                InvertedColors = invertedColors,
                TitleText = titleText,
                TitleImage = mainImageSource,
                FirstImage = firstImageSource,
                SecondImage = secondImageSource,
                Text = text1,
                SecondText = text2,
                ThirdImage = thirdImageSource,
                FourthImage = fourthImageSource,
                FirstLayout = firstLayout,
                ThirdText = text3,
            };
            await Navigation.PushAsync(page);
            if (Device.RuntimePlatform != Device.Tizen) page.ScaleToScreen();
        }
    }
}