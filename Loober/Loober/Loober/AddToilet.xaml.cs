using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Loober.Helper;
using Loober.Models;
using Plugin.Geolocator;
using Xamarin.Forms.Maps;


namespace Loober
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddToilet : ContentPage
    {
        FirebaseHelper firebaseHelper = new FirebaseHelper();
        public AddToilet()
        {
            InitializeComponent();
            toiletClean.Items.Add("Very Good");
            toiletClean.Items.Add("Good");
            toiletClean.Items.Add("Average");
            toiletClean.Items.Add("Poor");
            toiletClean.Items.Add("Very Poor");
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await RetriveLocation();
        }

        private void CleanPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var clean = toiletClean.Items[toiletClean.SelectedIndex];
        }

        private async Task RetriveLocation()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 20;

            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(100000), null, true);

            toiletLat.Text = position.Latitude.ToString();
            toiletLong.Text = position.Longitude.ToString();

        }

        private async void BtnAddToilet_Clicked(object sender, EventArgs e)
        {
            await firebaseHelper.AddToilet(Convert.ToInt32(toiletId.Text), toiletName.Text, toiletDescription.Text, toiletLat.Text, toiletLong.Text);
            toiletId.Text = string.Empty;
            toiletName.Text = string.Empty;
            toiletDescription.Text = string.Empty;
            toiletLat.Text = string.Empty;
            toiletLong.Text = string.Empty;

            await DisplayAlert("Success", "Toilet Added Successfully", "OK");

            await Navigation.PushAsync(new MainPage());
            {

            }
        }
    }
}