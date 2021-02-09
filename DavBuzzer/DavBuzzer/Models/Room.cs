using System.Collections.Generic;

namespace DavBuzzer.Models
{
    public class Room : BaseModel
    {
        public string IdRoom { get; set; }

        public List<Player> Players { get; set; }

        public int CurrentRound { get; set; }
    }
}