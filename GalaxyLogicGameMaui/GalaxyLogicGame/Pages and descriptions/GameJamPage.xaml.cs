using GalaxyLogicGame.Particles;

namespace GalaxyLogicGame.Pages_and_descriptions;

public partial class GameJamPage : ContentPage, IStarsPage
{
    private StarsParticlesLayout stars;
    public GameJamPage()
	{
        NavigationPage.SetHasNavigationBar(this, false);
        InitializeComponent();
        Functions.ScaleToScreen(this, scaleLayout);
        Functions.FillHeight(scaleLayout);
        SizeChanged += OnDisplaySizeChanged;
    }
    public void AssingStars(StarsParticlesLayout stars)
    {
        this.stars = stars;
        starsLayout.Children.Add(stars);
    }
    private void OnDisplaySizeChanged(object sender, EventArgs args)
    {
        Functions.ScaleToScreen(this, scaleLayout);
        Functions.FillHeight(scaleLayout);
    }
    public async Task TransitionIn()
    {
        await Task.WhenAll(
            this.TranslateTo(0, 0, 500, Easing.SinOut),
            this.FadeTo(1, 500, Easing.SinOut),
            stars.TransitionUpOut(),
            mainLayout.TranslateTo(0, 0, 500, Easing.SinOut),
            mainLayout.FadeTo(1, 500));

        starsLayout.Children.Clear();
    }
}