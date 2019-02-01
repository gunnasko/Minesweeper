using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Minesweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Board _board;
        public MainWindow()
        {
            InitializeComponent();

            //Currently use default values
            _board = new Board();
            for (int x = 0; x < _board.RowSize; x++)
            {
                boardGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (int y = 0; y < _board.ColumnSize; y++)
            {
                boardGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int x = 0; x < _board.RowSize; x++)
            {
                for (int y = 0; y < _board.ColumnSize; y++)
                {
                    Point point = _board.AccessPoint(new Coordinate(x, y));
                    Button pointButton = new Button();
                    pointButton.Click += OnPointButtonClicked;

                    Binding pointBinding = new Binding("IsOpened");
                    pointBinding.Converter = new OpenedPointConvert();
                    pointBinding.Source = point;
                    pointButton.SetBinding(Button.StyleProperty, pointBinding);


                    pointButton.DataContext = point;

                    Grid.SetRow(pointButton, x);
                    Grid.SetColumn(pointButton, y);
                    boardGrid.Children.Add(pointButton);
                }
            }
        }

        public void OnPointButtonClicked(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var point = button.DataContext as Point;
            try
            {
                BoardActionResult result = _board.OpenPoint(point.PointCoordinate);

                if (result.SteppedOnMine)
                {
                    button.Content = "X";
                }
                else if (result.AdjacentMines > 0)
                {
                    button.Content = result.AdjacentMines.ToString();
                }
            }
            catch (ArgumentException)
            {
                //Do nothing
            }
        }
    }

    public class OpenedPointConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? (Style)Application.Current.FindResource("openedStyle") : (Style)Application.Current.FindResource("closedStyle");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
