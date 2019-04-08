using System;
using Android.OS;
using Android.Support.V7.App;
using AndroidMenuTest.Components;
using AndroidMenuTest.Interfaces;

namespace AndroidMenuTest.Views {

    public class BaseActivity : AppCompatActivity {

        protected ITopBarComponent topBarComponent { get; set; }

        public BaseActivity()
        {
        }

       
        protected virtual void InitComponents() 
        {
            InitTopBar();
        }

        private void InitTopBar() {

            topBarComponent = new TopBarComponent(this);
            topBarComponent.Init();
        }
    }
}
