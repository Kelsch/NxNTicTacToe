using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace NxNTicTacToe
{
    /// <summary>
    /// Interaction logic for TicTacToe.xaml
    /// </summary>
    public partial class TicTacToe : Page
    {
        public int playerTurn = 1;
        public int dimension;

        public IDictionary<(int, int), string> dictionaryBoard { get; set; }

        /// <summary>
        /// Created a Tic Tac Tow game board
        /// </summary>
        /// <param name="n">nxn dimension for the game board</param>
        public TicTacToe(int n)
        {
            InitializeComponent();
            dimension = n;

            int numberOfButtons = n * n;

            dictionaryBoard = new Dictionary<(int, int), string>();

            for (int i = 1; i < numberOfButtons + 1; i++)
            {
                Button newButton = new();
                newButton.Name = "btn" + i.ToString();
                newButton.Margin = new Thickness(5);
                newButton.FontSize = 36;

                int rowNumber = 1 + ((i - 1) / dimension);
                int columnNumber = 1 + (i - 1) % dimension;

                newButton.Tag = $"{rowNumber}:{columnNumber}";

                dictionaryBoard.Add(new KeyValuePair<(int, int), string>((rowNumber, columnNumber), ""));

                newButton.Click += NewButton_Click;

                uniformGrid.Children.Add(newButton);
            }
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            button.Content = playerTurn == 1 ? "X" : "O";
            button.IsEnabled = false;

            int rowNumber = Convert.ToInt32(button.Tag.ToString().Split(":")[0]);
            int columnNumber = Convert.ToInt32(button.Tag.ToString().Split(":")[1]);

            int winningPlayer = PlacePiece(rowNumber, columnNumber, playerTurn);

            if (winningPlayer != 0)
            {
                uniformGrid.IsEnabled = false;

                MessageBox.Show($"Player {winningPlayer} wins!");
            }
        }

        /// <summary>
        /// Place a piece on the game board
        /// </summary>
        /// <param name="row">row to place a piece</param>
        /// <param name="col">column to place a piece</param>
        /// <param name="player">the player (1 or 2) the piece is for</param>
        /// <returns>0 = no winner, 1 = player 1 won, 2 = player 2 won</returns>
        public int PlacePiece(int row, int col, int player)
        {
            dictionaryBoard[(row, col)] = player == 1 ? "X" : "O";

            int winningPlayer = CheckForWinner();

            //Change the player turn
            playerTurn = player == 1 ? 2 : 1;

            return winningPlayer;
        }

        private int CheckForWinner()
        {
            int winningPlayer = 0;
            string winningValue = playerTurn == 1 ? "X" : "O";

            int n = Convert.ToInt32(Math.Sqrt(dictionaryBoard.Count));

            winningPlayer = RowCheck(n, winningValue);
            if (winningPlayer != 0)
            {
                return winningPlayer;
            }

            winningPlayer = ColumnCheck(n, winningValue);
            if (winningPlayer != 0)
            {
                return winningPlayer;
            }

            winningPlayer = DiagnalLeftToRightCheck(n, winningValue);
            if (winningPlayer != 0)
            {
                return winningPlayer;
            }

            winningPlayer = DiagnalRightToLeftCheck(n, winningValue);

            return winningPlayer;
        }

        private int RowCheck(int n, string winningValue)
        {
            int winningPlayer = 0;
            for(int r = 1; r <= n; r++)
            {
                int columnsInRow = 0;
                for (int c = 1; c <= n; c++)
                {
                    if (dictionaryBoard[(r, c)] == winningValue)
                    {
                        columnsInRow += 1;
                    }

                    // return if a player has all the rows in that column
                    if (columnsInRow == n)
                    {
                        winningPlayer = playerTurn;
                        return winningPlayer;
                    }
                }
            }

            return winningPlayer;
        }

        private int ColumnCheck(int n, string winningValue)
        {
            int winningPlayer = 0;
            for (int c = 1; c <= n; c++)
            {
                int rowsInColumn = 0;
                for (int r = 1; r <= n; r++)
                {
                    if (dictionaryBoard[(r, c)] == winningValue)
                    {
                        rowsInColumn += 1;
                    }

                    // return if a player has all the columns in that row
                    if (rowsInColumn == n)
                    {
                        winningPlayer = playerTurn;
                        return winningPlayer;
                    }
                }
            }

            return winningPlayer;
        }

        private int DiagnalLeftToRightCheck(int n, string winningValue)
        {
            int winningPlayer = 0;
            int diagnalLeftToRightCount = 0;
            for (int i = 1; i <= n; i++)
            {
                if (dictionaryBoard[(i, i)] == winningValue)
                {
                    diagnalLeftToRightCount += 1;
                }

                // return if a player has all the rows in that column
                if (diagnalLeftToRightCount == n)
                {
                    winningPlayer = playerTurn;
                    return winningPlayer;
                }
            }

            return winningPlayer;
        }

        private int DiagnalRightToLeftCheck(int n, string winningValue)
        {
            int winningPlayer = 0;
            int diagnalRightToLeftCount = 0;
            for (int i = 1; i <= n; i++)
            {
                if (dictionaryBoard[((n + 1) - i, i)] == winningValue)
                {
                    diagnalRightToLeftCount += 1;
                }

                // return if a player has all the rows in that column
                if (diagnalRightToLeftCount == n)
                {
                    winningPlayer = playerTurn;
                    return winningPlayer;
                }
            }

            return winningPlayer;
        }
    }
}
