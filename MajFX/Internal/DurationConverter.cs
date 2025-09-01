using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MajFX.Internal
{
	public class DurationConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is int ms)
				return new Duration(TimeSpan.FromMilliseconds(ms));
			return new Duration(TimeSpan.Zero);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}