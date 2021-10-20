using LumenWorks.Framework.IO.Csv;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace BillingEngine.Parser
{
   public class CustomerCsvParser
   {
      public List<ParsedCustomerRecord> Parse(string filePath)
      {
         var csvTable = new DataTable();
         using (var csvReader = new CsvReader(new StreamReader(File.OpenRead(filePath)), true))
         {
            csvTable.Load(csvReader);
         }

         var customers = new List<ParsedCustomerRecord>();

         for (int i = 0; i < csvTable.Rows.Count; i++)
         {
            customers.Add(new ParsedCustomerRecord
            {
               CustomerId = csvTable.Rows[i][1].ToString().Replace("-", string.Empty).Replace("\"", string.Empty),
               CustomerName = csvTable.Rows[i][2].ToString().Replace("\"", string.Empty)
            });
         }

         return customers;
      }
   }
}
