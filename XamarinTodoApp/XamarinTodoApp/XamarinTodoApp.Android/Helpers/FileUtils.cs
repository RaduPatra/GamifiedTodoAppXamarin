/*using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XamarinTodoApp.Droid.Helpers
{
	public static class FileUtils
	{
		public static Java.IO.File GetDirectoryForPictures(string collectionName)
		{
			var fileDir = new Java.IO.File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), collectionName);
			if (!fileDir.Exists())
			{
				fileDir.Mkdirs();
			}

			return fileDir;

		}

		public static Java.IO.File GetDirectoryForVideos(string collectionName)
		{
			var fileDir = new Java.IO.File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryMovies), collectionName);
			if (!fileDir.Exists())
			{
				fileDir.Mkdirs();
			}

			return fileDir;
		}
	}
}*/