using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using BillingEngine.Parser.Models;
using LumenWorks.Framework.IO.Csv;

namespace BillingEngine.Parser
{
   public class ElasticIPAllocationCsvParser
   {
      public List<ParsedElasticIpAllocationRecord> Parse(string filePath)
      {
         var csvTable = new DataTable();
         using (var csvReader = new CsvReader(new StreamReader(File.OpenRead(filePath)), true))
         {
            csvTable.Load(csvReader);
         }

         var elasticIPAllocationRecords = new List<ParsedElasticIpAllocationRecord>();

         for (int i = 0; i < csvTable.Rows.Count; i++)
         {
            var col = 0;
            elasticIPAllocationRecords.Add(new ParsedElasticIpAllocationRecord
            {
               CustomerId = csvTable.Rows[i][col++].ToString().Replace("\"", string.Empty),
               Region = csvTable.Rows[i][col++].ToString().Replace("\"", string.Empty),
               IPAddress = csvTable.Rows[i][col++].ToString().Replace("\"", string.Empty),
               UsedFrom = DateTime.Parse(csvTable.Rows[i][col++].ToString().Replace("\"", string.Empty)),
               UsedUntil = DateTime.Parse(csvTable.Rows[i][col++].ToString().Replace("\"", string.Empty)),
               IsYourOwnIP = csvTable.Rows[i][col++].ToString().Replace("\"", string.Empty).Equals("Yes")
            });
         }

         return elasticIPAllocationRecords;
      }
   }
}
