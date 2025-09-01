using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MajFX
{
	public static partial class ToggleButtonHelper
	{
		#region ClickForeground
		public static readonly DependencyProperty ClickForegroundProperty =
			DependencyProperty.RegisterAttached(
				"ClickForeground",
				typeof(Brush),
				typeof(ToggleButtonHelper),
				new PropertyMetadata(null, OnClickForegroundChanged));

		public static void SetClickForeground(DependencyObject element, Brush value)
			=> element.SetValue(ClickForegroundProperty, value);

		public static Brush GetClickForeground(DependencyObject element)
			=> (Brush)element.GetValue(ClickForegroundProperty);
		#endregion

		#region ClickForegroundDuration
		public static readonly DependencyProperty ClickForegroundDurationProperty =
			DependencyProperty.RegisterAttached(
				"ClickForegroundDuration",
				typeof(int),
				typeof(ToggleButtonHelper),
				new PropertyMetadata(0));

		public static void SetClickForegroundDuration(DependencyObject element, int value)
			=> element.SetValue(ClickForegroundDurationProperty, value);

		public static int GetClickForegroundDuration(DependencyObject element)
			=> (int)element.GetValue(ClickForegroundDurationProperty);
		#endregion

		#region OriginalClickForeground
		private static readonly DependencyProperty OriginalClickForegroundProperty =
			DependencyProperty.RegisterAttached("OriginalClickForegroundInternal", typeof(Brush), typeof(ToggleButtonHelper), new PropertyMetadata(null));

		private static void SetOriginalClickForeground(DependencyObject element, Brush value)
			=> element.SetValue(OriginalClickForegroundProperty, value);

		private static Brush GetOriginalClickForeground(DependencyObject element)
			=> (Brush)element.GetValue(OriginalClickForegroundProperty);
		#endregion

		#region Mouse Events
		private static void OnClickForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is not ToggleButton button) return;

			button.PreviewMouseLeftButtonDown -= ClickForeground_MouseDown;
			button.PreviewMouseLeftButtonUp -= ClickForeground_MouseUp;

			if (e.NewValue != null)
			{
				if (GetOriginalClickForeground(button) == null)
					SetOriginalClickForeground(button, button.Foreground);

				button.PreviewMouseLeftButtonDown += ClickForeground_MouseDown;
				button.PreviewMouseLeftButtonUp += ClickForeground_MouseUp;
			}
			else
			{
				var original = GetOriginalClickForeground(button);
				if (original != null)
					button.Foreground = original;
			}
		}

		private static void ClickForeground_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (sender is not Control control) return;

			var targetBrush = GetClickForeground(control);
			if (targetBrush == null) return;

			int durationMs = GetClickForegroundDuration(control);
			TimeSpan duration = TimeSpan.FromMilliseconds(durationMs);

			SolidColorBrush currentBrush = control.Foreground as SolidColorBrush;
			if (currentBrush == null || currentBrush.IsFrozen)
				currentBrush = new SolidColorBrush(((SolidColorBrush)GetOriginalClickForeground(control)).Color);

			control.Foreground = currentBrush;

			if (targetBrush is SolidColorBrush targetSolid)
				currentBrush.BeginAnimation(SolidColorBrush.ColorProperty, new ColorAnimation(targetSolid.Color, duration));
			else
				control.Foreground = targetBrush;
		}

		private static void ClickForeground_MouseUp(object sender, MouseButtonEventArgs e)
		{
			if (sender is not Control control) return;

			var original = GetOriginalClickForeground(control);
			if (original == null) return;

			int durationMs = GetClickForegroundDuration(control);
			TimeSpan duration = TimeSpan.FromMilliseconds(durationMs);

			SolidColorBrush currentBrush = control.Foreground as SolidColorBrush;

			if (original is SolidColorBrush originalSolid)
			{
				if (currentBrush == null || currentBrush.IsFrozen)
					currentBrush = new SolidColorBrush(currentBrush?.Color ?? originalSolid.Color);

				control.Foreground = currentBrush;
				currentBrush.BeginAnimation(SolidColorBrush.ColorProperty, new ColorAnimation(originalSolid.Color, duration));
			}
			else
			{
				control.Foreground = original;
			}
		}
		#endregion
	}
}