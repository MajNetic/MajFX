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
// 		#region HoverBackground
// 		public static readonly DependencyProperty HoverBackgroundProperty =
// 			DependencyProperty.RegisterAttached(
// 				"HoverBackground",
// 				typeof(Brush),
// 				typeof(ButtonHelper),
// 				new PropertyMetadata(null, OnHoverBackgroundChanged));
//
// 		public static void SetHoverBackground(DependencyObject element, Brush value)
// 			=> element.SetValue(HoverBackgroundProperty, value);
//
// 		public static Brush GetHoverBackground(DependencyObject element)
// 			=> (Brush)element.GetValue(HoverBackgroundProperty);
// 		#endregion
//
// 		#region BackgroundHoverDuration
// 		public static readonly DependencyProperty BackgroundHoverDurationProperty =
// 			DependencyProperty.RegisterAttached(
// 				"BackgroundHoverDuration",
// 				typeof(int),
// 				typeof(ButtonHelper),
// 				new PropertyMetadata(0));
//
// 		public static void SetBackgroundHoverDuration(DependencyObject element, int value)
// 			=> element.SetValue(BackgroundHoverDurationProperty, value);
//
// 		public static int GetBackgroundHoverDuration(DependencyObject element)
// 			=> (int)element.GetValue(BackgroundHoverDurationProperty);
// 		#endregion
//
// 		#region OriginalBackground
// 		private static readonly DependencyProperty OriginalBackgroundProperty =
// 			DependencyProperty.RegisterAttached(
// 				"OriginalBackground",
// 				typeof(Brush),
// 				typeof(ButtonHelper),
// 				new PropertyMetadata(null));
//
// 		private static void SetOriginalBackground(DependencyObject element, Brush value)
// 			=> element.SetValue(OriginalBackgroundProperty, value);
//
// 		private static Brush GetOriginalBackground(DependencyObject element)
// 			=> (Brush)element.GetValue(OriginalBackgroundProperty);
// 		#endregion
//
// 		#region Mouse Events
// 		private static void OnHoverBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
// 		{
// 			if (d is not Control control) return;
// 		
// 			control.MouseEnter -= HoverBackground_MouseEnter;
// 			control.MouseLeave -= HoverBackground_MouseLeave;
// 		
// 			if (e.NewValue != null)
// 			{
// 				if (GetOriginalBackground(control) == null)
// 				{
// 					if (control.Background is SolidColorBrush scb)
// 						SetOriginalBackground(control, new SolidColorBrush(scb.Color));
// 					else
// 						SetOriginalBackground(control, control.Background ?? new SolidColorBrush(Colors.Transparent));
// 				}
// 		
// 				control.MouseEnter += HoverBackground_MouseEnter;
// 				control.MouseLeave += HoverBackground_MouseLeave;
// 			}
// 			else
// 			{
// 				control.Background = GetOriginalBackground(control);
// 			}
// 		}
//
// 		private static void HoverBackground_MouseEnter(object sender, MouseEventArgs e)
// 		{
// 			if (sender is not Control control) return;
//
// 			Brush targetBrush = GetHoverBackground(control);
// 			if (targetBrush == null) return;
//
// 			int durationMs = GetBackgroundHoverDuration(control);
// 			TimeSpan duration = TimeSpan.FromMilliseconds(durationMs);
//
// 			SolidColorBrush currentBrush = (control.Background as SolidColorBrush)?.Clone();
// 			if (currentBrush == null)
// 				currentBrush = new SolidColorBrush(Colors.Transparent);
//
// 			control.Background = currentBrush;
//
// 			if (targetBrush is SolidColorBrush targetSolid)
// 			{
// 				currentBrush.BeginAnimation(
// 					SolidColorBrush.ColorProperty,
// 					new ColorAnimation(targetSolid.Color, duration));
// 			}
// 			else
// 			{
// 				control.Background = targetBrush.CloneCurrentValue();
// 			}
// 		}
//
// 		private static void HoverBackground_MouseLeave(object sender, MouseEventArgs e)
// 		{
// 			if (sender is not Control control) return;
//
// 			Brush original = GetOriginalBackground(control);
// 			if (original == null) return;
//
// 			int durationMs = GetBackgroundHoverDuration(control);
// 			TimeSpan duration = TimeSpan.FromMilliseconds(durationMs);
//
// 			if (original is SolidColorBrush target)
// 			{
// 				if (control.Background is not SolidColorBrush current || current.IsFrozen)
// 				{
// 					current = new SolidColorBrush(target.Color);
// 					control.Background = current;
// 				}
//
// 				current.BeginAnimation(
// 					SolidColorBrush.ColorProperty,
// 					new ColorAnimation(target.Color, duration));
//
// 				// current.BeginAnimation(
// 				// 	SolidColorBrush.ColorProperty,
// 				// 	new ColorAnimation((control.Background as SolidColorBrush).Color, duration));
// 			}
// 			else
// 			{
// 				control.Background = original.CloneCurrentValue();
// 			}
// 		}
//
// 		#endregion
// 	}
// }
//
//
//



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
		#region HoverBackground
		public static readonly DependencyProperty HoverBackgroundProperty =
			DependencyProperty.RegisterAttached(
				"HoverBackground",
				typeof(Brush),
				typeof(ButtonHelper),
				new PropertyMetadata(null, OnHoverBackgroundChanged));

		public static void SetHoverBackground(DependencyObject element, Brush value)
			=> element.SetValue(HoverBackgroundProperty, value);

		public static Brush GetHoverBackground(DependencyObject element)
			=> (Brush)element.GetValue(HoverBackgroundProperty);
		#endregion

		#region BackgroundHoverDuration
		public static readonly DependencyProperty BackgroundHoverDurationProperty =
			DependencyProperty.RegisterAttached(
				"BackgroundHoverDuration",
				typeof(int),
				typeof(ButtonHelper),
				new PropertyMetadata(0));

		public static void SetBackgroundHoverDuration(DependencyObject element, int value)
			=> element.SetValue(BackgroundHoverDurationProperty, value);

		public static int GetBackgroundHoverDuration(DependencyObject element)
			=> (int)element.GetValue(BackgroundHoverDurationProperty);
		#endregion

		#region Mouse Events
		private static void OnHoverBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is not Control control) return;

			control.MouseEnter -= HoverBackground_MouseEnter;
			control.MouseLeave -= HoverBackground_MouseLeave;

			if (e.NewValue != null)
			{
				control.MouseEnter += HoverBackground_MouseEnter;
				control.MouseLeave += HoverBackground_MouseLeave;
			}
			else
			{
				// اگر HoverBackground پاک شد → Overlay یا Background ریست شود
				var overlay = GetOverlay(control);
				if (overlay != null)
					overlay.Background = Brushes.Transparent;
				else
					control.ClearValue(Control.BackgroundProperty);
			}
		}

		private static void HoverBackground_MouseEnter(object sender, MouseEventArgs e)
		{
			if (sender is not Control control) return;

			Brush targetBrush = GetHoverBackground(control);
			if (targetBrush == null) return;

			int durationMs = GetBackgroundHoverDuration(control);
			TimeSpan duration = TimeSpan.FromMilliseconds(durationMs);

			var overlay = GetOverlay(control);

			if (overlay != null)
			{
				ApplyAnimation(overlay, targetBrush, duration);
			}
			else
			{
				ApplyAnimation(control, targetBrush, duration);
			}
		}

		private static void HoverBackground_MouseLeave(object sender, MouseEventArgs e)
		{
			if (sender is not Control control) return;

			int durationMs = GetBackgroundHoverDuration(control);
			TimeSpan duration = TimeSpan.FromMilliseconds(durationMs);

			var overlay = GetOverlay(control);

			if (overlay != null)
			{
				ApplyAnimation(overlay, new SolidColorBrush(Colors.Transparent), duration);
			}
			else
			{
				// در حالت عادی به Transparent برگرده
				ApplyAnimation(control, new SolidColorBrush(Colors.Transparent), duration);
			}
		}
		#endregion

		#region Helpers
		/// <summary>
		/// سعی می‌کند Border به اسم PART_HoverOverlay را از Template کنترل بگیرد.
		/// اگر نبود → null برمی‌گرداند.
		/// </summary>
		private static Border? GetOverlay(Control control)
		{
			if (control.Template == null) return null;
			control.ApplyTemplate();
			return control.Template.FindName("PART_HoverOverlay", control) as Border;
		}

		/// <summary>
		/// انیمیشن رنگ روی یک UIElement (Control یا Border) اعمال می‌کند.
		/// </summary>
		private static void ApplyAnimation(FrameworkElement element, Brush targetBrush, TimeSpan duration)
		{
			SolidColorBrush currentBrush;

			if (element is Control control)
			{
				currentBrush = (control.Background as SolidColorBrush)?.Clone()
							   ?? new SolidColorBrush(Colors.Transparent);
				control.Background = currentBrush;
			}
			else if (element is Border border)
			{
				currentBrush = (border.Background as SolidColorBrush)?.Clone()
							   ?? new SolidColorBrush(Colors.Transparent);
				border.Background = currentBrush;
			}
			else
			{
				return;
			}

			if (targetBrush is SolidColorBrush targetSolid)
			{
				currentBrush.BeginAnimation(
					SolidColorBrush.ColorProperty,
					new ColorAnimation(targetSolid.Color, duration));
			}
			else
			{
				if (element is Control ctrl)
					ctrl.Background = targetBrush.CloneCurrentValue();
				else if (element is Border brd)
					brd.Background = targetBrush.CloneCurrentValue();
			}
		}
		#endregion
	}
}
