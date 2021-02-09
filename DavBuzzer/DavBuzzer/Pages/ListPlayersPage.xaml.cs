using DavBuzzer.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;

namespace DavBuzzer.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListPlayersPage : ContentPage
    {
        ListPlayersPageViewModel ViewModel;
        public ListPlayersPage(ListPlayersPageViewModel viewModel)
        {
            InitializeComponent();
            this.BindingContext = ViewModel = viewModel;
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ViewModel.eventNewPlayer.Dispose();
        }
    }
}