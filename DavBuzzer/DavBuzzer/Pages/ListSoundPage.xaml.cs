using DavBuzzer.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DavBuzzer.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListSoundPage : ContentPage
    {
        ListSoundPageViewModel ViewModel;
        public ListSoundPage()
        {
            InitializeComponent();
            BindingContext = ViewModel = new ListSoundPageViewModel();
        }
    }
}