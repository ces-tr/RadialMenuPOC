using System;
using Android.App;
using Android.Views;
using Android.Widget;
using AndroidMenuTest.Interfaces;

namespace AndroidMenuTest.Components {

    public class TopBarComponent : ITopBarComponent {

        private TextView lblTitle;
        private ImageButton btnBack;

        private Activity context;

     
        public event Action OnBackButtonPressedEvent;

        public TopBarComponent(Activity context)
        {
            this.context = context;

        }

        public void Init()
        {
            InitTopBarElements();

        }

        private void InitTopBarElements()
        {
            lblTitle = context.FindViewById<TextView>(Resource.Id.navbar_lblTitle);
            btnBack = context.FindViewById<ImageButton>(Resource.Id.navbar_btnBack);

        }

        public void AddTopBarEvents()
        {
            btnBack.Click += BackButtonAction;

        }

        public void RemoveTopBarEvents()
        {
            btnBack.Click -= BackButtonAction;

        }

        private void BackButtonAction(object sender, EventArgs e)
        {
            OnBackButtonPressedEvent?.Invoke();
        }

        public void HideBackButton()
        {
            btnBack.Visibility = ViewStates.Gone;
        }

        public void HideTitle()
        {
            lblTitle.Visibility = ViewStates.Gone;
        }

        public void NavigateBack()
        {
            OnBackButtonPressedEvent?.Invoke();
        }

        public void SetTitle(string title)
        {
            lblTitle.Text = title;
        }
    }
}
