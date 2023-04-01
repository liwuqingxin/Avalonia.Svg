using System;
using System.Collections.Generic;
using Avalonia.Media;

namespace Nlnet.Avalonia.Svg.Utils
{
    internal class StateStack : Stack<DrawingContext.PushedState>, IDisposable
    {
        public void Dispose()
        {
            while (this.Count > 0)
            {
                var disposable = this.Pop();
                disposable.Dispose();
            }
        }
    }
}
