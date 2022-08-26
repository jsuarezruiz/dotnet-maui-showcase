namespace GalaxyLogicGame.Pages_and_descriptions;

public partial class WalletDownloadThumbnail : AbsoluteLayout
{
	public WalletDownloadThumbnail()
	{
		InitializeComponent();
	}

    private async void OnClicked(object sender, EventArgs args)
    {
        try
        {
            Uri uri = new Uri(Link);
            await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);

            await ConnectWalletMethod();
        }
        catch
        {

        }
    }
    public Func<Task> ConnectWalletMethod { set; get; }
    public Color TitleColor { set { title.TextColor = value; } }
    public string Title { set { title.Text = value; } get { return title.Text; } }
    public string Description { set { description.Text = value; } get { return description.Text; } }
    public string Icon { set { icon.Source = value; } }
    public ImageSource IconImageSource { set { icon.Source = value; } }
    public string Link { set; get; }
}