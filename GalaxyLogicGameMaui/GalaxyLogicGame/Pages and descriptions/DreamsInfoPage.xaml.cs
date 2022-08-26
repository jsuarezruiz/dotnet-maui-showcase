using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GalaxyLogicGame.Pages_and_descriptions
{

    public partial class DreamsInfoPage : ContentPage
    {
        public DreamsInfoPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();
            Functions.ScaleToScreen(this, scaleLayout);
            Functions.FillHeight(scaleLayout);
            SizeChanged += OnDisplaySizeChanged;

        }
        private void OnDisplaySizeChanged(object sender, EventArgs args)
        {
            Functions.ScaleToScreen(this, scaleLayout);
            Functions.FillHeight(scaleLayout);
        }
    }
}