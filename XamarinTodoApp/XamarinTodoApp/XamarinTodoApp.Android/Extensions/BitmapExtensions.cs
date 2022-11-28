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
using System.IO;

using Android.Graphics;
using Android.Graphics.Drawables;

namespace XamarinTodoApp.Droid.Extensions
{
	public static class BitmapExtensions
	{
		/// <summary>
		/// Helper method to get the sample size of the image for resampling.
		/// </summary>
		public static int ToSampleSize(this byte[] bytes, int maxWidth, int maxHeight)
		{
			var sampleSize = 0;
			BitmapFactory.Options options = new BitmapFactory.Options();
			options.InJustDecodeBounds = true;
			BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length, options);
			sampleSize = (int)Math.Ceiling((double)Math.Max(options.OutWidth / maxWidth, options.OutHeight / maxHeight));
			return sampleSize;
		}

		/// <summary>
		/// Helper method to turn bmp image into byte array
		/// </summary>
		public static byte[] ToByteArray(this Bitmap bmp, int quality = 100, bool recycle = true)
		{
			byte[] bytes = null;
			if (bmp != null)
			{
				using (MemoryStream stream = new MemoryStream())
				{
					if (bmp.Compress(Bitmap.CompressFormat.Jpeg, quality, stream))
					{
						if (recycle)
							bmp.Recycle();

						bytes = stream.ToArray();
					}
				}
			}
			return bytes;
		}


		public static Stream GetStream(this Bitmap bmp, int quality, Bitmap.CompressFormat format)
		{
			using (var image = bmp)
			{
				var stream = new MemoryStream();
				image.Compress(format, quality, stream); // TODO: quality control
				return stream; // TODO: careful
			}
		}

		public static Bitmap CreateCircle(int width, int height)
		{
			Paint paint = new Paint();
			*//*paint.SetStyle(Paint.Style.Fill);
			paint.Color = Color.Blue;*//*

			Bitmap bitmap = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
			Canvas canvas = new Canvas(bitmap);
			float radius = Math.Min(width, height) * 0.45f;
			canvas.DrawCircle(width / 2, height / 2, radius, paint);

			return bitmap;
		}


		/// <summary>
		/// Helper method to resize the bitmap to a smaller image size
		/// </summary>
		public static Bitmap ResizeBitmap(this Bitmap input, int destWidth, int destHeight)
		{
			int srcWidth = input.Width,
				srcHeight = input.Height;
			bool needsResize = false;
			float p;
			if (srcWidth > destWidth || srcHeight > destHeight)
			{
				needsResize = true;
				if (srcWidth > srcHeight && srcWidth > destWidth)
				{
					p = (float)destWidth / (float)srcWidth;
					destHeight = (int)(srcHeight * p);
				}
				else
				{
					p = (float)destHeight / (float)srcHeight;
					destWidth = (int)(srcWidth * p);
				}
			}
			else
			{
				destWidth = srcWidth;
				destHeight = srcHeight;
			}
			if (needsResize)
			{
				return Bitmap.CreateScaledBitmap(input, destWidth, destHeight, true);
			}
			return input;
		}

		public static Bitmap LoadAndResizeBitmap(this string fileName, int width, int height)
		{
			// First we get the the dimensions of the file on disk
			BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
			BitmapFactory.DecodeFile(fileName, options);

			// Next we calculate the ratio that we need to resize the image by
			// in order to fit the requested dimensions.
			int outHeight = options.OutHeight;
			int outWidth = options.OutWidth;
			int inSampleSize = 1;

			if (outHeight > height || outWidth > width)
			{
				inSampleSize = outWidth > outHeight
					? outHeight / height
						: outWidth / width;
			}

			// Now we will load the image and have BitmapFactory resize it for us.
			options.InSampleSize = inSampleSize;
			options.InJustDecodeBounds = false;
			Bitmap resizedBitmap = BitmapFactory.DecodeFile(fileName, options);

			// Images are being saved in landscape, so rotate them back to portrait if they were taken in portrait
			Matrix mtx = new Matrix();
			Android.Media.ExifInterface exif = new Android.Media.ExifInterface(fileName);
			Android.Media.Orientation orientation = (Android.Media.Orientation)exif.GetAttributeInt(Android.Media.ExifInterface.TagOrientation, (int)Android.Media.Orientation.Undefined);

			switch (orientation)
			{
				case Android.Media.Orientation.Rotate90: // portrait
					mtx.PreRotate(90);
					resizedBitmap = Bitmap.CreateBitmap(resizedBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height, mtx, false);
					mtx.Dispose();
					mtx = null;
					break;
				case Android.Media.Orientation.FlipHorizontal: // landscape
					break;
				default:
					mtx.PreRotate(90);
					resizedBitmap = Bitmap.CreateBitmap(resizedBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height, mtx, false);
					mtx.Dispose();
					mtx = null;
					break;
			}



			return resizedBitmap;
		}

		public static Bitmap Copy(this Bitmap b) => b.Copy(b.GetConfig(), true);

		public static Drawable AsDrawable(this Bitmap b) => new BitmapDrawable(Application.Context.Resources, b);
	}
}*/