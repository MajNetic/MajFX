using System.Windows;
using System.Windows.Controls;

namespace MajFX
{
    public static partial class ToggleButtonHelper
    {
	    #region CornerRadius
	    public static readonly DependencyProperty CornerRadiusProperty =
		    DependencyProperty.RegisterAttached(
			    "CornerRadius",
			    typeof(CornerRadius),
			    typeof(ToggleButtonHelper),
			    new PropertyMetadata(new CornerRadius(0), OnCornerRadiusChanged));

	    public static void SetCornerRadius(DependencyObject element, CornerRadius value)
		    => element.SetValue(CornerRadiusProperty, value);

	    public static CornerRadius GetCornerRadius(DependencyObject element)
		    => (CornerRadius)element.GetValue(CornerRadiusProperty);
	    #endregion

	    private static void OnCornerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	    {
		    if (d is not Control control) return;

		    control.Loaded -= ControlWithCornerRadius_Loaded;
		    control.Loaded += ControlWithCornerRadius_Loaded;

		    ApplyCornerRadius(control);
	    }

	    private static void ControlWithCornerRadius_Loaded(object sender, RoutedEventArgs e)
	    {
		    if (sender is Control control)
			    ApplyCornerRadius(control);
	    }

	    private static void ApplyCornerRadius(Control control)
	    {
		    var border = control.Template?.FindName("PART_Border", control) as Border;
		    if (border == null)
			    return;

		    var radius = GetCornerRadius(control);
		    border.CornerRadius = radius;
	    }
	}
}