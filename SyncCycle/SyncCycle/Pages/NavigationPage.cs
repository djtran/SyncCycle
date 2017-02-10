using System;
using System.Collections.Generic;
using System.Text;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace SyncCycle
{
    class NavigationPage : ContentPage
    {
        public NavigationPage()
        {

            Title = "Navigation";
            BackgroundColor = Color.FromRgb(32, 64, 128);

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            checkPerms();
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
                    await DisplayAlert("Location Allowed", "Hopefully this means we can continue!", "OK");
                }
                else if (stat != PermissionStatus.Unknown)
                {
                    await DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
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
