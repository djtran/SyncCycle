using System;
using System.Collections.Generic;
using System.Text;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;
using Plugin.Geolocator;


namespace SyncCycle
{
    class NavigationPage : ContentPage
    {

        Position p;
        Map map = new Map(
              MapSpan.FromCenterAndRadius(new Position(42.3432733, -71.1074225), Distance.FromMiles(0.3)));

        public NavigationPage()
        {

            Title = "Navigation";
            Content = new StackLayout()
            {

                Children =
                    {
                          map
                    }
            };

        }


        public async void help()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;
            var temp = await locator.GetPositionAsync(timeoutMilliseconds: 10000);

            p = new Position(temp.Latitude, temp.Longitude);
            Console.WriteLine("THIS IS THE POSTION: (" + p + ")");
            map.MoveToRegion(new MapSpan(map.VisibleRegion.Center, p.Latitude, p.Longitude));

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            checkPerms();
        }

        async Task checkBluetoothOn()
        {

        }

        async Task checkPerms()
        {
            Console.WriteLine("!                           CHECKING PERMISSIONS");
            Console.WriteLine("!                           CHECKING PERMISSIONS");
            Console.WriteLine("!                           CHECKING PERMISSIONS");

            try
            {
                var stat = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (stat != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        await DisplayAlert("Need location", "Gunna need that location", "OK");
                    }
                    await DisplayAlert("Bout to ask you for some nice permissions", "", "okay");
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    stat = results[Permission.Location];
                }


                if (stat == PermissionStatus.Granted)
                {
                    Console.WriteLine("We got permissions-desu.");
                }
                else if (stat != PermissionStatus.Unknown)
                {
                    await DisplayAlert("Location Denied", "Can not continue.", "OK");
                }
                else
                {
                    Console.WriteLine("UHGH>GSD>F this sucks.");
                    Console.WriteLine(String.Format("stat : {0} ",stat ));
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("Exception " + ex.Message + " encountered when checking for permissions.");

            }
        }
    }
}
