using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Loober.Helper;
using Loober.Models;
using Plugin.Geolocator;

namespace Loober
{
    public partial class MainPage : ContentPage
    {
        FirebaseHelper firebaseHelper = new FirebaseHelper();
        public MainPage()
        {
            InitializeComponent();
            CleanPicker.Items.Add("Very Good");
            CleanPicker.Items.Add("Good");
            CleanPicker.Items.Add("Average");
            CleanPicker.Items.Add("Poor");
            CleanPicker.Items.Add("Very Poor");
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

            txtLat.Text = position.Latitude.ToString();
            txtLong.Text = position.Longitude.ToString();

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            var allToilets = await firebaseHelper.GetAllToilets();
            lstToilets.ItemsSource = allToilets;
            await RetriveLocation();
        }

        private void CleanPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var clean = CleanPicker.Items[CleanPicker.SelectedIndex];


        }

        private async void BtnAdd_Clicked(object sender, EventArgs e)
        {
            await firebaseHelper.AddToilet(Convert.ToInt32(txtId.Text), txtName.Text, txtDescription.Text, txtLat.Text, txtLong.Text);
            txtId.Text = string.Empty;
            txtName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtLat.Text = string.Empty;
            txtLong.Text = string.Empty;

            await DisplayAlert("Success", "Toilet Added Successfully", "OK");
            var allToilets = await firebaseHelper.GetAllToilets();
            lstToilets.ItemsSource = allToilets;
        }

        private async void BtnRead_Clicked(object sender, EventArgs e)
        {
            var toilet = await firebaseHelper.GetToilet(Convert.ToInt32(txtId.Text));
            if (toilet != null)
            {
                txtId.Text = toilet.ToiletId.ToString();
                txtName.Text = toilet.Name;
                txtDescription.Text = toilet.Description;
                txtLong.Text = toilet.Longitude;
                txtLat.Text = toilet.Latitude;
                await DisplayAlert("Success", "Toilet Retrive Successfully", "OK");
            }
            else
            {
                await DisplayAlert("Success", "No Toilet Available", "OK");
            }
        }

        private async void BtnUpdate_Clicked(object sender, EventArgs e)
        {
            await firebaseHelper.UpdateToilet(Convert.ToInt32(txtId.Text), txtName.Text, txtDescription.Text, txtLat.Text, txtLong.Text);
            txtId.Text = string.Empty;
            txtName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtLat.Text = string.Empty;
            txtLong.Text = string.Empty;

            await DisplayAlert("Success", "Toilet Updated Successfully", "OK");
            var allToilets = await firebaseHelper.GetAllToilets();
            lstToilets.ItemsSource = allToilets;
        }

        private async void BtnDelete_Clicked(object sender, EventArgs e)
        {
            await firebaseHelper.DeleteToilet(Convert.ToInt32(txtId.Text));
            await DisplayAlert("Success", "Toilet Deleted Successfully", "OK");

            var allToilets = await firebaseHelper.GetAllToilets();
            lstToilets.ItemsSource = allToilets;
        }

        //public bool IsLocationAvailable()
        //{
        // if (!CrossGeolocator.IsSupported)
        // return false;

        //return CrossGeolocator.Current.IsGeolocationAvailable;
        // }
    }
}
