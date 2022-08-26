namespace GalaxyLogicGame.Pages_and_descriptions;

public partial class WalletPreview : AbsoluteLayout
{
    private bool owns = false;

    public WalletPreview()
	{
		InitializeComponent();
		Load();
	}
	public async Task Load()
    {
        if (Preferences.ContainsKey("pubKey"))
        {
            try
            {
                label.Text = "Checking NFTs";
                if (await EthFunctions.CheckNFTOwnership(Preferences.Get("pubKey", "Failed")))
                {
                    label.Text = Preferences.Get("pubKey", "Failed").Substring(0, 6) + "..";
                    owns = true;
                }
                else
                {
                    owns = false;
                    label.Text = "Connect wallet";
                }

            }
            catch
            {
                owns = false;
                label.Text = Preferences.Get("pubKey", "Failed").Substring(0, 6) + "..";
            }

        }
        else { label.Text = "Connect wallet"; owns = false; }
    }
    public bool Owns => owns;
}