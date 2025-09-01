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
		#region HoverForeground
		public static readonly DependencyProperty HoverForegroundProperty =
			DependencyProperty.RegisterAttached(
				"HoverForeground",
				typeof(Brush),
				typeof(ButtonHelper),
				new PropertyMetadata(null, OnHoverForegroundChanged));

		public static void SetHoverForeground(DependencyObject element, Brush value)
			=> element.SetValue(HoverForegroundProperty, value);

		public static Brush GetHoverForeground(DependencyObject element)
			=> (Brush)element.GetValue(HoverForegroundProperty);
		#endregion

		#region ForegroundHoverDuration
		public static readonly DependencyProperty ForegroundHoverDurationProperty =
			DependencyProperty.RegisterAttached(
				"ForegroundHoverDuration",
				typeof(int),
				typeof(ButtonHelper),
				new PropertyMetadata(0));

		public static void SetForegroundHoverDuration(DependencyObject element, int value)
			=> element.SetValue(ForegroundHoverDurationProperty, value);

		public static int GetForegroundHoverDuration(DependencyObject element)
			=> (int)element.GetValue(ForegroundHoverDurationProperty);
		#endregion

		#region OriginalForeground
		private static readonly DependencyProperty OriginalForegroundProperty =
			DependencyProperty.RegisterAttached("OriginalForegroundInternal", typeof(Brush), typeof(ButtonHelper), new PropertyMetadata(null));

		private static void SetOriginalForeground(DependencyObject element, Brush value)
			=> element.SetValue(OriginalForegroundProperty, value);

		private static Brush GetOriginalForeground(DependencyObject element)
			=> (Brush)element.GetValue(OriginalForegroundProperty);
		#endregion

		#region Mouse Events
		private static void OnHoverForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is not Control control) return;

			control.MouseEnter -= HoverForeground_MouseEnter;
			control.MouseLeave -= HoverForeground_MouseLeave;

			if (e.NewValue != null)
			{
				if (GetOriginalForeground(control) == null)
					SetOriginalForeground(control, control.Foreground);

				control.MouseEnter += HoverForeground_MouseEnter;
				control.MouseLeave += HoverForeground_MouseLeave;
			}
			else
			{
				var original = GetOriginalForeground(control);
				if (original != null)
					control.Foreground = original;
			}
		}

		private static void HoverForeground_MouseEnter(object sender, MouseEventArgs e)
		{
			if (sender is not Control control) return;

			var targetBrush = GetHoverForeground(control);
			if (targetBrush == null) return;

			int durationMs = GetForegroundHoverDuration(control);
			TimeSpan duration = TimeSpan.FromMilliseconds(durationMs);

			SolidColorBrush currentBrush = control.Foreground as SolidColorBrush;
			if (currentBrush == null || currentBrush.IsFrozen)
				currentBrush = new SolidColorBrush(((SolidColorBrush)GetOriginalForeground(control)).Color);

			control.Foreground = currentBrush;

			if (targetBrush is SolidColorBrush targetSolid)
				currentBrush.BeginAnimation(SolidColorBrush.ColorProperty, new ColorAnimation(targetSolid.Color, duration));
			else
				control.Foreground = targetBrush;
		}

		private static void HoverForeground_MouseLeave(object sender, MouseEventArgs e)
		{
			if (sender is not Control control) return;

			var original = GetOriginalForeground(control);
			if (original == null) return;

			int durationMs = GetForegroundHoverDuration(control);
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
