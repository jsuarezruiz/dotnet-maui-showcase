using GalaxyLogicGame.Particles;
using Newtonsoft.Json;
using Solnet.Programs;
using Solnet.Rpc;
using Solnet.Rpc.Builders;
using Solnet.Wallet;
using Solnet.Wallet.Bip39;
using System.Text;

namespace GalaxyLogicGame.Pages_and_descriptions;

public partial class SolanaConnectPage : ContentPage
{
    private StarsParticlesLayout stars;
    private bool clicked = true;
    private WalletPreview wallet;

    private Wallet solanaWallet;

    private IRpcClient rpcClient = ClientFactory.GetClient(Cluster.DevNet);
    private IStreamingRpcClient streamingRpcClient = ClientFactory.GetStreamingClient(Cluster.DevNet);
    public SolanaConnectPage(WalletPreview wallet)
    {

        NavigationPage.SetHasNavigationBar(this, false);

        InitializeComponent();

        this.wallet = wallet;

        Functions.ScaleToScreen(this, scaleLayout);
        Functions.FillHeight(scaleLayout);

        SizeChanged += OnDisplaySizeChanged;

        // logging into the wallet
        if (Preferences.ContainsKey("solanaPrivKey"))
        {
            solanaWallet = new Wallet(Preferences.Get("solanaPrivKey", "failed"), WordList.English);
        }
        else
        {
            var newMnemonic = new Mnemonic(WordList.English, WordCount.Twelve);

            string[] words = newMnemonic.Words;

            string realNewMnemonics = "";

            for(int i = 0; i < words.Length-1; i++)
            {
                realNewMnemonics += words[i] + " ";
            }
            realNewMnemonics += words[words.Length - 1];

            Preferences.Set("solanaPrivKey", realNewMnemonics);
            
            solanaWallet = new Wallet(newMnemonic);
        }
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


        await GenerateTransactionQRCode();


        // getting balance 
        /*

        var balanceResponse = await rpcClient.GetBalanceAsync(solanaWallet.Account.PublicKey);

        var balance = JsonConvert.DeserializeObject<Balance>(balanceResponse.RawRpcResponse);

        balanceLabel.Text = "Balance: " + balance.result.value;


        accountLabel.Text = solanaWallet.Account + "    " + solanaWallet.GetAccount(0) + "    "  + solanaWallet.GetAccount(2)+"    " + solanaWallet.GetAccount(2).PublicKey;


        var person = new PostData { URI = solanaWallet.GetAccount(0).PublicKey, FileName = "Solana" + solanaWallet.GetAccount(0).PublicKey };

        var json = JsonConvert.SerializeObject(person);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var url = "http://rostislavlitovkin.pythonanywhere.com/GenerateQR";
        using var client = new HttpClient();

        await client.PostAsync(url, data);

        link.Source = "http://rostislavlitovkin.pythonanywhere.com/QR/Solana" + solanaWallet.GetAccount(0).PublicKey;
        /*
        var blockHash = rpcClient.GetRecentBlockHash();

        var tx = new TransactionBuilder().
            SetRecentBlockHash(blockHash.Result.Value.Blockhash).
            SetFeePayer();
        */

    }

    private async Task GenerateTransactionQRCode()
    {

    }

    private async void KeyPhraseClicked(object sender, EventArgs e)
    {
        KeyPhrasePage page = new KeyPhrasePage(wallet);

        DisplayInfo display = DeviceDisplay.MainDisplayInfo;
        double ratio = display.Height / display.Width > 1 ? display.Height / display.Width : 1;

        await Task.WhenAll(
            this.stars.TransitionUpIn(),

            mainLayout.TranslateTo(0, -360 * ratio, 500, Easing.SinIn),

            //wallpaper.TranslateTo(0, -180, 500, Easing.SinIn),
            wallpaper.FadeTo(0, 500, Easing.SinIn));

        starsLayout.Children.Remove(this.stars);
        page.AssignStars(stars);
        await Navigation.PushAsync((Page)page, false);
        await page.TransitionIn();

        mainLayout.TranslationX = 0;
        mainLayout.TranslationY = 0;
        wallpaper.Opacity = 1;

        this.stars = new StarsParticlesLayout();
        starsLayout.Children.Add(this.stars);
    }


    // Just DTOs
    public class Context
    {
        public string apiVersion { get; set; }
        public int slot { get; set; }
    }

    public class Result
    {
        public Context context { get; set; }
        public int value { get; set; }
    }

    public class Balance
    {
        public string jsonrpc { get; set; }
        public Result result { get; set; }
        public int id { get; set; }
    }

    public class PostData
    {
        public string URI { get; set; }
        public string FileName { get; set; }

    }

    private async void MintNFTClicked(object sender, EventArgs e)
    {
        var blockHash = rpcClient.GetRecentBlockHash();

        /*var txFake = new TransactionBuilder().SetRecentBlockHash(blockHash.Result.Value.Blockhash).
            SetFeePayer(solanaWallet.GetAccount(0).PublicKey).
            AddInstruction(TokenProgram.Transfer(
                solanaWallet.GetAccount(0).PublicKey,
                new PublicKey("2VsUVaEuMuQHC7HNKGdV61N3hUTmSFcBiG6EgHWycqCJ"),
                25000,
                solanaWallet.GetAccount(0).PublicKey)).Build(solanaWallet.Account);*/

        var fromAccount = solanaWallet.Account.PublicKey;

        var tx = new TransactionBuilder().
            SetRecentBlockHash(blockHash.Result.Value.Blockhash).
            SetFeePayer(fromAccount).
            AddInstruction(MemoProgram.NewMemo(fromAccount, "Hello from GLG :)")).
            AddInstruction(SystemProgram.Transfer(fromAccount, new PublicKey("2VsUVaEuMuQHC7HNKGdV61N3hUTmSFcBiG6EgHWycqCJ"), 1230)).
            Build(solanaWallet.Account);

        var firstSig = rpcClient.SendTransaction(tx);



        var balanceResponse = await rpcClient.GetBalanceAsync(solanaWallet.Account.PublicKey);

        var balance = JsonConvert.DeserializeObject<Balance>(balanceResponse.RawRpcResponse);

        //balanceLabel.Text = "Balance: " + balance.result.value;
    }
}