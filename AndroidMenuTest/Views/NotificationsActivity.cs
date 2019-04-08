
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AndroidMenuTest.Views {
    [Activity(Theme = "@style/Theme.MainTheme", Label = "NotificationsActivity")]
    public class NotificationsActivity : BaseActivity {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.NotificationsLayout);
            InitComponents();
        }

        protected override void InitComponents()
        {
            base.InitComponents();
            topBarComponent.SetTitle("My Notifications");

        }


        protected override void OnStart()
        {
            base.OnStart();
            AddEventHandlers();
        }


        protected override void OnStop()
        {
            RemoveEventHandlers();
            base.OnStop();
        }

        private void AddEventHandlers()
        {
            topBarComponent.AddTopBarEvents();
            topBarComponent.OnBackButtonPressedEvent += TopBarComponent_OnBackButtonPressedEvent;
        }

        private void RemoveEventHandlers()
        {

            topBarComponent.RemoveTopBarEvents();
            topBarComponent.OnBackButtonPressedEvent -= TopBarComponent_OnBackButtonPressedEvent;
        }

        public override void OnBackPressed()
        {
            topBarComponent.NavigateBack();
        }

        void TopBarComponent_OnBackButtonPressedEvent()
        {
            Finish();
        }
    }
}
