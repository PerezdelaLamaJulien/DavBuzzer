using DavBuzzer.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DavBuzzer.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BuzzPage : ContentPage
    {
        BuzzPageViewModel ViewModel;
        public BuzzPage(BuzzPageViewModel viewModel)
        {
            InitializeComponent();
            this.BindingContext = viewModel;
            ViewModel = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.SetSound(App.Current.Properties["KeySound"].ToString());
        }
    }
}