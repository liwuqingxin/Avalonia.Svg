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
            var svgDir = Path.Combine(Directory.GetCurrentDirectory(), "resources/svg");
            var files = Directory.GetFiles(svgDir);
            var viewModel = new MainWindowViewModel();
            
            foreach (var file in files)
            {
                var svgData = File.ReadAllText(file);
                viewModel.SvgList.Add(new SvgFileItem(Path.GetFileName(file), svgData));
            }

            this.DataContext = viewModel;
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);
        }
    }
}
