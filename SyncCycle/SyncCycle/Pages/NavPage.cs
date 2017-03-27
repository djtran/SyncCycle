using System;
using System.Collections.Generic;
using System.Text;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;
using Plugin.Geolocator;
using System.Net;

namespace SyncCycle
{
    
    class NavPage : ContentPage
    {

        Position p;
        
        Map map = new Map(
              MapSpan.FromCenterAndRadius(new Position(0, 0), Distance.FromMiles(0.3)));

        public NavPage()
        {

            Back_2_Home.Clicked += On_Back_2_Home_Clicked;
            Title = "Navigation";
            Content = new StackLayout()
            {

                Children =
                    {
                          map, 
                          Back_2_Home
                    }
            };       
        }



        Button Back_2_Home = new Button
        {
            Text = "Return to Hub",
            HorizontalOptions = LayoutOptions.Center
        };
       

       void On_Back_2_Home_Clicked(object sender, EventArgs e)
        {
            map_nv("912 Main St, Worcester, MA 01610");
        }


        public void Pins()
        {
            var pin1 = new Pin()
            {
                Position = new Position(42.251660, -71.823402),
                Label = "Clark University",
                Address = "950 Main St, Worcester, MA 01610",
            };

            var pin2 = new Pin()
            {
                Position = new Position(42.274215, -71.810691),
                Label = "Worcester Polytechnic Institute",
                Address = "100 Institute Rd, Worcester, MA 01610"
            };

            var pin3 = new Pin()
            {
                Position = new Position(42.269026, -71.811912),
                Label = "Becker",
                Address = "61 Sever St, Worcester, MA 01609"
            };

            var pin4 = new Pin()
            {
                Position = new Position(42.268066, -71.843809),
                Label = "Worcester State",
                Address = "486 Chandler St, Worcester, MA 01602"
            };

            var pin5 = new Pin()
            {
                Position = new Position(42.292355, -71.828869),
                Label = "Assumption College",
                Address = "500 Salisbury St, Worcester, MA 01609"
            };

            var pin6 = new Pin()
            {
                Position = new Position(42.239200, -71.808060),
                Label = "Holy Cross",
                Address = "1 College St, Worcester, MA 01610"
            };

            var pin7 = new Pin()
            {
                Position = new Position(42.314628, -71.793882),
                Label = "Quinsigamond Community College",
                Address = "670 W Boylston St, Worcester, MA 01606"
            };


            var pin8 = new Pin()
            {
                Position = new Position(42.327075, -71.917944),
                Label = "Anna Maria",
                Address = "50 Sunset Ln, Paxton, MA 01612"
            };

            var pin9 = new Pin()
            {
                Position = new Position(42.046523, -71.929542),
                Label = "Nichols",
                Address = "129 Center Rd, Dudley, MA 01571"
            };

            var pin10 = new Pin()
            {
                Position = new Position(42.264348, -71.800347),
                Label = "Massachusetts College of Pharmacy and Health Sciences: MCPHS",
                Address = "19 Foster St, Worcester, MA 01608"
            };


            var pin11 = new Pin()
            {
                Position = new Position(42.240033, -71.842274),
                Label = "Zorba's",
                Address = "97 Stafford St,Worcester,MA 01603"
            };

            var pin12 = new Pin()
            {
                Position = new Position(42.248483, -71.832013),
                Label = "Pho Dakao",
                Address = "593 Park Ave, Worcester, MA 01603"
            };

            var pin13 = new Pin()
            {
                Position = new Position(42.254608, -71.825480),
                Label = "Peppercorns",
                Address = "455 Park Ave, Worcester, MA 01610"
            };

            var pin14 = new Pin()
            {
                Position = new Position(42.246909, -71.834810),
                Label = "Applebee's",
                Address = "632 Park Ave, Worcester, MA 01603"
            };

            var pin15 = new Pin()
            {
                Position = new Position(42.113431, -72.103309),
                Label = "Baba Sushi",
                Address = "453 Main St, Fiskdale, MA 01518"
            };

           var pin16 = new Pin()
            {
                Position = new Position(42.243930, -71.836901),
                Label = "Moe's",
                Address = "3 Stafford St, Worcester, MA 01603"
            };

           var pin17 = new Pin()
            {
                Position = new Position(42.261703, -71.802738),
                Label = "Great Wall",
                Address = "521 Main St, Worcester, MA 01608"
            };

           var pin18 = new Pin()
            {
                Position = new Position(42.266889, -71.794324),
                Label = "Starbucks",
                Address = "11 E Central St, Worcester, MA 01605"
            };

           var pin19 = new Pin()
            {
                Position = new Position(42.247462, -71.826644),
                Label = "Dippin Donuts",
                Address = "1001 Main St,Worcester,MA 01603"
            };


           var pin20 = new Pin()
            {
                Position = new Position(42.251062, -71.821024),
                Label = "Acoutic Java",
                Address = "932 Main St # B, Worcester, MA 01610"
            };

           var pin21 = new Pin()
            {
                Position = new Position(42.262777, -71.826625),
                Label = "NU Café",
                Address = "335 Chandler St,Worcester,MA 01602"
            };

           var pin22 = new Pin()
            {
                Position = new Position(42.243581, -71.810172),
                Label = "Culpeppers",
                Address = "500 Cambridge St # 3, Worcester, MA 01610"
            };

           var pin23 = new Pin()
            {
                Position = new Position(42.260501, -71.821524),
                Label = "Boston Donuts",
                Address = "338 Park Ave, Worcester, MA 01610"
            };


           var pin24 = new Pin()
            {
                Position = new Position(42.270914, -71.814888),
                Label = "Price Chopper",
                Address = "221 Park Ave, Worcester, MA 01609"
            };

           var pin25 = new Pin()
            {
                Position = new Position(42.288593, -71.806487),
                Label = "Shaw's",
                Address = "14 W Boylston St, Worcester, MA 01605"
           };

           var pin26 = new Pin()
            {
                Position = new Position(42.247347, -71.808756),
                Label = "Price Rite",
                Address = "542 Southbridge St, Worcester, MA 01610"
            };
            map.Pins.Add(pin1);
            map.Pins.Add(pin2);
            map.Pins.Add(pin3);
            map.Pins.Add(pin4);
            map.Pins.Add(pin5);
            map.Pins.Add(pin6);
            map.Pins.Add(pin7);
            map.Pins.Add(pin8);
            map.Pins.Add(pin9);
            map.Pins.Add(pin10);
            map.Pins.Add(pin11);
            map.Pins.Add(pin12);
            map.Pins.Add(pin13);
            map.Pins.Add(pin14);
            map.Pins.Add(pin15);
            map.Pins.Add(pin16);
            map.Pins.Add(pin17);
            map.Pins.Add(pin18);
            map.Pins.Add(pin19);
            map.Pins.Add(pin20);
            map.Pins.Add(pin21);
            map.Pins.Add(pin22);
            map.Pins.Add(pin23);
            map.Pins.Add(pin24);
            map.Pins.Add(pin25);
            map.Pins.Add(pin26);
        } //We add all the pins for the locations that we determined to be hotpots manually in this method, which is then called in the OnAppearing method. The pins are added uses a skeleton code.
                               //Considering custom pins to differentiate between restaraunts and coffee shops etc.

        public async void current_loc()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 1;
            var temp = await locator.GetPositionAsync(timeoutMilliseconds: 10000);

            p = new Position(temp.Latitude, temp.Longitude);
            Console.WriteLine("THIS IS THE POSTION: (" + p + ")");
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(p.Latitude, p.Longitude), Distance.FromMiles(0.3)));

        }

        
        public void map_nv(string address)
        {
            var address_to_map = address;

            switch (Device.OS)
            {
                case TargetPlatform.iOS:
                    Device.OpenUri(
                      new Uri(string.Format("http://maps.apple.com/?q={0}", WebUtility.UrlEncode(address_to_map))));
                    break;
                case TargetPlatform.Android:
                    Device.OpenUri(
                      new Uri(string.Format("geo:0,0?q={0}", WebUtility.UrlEncode(address_to_map))));
                    break;
            }
        }
        
        protected override void OnAppearing()
        {
            base.OnAppearing();
            checkPerms();
            //current_loc();
            Pins();
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
