using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Loober.Helper;
using Loober.Models;
using Plugin.Geolocator;
using Xamarin.Forms.Maps;

namespace Loober
{
    public partial class MainPage : ContentPage
    {
        FirebaseHelper firebaseHelper = new FirebaseHelper();
        public MainPage()
        {
            InitializeComponent();
            
            btnGetLocation.Clicked += BtnGetLocation_Clicked;
        }

        private async void BtnGetLocation_Clicked(object sender, EventArgs e)
        {
            await RetriveLocation();
        }

        private async Task RetriveLocation()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 20;

            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(100000), null, true);

            

            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude)
                                , Distance.FromMiles(1)));
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            var allToilets = await firebaseHelper.GetAllToilets();
            lstToilets.ItemsSource = allToilets;
            await RetriveLocation();
        }

        


        public async void BtnAddNewToilet_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddToilet());
            {
                
            }
        }

        

    }
}
