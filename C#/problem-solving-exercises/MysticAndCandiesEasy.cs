using System;

namespace ProblemSolving
{
    class MysticAndCandiesEasy
    {
        public static int MinBoxes(int C, int X, int[] high)
        {
            int minBoxes = 0;
            int sum = 0;
            Array.Sort(high);

            for (int i = 0; i < high.Length; i++)
            {
                sum += high[i];
            }

            int candiesToEat = 0;

            for(int i= high.Length-1 ; i >=0; i--)
            {
                sum -= high[i];
                minBoxes++;

                if(sum < C)
                {
                    candiesToEat = C - sum;
                }

                if(candiesToEat >= X)
                {
                    break;
                }
            }
            return minBoxes;
        }
    }
}
