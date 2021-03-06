using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for HighScoreWindow.xaml
    /// </summary>
    public partial class HighScoreWindow : Window
    {
        public HighScoreWindow(HighScores highScores)
        {
            InitializeComponent();

            ShowHighScores(highScores);
        }

        private void ShowHighScores(HighScores highScores)
        {
            List<HighScoreEntry> beginnerScores = highScores.GetTopTen(GameDifficulty.Beginner);
            List<HighScoreEntry> intermediateScores = highScores.GetTopTen(GameDifficulty.Intermediate);
            List<HighScoreEntry> expertScores = highScores.GetTopTen(GameDifficulty.Expert);

            beginnerTopTen.HighScoreSource = beginnerScores;
            intermediateTopTen.HighScoreSource = intermediateScores;
            expertTopTen.HighScoreSource = expertScores;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
