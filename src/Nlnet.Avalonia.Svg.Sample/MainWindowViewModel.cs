using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
    private SvgFileItem?      _selectedSvg;
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

    public SvgFileItem? SelectedSvg
    {
        get => _selectedSvg;
        set
        {
            if (Equals(value, _selectedSvg))
                return;
            _selectedSvg = value;
            OnPropertyChanged();

            if (_selectedSvg != null)
            {
                EditableSvgData = _selectedSvg.SvgData;
            }
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



    public void OpenInBrowser()
    {
        if (string.IsNullOrEmpty(EditableSvgData))
        {
            return;
        }

        var guid = Guid.NewGuid();
        var file = Path.Combine(Directory.GetCurrentDirectory(), "temp");
        Directory.CreateDirectory(file);
        file = Path.Combine(file, $"{guid}.svg");
        File.WriteAllText(file, EditableSvgData);

        OpenFile(file);
    }

    private static bool OpenFile(string filePath)
    {
        try
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                filePath = filePath.Replace("&", "^&");
                Process.Start(new ProcessStartInfo("cmd", $"/c start {filePath}") { CreateNoWindow = true });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", filePath);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", filePath);
            }
        }
        catch (Exception ex)
        {
            return false;
        }
        return true;
    }



    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
