using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Avalonia;
using Avalonia.Platform.Storage;
using Avalonia.Platform.Storage.FileIO;

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
    private ObservableCollection<SvgFileItem> _svgList = new();
    private SvgFileItem? _selectedSvg;
    private string? _editableSvgData;
    private Thread? _thClipboardMonitor;
    private volatile bool _stopMonitorClipboard = true;
    private string? _lastCopy;
    private string? _saveInitDirectory;
    private string? _importInitDirectory;

    public MainWindowViewModel()
    {
        _mainWindow = new MainWindow();
    }

    public MainWindowViewModel(MainWindow mainWindow)
    {
        _mainWindow = mainWindow;
    }

    public ObservableCollection<SvgFileItem> SvgList
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

    public void Format()
    {
        try
        {
            var code = EditableSvgData;
            if (code == null)
            {
                return;
            }

            var xDocument  = LoadWithoutNamespace(code);

            var       builder = new StringBuilder();
            using var writer  = GetXmlWriter(builder, false);
            xDocument.WriteTo(writer);
            writer.Flush();

            EditableSvgData = builder.ToString();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public void FormatWithNewLine()
    {
        try
        {
            var code = EditableSvgData;
            if (code == null)
            {
                return;
            }

            var xDocument = LoadWithoutNamespace(code);

            var       builder = new StringBuilder();
            using var writer  = GetXmlWriter(builder, true);
            xDocument.WriteTo(writer);
            writer.Flush();

            EditableSvgData = builder.ToString();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private static XmlWriter GetXmlWriter(StringBuilder builder, bool newLineOnAttributes)
    {
        var writer = XmlWriter.Create(builder, new XmlWriterSettings()
        {
            DoNotEscapeUriAttributes = true,
            NewLineOnAttributes      = newLineOnAttributes,
            OmitXmlDeclaration       = true,
            Indent                   = true,
            IndentChars              = "    ",
            NamespaceHandling        = NamespaceHandling.OmitDuplicates,
            NewLineHandling          = NewLineHandling.Replace,

            // Token StartElement in state EndRootElement would result in an invalid XML document.
            // Make sure that the ConformanceLevel setting is set to ConformanceLevel.Fragment or
            // ConformanceLevel.Auto if you want to write an XML fragment.
            ConformanceLevel = ConformanceLevel.Auto,
        });

        return writer;
    }

    private static XDocument LoadWithoutNamespace(string xaml)
    {
        var xDocument = XDocument.Load(new StringReader(xaml));

        return xDocument;

        foreach (var xe in xDocument.Elements().DescendantsAndSelf())
        {
            // Stripping the namespace by setting the name of the element to it's local name only
            xe.Name = xe.Name.LocalName;
            // replacing all attributes with attributes that are not namespaces and their names are set to only the local name
            xe.ReplaceAttributes(
                xe.Attributes()
                    .Where(xa => !xa.IsNamespaceDeclaration)
                    .Select(xa => new XAttribute(xa.Name.LocalName, xa.Value))
            );
        }

        return xDocument;
    }

    public async Task Import()
    {
        const string applicationCache = "./tmp/.importFile.startLocation.cfg";
        if (_importInitDirectory == null && File.Exists(applicationCache))
        {
            _importInitDirectory = await File.ReadAllTextAsync(applicationCache);
        }

        var startFolder = await _mainWindow.StorageProvider.TryGetFolderFromPathAsync(_importInitDirectory ?? "./");
        var storage = await _mainWindow.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Import files",
            AllowMultiple = true,
            FileTypeFilter =new FilePickerFileType[]
            {
                new FilePickerFileType("Svg File")
                {
                    Patterns = (IReadOnlyList<string>) new string[]
                    {
                        "*.svg",
                    },
                }
            },
            SuggestedStartLocation = startFolder,
        });

        var dir = $"./resources/import";
        Directory.CreateDirectory(dir);

        foreach (var file in storage)
        {
            _importInitDirectory = Path.GetDirectoryName(file.Path.AbsolutePath);

            await using var stream = await file.OpenReadAsync();
            var reader = new StreamReader(stream);
            var text = await reader.ReadToEndAsync();
            var path = $"{dir}/{file.Name}";
            await File.WriteAllTextAsync(path, text);

            SvgList.Add(new SvgFileItem(file.Name, text));
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
            const string applicationCache = "./tmp/.saveFile.startLocation.cfg";
            if (_saveInitDirectory == null && File.Exists(applicationCache))
            {
                _saveInitDirectory = await File.ReadAllTextAsync(applicationCache);
            }

            var svg         = EditableSvgData;
            var startFolder = await _mainWindow.StorageProvider.TryGetFolderFromPathAsync(_saveInitDirectory ?? "./");
            var storage = await _mainWindow.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions()
            {
                Title                  = "Áí´æÎª",
                DefaultExtension       = "svg",
                ShowOverwritePrompt    = true,
                SuggestedFileName      = SelectedSvg?.SvgFileName ?? "untitled",
                SuggestedStartLocation = startFolder,
            });

            if (storage != null)
            {
                await using var stream = await storage.OpenWriteAsync();
                stream.Write(Encoding.UTF8.GetBytes(svg));
                await stream.FlushAsync();

                _saveInitDirectory = Path.GetDirectoryName(storage.Path.AbsolutePath);
                Directory.CreateDirectory("./tmp");
                await File.WriteAllTextAsync(applicationCache, _saveInitDirectory);
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
            var clipboard = _mainWindow.Clipboard;
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

    public void ViewKnownIssues()
    {
        OpenFile("https://github.com/liwuqingxin/Avalonia.Svg/wiki");
    }



    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
