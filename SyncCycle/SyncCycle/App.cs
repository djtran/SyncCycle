using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using BluetoothLE.Core;
using BluetoothLE.Core.Events;
using System.Collections;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Plugin.Geolocator;

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
        private static readonly IAdapter _bluetoothAdapter;
        public static IAdapter BluetoothAdapter { get { return _bluetoothAdapter; } }

        internal RideListPage RideListPage
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        internal SettingsPage SettingsPage
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        internal NavPage NavPage
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        static App()
        {
            _bluetoothAdapter = DependencyService.Get<IAdapter>();

            _bluetoothAdapter.ScanTimeout = TimeSpan.FromSeconds(10);
            _bluetoothAdapter.ConnectionTimeout = TimeSpan.FromSeconds(10);
        }

        public App ()
		{
            ArrayList pages = new ArrayList();

            var rideListContainer = new NavigationPage(new RideListPage());
            rideListContainer.Title = "Rides";

            pages.Add(new NavPage());
            pages.Add(rideListContainer);
            pages.Add(new SettingsPage());
            
            // The root page of your application
            var appContainer = new TabbedPage();
            appContainer.BarTextColor=(Color.Purple);

            appContainer.Children.Add(new NavPage());
            appContainer.Children.Add(rideListContainer);
            appContainer.Children.Add(new SettingsPage());

            MainPage = appContainer;
        }

       

        protected override void OnStart ()
		{
            // Handle when your app starts
        }

		protected override void OnSleep ()
		{
			// Handle when your app sleeps

		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
