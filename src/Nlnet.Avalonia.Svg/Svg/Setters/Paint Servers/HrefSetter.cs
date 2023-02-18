using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg
{
    /// <summary>
    /// https://www.w3.org/TR/SVG2/pservers.html#LinearGradientAttributes
    /// </summary>
    [SetterGenerator(typeof(string), SvgDefaultValues.Null)]
    public class HrefSetter : AbstractStringSetter
    {
        public override void Set(ISvgTag tag)
        {
            if (tag is not IHrefSetter setter)
            {
                return;
            }

            setter.Href = Value;
        }

        public override void InitializeValue(string setterValue)
        {
            Value = setterValue;
        }
    }

    /// <summary>
    /// https://www.w3.org/TR/SVG2/pservers.html#LinearGradientAttributes
    /// </summary>
    [SetterGenerator(typeof(string), SvgDefaultValues.Null)]
    public class XHrefSetter : AbstractStringSetter
    {
        public override void Set(ISvgTag tag)
        {
            if (tag is not IXHrefSetter setter)
            {
                return;
            }

            setter.XHref = Value;
        }

        public override void InitializeValue(string setterValue)
        {
            Value = setterValue;
        }
    }
}
