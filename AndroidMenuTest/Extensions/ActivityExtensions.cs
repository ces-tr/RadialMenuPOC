using System;
using Android.Content;
using Android.Support.V7.App;

namespace AndroidMenuTest.Extensions {

    public static class ActivityExtensions {

        public static void NavigateToAndClearStack(this AppCompatActivity activity, Type type)
        {
            var intent = new Intent(activity, type);
            intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);

            activity.StartActivity(intent);

            activity.Finish();
        }

        public static void NavigateTo(this AppCompatActivity activity, Type type)
        {
            var intent = new Intent(activity, type);
            intent.SetFlags(ActivityFlags.NewTask);

            activity.StartActivity(intent);

           
        }

    }
}
