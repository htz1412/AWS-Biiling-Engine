using System;

namespace ProblemSolving
{
    public class HuffmanDecoding
    {
        public static string Decode(string archive, string[] dictionary)
        {
            string decodedText = "";
            bool isFound = false;
            string bits = "";

            for (int i = 0; i < archive.Length; i++)
            {
                if (!isFound)
                {
                    bits += Convert.ToString(archive[i]);
                }
                else
                {
                    bits = Convert.ToString(archive[i]);
                    isFound = false;
                }

                for (int k = 0; k < dictionary.Length; k++)
                {
                    if (bits == dictionary[k])
                    {
                        decodedText += (char)(k+65);
                        isFound = true;
                        break;
                    }
                }
            }
            return decodedText;
        }
    }
}
