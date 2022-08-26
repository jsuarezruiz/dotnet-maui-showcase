namespace GalaxyLogicGame.Pages_and_descriptions;

public partial class BuyPowerupTemplate : AbsoluteLayout
{
	public BuyPowerupTemplate()
	{
		InitializeComponent();
	}
	public void AddPowerupDescription(View view)
	{
		powerupDescriptionLayout.Children.Add(view);
	}
	public Label NetworkLabel => networkLabel;
	public Label PriceLabel => priceLabel;
}