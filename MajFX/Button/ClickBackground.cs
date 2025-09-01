// using System;
// using System.Windows;
// using System.Windows.Controls;
// using System.Windows.Input;
// using System.Windows.Media;
// using System.Windows.Media.Animation;
//
// namespace MajFX
// {
// 	public static partial class ButtonHelper
// 	{
// 		#region ClickBackground
// 		public static readonly DependencyProperty ClickBackgroundProperty =
// 			DependencyProperty.RegisterAttached(
// 				"ClickBackground",
// 				typeof(Brush),
// 				typeof(ButtonHelper),
// 				new PropertyMetadata(null, OnClickBackgroundChanged));
//
// 		public static void SetClickBackground(DependencyObject element, Brush value)
// 			=> element.SetValue(ClickBackgroundProperty, value);
//
// 		public static Brush GetClickBackground(DependencyObject element)
// 			=> (Brush)element.GetValue(ClickBackgroundProperty);
// 		#endregion
//
// 		#region ClickBackgroundDuration
// 		public static readonly DependencyProperty ClickBackgroundDurationProperty =
// 			DependencyProperty.RegisterAttached(
// 				"ClickBackgroundDuration",
// 				typeof(int),
// 				typeof(ButtonHelper),
// 				new PropertyMetadata(0));
//
// 		public static void SetClickBackgroundDuration(DependencyObject element, int value)
// 			=> element.SetValue(ClickBackgroundDurationProperty, value);
//
// 		public static int GetClickBackgroundDuration(DependencyObject element)
// 			=> (int)element.GetValue(ClickBackgroundDurationProperty);
// 		#endregion
//
// 		#region Mouse Events
// 		private static void OnClickBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
// 		{
// 			if (d is not Button button) return;
//
// 			button.PreviewMouseLeftButtonDown -= ClickBackground_MouseDown;
// 			button.PreviewMouseLeftButtonUp -= ClickBackground_MouseUp;
//
// 			if (e.NewValue != null)
// 			{
// 				button.PreviewMouseLeftButtonDown += ClickBackground_MouseDown;
// 				button.PreviewMouseLeftButtonUp += ClickBackground_MouseUp;
// 			}
// 		}
//
// 		private static void ClickBackground_MouseDown(object sender, MouseButtonEventArgs e)
// 		{
// 			if (sender is not Button button) return;
//
// 			Brush targetBrush = GetClickBackground(button);
// 			if (targetBrush == null) return;
//
// 			int durationMs = GetClickBackgroundDuration(button);
// 			TimeSpan duration = TimeSpan.FromMilliseconds(durationMs);
//
// 			if (button.Background is not SolidColorBrush current || current.IsFrozen)
// 			{
// 				current = new SolidColorBrush((button.Background as SolidColorBrush)?.Color ?? Colors.Transparent);
// 				button.Background = current;
// 			}
//
// 			if (targetBrush is SolidColorBrush targetSolid)
// 			{
// 				current.BeginAnimation(SolidColorBrush.ColorProperty,
// 					new ColorAnimation(targetSolid.Color, duration));
// 			}
// 			else
// 			{
// 				button.Background = targetBrush.CloneCurrentValue();
// 			}
// 		}
//
// 		private static void ClickBackground_MouseUp(object sender, MouseButtonEventArgs e)
// 		{
// 			if (sender is not Button button) return;
//
// 			int durationMs = GetClickBackgroundDuration(button);
// 			TimeSpan duration = TimeSpan.FromMilliseconds(durationMs);
//
// 			if (GetHoverBackground(button) is SolidColorBrush hover && button.IsMouseOver)
// 			{
// 				if (button.Background is not SolidColorBrush current || current.IsFrozen)
// 				{
// 					current = new SolidColorBrush((button.Background as SolidColorBrush)?.Color ?? Colors.Transparent);
// 					button.Background = current;
// 				}
//
// 				current.BeginAnimation(SolidColorBrush.ColorProperty,
// 					new ColorAnimation(hover.Color, duration));
// 			}
// 			else
// 			{
// 				Brush original = GetOriginalBackground(button);
// 				if (original is SolidColorBrush target)
// 				{
// 					if (button.Background is not SolidColorBrush current || current.IsFrozen)
// 					{
// 						current = new SolidColorBrush(target.Color);
// 						button.Background = current;
// 					}
//
// 					current.BeginAnimation(SolidColorBrush.ColorProperty,
// 						new ColorAnimation(target.Color, duration));
// 				}
// 				else if (original != null)
// 				{
// 					button.Background = original.CloneCurrentValue();
// 				}
// 			}
// 		}
// 		#endregion
//
// 	}
// }


using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MajFX
{
	public static partial class ButtonHelper
	{
		#region ClickBackground
		public static readonly DependencyProperty ClickBackgroundProperty =
			DependencyProperty.RegisterAttached(
				"ClickBackground",
				typeof(Brush),
				typeof(ButtonHelper),
				new PropertyMetadata(null, OnClickBackgroundChanged));

		public static void SetClickBackground(DependencyObject element, Brush value)
			=> element.SetValue(ClickBackgroundProperty, value);

		public static Brush GetClickBackground(DependencyObject element)
			=> (Brush)element.GetValue(ClickBackgroundProperty);
		#endregion

		#region BackgroundClickDuration
		public static readonly DependencyProperty BackgroundClickDurationProperty =
			DependencyProperty.RegisterAttached(
				"BackgroundClickDuration",
				typeof(int),
				typeof(ButtonHelper),
				new PropertyMetadata(150)); // پیش‌فرض 150ms

		public static void SetBackgroundClickDuration(DependencyObject element, int value)
			=> element.SetValue(BackgroundClickDurationProperty, value);

		public static int GetBackgroundClickDuration(DependencyObject element)
			=> (int)element.GetValue(BackgroundClickDurationProperty);
		#endregion

		private static void OnClickBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is not Control control) return;

			control.PreviewMouseLeftButtonDown -= ClickBackground_MouseDown;
			control.PreviewMouseLeftButtonUp -= ClickBackground_MouseUp;

			if (e.NewValue != null)
			{
				control.PreviewMouseLeftButtonDown += ClickBackground_MouseDown;
				control.PreviewMouseLeftButtonUp += ClickBackground_MouseUp;
			}
			else
			{
				var overlay = GetOverlay(control);
				if (overlay != null)
					overlay.Background = Brushes.Transparent;
				else
					control.ClearValue(Control.BackgroundProperty);
			}
		}

		private static void ClickBackground_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (sender is not Control control) return;

			Brush clickBrush = GetClickBackground(control);
			if (clickBrush == null) return;

			TimeSpan duration = TimeSpan.FromMilliseconds(GetBackgroundClickDuration(control));
			var overlay = GetOverlay(control);

			if (overlay != null)
				ApplyAnimation(overlay, clickBrush, duration);
			else
				ApplyAnimation(control, clickBrush, duration);
		}

		private static void ClickBackground_MouseUp(object sender, MouseButtonEventArgs e)
		{
			if (sender is not Control control) return;

			// بعد از کلیک، برگرده به Hover یا حالت نرمال
			Brush hoverBrush = GetHoverBackground(control);
			Brush targetBrush = control.IsMouseOver && hoverBrush != null
				? hoverBrush
				: new SolidColorBrush(Colors.Transparent);

			TimeSpan duration = TimeSpan.FromMilliseconds(GetBackgroundClickDuration(control));
			var overlay = GetOverlay(control);

			if (overlay != null)
				ApplyAnimation(overlay, targetBrush, duration);
			else
				ApplyAnimation(control, targetBrush, duration);
		}
	}
}
