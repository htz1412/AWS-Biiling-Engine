using System;
using System.Text;

namespace ProblemSolving
{
    public class PalindromeDecoding
    {
        public static string Decode(string code, int[] position, int[] length)
        {
            StringBuilder decodedText = new(code);

            for(int i=0; i<position.Length; i++)
            {
                string tempStr = decodedText.ToString();
                string substr = tempStr.Substring(position[i], length[i]);
                char[] charArr = substr.ToCharArray();
                Array.Reverse(charArr);

                decodedText.Insert(position[i] + length[i], charArr);
            }

            return decodedText.ToString();
        }
    }
}
