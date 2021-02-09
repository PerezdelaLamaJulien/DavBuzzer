using DavBuzzer.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DavBuzzer.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminPage : ContentPage
    {
        public AdminPage(AdminPageViewModel viewModel)
        {
            InitializeComponent();
            this.BindingContext = viewModel;
        }
    }
}