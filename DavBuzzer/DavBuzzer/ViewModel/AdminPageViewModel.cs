using DavBuzzer.Models;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XF.Material.Forms;
using XF.Material.Forms.Resources;
using XF.Material.Forms.UI;
using XF.Material.Forms.UI.Dialogs;
using XF.Material.Forms.UI.Dialogs.Configurations;

namespace DavBuzzer.ViewModel
{
    public class AdminPageViewModel : BaseViewModel
    {

        FirebaseClient databaseClient = new FirebaseClient("https://davbuzz-fe92c.firebaseio.com/");
        private ObservableCollection<Player> players;
        public ObservableCollection<Player> Players
        {
            get { return players; }
            set { SetProperty(ref players, value); }
        }

        private string roundLabel = string.Empty;
        public string RoundLabel
        {
            get { return roundLabel; }
            set { SetProperty(ref roundLabel, value); }
        }

        public Room CurrentRoom { get; set; }

        public ICommand RoundDownCommand { get; set; }
        public ICommand RoundUpCommand { get; set; }
        public ICommand SendIndiceCommand { get; set; }

        public AdminPageViewModel()
        {
            RoundDownCommand = new Command(async () => await RoundDownAction());
            RoundUpCommand = new Command(async () => await RoundUpAction());
            SendIndiceCommand = new Command(async () => await SendIndiceAction());
        }

        public void Init()
        {
            //CurrentRoom = await App.DatabaseUtil.GetRoom(App.CurrentRoom);
            Player player1 = new Player() { Name = "Player1", Score = 0, StringColor = "Red" };
            Player player2 = new Player() { Name = "Player2", Score = 0, StringColor = "Blue" };
            Player player3 = new Player() { Name = "Player3", Score = 0, StringColor = "Yellow" };
            Player player4 = new Player() { Name = "Player4", Score = 0, StringColor = "Green" };
            Player player5 = new Player() { Name = "Player5", Score = 0, StringColor = "Purple" };
            List<Player> players = new List<Player>() { player1, player2, player3, player4, player5 };
            CurrentRoom = new Room() { IdRoom = "Demo", Players = players, CurrentRound = 1 };
            Players = new ObservableCollection<Player>();
            ColorTypeConverter converter = new ColorTypeConverter();

            foreach (Player p in CurrentRoom.Players)
            {
                Players.Add(new Player()
                {
                    Name = p.Name,
                    Score = p.Score,
                    UpCommand = new Command(async (player) => await UpAction(player)),
                    DownCommand = new Command(async (player) => await DownAction(player)),
                    SelectedColor = (Color)converter.ConvertFromInvariantString(p.StringColor)
                });
            }
            RoundLabel = String.Concat("Round N°", CurrentRoom.CurrentRound);
        }

        private async Task SendIndiceAction()
        {
            string[] choices = new string[] { "Artiste", "Titre" };

            var result = await MaterialDialog.Instance.SelectActionAsync(title: "Indiquez le type d'indice",
                                                                         actions: choices);

            string input = await MaterialDialog.Instance.InputAsync(title: "Rentrez votre indice",
              message: "Il sera partiellement caché aux joueurs",
              inputText: null,
              inputPlaceholder: "indice",
              confirmingText: "Ok",
              dismissiveText: "Annuler");

            string numberHiddenChar = await MaterialDialog.Instance.InputAsync(title: "Cachez la réponse",
               message: "Choisissez le nombre de lettres cachées aux joueurs",
               inputText: null,
               inputPlaceholder: (input.Replace(" ", "").Length / 2).ToString(),
               confirmingText: "Ok",
               dismissiveText: "Annuler",
               configuration: new MaterialInputDialogConfiguration()
               {
                   BackgroundColor = Color.White,
                   TitleTextColor = Material.GetResource<Color>(MaterialConstants.Color.PRIMARY_VARIANT).MultiplyAlpha(0.8),
                   MessageTextColor = Material.GetResource<Color>(MaterialConstants.Color.PRIMARY_VARIANT).MultiplyAlpha(0.8),
                   TintColor = Material.GetResource<Color>(MaterialConstants.Color.PRIMARY),
                   CornerRadius = 8,
                   ScrimColor = Color.FromHex("#232F34").MultiplyAlpha(0.32),
                   InputType = MaterialTextFieldInputType.Numeric
               });

            if (input != null && numberHiddenChar != null)
            {
                Hint indice = new Hint() { Round = CurrentRoom.CurrentRound, Value = input, NumberCharacterToHide = Int32.Parse(numberHiddenChar) };
                if (result == 0)
                {
                    indice.Type = "Artiste";
                    await App.DatabaseUtil.AddIndice(CurrentRoom, indice);
                }
                else
                {
                    indice.Type = "Titre";
                    await App.DatabaseUtil.AddIndice(CurrentRoom, indice);
                }
            }
        }

        private async Task RoundUpAction()
        {
            CurrentRoom.CurrentRound++;
            RoundLabel = String.Concat("Round N°", CurrentRoom.CurrentRound);

            await App.DatabaseUtil.EditRound(CurrentRoom);
        }

        private async Task RoundDownAction()
        {
            if (CurrentRoom.CurrentRound > 0)
            {
                CurrentRoom.CurrentRound--;
                RoundLabel = String.Concat("Round N°", CurrentRoom.CurrentRound);

                await App.DatabaseUtil.EditRound(CurrentRoom);
            }
        }

        private async Task DownAction(object obj)
        {
            Player player = (Player)obj;
            int index = CurrentRoom.Players.FindIndex(0, CurrentRoom.Players.Count, x => x.Name == player.Name);

            player.Score--;
            player.StringColor = CurrentRoom.Players[index].StringColor;

            CurrentRoom.Players[index] = player;
            await App.DatabaseUtil.EditRoom(CurrentRoom);
        }

        private async Task UpAction(object obj)
        {
            Player player = (Player)obj;
            int index = CurrentRoom.Players.FindIndex(0, CurrentRoom.Players.Count, x => x.Name == player.Name);

            player.Score++;
            player.StringColor = CurrentRoom.Players[index].StringColor;

            CurrentRoom.Players[index] = player;
            await App.DatabaseUtil.EditRoom(CurrentRoom);
        }
    }
}
