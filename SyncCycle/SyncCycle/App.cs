using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace SyncCycle
{
    /// <summary>
    ///// ******* OVERVIEW *******
    /////
    ///// NuGet library         -   Purpose
    ///
    ///// Xamarin.Forms.Maps    -   Cross-platform maps using native APIs
    ///// Acr.Ble               -   Bluetooth LE Connectivity and communications.
    ///// OxyPlot               -   Data Visualization - Charts and Plots
    /// 
    /// App.cs      -       Purely for visuals. No logic should be done here.
    /// 
    /// </summary>




    //SCREENS AND VISUAL LAYOUT
    public class App : Application
	{
		public App ()
		{
            List<BikeData> dummy = new List<BikeData>();
            dummy.Add(new BikeData());
            dummy.Add(new BikeData());
            dummy.Add(new BikeData());
            // The root page of your application
            MainPage = new DataPage(dummy);
            
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
