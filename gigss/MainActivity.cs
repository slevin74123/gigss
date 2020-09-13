using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Firebase.Events;
using Firebase.Database;
using Firebase;

namespace gigss
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class MainActivity : AppCompatActivity
    {
        FirebaseDatabase database;
        Button btntestConnection;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            btntestConnection = (Button)FindViewById(Resource.Id.butonTest);
            btntestConnection.Click += BtntestConnection_Click;
        }

        private void BtntestConnection_Click(object sender, System.EventArgs e)
        {
            Initializedatabase();
        }

        void Initializedatabase()
        {
            var app = FirebaseApp.InitializeApp(this);

            if (app == null)
            {
                var options = new FirebaseOptions.Builder()
                    .SetApplicationId("gigss-d72a2")
                    .SetApiKey("AIzaSyAaVh4GwlaLyPJsVj5scAEyPRVpn_bf7ww")
                    .SetDatabaseUrl("https://gigss-d72a2.firebaseio.com")
                    .SetStorageBucket("gigss-d72a2.appspot.com")
                    .Build();
                app = FirebaseApp.InitializeApp(this, options);
                database = FirebaseDatabase.GetInstance(app);
            }
            else
            {
                database = FirebaseDatabase.GetInstance(app);
            }
            DatabaseReference dbref = database.GetReference("UserSupporttt");
            dbref.SetValue("Ticket13");
            Toast.MakeText(this, "Completed", ToastLength.Short);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}