using System.ComponentModel;
using System.Globalization;
using System;
using Nlnet.Avalonia.Svg.Utils;

namespace Nlnet.Avalonia.Svg
{
    public enum PreserveAspectRatioAlign
    {
        none,
        xMinYMin,
        xMidYMin,
        xMaxYMin,
        xMinYMid,
        xMidYMid,
        xMaxYMid,
        xMinYMax,
        xMidYMax,
        xMaxYMax
    }

    public enum PreserveAspectRatioMeetOrSlice
    {
        meet,
        slice
    }

    /// <summary>
    /// Creates an <see cref="PreserveAspectRatio"/> from a string representation.
    /// </summary>
    public class PreserveAspectRatioConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object? value)
        {
            return value is string s ? PreserveAspectRatio.Parse(s) : null;
        }
    }

    [TypeConverter(typeof(PreserveAspectRatioConverter))]
    public class PreserveAspectRatio
    {
        public PreserveAspectRatioAlign Align { get; }
        public PreserveAspectRatioMeetOrSlice MeetOrSlice { get; }

        public PreserveAspectRatio(PreserveAspectRatioAlign align, PreserveAspectRatioMeetOrSlice meetOrSlice)
        {
            Align       = align;
            MeetOrSlice = meetOrSlice;
        }

        public static PreserveAspectRatio? Parse(string? str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return null;
            }
            if (str == "none")
            {
                return new PreserveAspectRatio(PreserveAspectRatioAlign.none, PreserveAspectRatioMeetOrSlice.meet);
            }

            var tokenizer = new SafeStringTokenizer(str, ' ', ',');
            if (tokenizer.GetCount() == 0)
            {
                return null;
            }

            var align       = PreserveAspectRatioAlign.none;
            var meetOrSlice = PreserveAspectRatioMeetOrSlice.meet;

            align = Enum.Parse<PreserveAspectRatioAlign>(tokenizer.Item1);
            if (tokenizer.GetCount() > 1)
            {
                meetOrSlice = Enum.Parse<PreserveAspectRatioMeetOrSlice>(tokenizer.Item2);
            }

            return new PreserveAspectRatio(align, meetOrSlice);
        }
    }
}
