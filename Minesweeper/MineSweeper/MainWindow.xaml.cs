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

                    //We bind IsOpened to set the style on pointbutton when it is opened.
                    Binding isOpenedStyleBinding = new Binding("IsOpened");
                    isOpenedStyleBinding.Converter = new OpenedPointStyleConvert();
                    isOpenedStyleBinding.Source = point;
                    pointButton.SetBinding(Button.StyleProperty, isOpenedStyleBinding);


                    //We add empty binding so that we can access all properties in Point in convert function
                    Binding emptyContentBinding = new Binding("");
                    emptyContentBinding.Source = point;

                    //We add IsOpened binding so that convert is called everytime IsOpened changes
                    Binding isOpenedContentBinding = new Binding("IsOpened");
                    isOpenedContentBinding.Source = point;

                    MultiBinding contentMultiBinding = new MultiBinding();

                    contentMultiBinding.Bindings.Add(emptyContentBinding);
                    contentMultiBinding.Bindings.Add(isOpenedContentBinding);
                    contentMultiBinding.Converter = new OpenedPointContentConvert();
                    pointButton.SetBinding(Button.ContentProperty, contentMultiBinding);

                    //We save point data so we can access it in callbacks from button click.
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
                _board.OpenPoint(point.PointCoordinate);
            }
            catch (ArgumentException)
            {
                //Do nothing
            }
        }
    }

    public class OpenedPointStyleConvert : IValueConverter
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

    public class OpenedPointContentConvert : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var query = (from v in values where v.GetType() == typeof(Point) select v);
            var point = query.First() as Point;
            if (point == null || !point.IsOpened)
            {
                return "";
            }

            if (point.HasMine)
            {
                return "X";
            }
            else if (point.AdjacentMines > 0)
            {
                return point.AdjacentMines.ToString();
            }
            return "";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


}
