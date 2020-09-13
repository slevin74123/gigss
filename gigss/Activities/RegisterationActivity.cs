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
using Firebase.Auth;
using Firebase.Database;
using Firebase;
using gigss.EventListeners;
using Java.Util;

namespace gigss.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/UberTheme", MainLauncher = false)]
    public class RegisterationActivity : AppCompatActivity
    {
        // variabile globale
        TextInputLayout fullNameText;
        TextInputLayout phoneText;
        TextInputLayout emailText;
        TextInputLayout passwordText;
        Button registerButton;
        CoordinatorLayout rootView;
        TextView ClickToLoginText;
        TaskCompletionListener TaskCompletionListener = new TaskCompletionListener();

        FirebaseAuth mAuth;
        FirebaseDatabase database;
        string fullname, phone, email, password;

        ISharedPreferences preferences = Application.Context.GetSharedPreferences("userinfo", FileCreationMode.Private);
        ISharedPreferencesEditor editor;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.register);

            Initializedatabase();
            mAuth = FirebaseAuth.Instance;
            ConnectControl();
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
                database = FirebaseDatabase.GetInstance(app);
            }
            else
            {
                database = FirebaseDatabase.GetInstance(app);
            }

        }
        void ConnectControl()
        {   //preluare id'uri din view de registration
            fullNameText = (TextInputLayout)FindViewById(Resource.Id.fullNameText);
            phoneText = (TextInputLayout)FindViewById(Resource.Id.phoneText);
            emailText = (TextInputLayout)FindViewById(Resource.Id.emailText);
            passwordText = (TextInputLayout)FindViewById(Resource.Id.passwordText);
            rootView = (CoordinatorLayout)FindViewById(Resource.Id.rootView);
            registerButton = (Button)FindViewById(Resource.Id.registerButton);
            ClickToLoginText = (TextView)FindViewById(Resource.Id.clickToLogin);

            //click handler pt buton login
            ClickToLoginText.Click += ClickToLoginText_Click;
            //executare registration
            registerButton.Click += RegisterButton_Click;
        }

        private void ClickToLoginText_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(login));
            Finish();
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            fullname = fullNameText.EditText.Text;
            phone = phoneText.EditText.Text;
            email = emailText.EditText.Text;
            password = passwordText.EditText.Text;

            if (fullname.Length < 3)
            {
                Snackbar.Make(rootView, "Introduceti un nume valid", Snackbar.LengthShort).Show();
                return;
            }
            else if (phone.Length < 9)
            {
                Snackbar.Make(rootView, "Introduceti un nr de telefon valid", Snackbar.LengthShort).Show();
                return;
            }
            else if (!email.Contains("@"))
            {
                Snackbar.Make(rootView, "Introduceti o adresa de email valida", Snackbar.LengthShort).Show();
                return;
            }
            else if (password.Length < 8)
            {
                Snackbar.Make(rootView, "Parola trebuie sa aibe o lungime de cel putin 8 caractere", Snackbar.LengthShort).Show();
                return;
            }

            RegisterUser(fullname, phone, email, password);

        }

        void RegisterUser(string name, string phone, string email, string password)
        {
            TaskCompletionListener.Success += TaskCompletionListener_Success;
            TaskCompletionListener.Failure += TaskCompletionListener_Failure;

            mAuth.CreateUserWithEmailAndPassword(email, password)
                .AddOnSuccessListener(this, TaskCompletionListener)
                .AddOnFailureListener(this, TaskCompletionListener);
        }

        private void TaskCompletionListener_Failure(object sender, EventArgs e)
        {
            Snackbar.Make(rootView, "Inregistrare user esuata", Snackbar.LengthShort).Show();
        }
        private void TaskCompletionListener_Success(object sender, EventArgs e)
        {
            Snackbar.Make(rootView, "User inregistrat cu success!", Snackbar.LengthShort).Show();
            //introducere user in bd folosind hashmap
            HashMap userMap = new HashMap();
            //setam valori si keys in hashmap
            userMap.Put("email", email);
            userMap.Put("phone", phone);
            userMap.Put("fullname", fullname);
            //creare referinta la bd pt a salva acest hashmap in bd
            DatabaseReference userReference = database.GetReference("users/" + mAuth.CurrentUser.Uid); // referinta catre tabelul users
            userReference.SetValue(userMap); //salvam hashmap in referinta bd
        }
        void SaveToSharedPreference()
        {   //salvare date de login pe fisier local; scrie in editor valorile
            editor = preferences.Edit();
            editor.PutString("email", email);
            editor.PutString("fullname", fullname);
            editor.PutString("phone", phone);

            //salveaza in editor valorile
            editor.Apply();
        }

        void RetriveData()
        { //acceseaza datele din fisierul creat cu SharedPreference folosind keys pt fiecare valoare
            string email = preferences.GetString("email", "");
        }


    }
}