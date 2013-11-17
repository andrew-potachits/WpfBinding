using System.Windows;
using System.Windows.Controls;

namespace WpfBinding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly RebarSet _rebarSet = new RebarSet();
        public RebarSet Model { get { return _rebarSet; } }
        private Rect _bounds = new Rect(10, 0, 300, 300);
        public MainWindow()
        {
            InitializeComponent();
            Model.Generate(_bounds, 5);
        }
    
        private void OnClearClick(object sender, RoutedEventArgs e)
        {
            Model.Clear();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = this;
        }

        private void CreateRebarSet(object sender, RoutedEventArgs e)
        {
            int count;
            if (int.TryParse(RebarCount.Text, out count))
            {
                Model.Generate(_bounds, count);
            }
        }
    }
}
