using System;
using System.Windows.Input;
using FFImageLoading.Forms;
using Xamarin.Forms;
using XamarinTodoApp.Helpers;


namespace XamarinTodoApp.CustomControls
{
	public class FilterImage : CachedImage
	{
		/// <summary>
		/// Backing field for the Original Source Image property.
		/// </summary>
		public static readonly BindableProperty OriginalSourceProperty = BindableProperty.Create(nameof(OriginalSource), typeof(ImageSource), typeof(FilterImage), null,
			BindingMode.TwoWay,
			null,
			(bindable, oldvalue, newvalue) => ((VisualElement)bindable).ToString());


		/// <summary>
		/// Gets or sets the ImageSource to use with the control.
		/// </summary>
		/// <value>
		/// The Source property gets/sets the value of the backing field, SourceProperty.
		/// </value>
		[TypeConverter(typeof(FFImageLoading.Forms.ImageSourceConverter))]
		public ImageSource OriginalSource
		{
			get { return (ImageSource)GetValue(OriginalSourceProperty); }
			set { SetValue(OriginalSourceProperty, value); }
		}
		/// <summary>
		/// Command for trigger
		/// </summary>
		public static readonly BindableProperty SelectFilterCommandProperty = BindableProperty.Create(nameof(SelectFilterCommand), typeof(Command<FilterType>), typeof(FilterImage), default(Command<FilterType>), BindingMode.TwoWay, null, SelectFilterCommandPropertyChanged);

		static void SelectFilterCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var source = bindable as FilterImage;
			if (source == null)
			{
				return;
			}
			source.SelectFilterCommandChanged();
		}

		private void SelectFilterCommandChanged()
		{
			OnPropertyChanged("SelectFilterCommand");
		}

		public Command<FilterType> SelectFilterCommand
		{
			get
			{
				return (Command<FilterType>)GetValue(SelectFilterCommandProperty);
			}
			set
			{
				SetValue(SelectFilterCommandProperty, value);
			}
		}

		public static readonly BindableProperty ApplyFilterCommandProperty = BindableProperty.Create(nameof(ApplyFilterCommand), typeof(ICommand), typeof(FilterImage), default(ICommand), BindingMode.TwoWay, null, ApplyFilterCommandPropertyChanged);

		static void ApplyFilterCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var source = bindable as FilterImage;
			if (source == null)
			{
				return;
			}
			source.ApplyFilterCommandChanged();
		}

		private void ApplyFilterCommandChanged()
		{
			OnPropertyChanged("ApplyFilterCommand");
		}

		public ICommand ApplyFilterCommand
		{
			get
			{
				return (ICommand)GetValue(ApplyFilterCommandProperty);
			}
			set
			{
				SetValue(ApplyFilterCommandProperty, value);
			}
		}

		public event EventHandler<Tuple<string, byte[], long>> FilterApplied = delegate { };

		public void OnFilterApplied(object sender, Tuple<string, byte[], long> imgParams)
		{
			var handler = FilterApplied;
			if (handler != null)
			{
				handler(this, imgParams);
			}

			if (FilterAppliedCommand != null && FilterAppliedCommand.CanExecute(imgParams))
			{
				FilterAppliedCommand.Execute(imgParams);
			}
		}

		public static readonly BindableProperty FilterAppliedCommandProperty = BindableProperty.Create(nameof(FilterAppliedCommand), typeof(Command<Tuple<string, byte[], long>>), typeof(FilterImage), default(Command<Tuple<string, byte[], long>>), BindingMode.OneWay, null, FilterAppliedCommandPropertyChanged);

		static void FilterAppliedCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var source = bindable as FilterImage;
			if (source == null)
			{
				return;
			}
			source.FilterAppliedCommandChanged();
		}

		private void FilterAppliedCommandChanged()
		{
			OnPropertyChanged("FilterAppliedCommand");
		}

		public Command<Tuple<string, byte[], long>> FilterAppliedCommand
		{
			get
			{
				return (Command<Tuple<string, byte[], long>>)GetValue(FilterAppliedCommandProperty);
			}
			set
			{
				SetValue(FilterAppliedCommandProperty, value);
			}
		}
	}
}
