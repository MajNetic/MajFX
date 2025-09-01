using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace MajFX.Internal
{
	internal sealed class ColorOverlayAdorner : Adorner
	{
		private readonly VisualCollection _visuals;
		private readonly Border _overlay;

		public ColorOverlayAdorner(UIElement adornedElement) : base(adornedElement)
		{
			IsHitTestVisible = false;

			_overlay = new Border
			{
				Background = Brushes.Transparent,
				Opacity = 0,
				IsHitTestVisible = false
			};

			// اول VisualCollection رو بسازیم
			_visuals = new VisualCollection(this);
			_visuals.Add(_overlay);
		}

		public Brush OverlayBrush
		{
			get => _overlay.Background;
			set => _overlay.Background = value ?? Brushes.Transparent;
		}

		public double OverlayOpacity
		{
			get => _overlay.Opacity;
			set => _overlay.Opacity = value;
		}

		public CornerRadius CornerRadius
		{
			get => _overlay.CornerRadius;
			set => _overlay.CornerRadius = value;
		}

		protected override int VisualChildrenCount => _visuals?.Count ?? 0;

		protected override Visual GetVisualChild(int index) => _visuals[index];

		protected override Size ArrangeOverride(Size finalSize)
		{
			_overlay.Arrange(new Rect(finalSize));
			return finalSize;
		}
	}
}