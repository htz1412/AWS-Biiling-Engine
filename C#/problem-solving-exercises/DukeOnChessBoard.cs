namespace ProblemSolving
{
    class DukeOnChessBoard
    {
        public static string DukePath(int n, string initPosition)
        {
            int[,] board = new int[n,n];
            string result = initPosition;

            int row = n - (initPosition[1] - 48);
            int col = initPosition[0] - 'a';
            board[row, col] = 1;
            int cells = n * n;

            while(true)
            { 
                if(col+1 < n && board[row, col+1] == 0)
                {
                    board[row, ++col] = 1;
                }
                else if(row-1 >= 0 && board[row-1, col] == 0)
                {
                    board[--row, col] = 1;
                }
                else if (row+1 < n && board[row+1, col] == 0)
                {
                    board[++row, col] = 1;
                }
                else if(col-1 >= 0 && board[row, col-1] == 0)
                {
                    board[row, --col] = 1;
                }
                else
                {
                    break;
                }
                result += "-" + (char)(col + 'a') + (n - row).ToString();
            }

            if(result.Length > 40)
            {
                result = result.Substring(0, 20) + "..." + result.Substring(result.Length-20);
            }

            return result;
        }
    }
}
