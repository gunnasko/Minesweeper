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
        public HighScoreWindow()
        {
            InitializeComponent();

            LoadHighScores();
        }

        private void LoadHighScores()
        {
            var highscoresRepo = new HighScoreRepository();
            HighScores highscores = null;
            using (var fs = new FileStream("TestResources/TestHighscores.xml", FileMode.Open))
            {
                highscores = highscoresRepo.LoadFromStream(fs);
            }

            List<HighScoreEntry> beginnerScores = highscores.GetTopTen(GameDifficulty.Beginner);
            List<HighScoreEntry> intermediateScores = highscores.GetTopTen(GameDifficulty.Intermediate);
            List<HighScoreEntry> expertScores = highscores.GetTopTen(GameDifficulty.Expert);

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
