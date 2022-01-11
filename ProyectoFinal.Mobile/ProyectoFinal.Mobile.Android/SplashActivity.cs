using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProyectoFinal.Mobile.Droid
{
    [Activity(
        Label = "SmartSell",
        Theme = "@style/SplashTheme",
        MainLauncher = true,
        NoHistory = true,
        ConfigurationChanges = ConfigChanges.ScreenSize
    )]
    public class SplashActivity : Activity
    {
        /* https://elcamino.dev/xamarin-forms-splash-screen/ */
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            // Create your application here
        }

        public override void OnBackPressed() { }
    }
}