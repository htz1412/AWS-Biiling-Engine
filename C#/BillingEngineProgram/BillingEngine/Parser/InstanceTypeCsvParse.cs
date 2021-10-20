using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using BillingEngine.Parser.Models;

namespace BillingEngine.Parser
{
   public class InstanceTypeCsvParse
   {
      public List<ParsedInstanceType> Parse(string filePath)
      {
         var csvTable = new DataTable();
         using (var csvReader = new CsvReader(new StreamReader(File.OpenRead(filePath)), true))
         {
            csvTable.Load(csvReader);
         }

         var types = new List<ParsedInstanceType>();

         for (int i = 0; i < csvTable.Rows.Count; i++)
         {
            types.Add(new ParsedInstanceType
            {
               InstanceType = csvTable.Rows[i][1].ToString(),
               OnDemandCharge = Convert.ToDouble(csvTable.Rows[i][2].ToString().Replace("$", string.Empty).Replace("\"", string.Empty)),
               ReservedCharge = Convert.ToDouble(csvTable.Rows[i][3].ToString().Replace("$", string.Empty).Replace("\"", string.Empty)),
               Region = csvTable.Rows[i][4].ToString().Replace("\"", string.Empty)
            });

         }
         return types;
      }
   }
}
