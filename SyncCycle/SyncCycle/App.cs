﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using BluetoothLE.Core;
using BluetoothLE.Core.Events;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;


namespace SyncCycle
{
    /// <summary>
    ///// ******* OVERVIEW *******
    /////
    ///// NuGet library         -   Purpose
    ///
    ///// Xamarin.Forms.Maps    -   Cross-platform maps using native APIs
    ///// Xamarin.BluetoothLE   -   Bluetooth LE Connectivity and communications.
    ///// OxyPlot               -   Data Visualization - Charts and Plots
    /// 
    /// 
    /// </summary>

    public class App : Application
	{
        private DataPage defaultPage;
        private static readonly IAdapter _bluetoothAdapter;
        public static IAdapter BluetoothAdapter { get { return _bluetoothAdapter; } }

        static App()
        {
            _bluetoothAdapter = DependencyService.Get<IAdapter>();

            _bluetoothAdapter.ScanTimeout = TimeSpan.FromSeconds(10);
            _bluetoothAdapter.ConnectionTimeout = TimeSpan.FromSeconds(10);
        }

        public App ()
		{

            defaultPage = new DataPage();

            //delegate instance method to the event
            _bluetoothAdapter.DeviceDiscovered += defaultPage.b.DeviceDiscovered;
            
            // The root page of your application
            MainPage = defaultPage;

        }

        async void checkPerms(ContentPage mainpage)
        {
            try
            {
                var stat = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if(stat != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        await mainpage.DisplayAlert("Need location", "Gunna need that location", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Location });
                    stat = results[Permission.Location];
                }
                if (stat == PermissionStatus.Granted)
                {
                    await mainpage.DisplayAlert("Location Allowed", "Hopefully this means we can continue!", "OK");
                }
                else if (stat != PermissionStatus.Unknown)
                {
                    await mainpage.DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("Exception " + ex.Message + " encountered when checking for permissions.");
            }
        }

        protected override void OnStart ()
		{
            // Handle when your app starts
        }

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
            foreach(BikeData dataCell in defaultPage.Data)
            {
                dataCell.tmr.Dispose();
            }
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
