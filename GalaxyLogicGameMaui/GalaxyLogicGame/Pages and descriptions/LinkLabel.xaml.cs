using WalletConnectSharp.Desktop;

namespace GalaxyLogicGame.Pages_and_descriptions;

public partial class LinkLabel : StackLayout
{
	public LinkLabel()
	{
		InitializeComponent();
	}
    public int FontSize { set { label.FontSize = value; externalLinkImage.WidthRequest = value; externalLinkImage.HeightRequest = value; } }
	public string Text { set { label.Text = value; } } 
	private async void OnClicked(object sender, EventArgs e)
	{
        try
        {
            Uri uri = new Uri(label.Text);
            await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }
        catch
        {

        }
    }
}