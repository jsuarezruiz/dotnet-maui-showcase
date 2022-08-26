using GalaxyLogicGame.Particles;

namespace GalaxyLogicGame.Pages_and_descriptions;

public partial class KeyPhrasePage : ContentPage
{
    private StarsParticlesLayout stars;
    private bool clicked = true;
    private WalletPreview wallet;
    public KeyPhrasePage(WalletPreview wallet)
    {

        NavigationPage.SetHasNavigationBar(this, false);

        InitializeComponent();

        this.wallet = wallet;

        Functions.ScaleToScreen(this, scaleLayout);
        Functions.FillHeight(scaleLayout);

        SizeChanged += OnDisplaySizeChanged;

        keyPhraseLabel.Text = Preferences.Get("solanaPrivKey", "key phrase not saved");
    }

    private void OnDisplaySizeChanged(object sender, EventArgs args)
    {
        Functions.ScaleToScreen(this, scaleLayout);
        Functions.FillHeight(scaleLayout);
    }
    public void AssignStars(StarsParticlesLayout stars)
    {
        this.stars = stars;
        starsLayout.Children.Add(stars);
    }

    public async Task TransitionIn()
    {
        await Task.WhenAll(
            wallpaper.TranslateTo(0, 0, 500, Easing.SinOut),
            wallpaper.FadeTo(1, 500, Easing.SinOut),
            stars.TransitionUpOut(),
            mainLayout.TranslateTo(0, 0, 500, Easing.SinOut),
            mainLayout.FadeTo(1, 500));

        starsLayout.Children.Clear();


    }
}