using DavBuzzer.Models;
using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DavBuzzer.Utils
{
    public class DatabaseUtil
    {
        FirebaseClient databaseClient = new FirebaseClient("https://davbuzz-fe92c.firebaseio.com/");

        public async Task AddRoom(string IdRoom)
        {
            //TODO Remove les players
            await databaseClient.Child("Rooms").PostAsync<Room>(new Room() { IdRoom = IdRoom, CurrentRound = 1 });
        }

        public async Task<Room> GetRoom(string IdRoom)
        {
            List<Room> rooms = (await databaseClient.Child("Rooms").OnceAsync<Room>()).Select(item => new Room
            {
                IdRoom = item.Object.IdRoom,
                Players = item.Object.Players,
                CurrentRound = item.Object.CurrentRound
            }).ToList();

            return rooms.Where(a => a.IdRoom == IdRoom).FirstOrDefault();

            //var list = await databaseClient.Child("Rooms").OnceAsync<Room>();
            //Debug.WriteLine(list.FirstOrDefault(x => x.Object.IdRoom == IdRoom).Object);
            //return list.FirstOrDefault(x => x.Object.IdRoom == IdRoom).Object;
        }

        public async Task EditRoom(Room roomToEdit)
        {
            FirebaseObject<Room> toUpdateRoom = (await databaseClient.Child("Rooms").OnceAsync<Room>()).Where(a => a.Object.IdRoom == roomToEdit.IdRoom).FirstOrDefault();

            await databaseClient.Child("Rooms").Child(toUpdateRoom.Key).PutAsync(roomToEdit);
        }

        public async Task EditRound(Room roomToEdit)
        {
           FirebaseObject<Room> toUpdateRoom = (await databaseClient.Child("Rooms").OnceAsync<Room>()).Where(a => a.Object.IdRoom == roomToEdit.IdRoom).FirstOrDefault();

            await databaseClient.Child("Rooms").Child(toUpdateRoom.Key).Child("CurrentRound").PutAsync(roomToEdit.CurrentRound);
        }

        public async Task DeleteRoom(string IdRoom)
        {
            var toDeletePerson = (await databaseClient.Child("Rooms").OnceAsync<Room>()).Where(a => a.Object.IdRoom == IdRoom).FirstOrDefault();
            await databaseClient.Child("Rooms").Child(toDeletePerson.Key).DeleteAsync();
        }

        public async Task DeletePlayer(string roomId, string playerName)
        {
            FirebaseObject<Room> roomObject = (await databaseClient.Child("Rooms").OnceAsync<Room>()).Where(a => a.Object.IdRoom == roomId).FirstOrDefault();
            FirebaseObject<Player> playerToDelete = (await databaseClient.Child("Rooms").Child(roomObject.Key).Child("Players").OnceAsync<Player>()).Where(p => p.Object.Name == playerName).FirstOrDefault();
            await databaseClient.Child("Rooms").Child(roomObject.Key).Child("Players").Child(playerToDelete.Key).DeleteAsync();
        }

        public async Task AddIndice(Room roomToEdit, Hint indice)
        {
            await databaseClient.Child("Hints").Child(roomToEdit.IdRoom).PostAsync<Hint>(indice);
        }

        public async Task AddTime(Room roomToEdit, Time time)
        {
            FirebaseObject<Room> toUpdateRoom = (await databaseClient.Child("Rooms").OnceAsync<Room>()).Where(a => a.Object.IdRoom == roomToEdit.IdRoom).FirstOrDefault();

            await databaseClient.Child("Rooms").Child(toUpdateRoom.Key).Child("Times").Child(time.Round).PostAsync<Time>(new Time() { PlayerName = time.PlayerName, SendTime = time.SendTime });
        }
    }
}
