using System;
using System.Text;

namespace ProblemSolving
{
    public class LexmaxReplace
    {
        public static string Get(String s, String t)
        {
            StringBuilder str = new(s);
            StringBuilder test = new(t);

            for(int i=0; i < str.Length; i++) 
            {
                int max = 0;
                int positionOfMax = 0;
                bool remove = false;

                for(int k=0; k<test.Length; k++)
                {
                    if(str[i] < test[k] && test[k] > max)
                    {
                        max = test[k];
                        positionOfMax = k;
                        remove = true;
                    }
                }

                if (remove)
                {
                    str[i] = test[positionOfMax];
                    test.Remove(positionOfMax, 1);
                }
            }
            return str.ToString();
        }
    }
}
