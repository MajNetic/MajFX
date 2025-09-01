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
		#region HoverBackground
		public static readonly DependencyProperty HoverBackgroundProperty =
			DependencyProperty.RegisterAttached(
				"HoverBackground",
				typeof(Brush),
				typeof(ToggleButtonHelper),
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
				typeof(ToggleButtonHelper),
				new PropertyMetadata(0));

		public static void SetBackgroundHoverDuration(DependencyObject element, int value)
			=> element.SetValue(BackgroundHoverDurationProperty, value);

		public static int GetBackgroundHoverDuration(DependencyObject element)
			=> (int)element.GetValue(BackgroundHoverDurationProperty);
		#endregion

		#region OriginalBackground
		private static readonly DependencyProperty OriginalBackgroundProperty =
			DependencyProperty.RegisterAttached("OriginalBackground", typeof(Brush), typeof(ToggleButtonHelper), new PropertyMetadata(null));

		private static void SetOriginalBackground(DependencyObject element, Brush value)
			=> element.SetValue(OriginalBackgroundProperty, value);

		private static Brush GetOriginalBackground(DependencyObject element)
			=> (Brush)element.GetValue(OriginalBackgroundProperty);
		#endregion

		#region Mouse Events
		private static void OnHoverBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is not Control control) return;

			control.MouseEnter -= HoverBackground_MouseEnter;
			control.MouseLeave -= HoverBackground_MouseLeave;

			if (e.NewValue != null)
			{
				if (GetOriginalBackground(control) == null)
				{
					if (control.Background is SolidColorBrush scb)
						SetOriginalBackground(control, new SolidColorBrush(scb.Color));
					else
						SetOriginalBackground(control, control.Background ?? new SolidColorBrush(Colors.Transparent));
				}

				control.MouseEnter += HoverBackground_MouseEnter;
				control.MouseLeave += HoverBackground_MouseLeave;
			}
			else
			{
				control.Background = GetOriginalBackground(control);
			}
		}

		private static void HoverBackground_MouseEnter(object sender, MouseEventArgs e)
		{
			if (sender is not Control control) return;

			Brush targetBrush = GetHoverBackground(control);
			if (targetBrush == null) return;

			int durationMs = GetBackgroundHoverDuration(control);
			TimeSpan duration = TimeSpan.FromMilliseconds(durationMs);

			SolidColorBrush currentBrush = (control.Background as SolidColorBrush)?.Clone();
			if (currentBrush == null)
				currentBrush = new SolidColorBrush(Colors.Transparent);

			control.Background = currentBrush;

			if (targetBrush is SolidColorBrush targetSolid)
			{
				currentBrush.BeginAnimation(
					SolidColorBrush.ColorProperty,
					new ColorAnimation(targetSolid.Color, duration));
			}
			else
			{
				control.Background = targetBrush.CloneCurrentValue();
			}
		}

		private static void HoverBackground_MouseLeave(object sender, MouseEventArgs e)
		{
			if (sender is not Control control) return;

			if (control is ToggleButton toggle && toggle.IsChecked == true)
			{
				Brush checkedBrush = GetCheckedBackground(toggle);
				if (checkedBrush != null)
				{
					int durationMs = GetBackgroundHoverDuration(control);
					TimeSpan duration = TimeSpan.FromMilliseconds(durationMs);

					if (checkedBrush is SolidColorBrush target)
					{
						if (control.Background is SolidColorBrush current)
						{
							current.BeginAnimation(SolidColorBrush.ColorProperty, new ColorAnimation(target.Color, duration));
						}
						else
						{
							control.Background = target;
						}
					}
					else
					{
						control.Background = checkedBrush;
					}
				}
				return;
			}

			Brush original = GetOriginalBackground(control);
			if (original == null) return;

			int normalDurationMs = GetBackgroundHoverDuration(control);
			TimeSpan normalDuration = TimeSpan.FromMilliseconds(normalDurationMs);

			if (original is SolidColorBrush normalTarget)
			{
				if (control.Background is SolidColorBrush current)
				{
					current.BeginAnimation(SolidColorBrush.ColorProperty, new ColorAnimation(normalTarget.Color, normalDuration));
				}
				else
				{
					control.Background = normalTarget;
				}
			}
			else
			{
				control.Background = original;
			}
		}


		#endregion
	}
}