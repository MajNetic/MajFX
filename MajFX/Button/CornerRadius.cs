using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MajFX
{
	public static partial class ButtonHelper
	{
		#region CornerRadius
		public static readonly DependencyProperty CornerRadiusProperty =
			DependencyProperty.RegisterAttached(
				"CornerRadius",
				typeof(CornerRadius),
				typeof(ButtonHelper),
				new PropertyMetadata(new CornerRadius(0), OnCornerRadiusChanged));

		public static void SetCornerRadius(DependencyObject element, CornerRadius value)
			=> element.SetValue(CornerRadiusProperty, value);

		public static CornerRadius GetCornerRadius(DependencyObject element)
			=> (CornerRadius)element.GetValue(CornerRadiusProperty);
		#endregion

		private static void OnCornerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is not Control control) return;

			control.Loaded -= Control_Loaded;
			control.Loaded += Control_Loaded;

			control.IsVisibleChanged -= Control_IsVisibleChanged;
			control.IsVisibleChanged += Control_IsVisibleChanged;

			ApplyCornerRadiusWhenReady(control);
		}

		private static void Control_Loaded(object sender, RoutedEventArgs e)
		{
			if (sender is Control control)
				ApplyCornerRadiusWhenReady(control);
		}

		private static void Control_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (sender is Control control && control.IsVisible)
				ApplyCornerRadiusWhenReady(control);
		}

		private static void ApplyCornerRadiusWhenReady(Control control)
		{
			control.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
			{
				if (control.Template == null) return;

				if (control.Template.FindName("PART_Border", control) is not Border border)
					return;

				if (control.Template.FindName("PART_HoverOverlay", control) is not Border borderOverlay)
					return;

				var radius = GetCornerRadius(control);
				border.CornerRadius = radius;
				borderOverlay.CornerRadius = radius;
			}));
		}
	}
}

