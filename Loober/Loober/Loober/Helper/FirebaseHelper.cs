using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Loober.Models;

namespace Loober.Helper
{
    class FirebaseHelper
    {
        FirebaseClient firebase = new FirebaseClient("https://loober-42c73.firebaseio.com/");

        public async Task<List<Toilet>> GetAllToilets()
        {
            return (await firebase.Child("Toilets").OnceAsync<Toilet>()).Select(item => new Toilet
            {
                ToiletId = item.Object.ToiletId,
                Name = item.Object.Name,
                Description = item.Object.Description,
                Latitude = item.Object.Latitude,
                Longitude = item.Object.Longitude
            }).ToList();
        }

        public async Task AddToilet(int toiletId, string name, string description, string Latitude, string Longitude)
        {
            await firebase.Child("Toilets").PostAsync(new Toilet()
            {
                ToiletId = toiletId,
                Name = name,
                Description = description,
                Latitude = Latitude,
                Longitude = Longitude
            });
        }

        public async Task<Toilet> GetToilet(int toiletId)
        {
            var allToilet = await GetAllToilets();
            await firebase.Child("Toilets").OnceAsync<Toilet>();
            return allToilet.Where(a => a.ToiletId == toiletId).FirstOrDefault();
        }

        public async Task UpdateToilet(int toiletId, string name, string description, string Latitude, string Longitude)
        {
            var toUpdateToilet = (await firebase.Child("Toilets").OnceAsync<Toilet>()).Where(a => a.Object.ToiletId == toiletId).FirstOrDefault();
            await firebase.Child("Toilets").Child(toUpdateToilet.Key).PutAsync(new Toilet()
            {
                ToiletId = toiletId,
                Name = name,
                Description = description,
                Latitude = Latitude,
                Longitude = Longitude
            });
        }

        public async Task DeleteToilet(int toiletId)
        {
            var toDeleteToilet = (await firebase.Child("Toilets").OnceAsync<Toilet>()).Where(a => a.Object.ToiletId == toiletId).FirstOrDefault();
            await firebase.Child("Toilets").Child(toDeleteToilet.Key).DeleteAsync();
        }
    }
}
