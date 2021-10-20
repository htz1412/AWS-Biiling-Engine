using System;

namespace ProblemSolving
{
    public class MovingAvg
    {
        public static double Difference(int k, double[] data)
        {
            double sum;
            double max = double.MinValue;
            double min = double.MaxValue;

            for(int i=0; i<=data.Length-k; i++)
            {
                sum = 0;
                for(int j = i; j < k + i; j++)
                {
                    sum += data[j];
                }

                sum /= k;

                if(max < sum)
                {
                    max = sum;
                }

                if(min > sum)
                {
                    min = sum;
                }
            }

            return max-min;
        }
    }
}
