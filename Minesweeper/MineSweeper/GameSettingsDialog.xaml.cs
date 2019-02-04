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
    /// Interaction logic for GameSettingsDialog.xaml
    /// </summary>
    public partial class GameSettingsDialog : Window
    {
        public GameDifficulty SelectedDifficulty { get; set; }
        public GameSettingsDialog()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void DifficultyRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null)
            {
                SelectedDifficulty = (GameDifficulty)Enum.Parse(typeof(GameDifficulty), rb.Content.ToString());
            }
        }
    }

}
