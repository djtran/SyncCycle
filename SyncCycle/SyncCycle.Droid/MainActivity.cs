﻿using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace SyncCycle.Droid
{
	[Activity (Label = "SyncCycle", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{

        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            Xamarin.Forms.DependencyService.Register<BluetoothLE.Core.IAdapter, BluetoothLE.Droid.Adapter>();
            global::Xamarin.Forms.Forms.Init (this, bundle);
            LoadApplication (new SyncCycle.App ());
		}
	}
}

