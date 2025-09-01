using System.Windows;

namespace MajFX
{
	public static class StyleKeys
	{
		public static ComponentResourceKey ButtonStyleKey { get; } =
			new ComponentResourceKey(typeof(StyleKeys), nameof(ButtonStyleKey));
	}
}