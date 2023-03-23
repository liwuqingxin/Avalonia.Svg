using Avalonia;
using Avalonia.Controls;

namespace Nlnet.Avalonia.MessageBox
{
    public partial class MainWindow : Window
    {
        public string Message
        {
            get { return GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }
        public static readonly StyledProperty<string> MessageProperty = AvaloniaProperty
            .Register<MainWindow, string>(nameof(Message));

        public string StackTrace
        {
            get { return GetValue(StackTraceProperty); }
            set { SetValue(StackTraceProperty, value); }
        }
        public static readonly StyledProperty<string> StackTraceProperty = AvaloniaProperty
            .Register<MainWindow, string>(nameof(StackTrace));
        


        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += (sender, args) =>
            {
                if (Args == null || Args.Length == 0)
                {
                    return;
                }

                if (Args.Length > 0)
                {
                    this.Message = Args[0];
                }
                if (Args.Length > 1)
                {
                    this.StackTrace = Args[1];
                }
                if (Args.Length > 2)
                {
                    this.Title = Args[2];
                }
            };
        }

        public string[]? Args { get; set; }
    }
}
