using System.IO;
using Avalonia.Controls;
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
        }

        private void OnLoaded(object? sender, RoutedEventArgs e)
        {
            var svgDir    = Path.Combine(Directory.GetCurrentDirectory(), "resources/svg");
            var files     = Directory.GetFiles(svgDir);
            var viewModel = new MainWindowViewModel();
            
            foreach (var file in files)
            {
                var svgData = File.ReadAllText(file);
                viewModel.SvgList.Add(svgData);
            }

            this.DataContext = viewModel;
        }
    }
}
