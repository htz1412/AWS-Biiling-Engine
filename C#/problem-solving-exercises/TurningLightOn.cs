using System;
using System.Text;

namespace ProblemSolving
{
    class TurningLightOn
    {
        public static int MinFlips(string[] board)
        {
            int minFlips = 0;

            for (int i = board.Length - 1; i >= 0; i--)
            {
                StringBuilder curRow = new StringBuilder(board[i]);
                for (int j = curRow.Length - 1; j >= 0; j--)
                {
                    int cell = Convert.ToInt32(curRow[j]) - 48;
                    if (cell == 0)
                    {
                        Flip(ref board, i, j);
                        curRow = new StringBuilder(board[i]);
                        minFlips++;
                    }
                }
            }
            return minFlips;
        }

        static void Flip(ref string[] board,int row, int col)
        {
            for (int i = row; i >= 0; i--)
            {
                StringBuilder curRow = new StringBuilder(board[i]);
                for (int j = col; j >= 0; j--)
                {
                    int cell = Convert.ToInt32(curRow[j]) - 48;
                    if (cell == 0)
                    {
                        curRow[j] = '1';
                    }
                    else
                    {
                        curRow[j] = '0';
                    }
                }
                board[i] = curRow.ToString();
            }
        }
    }
}
