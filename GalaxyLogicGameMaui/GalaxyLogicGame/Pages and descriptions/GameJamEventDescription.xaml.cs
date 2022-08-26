namespace GalaxyLogicGame.Pages_and_descriptions;

public partial class GameJamEventDescription : AbsoluteLayout
{
    private int index = 0;
    public GameJamEventDescription()
	{
		InitializeComponent();
	}
    public TapGestureRecognizer OpenGameJamPage { set { this.GestureRecognizers.Add(value); } }
    public Color TitleColor { set { title.TextColor = value; } }
    public string Title { set { title.Text = value; } get { return title.Text; } }
    public string Description { set { description.Text = value; } get { return description.Text; } }
    public string Icon { set { thumbnail.Source = value; } }
    public ImageSource IconImageSource { set { thumbnail.Source = value; } }
    public int Index { get => index; set { index = value; } }
}