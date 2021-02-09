using Plugin.SimpleAudioPlayer;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace DavBuzzer.ViewModel
{
    public class ListSoundPageViewModel : BaseViewModel
    {
        private ObservableCollection<Sound> _sounds;
        public ObservableCollection<Sound> Sounds
        {
            get { return _sounds; }
            set
            {
                SetProperty(ref _sounds, value);
            }
        }

        private ISimpleAudioPlayer player;
        public ICommand SetSoundCommand { get; set; }

        public ListSoundPageViewModel()
        {
            player = CrossSimpleAudioPlayer.Current;
            SetSoundCommand = new Command(SetSoundAction);
            PopulateList();

            string selectedSound = App.Current.Properties["KeySound"].ToString();
            foreach (Sound s in Sounds)
            {
                if (s.FileName == selectedSound)
                {
                    s.IsSelected = true;
                }
            }

            MessagingCenter.Subscribe<Sound>(this, "ResetSelected", (c) =>
            {
                foreach (Sound s in Sounds)
                {
                    if (s.Name != c.Name)
                    {
                        s.IsSelected = false;
                    }
                    else
                    {
                        s.IsSelected = true;
                        player.Load(GetStreamFromFile(s.FileName));
                        player.Play();
                    }
                }
            });
        }

        private void PopulateList()
        {
            Sounds = new ObservableCollection<Sound>()
            {
                new Sound { Name = "Bip bip", FileName="Bip_Bip.mp3"},
                new Sound { Name = "Boite à Meuh", FileName="Boite_Meuh.mp3"},
                new Sound { Name = "Bourvil", FileName="Bourvil.mp3"},
                new Sound { Name = "Bugs Bunny", FileName="Bugs_Bunny.mp3"},
                new Sound { Name = "Ca Farte ?", FileName="Ca_Farte.mp3"},
                new Sound { Name = "Caisse Enregistreuse", FileName="Caisse_Enregistreuse.mp3"},
                new Sound { Name = "Cartoon Boom", FileName="Cartoon_Boom.mp3"},
                new Sound { Name = "Cartoon Rire", FileName="Cartoon_Rire.mp3"},
                new Sound { Name = "Chat", FileName="Chat.mp3"},
                new Sound { Name = "Cri Wilhelm", FileName="Cri_Wilhelm.mp3"},
                new Sound { Name = "Louis de Funès", FileName="DeFunes.mp3"},
                new Sound { Name = "Gong", FileName="Gong.mp3"},
                new Sound { Name = "Homer", FileName="Homer.mp3"},
                new Sound { Name = "Jouet", FileName="Jouet.mp3"},
                new Sound { Name = "Jouet2", FileName="Cri_Wilhelm.mp3"},
                new Sound { Name = "Klaxon", FileName="Klaxon.mp3"},
                new Sound { Name = "Klaxon2", FileName="Klaxon2.mp3"},
                new Sound { Name = "Mario", FileName="Mario.mp3"},
                new Sound { Name = "PacMan", FileName="PacMan.mp3"},
                new Sound { Name = "Panoramix", FileName="Panoramix.mp3"},
                new Sound { Name = "Question pour un champion", FileName="Question_Pour_Un_Champion.mp3"},
                new Sound { Name = "Windows", FileName="Windows.mp3"},
                new Sound { Name = "Wizz", FileName="Wizz_Messenger.mp3"},
            };
        }

        private async void SetSoundAction(object obj)
        {
            string selectedSound = "";
            foreach (Sound s in Sounds)
            {
                if (s.IsSelected)
                {
                    selectedSound = s.FileName;
                }
            }

            PropertyUtils.SavesProperty("KeySound", selectedSound);
            await App.Current.MainPage.Navigation.PopAsync();
            await MaterialDialog.Instance.SnackbarAsync(message: "Modification sauvegardée.",
                            msDuration: MaterialSnackbar.DurationLong);
        }

        Stream GetStreamFromFile(string filename)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream("DavBuzzer.Audio." + filename);
            return stream;
        }
    }

    public class Sound : BaseViewModel
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set { SetProperty(ref _fileName, value); }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

        public ICommand ClickCardCommand { get; set; }

        public Sound()
        {
            ClickCardCommand = new Command(ClickCardAction);
        }

        private void ClickCardAction(object param)
        {
            MessagingCenter.Send(this, "ResetSelected");
        }
    }
}