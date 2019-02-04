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
            SetupNewBoard();
        }

        private void SetupNewBoard()
        {
            ResetBoardGrid();
            StatusTextBox.Text = "Running...";
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
                    Button pointButton = CreateNewPointButton(point);

                    Grid.SetRow(pointButton, x);
                    Grid.SetColumn(pointButton, y);
                    boardGrid.Children.Add(pointButton);
                }
            }
        }

        private void ResetBoardGrid()
        {
            boardGrid.Children.Clear();
            boardGrid.RowDefinitions.Clear();
            boardGrid.ColumnDefinitions.Clear();
        }

        private Button CreateNewPointButton(Point point)
        {
            Button pointButton = new Button();
            pointButton.Click += PointButton_MouseLeftClick;
            pointButton.MouseRightButtonUp += PointButton_MouseRightButtonUp;
            SetupPointButtonBindings(point, pointButton);

            pointButton.DataContext = point;
            return pointButton;
        }

        private void PointButton_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            var button = (Button)sender;
            var point = button.DataContext as Point;

            bool currentFlag = _board.PointIsFlagged(point.PointCoordinate);
            _board.FlagPoint(point.PointCoordinate, !currentFlag);
        }

        private void PointButton_MouseLeftClick(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var point = button.DataContext as Point;

            BoardActionResult result = _board.OpenPoint(point.PointCoordinate);
            
            if (result.SteppedOnMine)
            {
                GameLost();
            }
            else if (_board.HasWonGame())
            {
                GameWon();
            }
        }

        private void GameLost()
        {
            StatusTextBox.Text = "Lost!";
            FreezeBoardGrid();
        }

        private void GameWon()
        {
            StatusTextBox.Text = "Won!";
            FreezeBoardGrid();
        }


        private void FreezeBoardGrid()
        {
            foreach (var child in boardGrid.Children)
            {
                var button = child as Button;
                if (button != null)
                {
                    button.Click -= PointButton_MouseLeftClick;
                    button.MouseRightButtonUp -= PointButton_MouseRightButtonUp;
                }
            }
        }

        private static void SetupPointButtonBindings(Point point, Button pointButton)
        {
            SetupStyleBinding(point, pointButton);
            SetupContentBinding(point, pointButton);
        }

        private static void SetupContentBinding(Point point, Button pointButton)
        {
			//We add empty binding so that we can access all properties in Point in convert function
            Binding emptyContentBinding = new Binding("");
            emptyContentBinding.Source = point;
 			//We add IsOpened binding so that convert is called everytime IsOpened changes
	        Binding isOpenedContentBinding = new Binding("IsOpened");
	        isOpenedContentBinding.Source = point;

            //We add IsFlagged binding so that convert is called everytime IsFlagged changes
            Binding isFlaggedContentBinding = new Binding("IsFlagged");
            isFlaggedContentBinding.Source = point;

            MultiBinding contentMultiBinding = new MultiBinding();
	        contentMultiBinding.Bindings.Add(emptyContentBinding);
	        contentMultiBinding.Bindings.Add(isOpenedContentBinding);
            contentMultiBinding.Bindings.Add(isFlaggedContentBinding);
	        contentMultiBinding.Converter = new OpenedPointContentConvert();
	        pointButton.SetBinding(Button.ContentProperty, contentMultiBinding);
        }

        private static void SetupStyleBinding(Point point, Button pointButton)
        {
	        //We bind IsOpened to set the style on pointbutton when it is opened.
	        Binding isOpenedStyleBinding = new Binding("IsOpened");
	        isOpenedStyleBinding.Converter = new OpenedPointStyleConvert();
	        isOpenedStyleBinding.Source = point;
	        pointButton.SetBinding(Button.StyleProperty, isOpenedStyleBinding);

        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            SetupNewBoard();
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
            if (point == null)
            {
                return "";
            }

            if (!point.IsOpened)
            {
                if (point.IsFlagged)
                {
                    return "F";
                }
                else
                {
                    return "";
                }
            }
            else
            {
                if (point.HasMine)
                {
                    return "X";
                }
                else if (point.AdjacentMines > 0)
                {
                    return point.AdjacentMines.ToString();
                }
            }
            return "";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


}
