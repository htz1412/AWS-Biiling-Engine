using LumenWorks.Framework.IO.Csv;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace BillingEngine.Parser
{
   public class RegionCsvParser
   {
      public List<ParsedRegionRecord> Parse(string filePath)
      {
         var csvTable = new DataTable();
         using (var csvReader = new CsvReader(new StreamReader(File.OpenRead(filePath)),true))
         {
            csvTable.Load(csvReader);
         }

         var list = new List<ParsedRegionRecord>();

         for(int i=0; i<csvTable.Rows.Count; i++)
         {
            list.Add(new ParsedRegionRecord
            {
               Region = csvTable.Rows[i][0].ToString().Replace("\"", string.Empty),
               FreeTierEligible = csvTable.Rows[i][1].ToString().Replace("\"", string.Empty)
            });
         }

         return list;
      }
   }
}
