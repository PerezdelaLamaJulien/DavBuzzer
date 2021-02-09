using DavBuzzer.Pages;
using DavBuzzer.Utils;
using DavBuzzer.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms;
using XF.Material.Forms.Resources;
using XF.Material.Forms.UI;
using XF.Material.Forms.UI.Dialogs;
using XF.Material.Forms.UI.Dialogs.Configurations;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace DavBuzzer
{
    public partial class App : Application
    {
        public static DatabaseUtil DatabaseUtil;
        public static string CurrentRoom;
        public static string CurrentPlayer;
        public static bool? IsAdmin;

        public App()
        {
            InitializeComponent();
            DatabaseUtil = new DatabaseUtil();
            PropertyUtils.SavesProperty("KeySound", "Bip_Bip.mp3");
            Material.Init(Application.Current, "Material.Configuration");
            Material.PlatformConfiguration.ChangeStatusBarColor(Color.FromHex("#651758"));

            var loadingDialogConfiguration = new MaterialLoadingDialogConfiguration()
            {
                BackgroundColor = Color.White,
                MessageTextColor = Material.GetResource<Color>(MaterialConstants.Color.PRIMARY_VARIANT).MultiplyAlpha(0.8),
                TintColor = Material.GetResource<Color>(MaterialConstants.Color.PRIMARY),
                CornerRadius = 8,
                ScrimColor = Color.FromHex("#232F34").MultiplyAlpha(0.32)
            };
            var inputDialogConfiguration = new MaterialInputDialogConfiguration()
            {
                BackgroundColor = Color.White,
                TitleTextColor = Material.GetResource<Color>(MaterialConstants.Color.PRIMARY_VARIANT).MultiplyAlpha(0.8),
                MessageTextColor = Material.GetResource<Color>(MaterialConstants.Color.PRIMARY_VARIANT).MultiplyAlpha(0.8),
                TintColor = Material.GetResource<Color>(MaterialConstants.Color.PRIMARY),
                CornerRadius = 8,
                ScrimColor = Color.FromHex("#232F34").MultiplyAlpha(0.32)
            };
            var simpleDialogConfiguration = new MaterialSimpleDialogConfiguration()
            {
                BackgroundColor = Color.White,
                TitleTextColor = Material.GetResource<Color>(MaterialConstants.Color.PRIMARY_VARIANT).MultiplyAlpha(0.8),
                TextColor = Material.GetResource<Color>(MaterialConstants.Color.PRIMARY_VARIANT).MultiplyAlpha(0.8),
                TintColor = Material.GetResource<Color>(MaterialConstants.Color.PRIMARY),
                CornerRadius = 8,
                ScrimColor = Color.FromHex("#232F34").MultiplyAlpha(0.32)
            };

            MaterialDialog.Instance.SetGlobalStyles(loadingDialogConfiguration: loadingDialogConfiguration, inputDialogConfiguration: inputDialogConfiguration, simpleDialogConfiguration: simpleDialogConfiguration);
        }

        protected override void OnStart()
        {
            MainPage = new MaterialNavigationPage(new MainPage());
            /*
             * LISTE TODO : 
             * - Gérer en cas de back sur les différents screens
             * - Recevoir Round, Indice
             * - Envoye des temps (joueurs)
             * - Listener des temps (admin)
             */
        }
    }
}
