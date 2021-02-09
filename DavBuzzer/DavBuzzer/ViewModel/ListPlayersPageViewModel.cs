using DavBuzzer.Models;
using DavBuzzer.Pages;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Database.Streaming;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;
using XF.Material.Forms.UI.Dialogs.Configurations;

namespace DavBuzzer.ViewModel
{
    public class ListPlayersPageViewModel : BaseViewModel
    {
        public ObservableCollection<Player> Players { get; set; }
        FirebaseClient databaseClient = new FirebaseClient("https://davbuzz-fe92c.firebaseio.com/");
        public IDisposable eventNewPlayer;
        public ICommand BackButtonCommand { get; set; }
        public ICommand GoNextCommand { get; set; }

        public string TitlePage { get; set; }

        public ListPlayersPageViewModel()
        {
            Players = new ObservableCollection<Player>();
            BackButtonCommand = new Command(async () => await BackButtonAction());
            GoNextCommand = new Command(GoNextAction);
            TitlePage = "Liste des joueurs";
        }

        private async void GoNextAction(object obj)
        {
            if (App.IsAdmin.Value)
            {
                AdminPageViewModel adminPageVM = new AdminPageViewModel();
                adminPageVM.Init();
                await App.Current.MainPage.Navigation.PushAsync(new AdminPage(adminPageVM), true);
            }
            else
            {
                BuzzPageViewModel viewModel = new BuzzPageViewModel();
                viewModel.Init();
                await App.Current.MainPage.Navigation.PushAsync(new BuzzPage(viewModel), true);
            }
        }

        private async Task BackButtonAction()
        {

            MaterialAlertDialogConfiguration materialAlertDialogConfiguration = new MaterialAlertDialogConfiguration()
            {
                TintColor = (Color)App.Current.Resources["Material.Color.Primary"]
            };

            bool? action = await MaterialDialog.Instance.ConfirmAsync(message: "Êtes vous sûr de vouloir quitter la partie?",
                                                                      title: "Quitter la partie?",
                                                                      confirmingText: "Oui",
                                                                      dismissiveText: "Non",
                                                                      configuration: materialAlertDialogConfiguration);

            if (action.HasValue)
            {
                if (action.Value)
                {
                    using (await MaterialDialog.Instance.LoadingDialogAsync(message: "Chargement"))
                    {
                        if (App.IsAdmin.Value)
                        {
                            //await App.DatabaseUtil.DeleteRoom(App.CurrentRoom);
                            await App.Current.MainPage.Navigation.PopAsync(true);

                            //TODO : Avertir les joueurs que la partie a été supprimée
                        }
                        else
                        {
                            await App.DatabaseUtil.DeletePlayer(App.CurrentRoom, App.CurrentPlayer);
                            await App.Current.MainPage.Navigation.PopAsync(true);
                        }
                    }
                }
            }
        }

        public async Task Init()
        {
            Room currentRoom = await App.DatabaseUtil.GetRoom(App.CurrentRoom);
            ColorTypeConverter converter = new ColorTypeConverter();
            if (currentRoom.Players != null)
            {
                foreach (Player p in currentRoom.Players)
                {
                    p.SelectedColor = (Color)converter.ConvertFromInvariantString(p.StringColor);
                    Players.Add(p);
                }
            }

            FirebaseObject<Room> toUpdateRoom = (await databaseClient.Child("Rooms").OnceAsync<Room>()).Where(a => a.Object.IdRoom == App.CurrentRoom).FirstOrDefault();

            eventNewPlayer = databaseClient.Child("Rooms").Child(toUpdateRoom.Key).Child("Players").AsObservable<Player>().Subscribe(onNext: (FirebaseEvent<Player> p) =>
            {
                if (p.Object != null)
                {
                    if (Players.Where(a => a.Name == p.Object.Name).FirstOrDefault() == null)
                    {
                        Players.Add(new Player() { Name = p.Object.Name });
                    }
                }
            });
        }
    }
}
