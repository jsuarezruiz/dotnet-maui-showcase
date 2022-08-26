
using CommunityToolkit.Maui.Alerts;
using GalaxyLogicGame.Events;
using GalaxyLogicGame.Particles;
using Microsoft.Maui.Storage;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Util;
using Nethereum.Web3;
using Newtonsoft.Json;
using System.Text;
using WalletConnectSharp.Core.Models;
using WalletConnectSharp.Desktop;
using WalletConnectSharp.NEthereum;

namespace GalaxyLogicGame.Pages_and_descriptions;

public partial class CryptoConnectPage : ContentPage
{
    private StarsParticlesLayout stars;
    private Web3 web3;
    private string account;
    private readonly string contractAddress = EthFunctions.GetEthereumContractAddress;
    private readonly string providerAddress = EthFunctions.GetEthereumProvider;
    private bool clicked = true;
    private WalletPreview wallet;
    public CryptoConnectPage(WalletPreview wallet)
    {
        NavigationPage.SetHasNavigationBar(this, false);

        InitializeComponent();

        this.wallet = wallet;

        Functions.ScaleToScreen(this, scaleLayout);
        Functions.FillHeight(scaleLayout);

        SizeChanged += OnDisplaySizeChanged;
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

    public async Task Connect() {
        if (Preferences.ContainsKey("pubKey"))
        {
            try
            {
                if (await CheckNFTs())
                {
                    await ShowNFTs();
                }
                else
                {
                    OfferConnectingNewWallet();
                }
            }
            catch
            {
                NoInternetError();
            }
        }

        else await OfferConnectingNewWallet();
    }
    private async Task<bool> CheckNFTs()
    {
        ClearMainStackLayout();
        mainStackLayout.Children.Add(new Image { Source = "checkingnfts.png", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center,
            WidthRequest = 270, HeightRequest = 270 });
        return await EthFunctions.CheckNFTOwnership(Preferences.Get("pubKey", ""));
    }
    private async Task OfferConnectingNewWallet()
    {
        ClearMainStackLayout();
        try
        {
            var metadata = new ClientMeta()
            {
                Description = "This is a test of the Nethereum.WalletConnect feature",
                Icons = new[] { "https://rostislavlitovkin.pythonanywhere.com/logo" },
                Name = "WalletConnect Test",
                URL = "https://rostislavlitovkin.pythonanywhere.com"
            };

            var walletConnect = new WalletConnect(metadata);

            //linkLabel.Text = walletConnect.URI; // good for debugging
            if (Functions.IsSquareScreen())
            {
                mainStackLayout.Children.Add(new Image
                {
                    Source = "loadingqrcode.png",
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    WidthRequest = 270,
                    HeightRequest = 270
                });

                string fileName = walletConnect.URI.Substring(3, walletConnect.URI.IndexOf("@") - 3);


                var postData = new PostData { URI = walletConnect.URI, FileName = fileName };

                var json = JsonConvert.SerializeObject(postData);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var url = "http://rostislavlitovkin.pythonanywhere.com/GenerateQR";
                using var client = new HttpClient();

                await client.PostAsync(url, data);

                var img = await client.GetAsync("http://rostislavlitovkin.pythonanywhere.com/QR/" + fileName);

                ClearMainStackLayout();
                mainStackLayout.Children.Add(new Image
                {
                    Source = "http://rostislavlitovkin.pythonanywhere.com/QR/" + fileName,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    WidthRequest = 270,
                    HeightRequest = 270
                });

                await Task.Delay(500);

                await client.GetAsync("http://rostislavlitovkin.pythonanywhere.com/removeQR/" + fileName);
            }
            else
            {
                try
                {
                    Uri uri = new Uri(walletConnect.URI);
                    await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
                }
                catch (Exception ex)
                {
                    OfferDownloadingWallet();
                }
            }

            if (walletConnect != null) try { await walletConnect.Connect(); } catch (Exception ex) { AddLabelToStackLayout(ex.Message); }
            else AddLabelToStackLayout("It is null");

            account = walletConnect.Accounts[0];
            Preferences.Set("pubKey", account);
            await wallet.Load();

            mainStackLayout.Children.Add(new Image
            {
                Source = "checkingnfts.png",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 270,
                HeightRequest = 270
            });
            AddLabelToStackLayout("Connected to: " + account.Substring(0, 6) + "..");

            web3 = new Web3(walletConnect.CreateProvider(new Uri(providerAddress)));

            AddLogOutButton();

            if (await CheckNFTs())
            {
                await ShowNFTs();
            }
            else
            {
                OfferMinting();
            }
        }
        catch
        {
            NoInternetError();
        }
    }

    private async Task OfferDownloadingWallet()
    {
        mainStackLayout.Children.Add(new WalletDownloadThumbnail
        {
            Icon = "metamask.png",
            Title = "Metamask",
            Description = "The most popular Ethereum wallet",
            Link = "https://play.google.com/store/apps/details?id=io.metamask",
            ConnectWalletMethod = OfferConnectingNewWallet,
        });
    }
    private void OfferMinting()
    {
        ClearMainStackLayout();

        BuyPowerupTemplate template = new BuyPowerupTemplate();
        template.NetworkLabel.TextColor = Color.FromArgb("ff0000");
        template.NetworkLabel.Text = "Optimism testnet";

        template.PriceLabel.Text = "Mint for 0 Eth";
        template.PriceLabel.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async () => { await OnMintClicked(); }) });
        
        template.AddPowerupDescription(new AtomicBombEvent().GetEventDescription);

        mainStackLayout.Children.Add(template);
    }
    private async Task ShowNFTs()
    {
        ClearMainStackLayout();

        TokenInfoTemplate template = new TokenInfoTemplate();
        template.ContractAddressLabel.Text = contractAddress.Substring(0, 20) + "..";
        template.ContractAddressLabel.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async () => {
            if (Functions.IsSquareScreen()) { qrCodeLayout.IsVisible = true; await qrCodeLayout.FadeTo(1, 500); }
            else if (template.ContractAddressLabel.Text != "Copied to clipboard")
            {
                await Clipboard.Default.SetTextAsync(contractAddress);

                template.ContractAddressLabel.Text = "Copied to clipboard";

                var toast = Toast.Make("Copied to clipboard");
                await toast.Show();

                template.ContractAddressLabel.Text = contractAddress.Substring(0, 20) + "..";
            }
        }) });

        string[] address = { contractAddress };
        var tempWeb3 = new Web3(providerAddress);
        try
        {
            var thing = await tempWeb3.Eth.ERC721.GetAllTokenUrlsOfOwnerUsingTokenOfOwnerByIndexAndMultiCallAsync(Preferences.Get("pubKey", "Failed"), address);
            template.TokenIdLabel.Text = "Token ID: " + thing[0].TokenId;
        }
        catch
        {
            template.TokenIdLabel.Text = "Token ID not loaded";
            template.TokenIdLabel.TextColor = Color.FromArgb("bbb");
        }

        template.AddPowerupDescription(new AtomicBombEvent().GetEventDescription);
        mainStackLayout.Children.Add(template);

        mainStackLayout.Children.Add(new BoxView { HeightRequest = 10 });

        AddLogOutButton();
    }
    private void NoInternetError()
    {
        ClearMainStackLayout();
        mainStackLayout.Children.Add(new Image
        {
            Source = "nointernet.png",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            WidthRequest = 270,
            HeightRequest = 270
        });
    }
    private void ClearMainStackLayout()
    {
        if (mainStackLayout.Children.Count > 0) mainStackLayout.Children.Clear();
    }
    private void AddLabelToStackLayout(string txt)
    {
        mainStackLayout.Children.Add(new Label
        {
            Text = txt,
            HorizontalTextAlignment = TextAlignment.Center,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            WidthRequest = 270,
            FontSize = 40
        });
    }
    private void AddLogOutButton()
    {
        Label logOutButton = new Label
        {
            Text = "Log out",
            TextColor = Color.FromArgb("FF8B0000"),
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            FontAttributes = FontAttributes.Bold,
            FontFamily = "BigNoodleTitling",
            FontSize = 40
        };
        logOutButton.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async () => { await OnLogOutClicked(); }) });
        mainStackLayout.Children.Add(logOutButton);
    }
    private async Task OnMintClicked()
    {
        ClearMainStackLayout();
        if (clicked)
        {
            clicked = false;

            await MintNFT();

            clicked = true;
        }
    }
    public async Task MintNFT()
    {
        /*var mintFunctionMessage = new MintFunction()
        {
            Uri = "http://rostislavlitovkin.pythonanywhere.com/tGLG",
        };*/
        //var balanceHandler = web3.Eth.GetContractQueryHandler<MintFunction>();
        //await balanceHandler.QueryAsync<IFunctionOutputDTO>(account, mintFunctionMessage);

        /*var abi = @"function mint(string memory _uri) public payable returns(bool)";
        var contract = web3.Eth.GetContract(abi, contractAddress);
        var result = await contract.GetFunction("mint").SendTransactionAsync(contractAddress, "http://rostislavlitovkin.pythonanywhere.com/tGLG");  //.CallAsync<bool>("http://rostislavlitovkin.pythonanywhere.com/tGLG", account);
        */

        try
        {
            mainStackLayout.Children.Add(new Image
{
                Source = "checkyourwallet.png",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 270,
                HeightRequest = 270
            });
            var transferHandler = web3.Eth.GetContractTransactionHandler<MintFunction>();
            var mint = new MintFunction()
            {
                Uri = "https://rostislavlitovkin.pythonanywhere.com/tGLG",
                Address = account
            };

            var index = await transferHandler.SendRequestAsync(contractAddress, mint);
            if (/*index != -1 ||*/ true)
            {
                /*ClearMainStackLayout();
                
                var person = new PostData { URI = "https://kovan-optimistic.etherscan.io/tx/" + index, FileName = index };

                var json = JsonConvert.SerializeObject(person);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var url = "http://rostislavlitovkin.pythonanywhere.com/GenerateQR";
                using var client = new HttpClient();

                await client.PostAsync(url, data);

                var img = await client.GetAsync("http://rostislavlitovkin.pythonanywhere.com/QR/" + index);

                mainStackLayout.Children.Add(new Image
                {
                    Source = "http://rostislavlitovkin.pythonanywhere.com/QR/" + index,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    WidthRequest = 270,
                    HeightRequest = 270
                });
                await client.GetAsync("http://rostislavlitovkin.pythonanywhere.com/removeQR/" + index);

                AddLabelToStackLayout("NFT minted!");
                */

                await ShowNFTs();
            }
        }
        catch
        {
            if (await CheckNFTs())
            {
                await ShowNFTs();
            }

            else OfferMinting();
        }

    }

    [Function("mint", "int")]
    public class MintFunction : FunctionMessage
    {
        [Parameter("address", "payer", 1)]
        public string Address { get; set; }

        [Parameter("string", "tokenURI", 2)]
        public string Uri { get; set; }
    }

    public class PostData
    {
        public string URI { get; set; }
        public string FileName { get; set; }

    }

    private async Task OnLogOutClicked()
    {
        Preferences.Remove("pubKey");

        ClearMainStackLayout();
        
        await wallet.Load();
        await Connect();
    }

    private async void OnQRCodeLayoutClicked(object sender, EventArgs e)
    {
        await qrCodeLayout.FadeTo(0, 500);
        qrCodeLayout.IsVisible = false;
    }
}