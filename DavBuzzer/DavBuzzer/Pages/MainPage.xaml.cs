using DavBuzzer.ViewModel;
using Xamarin.Forms;

namespace DavBuzzer.Pages
{
    public partial class MainPage : ContentPage
    {
        MainPageViewModel ViewModel;
        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = ViewModel = new MainPageViewModel();
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
