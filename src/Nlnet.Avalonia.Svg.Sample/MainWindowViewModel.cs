using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Nlnet.Avalonia.Svg.Sample;

public sealed class MainWindowViewModel : INotifyPropertyChanged
{
    private List<string> _svgList = new();
    private string?      _editableSvgData;

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

    public string? EditableSvgData
    {
        get => _editableSvgData;
        set
        {
            if (value == _editableSvgData)
                return;
            _editableSvgData = value;
            OnPropertyChanged();
        }
    }



    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
