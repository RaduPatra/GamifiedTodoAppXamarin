/*using FFImageLoading.Forms;
using Xamarin.Forms;
using XamarinTodoApp.CustomControls;
using XamarinTodoApp.Droid.Renderers;
using Android.Graphics;
using XamarinTodoApp.Helpers;
using Xamarin.Forms.Platform.Android;
using System.Threading.Tasks;
using Android.Widget;
using System;
using FFImageLoading.Forms.Platform;
using XamarinTodoApp.Droid.Helpers;
using XamarinTodoApp.Droid.Extensions;


[assembly: ExportRenderer(typeof(FilterImage), typeof(FilterImageRenderer))]

namespace XamarinTodoApp.Droid.Renderers
{

    [Obsolete]
    public class FilterImageRenderer : CachedImageRenderer
	{
		//Bitmap originalBitmap;
		//Bitmap filteredImage;
		Bitmap filteredBitmap;
		FilterType currentFilterType = FilterType.NoFilter;
		private static IImageSourceHandler GetHandler(ImageSource source)
		{
			IImageSourceHandler returnValue = null;
			if (source is UriImageSource)
			{
				returnValue = new ImageLoaderSourceHandler();
			}
			else if (source is FileImageSource)
			{
				returnValue = new FileImageSourceHandler();
			}
			else if (source is StreamImageSource)
			{
				returnValue = new StreamImagesourceHandler();
			}
			return returnValue;
		}


		private async Task SetImageSourceAsync(ImageView target, CachedImage model)
		{
			if (target == null || model == null)
				return;
			var source = model.Source;

			using (var bitmap = await this.GetBitmapAsync(source))
			{
				if (bitmap != null)
				{
					target.SetImageBitmap(bitmap.ResizeBitmap(500, 500));

				}
			}
		}

		protected override void OnElementChanged(ElementChangedEventArgs<CachedImage> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
			{
			}

			if (e.NewElement != null)
			{
				var filterImage = Element as FilterImage;
				filterImage.SelectFilterCommand = new Command<FilterType>((fType) => ApplyFilter(fType), (b) => (filterImage.OriginalSource != null));
				filterImage.ApplyFilterCommand = new Command(async () =>
				{
					//Control.BuildDrawingCache(true);
					//Bitmap bmap = Control.GetDrawingCache(true);
					if (filteredBitmap == null)
						filteredBitmap = await GetBitmapAsync(filterImage.OriginalSource);

					byte[] imgBytes = filteredBitmap.ToByteArray(100, false);
					string name = "SAMPLE-" + System.DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";

					var file = new Java.IO.File(FileUtils.GetDirectoryForPictures("SAMPLE-TMP"), name);

					string filePath = file.Path;
					System.IO.File.WriteAllBytes(filePath, imgBytes);
					var fInfo = new System.IO.FileInfo(filePath);
					filterImage?.OnFilterApplied(this, new Tuple<string, byte[], long>(filePath, imgBytes, fInfo.Length));
				}, () => (filterImage.OriginalSource != null));
			}
		}

		//Filters
		async void ApplyFilter(FilterType filterType)
		{
			if (currentFilterType == filterType)
				return;

			var filterImage = Element as FilterImage;
			//originalBitmap = await GetBitmapAsync(filterImage.OriginalSource);
			using (Bitmap originalBitmap = await GetBitmapAsync(filterImage.OriginalSource))
			{
				var oldFilteredBitmap = filteredBitmap;
				Bitmap bmp = null;
				switch (filterType)
				{
					case FilterType.NoFilter:
						Element.Source = filterImage.OriginalSource;
						bmp = originalBitmap;
						break;
					case FilterType.BlackAndWhite:
						bmp = ApplyBlackAndWhite(originalBitmap);
						break;
					case FilterType.Hifi:
						bmp = ApplyHiFi(originalBitmap);
						break;
					case FilterType.Saturated:
						bmp = ApplySaturation(originalBitmap, 250);
						break;
					case FilterType.Vintage:
						bmp = ApplyVignette(originalBitmap);
						break;
				}

				filteredBitmap = filterType == FilterType.NoFilter ? null : bmp;
				currentFilterType = filterType;

				using (var resizedBitmap = bmp.ResizeBitmap(500, 500))
				{
					Control.SetImageBitmap(resizedBitmap);
				}

				oldFilteredBitmap?.Recycle();
			}
		}


		protected override async void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			var filterImage = Element as FilterImage;
			if (e.PropertyName == FilterImage.OriginalSourceProperty.PropertyName)
			{

				//originalBitmap = Control.;
				//await SetImageAsync(filterImage.OriginalSource, Control);

			}
			else if (e.PropertyName == FilterImage.SourceProperty.PropertyName)
			{
				await SetImageSourceAsync(this.Control, this.Element);

			}
		}


		private async Task<Bitmap> GetBitmapAsync(ImageSource source)
		{
			var handler = GetHandler(source);
			var returnValue = (Bitmap)null;

			if (handler != null)
				returnValue = await handler.LoadImageAsync(source, Forms.Context);

			return returnValue;
		}

		#region Filters

		Bitmap ApplyVignette(Bitmap src)
		{
			var bm = src.Copy();
			var width = bm.Width;
			var height = bm.Height;

			var radius = (float)(width / 1.2);
			int c = -0x1000000;
			var colors = new int[] { 0, 0x55000000, c };
			var positions = new float[] { 0.0f, 0.5f, 1.0f };

			var gradient = new RadialGradient(width / 2, height / 2, radius, colors, positions, Shader.TileMode.Clamp);

			//var gradient = new RadialGradient(width / 2, height / 2, radius, Android.Graphics.Color.Transparent, Android.Graphics.Color.Blue, Shader.TileMode.Clamp);

			var canvas = new Canvas(bm);
			canvas.DrawARGB(1, 0, 0, 0);

			var paint = new Paint();
			paint.AntiAlias = true;
			paint.Color = Android.Graphics.Color.Black;
			paint.SetShader(gradient);

			var rect = new Android.Graphics.Rect(0, 0, bm.Width, bm.Height);
			var rectf = new RectF(rect);

			canvas.DrawRect(rectf, paint);

			paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcIn));
			canvas.DrawBitmap(bm, rect, rect, paint);

			return bm;

			//Element.Source = ImageSource.FromStream(() => bm.GetStream(85, Bitmap.CompressFormat.Jpeg));
		}

		Bitmap ApplySaturation(Bitmap src, int value)
		{
			var bm = src.Copy();
			var f_value = (float)(value / 100.0);

			//var w = c.Width;
			//var h = c.Height;

			//var bm = Bitmap.CreateBitmap(w, h, Bitmap.Config.Argb8888);
			var canvasResult = new Canvas(bm);
			var paint = new Paint();
			var colorMatrix = new ColorMatrix();

			colorMatrix.SetSaturation(f_value);
			var filter = new ColorMatrixColorFilter(colorMatrix);
			paint.SetColorFilter(filter);
			canvasResult.DrawBitmap(bm, 0, 0, paint);

			return bm;
		}

		Bitmap ApplyBlackAndWhite(Bitmap src)
		{
			Bitmap bw = src.Copy();
			//Bitmap bw= Bitmap.CreateBitmap(c);
			//using (Bitmap bw = Bitmap.CreateBitmap(src))

			var canvas = new Canvas(bw);
			var ma = new ColorMatrix();
			ma.SetSaturation(0);

			var paint = new Paint();
			paint.SetColorFilter(new ColorMatrixColorFilter(ma));

			canvas.DrawBitmap(bw, 0, 0, paint);


			return bw;


		}

		Bitmap ApplyHiFi(Bitmap src)
		{
			var bm = src.Copy();

			*//*var w = c.Width;
			var h = c.Height;
			var bm = Bitmap.CreateBitmap(w, h, c.GetConfig());*//*

			float brightness = 0.25f;
			float contrast = 1.25f;


			var canvasResult = new Canvas(bm);
			var paint = new Paint();
			ColorMatrix colorMatrix = new ColorMatrix(new float[]
			{
				contrast, 0, 0, 0, brightness,
				0, contrast, 0, 0, brightness,
				0, 0, contrast, 0, brightness,
				0, 0, 0, 1, 0
			});

			var filter = new ColorMatrixColorFilter(colorMatrix);
			paint.SetColorFilter(filter);
			canvasResult.DrawBitmap(bm, 0, 0, paint);

			return bm;
		}

		#endregion

	}

	
}*/