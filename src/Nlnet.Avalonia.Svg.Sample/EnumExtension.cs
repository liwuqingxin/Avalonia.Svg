using System;
using Avalonia.Markup.Xaml;

namespace Nlnet.Avalonia.Svg.Sample
{
    /// <summary>
    /// Xaml markup to get the enum values.
    /// </summary>
    public class EnumExtension : MarkupExtension
    {
        [ConstructorArgument(nameof(Type))]
        public Type Type { get; set; }

        public EnumExtension(Type type)
        {
            Type = type;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetValues(Type);
        }
    }
}
