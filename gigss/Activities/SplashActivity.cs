
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using gigss;
using Firebase.Auth;
using gigss.Activities;

namespace Uber_Rider.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            Thread.Sleep(4000);
        }

        protected override void OnResume()
        {
            base.OnResume();
            StartActivity(typeof(login));
        }
    }
}
