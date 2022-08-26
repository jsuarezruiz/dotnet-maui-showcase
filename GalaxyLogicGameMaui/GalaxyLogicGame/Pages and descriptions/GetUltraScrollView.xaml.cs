using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Pages_and_descriptions
{

    public partial class GetUltraScrollView : AbsoluteLayout
    {
        private AbsoluteLayout layout;
        public GetUltraScrollView()
        {
            InitializeComponent();
        }
        public async Task Appear(AbsoluteLayout layout)
        {
            this.layout = layout;
            layout.IsVisible = true;
            layout.Children.Add(this);
            this.FadeTo(1, 500);

        }
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await this.FadeTo(0, 500);
            layout.Children.Remove(this);
            layout.IsVisible = false;
        }
    }
}