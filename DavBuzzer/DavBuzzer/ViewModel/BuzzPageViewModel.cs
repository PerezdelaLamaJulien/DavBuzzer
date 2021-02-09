using DavBuzzer.Models;
using DavBuzzer.Pages;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Database.Streaming;
using Plugin.SimpleAudioPlayer;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DavBuzzer.ViewModel
{
    public class BuzzPageViewModel : BaseViewModel
    {
        #region Attributes
        FirebaseClient databaseClient = new FirebaseClient("https://davbuzz-fe92c.firebaseio.com/");
        public IDisposable eventUpdateRound;
        public IDisposable eventNewHint;

        public ICommand SoundCommand { get; set; }
        public ICommand SettingCommand { get; set; }

        ISimpleAudioPlayer player;
        public Room CurrentRoom { get; set; }

        private string roundLabel = string.Empty;
        public string RoundLabel
        {
            get { return roundLabel; }
            set { SetProperty(ref roundLabel, value); }
        }

        private string hintType = string.Empty;
        public string HintType
        {
            get { return hintType; }
            set { SetProperty(ref hintType, value); }
        }

        private string hintValue = string.Empty;
        public string HintValue
        {
            get { return hintValue; }
            set { SetProperty(ref hintValue, value); }
        }

        private bool isNewHint;
        public bool IsNewHint
        {
            get { return isNewHint; }
            set { SetProperty(ref isNewHint, value); }
        }

        Random random = new Random();
        #endregion

        public BuzzPageViewModel()
        {
            player = CrossSimpleAudioPlayer.Current;
            SoundCommand = new Command(SoundAction);
            SettingCommand = new Command(SettingAction);
        }

        public async Task Init()
        {
            CurrentRoom = await App.DatabaseUtil.GetRoom(App.CurrentRoom);
            RoundLabel = String.Concat("Round N°", CurrentRoom.CurrentRound);

            eventNewHint = databaseClient.Child("Hints").Child(App.CurrentRoom).AsObservable<Hint>().Subscribe(onNext: (FirebaseEvent<Hint> h) =>
            {
                if (h.Object != null)
                {
                    if (h.Object.Round == CurrentRoom.CurrentRound)
                    {
                        HintType = h.Object.Type;
                        HintValue = h.Object.Value;
                        int randomIndex;
                        char randomChar;

                        for (int i = 0; i < h.Object.NumberCharacterToHide; i++)
                        {
                            do
                            {
                                randomIndex = random.Next(0, HintValue.Length);
                                randomChar = HintValue[randomIndex];
                            } while (HintValue[randomIndex] == ' ' || HintValue[randomIndex] == '_');

                            var regex = new Regex(Regex.Escape(HintValue[randomIndex].ToString()));
                            HintValue = regex.Replace(HintValue, "_", 1);
                        }

                        IsNewHint = true;
                    }
                }
            });
        }


        #region Sound
        internal void SetSound(string sound)
        {
            player.Load(GetStreamFromFile(sound));
        }

        private void SettingAction(object obj)
        {
            App.Current.MainPage.Navigation.PushAsync(new ListSoundPage());
        }

        private void SoundAction(object obj)
        {
            player.Play();
        }

        Stream GetStreamFromFile(string filename)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream("DavBuzzer.Audio." + filename);
            return stream;
        }
        #endregion
    }
}
