using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MajFX
{
	public static partial class ToggleButtonHelper
	{
		#region ClickBackground
		public static readonly DependencyProperty ClickBackgroundProperty =
			DependencyProperty.RegisterAttached(
				"ClickBackground",
				typeof(Brush),
				typeof(ToggleButtonHelper),
				new PropertyMetadata(null, OnClickBackgroundChanged));

		public static void SetClickBackground(DependencyObject element, Brush value)
			=> element.SetValue(ClickBackgroundProperty, value);

		public static Brush GetClickBackground(DependencyObject element)
			=> (Brush)element.GetValue(ClickBackgroundProperty);
		#endregion

		#region ClickBackgroundDuration
		public static readonly DependencyProperty ClickBackgroundDurationProperty =
			DependencyProperty.RegisterAttached(
				"ClickBackgroundDuration",
				typeof(int),
				typeof(ToggleButtonHelper),
				new PropertyMetadata(0));

		public static void SetClickBackgroundDuration(DependencyObject element, int value)
			=> element.SetValue(ClickBackgroundDurationProperty, value);

		public static int GetClickBackgroundDuration(DependencyObject element)
			=> (int)element.GetValue(ClickBackgroundDurationProperty);
		#endregion

		#region Mouse Events
		private static void OnClickBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is not ToggleButton button) return;

			button.PreviewMouseLeftButtonDown -= ClickBackground_MouseDown;
			button.PreviewMouseLeftButtonUp -= ClickBackground_MouseUp;

			if (e.NewValue != null)
			{
				button.PreviewMouseLeftButtonDown += ClickBackground_MouseDown;
				button.PreviewMouseLeftButtonUp += ClickBackground_MouseUp;
			}
		}

		private static void ClickBackground_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (sender is not ToggleButton button) return;

			Brush targetBrush = GetClickBackground(button);
			if (targetBrush == null) return;

			int durationMs = GetClickBackgroundDuration(button);
			TimeSpan duration = TimeSpan.FromMilliseconds(durationMs);

			SolidColorBrush currentBrush = button.Background as SolidColorBrush;

			if (currentBrush == null || currentBrush.IsFrozen)
			{
				Brush orig = button.Background ?? new SolidColorBrush(Colors.Transparent);
				currentBrush = new SolidColorBrush((orig as SolidColorBrush)?.Color ?? Colors.Transparent);
			}

			button.Background = currentBrush;

			if (targetBrush is SolidColorBrush targetSolid)
				currentBrush.BeginAnimation(SolidColorBrush.ColorProperty, new ColorAnimation(targetSolid.Color, duration));
			else
				button.Background = targetBrush;
		}

		private static void ClickBackground_MouseUp(object sender, MouseButtonEventArgs e)
		{
			if (sender is not ToggleButton button) return;

			Brush original = GetOriginalBackground(button);
			if (original == null) return;

			int durationMs = GetClickBackgroundDuration(button);
			TimeSpan duration = TimeSpan.FromMilliseconds(durationMs);

			if (original is SolidColorBrush target)
			{
				if (button.Background is SolidColorBrush current)
				{
					current.BeginAnimation(SolidColorBrush.ColorProperty, new ColorAnimation(target.Color, duration));
				}
				else
				{
					button.Background = target;
				}
			}
			else
			{
				button.Background = original;
			}
		}
		#endregion
	}
}