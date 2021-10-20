using System;

namespace ProblemSolving
{
    public class LargestSubsequence
    {
        public static string GetLargest(string s)
        {
            string largest = "";
            string cpyString = s;
            
            for (int i=0; i < s.Length; i++)
            {
                int maxPos = 0;

                for(int j=1; j < cpyString.Length; j++)
                {
                    if(cpyString[maxPos] < cpyString[j])
                    {
                        maxPos = j;
                    }
                }

                largest += cpyString[maxPos];
                cpyString = cpyString.Substring(maxPos + 1);

                if(cpyString.Length == 0)
                {
                    break;
                }
            } 

            return largest;
        }
    }
}
