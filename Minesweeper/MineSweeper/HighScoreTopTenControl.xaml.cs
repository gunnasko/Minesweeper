using System;
using System.Collections;
using System.Collections.Generic;
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
    /// Interaction logic for HighScoreUserControl.xaml
    /// </summary>
    public partial class HighScoreTopTenControl : UserControl
    {
        public HighScoreTopTenControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty HighScoreSourceProperty = DependencyProperty.Register("HighScoreSource", typeof(IEnumerable), typeof(HighScoreTopTenControl));

        public IEnumerable HighScoreSource
        {
            get { return (IEnumerable)GetValue(HighScoreSourceProperty); }
            set { SetValue(HighScoreSourceProperty, value); }
        }

    }
}
