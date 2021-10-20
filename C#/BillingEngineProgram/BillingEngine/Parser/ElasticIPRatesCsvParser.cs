using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using BillingEngine.Parser.Models;
using LumenWorks.Framework.IO.Csv;

namespace BillingEngine.Parser
{
   public class ElasticIPRatesCsvParser
   {
      public List<ParsedElasticIPRateRecord> Parse(string filePath)
      {
         var csvTable = new DataTable();
         using (var csvReader = new CsvReader(new StreamReader(File.OpenRead(filePath)), true))
         {
            csvTable.Load(csvReader);
         }

         var elasticIPRateRecords = new List<ParsedElasticIPRateRecord>();

         for (int i = 0; i < csvTable.Rows.Count; i++)
         {
            var col = 0;
            elasticIPRateRecords.Add(new ParsedElasticIPRateRecord
            {
               Region = csvTable.Rows[i][col++].ToString().Replace("\"", string.Empty),
               RatePerHour = Convert.ToDouble(csvTable.Rows[i][col++].ToString().Replace("$", string.Empty).Replace("\"", string.Empty))
            });
         }

         return elasticIPRateRecords;
      }
   }
}
