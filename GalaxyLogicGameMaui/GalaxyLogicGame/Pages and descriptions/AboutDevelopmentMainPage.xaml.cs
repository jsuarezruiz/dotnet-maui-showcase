using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
//using Newtonsoft.Json;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Pages_and_descriptions
{

    public partial class AboutDevelopmentMainPage : ContentPage
    {
        StackLayout update40Layout = new StackLayout
        {
            Spacing = 0,
            Children =
            {
                        new Label{ Text = "- new main menu design", FontSize = 20 },
                        new Label{ Text = "- 2 new objects", FontSize = 20 },
                        new Label{ Text = "- new game mode that replaces the casual mode", FontSize = 20 },
                        new Label{ Text = "- 8 new events", FontSize = 20 },
                        new Label{ Text = "- new icons for events", FontSize = 20 },
                        new Label{ Text = "- 15 new challenges", FontSize = 20 },
                        new Label{ Text = "- 3 new powerups", FontSize = 20 },
                        new Label{ Text = "- improved sudo-random system", FontSize = 20 },
                        new Label{ Text = "- new pauses between events", FontSize = 20 },
                        new Label{ Text = "- many bugs and errors previously found in \"experimental mode\" are now fixed", FontSize = 20 },
                        new Label{ Text = "- new particles", FontSize = 20 },
                        new Label{ Text = "- power-up animations added", FontSize = 20 },
                        new Label{ Text = "- improved graphics and menu descriptions", FontSize = 20 },
                        new Label{ Text = "- unified art style", FontSize = 20 },
                        new Label{ Text = "- About development section added", FontSize = 20 },
                        new Label{ Text = "- new About development articles", FontSize = 20 },
                        //new Label{ Text = "- a sorting system and categories were added to About development", FontSize = 20 },
                        new Label{ Text = "- Credits section added", FontSize = 20 },
                        new Label{ Text = "- a pause screen for saving a battery life", FontSize = 20 },
                        new Label{ Text = "- QoL improvements", FontSize = 20 },
                        new Label {},
                        new Label {},
                        new Label {},

            }
        };
        public AboutDevelopmentMainPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            ReloadOnlineArticles();

            update40Done.FirstLayout = update40Layout;
            if (Device.RuntimePlatform != Device.Tizen) ScaleToScreen();

        }
        private async Task ReloadOnlineArticles()
        {
            HttpClient client = new HttpClient();
            int totalArticles = int.Parse(await client.GetStringAsync("http://rostislavlitovkin.pythonanywhere.com/TotalArticles"));

            for (int i = 0; i < totalArticles; i++)
            {
                string jsonData = await client.GetStringAsync("http://rostislavlitovkin.pythonanywhere.com/ArticleData/" + i);

                //DevelopmentInfoThumbnail article = JsonConvert.DeserializeObject<DevelopmentInfoThumbnail>(jsonData);

                //stackLayout.Children.Add(article);
            }
        }
        public async Task OpenLatest()
        {
            await ((DevelopmentInfoThumbnail)stackLayout.Children[1]).Launch();
        }
        private void ScaleToScreen()
        {
            DisplayInfo displayInfo = DeviceDisplay.MainDisplayInfo;

            double width = displayInfo.Width;
            double height = displayInfo.Height;
            double density = displayInfo.Density;

            double screenWidth = width / density;
            //double screenHeight = height / density;


            //
            //double layoutAspectRatio = 1111111;
            //double screenAspectRatio = height / width;


            // put in if block
            double scale = screenWidth / 360;

            double stackHeight = 200 + 10 * 95;


            if (scale > 1)
            {

                BoxView top = new BoxView { HeightRequest = (stackHeight * (scale - 1)) / 2 }; // change this
                stackLayout.Children.Add(top);

                // Lower
                stackLayout.Children.Remove(top);
                stackLayout.Children.Insert(0, top);


                stackLayout.Children.Add(new BoxView { HeightRequest = (stackHeight * (scale - 1)) / 2 });

            }
            //AbsoluteLayout.SetLayoutBounds(stackLayout, new Rect(0, 0 , 320 * scale, stackHeight * scale));
            //AbsoluteLayout.SetLayoutBounds(mainLayout, new Rect(0, 0, 320 * scale, stackHeight * scale));

            stackLayout.Scale = scale;
        }
    }
}