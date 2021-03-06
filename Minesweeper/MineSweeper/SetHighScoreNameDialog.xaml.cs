using System;
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
using System.Windows.Shapes;

namespace Minesweeper
{
    /// <summary>
    /// Interaction logic for SetHighScoreNameDialog.xaml
    /// </summary>
    public partial class SetHighScoreNameDialog : Window
    {
        public SetHighScoreNameDialog(int newScore)
        {
            InitializeComponent();
            scoreLabel.Content = newScore.ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
