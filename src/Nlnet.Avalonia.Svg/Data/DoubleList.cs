using System.Collections.Generic;

namespace Nlnet.Avalonia.Svg;

public class DoubleList : List<double>
{
    public DoubleList()
    {
        
    }

    public DoubleList(IEnumerable<double> initDoubles)
    {
        this.AddRange(initDoubles);
    }
}
