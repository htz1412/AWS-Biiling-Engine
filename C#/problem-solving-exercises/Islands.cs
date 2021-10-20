using System;

namespace ProblemSolving
{
    class Islands
    {
        public static int BeachLength(string[] kingdom)
        {
            int beachLength = 0;
            int row = kingdom.Length;
            int col = kingdom[0].Length;

            for(int i=0; i < row; i++)
            {
                for(int j=0; j < col; j++)
                {
                    if(j > 0 && kingdom[i][j] != kingdom[i][j - 1])
                    {
                        beachLength++;
                    }

                    if (i > 0 && kingdom[i - 1][j] != kingdom[i][j])
                    {
                        beachLength++;
                    }

                    int c;
                    if (i % 2 == 0)
                    {
                        c = j - 1;
                    }
                    else
                    {
                        c = j + 1;

                    }

                    if (i > 0 && c >= 0 && c < col && kingdom[i - 1][c] != kingdom[i][j])
                    {
                        beachLength++;
                    }
                }
            }

            return beachLength;
        }
    }
}
