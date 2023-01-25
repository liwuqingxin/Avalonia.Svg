using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nlnet.Avalonia.Svg
{
    public interface ISvgProperty<T> where T : class, ISvgProperty<T>
    {
        public bool CanInherit { get; }
        public object? Value { get; }
    }
}
