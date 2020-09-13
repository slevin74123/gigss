using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using gigss.EventListeners;
using Firebase;
using Firebase.Auth;
using Xamarin.Essentials;

namespace gigss.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/UberTheme", MainLauncher = false)]
    public class login : AppCompatActivity
    {
        FirebaseAuth mauth;
        TextInputLayout emailText;
        TextInputLayout passwordText;
        Button loginButton;
        CoordinatorLayout rootview;
        TextView ClickToRegisterText;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.login);
            //preluare id'uri din layout login.axml
            rootview = (CoordinatorLayout)FindViewById(Resource.Id.rootView);
            emailText = (TextInputLayout)FindViewById(Resource.Id.emailText);
            passwordText = (TextInputLayout)FindViewById(Resource.Id.passwordText);
            loginButton = (Button)FindViewById(Resource.Id.loginButton);
            ClickToRegisterText = (TextView)FindViewById(Resource.Id.clickToRegister);

            //event handler pt buton register
            ClickToRegisterText.Click += ClickToRegisterText_Click;
            //preluare rezultat din butonul login
            loginButton.Click += LoginButton_Click;
            Initializedatabase();
        }

        private void ClickToRegisterText_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(RegisterationActivity));
        }

        private void LoginButton_Click(object sender, EventArgs e)
        { //preluare email si password din login view
            string email, password;
            email = emailText.EditText.Text;
            password = passwordText.EditText.Text;
            //validare campuri email si password
            if(!email.Contains("@"))
            {
                Snackbar.Make(rootview, "Email trebuie sa contina caracterul @", Snackbar.LengthShort).Show();
                return;
            }
            else if(password.Length < 8)
            {
                Snackbar.Make(rootview, "Parola trebuie sa contina cel putin 8 caractere", Snackbar.LengthShort).Show();
                return;
            }
            //instantiere clasa taskcompletionlistener
            TaskCompletionListener taskCompletionListener = new TaskCompletionListener();
            taskCompletionListener.Success += TaskCompletionListener_Success;
            taskCompletionListener.Failure += TaskCompletionListener_Failure;
            //executare login
            mauth.SignInWithEmailAndPassword(email, password).AddOnSuccessListener(taskCompletionListener).AddOnFailureListener(taskCompletionListener);

        }
        private void TaskCompletionListener_Failure(object sender, EventArgs e)
        {
            Snackbar.Make(rootview, "Logare Nereusita", Snackbar.LengthShort).Show();
        }
        private void TaskCompletionListener_Success(object sender, EventArgs e)
        {
            StartActivity(typeof(MainActivity));
        }
        void Initializedatabase()
        {
            var app = FirebaseApp.InitializeApp(this);

            if (app == null)
            {
                var options = new FirebaseOptions.Builder()
                    .SetApplicationId("test3-fb0e9")
                    .SetApiKey("AIzaSyCZ1rD8kCAgBiX7nkVSCmzAbLdPQIMMxPw")
                    .SetDatabaseUrl("https://test3-fb0e9.firebaseio.com")
                    .SetStorageBucket("test3-fb0e9.appspot.com")
                    .Build();
                app = FirebaseApp.InitializeApp(this, options);
                mauth = FirebaseAuth.Instance;
            }
            else
            {
                mauth = FirebaseAuth.Instance;
            }

        }
    }
}