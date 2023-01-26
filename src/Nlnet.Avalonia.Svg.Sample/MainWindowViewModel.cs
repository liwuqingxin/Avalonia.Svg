using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Nlnet.Avalonia.Svg.Sample;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private List<string> _svgList = new();

    public List<string> SvgList
    {
        get => _svgList;
        set
        {
            if (Equals(value, _svgList))
                return;
            _svgList = value;
            OnPropertyChanged();
        }
    }



    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
