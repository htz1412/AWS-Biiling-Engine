namespace ProblemSolving
{
    class Mailbox
    {
        public static int Impossible(string collection, string[] address)
        {
            string cpyString = collection;
            int impossibles = 0;
            int[] data = new int[36];

            for(int i=0; i<collection.Length; i++)
            {
                if(48 <= collection[i] && collection[i] <= 57)
                {
                    data[collection[i] - 22]++;
                }
                else
                {
                    data[collection[i] - 65]++;
                }
            }

            for(int i=0; i<address.Length; i++)
            {
                int[] d = new int[36];
                for(int j=0; j<address[i].Length; j++)
                {
                    int reduceNum = 0;

                    if(address[i][j] == ' ')
                    {
                        continue;
                    }

                    if (48 <= address[i][j] && address[i][j] <= 57)
                    {
                        reduceNum = 22;
                    }
                    else
                    {
                        reduceNum = 65;
                    }

                    if (d[address[i][j] - reduceNum] < data[address[i][j] - reduceNum])
                    {
                        d[address[i][j] - reduceNum]++;
                    }
                    else
                    {
                        impossibles ++;
                        break;
                    }
                }
            }

            return impossibles;
        }
    }
}
