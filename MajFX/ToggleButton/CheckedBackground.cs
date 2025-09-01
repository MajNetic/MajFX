using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MajFX
{
	public static partial class ToggleButtonHelper
	{
		#region CheckedBackground
		public static readonly DependencyProperty CheckedBackgroundProperty =
			DependencyProperty.RegisterAttached(
				"CheckedBackground",
				typeof(Brush),
				typeof(ToggleButtonHelper),
				new PropertyMetadata(null, OnCheckedBackgroundChanged));

		public static void SetCheckedBackground(DependencyObject element, Brush value)
			=> element.SetValue(CheckedBackgroundProperty, value);

		public static Brush GetCheckedBackground(DependencyObject element)
			=> (Brush)element.GetValue(CheckedBackgroundProperty);
		#endregion

		#region CheckedBackgroundDuration
		public static readonly DependencyProperty CheckedBackgroundDurationProperty =
			DependencyProperty.RegisterAttached(
				"CheckedBackgroundDuration",
				typeof(int),
				typeof(ToggleButtonHelper),
				new PropertyMetadata(0));

		public static void SetCheckedBackgroundDuration(DependencyObject element, int value)
			=> element.SetValue(CheckedBackgroundDurationProperty, value);

		public static int GetCheckedBackgroundDuration(DependencyObject element)
			=> (int)element.GetValue(CheckedBackgroundDurationProperty);
		#endregion

		#region Events
		private static void OnCheckedBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is not ToggleButton toggle) return;

			toggle.Checked -= Toggle_Checked;
			toggle.Unchecked -= Toggle_Unchecked;

			if (e.NewValue != null)
			{
				if (GetOriginalBackground(toggle) == null)
				{
					if (toggle.Background is SolidColorBrush scb)
						SetOriginalBackground(toggle, new SolidColorBrush(scb.Color));
					else
						SetOriginalBackground(toggle, toggle.Background ?? new SolidColorBrush(Colors.Transparent));
				}

				toggle.Checked += Toggle_Checked;
				toggle.Unchecked += Toggle_Unchecked;
			}
		}

		private static void Toggle_Checked(object sender, RoutedEventArgs e)
		{
			if (sender is not ToggleButton toggle) return;

			Brush targetBrush = GetCheckedBackground(toggle);
			if (targetBrush == null) return;

			int durationMs = GetCheckedBackgroundDuration(toggle);
			TimeSpan duration = TimeSpan.FromMilliseconds(durationMs);

			if (targetBrush is SolidColorBrush targetSolid)
			{
				var currentBrush = (toggle.Background as SolidColorBrush)?.Clone() ?? new SolidColorBrush(Colors.Transparent);
				toggle.Background = currentBrush;

				currentBrush.BeginAnimation(
					SolidColorBrush.ColorProperty,
					new ColorAnimation(targetSolid.Color, duration));
			}
			else
			{
				toggle.Background = targetBrush.CloneCurrentValue();
			}
		}

		private static void Toggle_Unchecked(object sender, RoutedEventArgs e)
		{
			if (sender is not ToggleButton toggle) return;

			Brush original = GetOriginalBackground(toggle);
			if (original == null) return;

			int durationMs = GetCheckedBackgroundDuration(toggle);
			TimeSpan duration = TimeSpan.FromMilliseconds(durationMs);

			if (original is SolidColorBrush target)
			{
				var currentBrush = (toggle.Background as SolidColorBrush)?.Clone() ?? new SolidColorBrush(target.Color);
				toggle.Background = currentBrush;

				currentBrush.BeginAnimation(
					SolidColorBrush.ColorProperty,
					new ColorAnimation(target.Color, duration));
			}
			else
			{
				toggle.Background = original.CloneCurrentValue();
			}
		}
		#endregion
	}
}
