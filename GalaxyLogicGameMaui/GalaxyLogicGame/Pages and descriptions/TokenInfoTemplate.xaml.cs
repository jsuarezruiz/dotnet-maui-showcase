namespace GalaxyLogicGame.Pages_and_descriptions;

public partial class TokenInfoTemplate : AbsoluteLayout
{
	public TokenInfoTemplate()
	{
		InitializeComponent();
	}
    public void AddPowerupDescription(View view)
    {
        powerupDescriptionLayout.Children.Add(view);
    }
    public Label ContractAddressLabel => contractAddressLabel;
	public Label TokenIdLabel => tokenIdLabel;
}