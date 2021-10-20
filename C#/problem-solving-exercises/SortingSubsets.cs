using System;
using System.Collections.Generic;

namespace ProblemSolving
{
    public class SortingSubsets
    {
        public static int GetMinimalSize(int[] a)
        {
            var newArr = new List<int>(a);
            Array.Sort(a);

            int counter = 0;
            for(int i=0; i<a.Length; i++)
            {
                if(a[i] != newArr[i])
                {
                    counter++;
                }
            }

            return counter;
        }
    }
}
