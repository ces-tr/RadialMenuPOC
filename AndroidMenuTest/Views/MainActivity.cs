using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views.Animations;
using Android.Content;
using Android.Views;
using Vapolia.Lib.Ui;
using System;
using System.Timers;
using Android.Support.Design.Widget;
using Android.Animation;
using System.Collections.Generic;
using System.Linq;
using AndroidMenuTest.Interfaces;
using AndroidMenuTest.Components;
using AndroidMenuTest.Extensions;
using AndroidMenuTest.Utils;
using System.Threading.Tasks;
using System.Threading;

namespace AndroidMenuTest.Views {
    [Activity(Theme = "@style/Theme.MainTheme", Label = "AndroidMenuTest", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : BaseActivity {


        RealtimeBlurView blurView;
        //private Timer timer;
        private FloatingActionButton btnView1, btnView2, btnView3, btnView4, btnProfile;
        private TextView txttime;
        private ImageView imageViewdog, imgMenuBack;
        private ValueAnimator animator;
        private Button btnShowMenu;
        private int RETRY_ANIM_TIME_INTERVAL_MS = 100;
        private int RETRY_SYNC_TIME_INTERVAL_MS = 1000;


        //private ITopBarComponent topBarComponent { get; set; }

        private bool MenuIsVisible { get; set; }
        public CancellationTokenSource AnimProcessTimerTaskCancellationToken { get; private set; }
        public CancellationTokenSource SyncTimeProcessTimerTaskCancellationToken { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            SetContentView(Resource.Layout.MainLayout);


            InitElements();
            InitComponents();
            InitMenuAnimator();
        }

        private void InitMenuAnimator()
        {
            animator = ValueAnimator.OfFloat(0, 1); // values from 0 to 1
            animator.SetDuration(1500); // 5 seconds duration from 0 to 1
            animator.AddUpdateListener(new AnimationListener(btnView1));
            animator.AddUpdateListener(new AnimationListener(btnView2));
            animator.AddUpdateListener(new AnimationListener(btnView3));
            animator.AddUpdateListener(new AnimationListener(btnView4));
        }

        private void InitElements()
        {
            btnShowMenu = FindViewById<Button>(Resource.Id.btnMenu);
            blurView = FindViewById<RealtimeBlurView>(Resource.Id.blurView);
            btnView1 = FindViewById<FloatingActionButton>(Resource.Id.btnView1);
            btnView2 = FindViewById<FloatingActionButton>(Resource.Id.btnView2);
            btnView3 = FindViewById<FloatingActionButton>(Resource.Id.btnView3);
            btnView4 = FindViewById<FloatingActionButton>(Resource.Id.btnView4);

            btnProfile = FindViewById<FloatingActionButton>(Resource.Id.btnProfile);

            imageViewdog = FindViewById<ImageView>(Resource.Id.imageViewdog);
            imgMenuBack = FindViewById<ImageView>(Resource.Id.imgMenuBack);

            txttime = FindViewById<TextView>(Resource.Id.txttime);

        }

       protected override void InitComponents()
        {
            base.InitComponents();
           
            topBarComponent.HideBackButton();
            topBarComponent.SetTitle("Home");
        }

        protected override void OnStart()
        {
            base.OnStart();
            AddEventHandlers();
            InitSetTimeTimerTask();
        }

        protected override void OnResume()
        {
            base.OnResume();
            //InitSetTimeTimerTask();
        }

        protected override void OnStop()
        {
            RemoveEventHandlers();
            SyncTimeProcessTimerTaskCancellationToken?.Cancel();
            base.OnStop();
        }

        private void AddEventHandlers()
        {
            blurView.OnViewTouchedEvent += BlurView_OnViewTouchedEvent;
            btnShowMenu.Click  += BtnShowMenu_Click;
            btnView1.Click += BtnView1_Click;
            btnView2.Click += BtnView2_Click;
            btnView3.Click += BtnView3_Click;
            btnView4.Click += BtnView4_Click;
            btnProfile.Click += BtnProfile_Click;

            topBarComponent.AddTopBarEvents();

            imageViewdog.Click += ImageViewdog_Click;
        }

        private void RemoveEventHandlers()
        {
            blurView.OnViewTouchedEvent -= BlurView_OnViewTouchedEvent;
            btnShowMenu.Click -= BtnShowMenu_Click;
            btnView1.Click -= BtnView1_Click;
            btnView2.Click -= BtnView2_Click;
            btnView3.Click -= BtnView3_Click;

            btnView4.Click -= BtnView4_Click;

            btnProfile.Click -= BtnProfile_Click;
            topBarComponent.RemoveTopBarEvents();

            imageViewdog.Click -= ImageViewdog_Click;
        }

        void BtnShowMenu_Click(object sender, EventArgs e)
        {
            BlurOutMainView();
            ShowMenu();

        }

        private void SetMenuItemsProperties()
        {
            btnView1.Visibility = MenuIsVisible ? ViewStates.Visible : ViewStates.Gone;
            btnView2.Visibility = MenuIsVisible ? ViewStates.Visible : ViewStates.Gone;
            btnView3.Visibility = MenuIsVisible ? ViewStates.Visible : ViewStates.Gone;
            btnView4.Visibility = MenuIsVisible ? ViewStates.Visible : ViewStates.Gone;
            btnProfile.Visibility = MenuIsVisible ? ViewStates.Visible : ViewStates.Gone;
            imgMenuBack.Visibility = MenuIsVisible ? ViewStates.Visible : ViewStates.Gone;

            if (MenuIsVisible) {
                AnimateButtons();
            }
            else {
                ResetPosition();
            }

        }

        private void ResetPosition()
        {
            btnView1.TranslationX = btnView1.TranslationY = 0;
            btnView2.TranslationX = btnView1.TranslationY = 0;
            btnView3.TranslationX = btnView1.TranslationY = 0;
            btnView4.TranslationX = btnView1.TranslationY = 0;

        }

        void ShowMenu() 
        {

            BlurOutMainView();
            MenuIsVisible = true;
            SetMenuItemsProperties();
        }

        void HideMenu() 
        {
            MenuIsVisible = false;
            SetMenuItemsProperties();
            FocusMainView();

        }

        private void FocusMainView()
        {
            blurView.SetBlurRadius(0);
            blurView.Visibility = ViewStates.Gone;

        }

        private void AnimateButtons()
        {
            animator.Start();
        }

        void BlurOutMainView() {

            blurView.Visibility = ViewStates.Visible;
            blurView.SetDownsampleFactor(10);
            InitBlurAnimationTimerTask();

        }

        private void InitBlurAnimationTimerTask()
        {
            AnimProcessTimerTaskCancellationToken?.Cancel();
            AnimProcessTimerTaskCancellationToken = new CancellationTokenSource();

            Task AnimTimerTask = TimerTaskFactory.Start(action: PerformBlurAnimation,
                                                       intervalInMilliseconds: RETRY_ANIM_TIME_INTERVAL_MS,
                                                       delayInMilliseconds: 0,
                                                       cancelToken: AnimProcessTimerTaskCancellationToken.Token,
                                                       maxIterations: Int32.MaxValue);
        }

        private void InitSetTimeTimerTask() 
        {
            SyncTimeProcessTimerTaskCancellationToken?.Cancel();
            SyncTimeProcessTimerTaskCancellationToken = new CancellationTokenSource();

            Task SyncTimerTask = TimerTaskFactory.Start(action: SetTime,
                                                       intervalInMilliseconds: RETRY_SYNC_TIME_INTERVAL_MS,
                                                       delayInMilliseconds: 0,
                                                       cancelToken: SyncTimeProcessTimerTaskCancellationToken.Token,
                                                       maxIterations: Int32.MaxValue);

        }

        private void SetTime()
        {
            RunOnUiThread(() => {
                DateTime currentTime = DateTime.Now;
                txttime.Text = currentTime.ToString("hh:mm:ss tt");
            });
        }

        private void PerformBlurAnimation()
        {
            RunOnUiThread(() => {

                float blurradius = blurView.GetBlurRadius();
                if (blurradius <= 15) {
                    blurView.SetBlurRadius(blurradius += 0.5f);
                }


                if (blurradius >= 15) {
                    AnimProcessTimerTaskCancellationToken?.Cancel();
                }

            });
        }

       

        void BlurView_OnViewTouchedEvent()
        {
            HideMenu();
        }

        #region Menu Actions
        void BtnView1_Click(object sender, EventArgs e)
        {
            this.NavigateTo(typeof(MyListActivity));
        }

        void BtnView2_Click(object sender, EventArgs e)
        {
            this.NavigateTo(typeof(NotificationsActivity));
        }


        void BtnView3_Click(object sender, EventArgs e)
        {
            this.NavigateTo(typeof(SettingsActivity));
        }


        void BtnView4_Click(object sender, EventArgs e)
        {

            this.NavigateTo(typeof(ShareActivity));
        }

        void BtnProfile_Click(object sender, EventArgs e)
        {
            this.NavigateTo(typeof(ProfileActivity));
        }


        #endregion

        void ImageViewdog_Click(object sender, EventArgs e)
        {
            Finish();
        }

    }


    public class AnimationListener : Java.Lang.Object, ValueAnimator.IAnimatorUpdateListener {

        View View;

        Dictionary<string, Action<float>> MenuItemPositionMappings;
    

        public AnimationListener(View view) {

            View = view;

            MenuItemPositionMappings = new Dictionary<string, Action<float>>() {

            { "topright" , AnimateTopRight },
            { "topleft" , AnimateTopLeft  },
            { "bottomleft" , AnimateBottomLeft },
            { "bottomright" , AnimateBottomRight  }

        };
        }

       public void OnAnimationUpdate(ValueAnimator animation)
       {
            float value = ((float)(animation.AnimatedValue));
            // Set translation of your view here. Position can be calculated
            // out of value. This code should move the view in a half circle.

            string tag = View.Tag.ToString();
            var itemViewFunc= MenuItemPositionMappings.FirstOrDefault(x=> x.Key.Equals(tag)).Value;
            itemViewFunc?.Invoke(value);
            //Console.WriteLine();
            Console.WriteLine("TX: "+View.TranslationX + " TY:  " + View.TranslationY);
        }

        void AnimateTopRight(float value)
        {
            View.TranslationX = (1) * (float)(250 * Math.Sin(value * (Math.PI / 2)));
            View.TranslationY = (-1) * (float)(250 * Math.Sin(value * Math.PI / 4));
        }

         void AnimateTopLeft(float value)
        {
            View.TranslationX = (-1) * (float)(250 * Math.Sin(value * (Math.PI / 2)));
            View.TranslationY = (-1) * (float)(250 * Math.Sin(value * Math.PI / 4));
        }

         void AnimateBottomRight(float value)
        {
            View.TranslationX = (1) * (float)(250 * Math.Sin(value * (Math.PI / 2)));
            View.TranslationY =  (float)(300 * Math.Sin(value * Math.PI / 4));
        }

         void AnimateBottomLeft(float value)
        {
            View.TranslationX = (-1) * (float)(250 * Math.Sin(value * (Math.PI / 2)));
            View.TranslationY =   (float)(300 * Math.Sin(value * Math.PI / 4));
        }

    }


}

