using System;
using System.Linq;

namespace ProblemSolving
{
    public class WordCompositionGame
    {
        public static string Score(string[] listA, string[] listB, string[] listC)
        {
            int scoreA = 0;
            int scoreB = 0;
            int scoreC = 0;

            for(int i=0; i < listA.Length; i++)
            {
                string word = listA[i];

                bool isBContaines = listB.Contains(word);
                bool isCContaines = listC.Contains(word);

                if (isBContaines && isCContaines)
                {
                    scoreA += 1;
                }
                else if (isBContaines)
                {
                    scoreA += 2;
                }
                else if (isCContaines)
                {
                    scoreA += 2;
                }
                else
                {
                    scoreA += 3;
                }
            }

            for (int i = 0; i < listB.Length; i++)
            {
                string word = listB[i];

                bool isAContaines = listA.Contains(word);
                bool isCContaines = listC.Contains(word);

                if (isAContaines && isCContaines)
                {
                    scoreB += 1;
                }
                else if (isAContaines)
                {
                    scoreB += 2;
                }
                else if (isCContaines)
                {
                    scoreB += 2;
                }
                else
                {
                    scoreB += 3;
                }
            }

            for (int i = 0; i < listC.Length; i++)
            {
                string word = listC[i];

                bool isAContaines = listA.Contains(word);
                bool isBContaines = listB.Contains(word);

                if (isAContaines && isBContaines)
                {
                    scoreC += 1;
                }
                else if (isAContaines)
                {
                    scoreC += 2;
                }
                else if (isBContaines)
                {
                    scoreC += 2;
                }
                else
                {
                    scoreC += 3;
                }
            }

            return new string(scoreA + "/" + scoreB + "/" + scoreC);
        }
    }
}
