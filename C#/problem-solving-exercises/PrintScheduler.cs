using System;

namespace ProblemSolving
{
    class PrintScheduler
    {
        public static string GetOutput(string[] threads, string[] slices)
        {
            string result = "";
            int[] threadState = new int[threads.Length];
            for(int i=0; i<slices.Length; i++)
            {
                string[] currentSlice = slices[i].Split(" ");
                int threadIndex = Convert.ToInt32(currentSlice[0]);
                int n = Convert.ToInt32(currentSlice[1]);
                int k = threadState[threadIndex];

                for (int j = 0; j < n; j++)
                {
                    result += threads[threadIndex][k++];
                    if (k == threads[threadIndex].Length)
                    {
                        k = 0;
                    }
                }
                threadState[threadIndex] = k;
            }
            return result;
        }
    }
}
