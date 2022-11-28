//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Xamarin.Forms;
//using Xamarin.Forms.Platform.Android;
//using XamarinTodoApp.CustomControls;
//using XamarinTodoApp.Droid.Renderers;

//[assembly: ExportRenderer(typeof(MyEntry), typeof(CustomEntryRendererAndroid))]

//namespace XamarinTodoApp.Droid.Renderers
//{

//    public class CustomEntryRendererAndroid : EntryRenderer
//    {
//        public CustomEntryRendererAndroid(Context context) : base(context)
//        {
//        }

//        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
//        {
//            //base.OnElementChanged(e);

//            //IntPtr IntPtrtextViewClass = JNIEnv.FindClass(typeof(TextView));
//            //IntPtr mCursorDrawableResProperty = JNIEnv.GetFieldID(IntPtrtextViewClass, "mCursorDrawableRes", "I");

//            //// my_cursor is the xml file name which we defined above
//            //JNIEnv.SetField(Control.Handle, mCursorDrawableResProperty, Resource.Drawable.my_cursor);
//        }
//    }
//}