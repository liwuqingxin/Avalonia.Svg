using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input.Platform;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Platform.Storage.FileIO;
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
    private readonly MainWindow _mainWindow;
    private List<SvgFileItem> _svgList = new();
    private SvgFileItem? _selectedSvg;
    private string? _editableSvgData;
    private Thread? _thClipboardMonitor;
    private volatile bool _stopMonitorClipboard = true;
    private string? _lastCopy;

    public MainWindowViewModel()
    {
        
    }

    public MainWindowViewModel(MainWindow mainWindow)
    {
        _mainWindow = mainWindow;
    }

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

    public async Task Save()
    {
        try
        {
            if (string.IsNullOrEmpty(EditableSvgData))
            {
                return;
            }

            var svg = EditableSvgData;

            var storage = await _mainWindow.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions()
            {
                Title                  = "Áí´æÎª",
                DefaultExtension       = "svg",
                ShowOverwritePrompt    = true,
                SuggestedFileName      = SelectedSvg?.SvgFileName ?? "untitled",
                SuggestedStartLocation = new BclStorageFolder("./"),
            });

            if (storage != null)
            {
                await using var stream = await storage.OpenWriteAsync();
                stream.Write(Encoding.UTF8.GetBytes(svg));
                await stream.FlushAsync();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public void MonitorClipboard()
    {
        if (_stopMonitorClipboard)
        {
            _stopMonitorClipboard = false;
            _thClipboardMonitor = new Thread(ClipboardMonitorWorker)
            {
                IsBackground = true,
            };

            _thClipboardMonitor.Start();
        }
        else
        {
            if (_thClipboardMonitor != null)
            {
                _stopMonitorClipboard = true;
                _thClipboardMonitor   = null;
            }
        }
    }

    private async void ClipboardMonitorWorker(object? obj)
    {
        try
        {
            var clipboard = Application.Current?.Clipboard;
            if (clipboard == null)
            {
                return;
            }

            while (_stopMonitorClipboard == false)
            {

                try
                {
                    var text = await clipboard.GetTextAsync();
                    if (_lastCopy == text)
                    {
                        continue;
                    }
                    new XmlDocument().LoadXml(text);
                    _lastCopy       = text;
                    EditableSvgData = text;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                Thread.Sleep(100);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public void ViewGithub()
    {
        OpenFile("https://github.com/liwuqingxin/Avalonia.Svg");
    }

    public void BugReport()
    {
        OpenFile("https://github.com/liwuqingxin/Avalonia.Svg/issues/new?assignees=liwuqingxin&labels=bug&template=bug_report.md&title=%5BBUG%5D");
    }



    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
