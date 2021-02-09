using DavBuzzer.Models;
using DavBuzzer.Pages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace DavBuzzer.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        public ICommand CreateRoomCommand { get; set; }
        public ICommand JoinRoomCommand { get; set; }
        public ICommand BackwardColorCommand { get; set; }
        public ICommand ForwardColorCommand { get; set; }

        public List<ListedColor> ListColor { get; set; }

        private Color selectedColor;
        public Color SelectedColor
        {
            get { return selectedColor; }
            set { SetProperty(ref selectedColor, value); }
        }

        public string RoomId { get; set; }
        public string PlayerName { get; set; }

        public MainPageViewModel()
        {
            CreateRoomCommand = new Command(async () => await CreateRoomActionAsync());
            JoinRoomCommand = new Command(async () => await JoinRoomActionAsync());
            BackwardColorCommand = new Command(() => BackwardColorAction());
            ForwardColorCommand = new Command(() => ForwardColorAction());

            ListColor = new List<ListedColor>()
            {
                new ListedColor(){ Name = "Red", Value = Color.Red},
                new ListedColor(){ Name = "Blue", Value = Color.Blue},
                new ListedColor(){ Name = "Green", Value = Color.Green},
                new ListedColor(){ Name = "Yellow", Value = Color.Yellow},
                new ListedColor(){ Name = "Indigo", Value = Color.Indigo},
                new ListedColor(){ Name = "Violet", Value = Color.Violet},
                new ListedColor(){ Name = "Orange", Value = Color.Orange},
                new ListedColor(){ Name = "Purple", Value = Color.Purple},
                new ListedColor(){ Name = "Silver", Value = Color.Silver},
                new ListedColor(){ Name = "Gold", Value = Color.Gold},
                new ListedColor(){ Name = "Lavender", Value = Color.Lavender}
            };
            SelectedColor = ListColor.First().Value;
        }

        private void ForwardColorAction()
        {
            if (SelectedColor == ListColor.Last().Value)
            {
                SelectedColor = ListColor.First().Value;
            }
            else
            {
                SelectedColor = ListColor.ElementAt(ListColor.IndexOf(ListColor.First(x => x.Value == SelectedColor)) + 1).Value;
            }
        }

        private void BackwardColorAction()
        {
            if (SelectedColor == ListColor.First().Value)
            {
                SelectedColor = ListColor.Last().Value;
            }
            else
            {
                SelectedColor = ListColor.ElementAt(ListColor.IndexOf(ListColor.First(x => x.Value == SelectedColor)) - 1).Value;
            }
        }

        private async Task CreateRoomActionAsync()
        {
            using (await MaterialDialog.Instance.LoadingDialogAsync(message: "Chargement"))
            {
                Room room = await App.DatabaseUtil.GetRoom(RoomId);
                if (room != null)
                {
                    MaterialDialog.Instance.SnackbarAsync("Une Partie a déjà cet identifiant, veuillez en choisir un autre svp", MaterialSnackbar.DurationLong);
                }
                else
                {
                    await App.DatabaseUtil.AddRoom(RoomId);
                    App.CurrentRoom = RoomId;
                    App.IsAdmin = true;
                    ListPlayersPageViewModel listPlayersPageVM = new ListPlayersPageViewModel();
                    await listPlayersPageVM.Init();
                    await App.Current.MainPage.Navigation.PushAsync(new ListPlayersPage(listPlayersPageVM), true);
                }
            }
        }

        private async Task JoinRoomActionAsync()
        {
            using (await MaterialDialog.Instance.LoadingDialogAsync(message: "Chargement"))
            {
                Room room = await App.DatabaseUtil.GetRoom(RoomId);
                if (room == null)
                {
                    MaterialDialog.Instance.SnackbarAsync("Aucune partie trouvée, veuillez réessayer", MaterialSnackbar.DurationLong);
                }
                if (room.Players == null)
                {
                    room.Players = new List<Player>();
                }
                if (room.Players.Where(a => a.Name == PlayerName).FirstOrDefault() != null)
                {
                    MaterialDialog.Instance.SnackbarAsync("Un joueur ou une équipe a déjà ce nom, veuillez réessayer", MaterialSnackbar.DurationLong);
                }
                else
                {
                    App.CurrentRoom = RoomId;
                    App.CurrentPlayer = PlayerName;
                    App.IsAdmin = false;

                    room.Players.Add(new Player() { Name = PlayerName, StringColor = ListColor.First(x => x.Value == SelectedColor).Name });
                    await App.DatabaseUtil.EditRoom(room);

                    ListPlayersPageViewModel listPlayersPageVM = new ListPlayersPageViewModel();
                    await listPlayersPageVM.Init();
                    await App.Current.MainPage.Navigation.PushAsync(new ListPlayersPage(listPlayersPageVM), true);
                }
            }
        }
    }

    public class ListedColor
    {
        public string Name { get; set; }
        public Color Value { get; set; }
    }
}
