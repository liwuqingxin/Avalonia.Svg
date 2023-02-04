using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Nlnet.Avalonia.Svg.Sample;

public class SvgFileItem
{
    public SvgFileItem(string svgFileName, string svgData)
    {
        SvgFileName = svgFileName;
        SvgData     = svgData;
    }

    public string SvgFileName { get; set; }
    public string SvgData     { get; set; }
}

public sealed class MainWindowViewModel : INotifyPropertyChanged
{
    private List<SvgFileItem> _svgList = new();
    private string?           _editableSvgData;

    public List<SvgFileItem> SvgList
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
