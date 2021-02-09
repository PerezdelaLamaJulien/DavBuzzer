using System.Windows.Input;
using Xamarin.Forms;

namespace DavBuzzer.Models
{
    public class Player : BaseModel
    {
        public string Name { get; set; }

        private int score;
        public int Score
        {
            get { return score; }
            set { SetProperty(ref score, value); }
        }

        public ICommand UpCommand { get; set; }
        public ICommand DownCommand { get; set; }

        public string StringColor { get; set; }
        public Color SelectedColor { get; set; }
    }
}
