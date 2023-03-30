using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Path = System.IO.Path;

namespace Nlnet.Avalonia.Svg.Sample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += OnLoaded;

            DragDrop.DropEvent.AddClassHandler<MainWindow>((window, args) =>
            {
                var files = args.Data.GetFileNames();
                if (files == null)
                {
                    return;
                }

                foreach (var file in files)
                {
                    if (Path.GetExtension(file).ToUpper() != ".SVG")
                    {
                        return;
                    }

                    var data = File.ReadAllText(file);
                    if (window.DataContext is MainWindowViewModel vm)
                    {
                        vm.EditableSvgData = data;
                    }

                    return;
                }
            });
        }

        private void OnLoaded(object? sender, RoutedEventArgs e)
        {
            var viewModel = new MainWindowViewModel(this);

            var svgDir = Path.Combine(Directory.GetCurrentDirectory(), "resources/svg");
            foreach (var fileItem in LoadFilesInFolder(svgDir))
            {
                viewModel.SvgList.Add(fileItem);
            }
            var svgDir2 = Path.Combine(Directory.GetCurrentDirectory(), "resources/import");
            foreach (var fileItem in LoadFilesInFolder(svgDir2))
            {
                viewModel.SvgList.Add(fileItem);
            }

            this.DataContext = viewModel;
        }

        private static IEnumerable<SvgFileItem> LoadFilesInFolder(string folder)
        {
            var files = Directory.GetFiles(folder);

            foreach (var file in files)
            {
                var svgData = File.ReadAllText(file);
                yield return new SvgFileItem(Path.GetFileName(file), svgData);
            }
        }

        private void EditorVisible_OnClick(object? sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                if (checkBox.IsChecked == true)
                {
                    IconPanel2.Children.Remove(IconComponent);
                    IconPanel1.Children.Add(IconComponent);
                }
                else
                {
                    IconPanel1.Children.Remove(IconComponent);
                    IconPanel2.Children.Add(IconComponent);
                }
            }
        }
    }
}
