using System;

namespace ProblemSolving
{
    public class MaximumBalances
    {
        public static int Solve(string s)
        {
            int openBrackets = s.Split('(').Length - 1;
            int closeBrackets = s.Split(')').Length - 1; 

            int pairs = Math.Min(openBrackets,closeBrackets);

            if(pairs == 0)
            {
                return 0;
            }
            else if(pairs == 1)
            {
                return 1;
            }
            else
            {
                int combos = pairs + ((pairs * (pairs - 1)) / 2);
                return combos;
            }
        }
    }
}
