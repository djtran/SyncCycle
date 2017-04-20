using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using System.Collections;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Plugin.Geolocator;
using Plugin.BLE;

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
        private static BTHandler _bluetoothHandler = new BTHandler();

        public static BTHandler BluetoothHandler { get { return _bluetoothHandler; } }

        static App()
        {
            _bluetoothHandler = new BTHandler();
        }

        public App ()
        {
            BluetoothHandler.init();
            var rideListContainer = new NavigationPage(new RideListPage());
            rideListContainer.Title = "Rides";

            var settingspage = new SettingsPage();
            _bluetoothHandler.addPage(settingspage);
            
            // The root page of your application
            var appContainer = new TabbedPage();

            appContainer.Children.Add(new NavPage());
            appContainer.Children.Add(rideListContainer);
            appContainer.Children.Add(settingspage);

            MainPage = appContainer;
        }

       

        protected override void OnStart ()
		{
            // Handle when your app starts
        }

		protected override void OnSleep ()
		{
            // Handle when your app sleeps
            //_bluetoothHandler.toggleTimer(false);

            if(BluetoothHandler.connected != null)
            {
                BluetoothHandler.adapter.DisconnectDeviceAsync(BluetoothHandler.connected);
            }

		}

		protected override void OnResume ()
		{
            // Handle when your app resumes
            if(BluetoothHandler.ble.IsOn)
            {
                BluetoothHandler.startSearch();
            }

        }
	}
}
