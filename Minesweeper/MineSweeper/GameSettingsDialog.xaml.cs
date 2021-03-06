using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public bool CustomInputReadOnly { get; set; } = true;

        public GameSettingsDialog(GameSettings initialGameSettings)
        {
            InitializeComponent();

            switch(initialGameSettings.BoardGameDifficulty)
            {
                case GameDifficulty.Beginner:
                    beginnerRadioButton.IsChecked = true;
                    break;
                case GameDifficulty.Intermediate:
                    intermediateRadioButton.IsChecked = true;
                    break;
                case GameDifficulty.Expert:
                    expertRadioButton.IsChecked = true;
                    break;
                case GameDifficulty.Custom:
                    customRadioButton.IsChecked = true;
                    break;
            }

            customHeight.Text = initialGameSettings.CustomBoardNumberOfColumns.ToString();
            customWidth.Text = initialGameSettings.CustomBoardNumberOfRows.ToString();
            customMines.Text = initialGameSettings.CustomBoardNumberOfMines.ToString();
        }

        public GameSettings GetDialogGameSettings()
        {
            return new GameSettings(
                int.Parse(customWidth.Text),
                int.Parse(customHeight.Text), 
                int.Parse(customMines.Text), 
                SelectedDifficulty);
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
                if (SelectedDifficulty == GameDifficulty.Custom)
                {
                    CustomInputReadOnly = false;
                }
            }
        }

        private void NumericOnlyPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static readonly Regex _regex = new Regex("[^0-9]+"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

    }

    public class BoolFlipConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException("The target must be a boolean");

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
