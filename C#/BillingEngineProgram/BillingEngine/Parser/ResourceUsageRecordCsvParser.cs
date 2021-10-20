using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace BillingEngine.Parser
{
   public class ResourceUsageRecordCsvParser
   {
      public List<ParsedResourceUsageRecord> Parse(string filePath,bool IsOnDemand)
      {
         var csvTable = new DataTable();
         using (var csvReader = new CsvReader(new StreamReader(File.OpenRead(filePath)), true))
         {
            csvTable.Load(csvReader);
         }

         var usages = new List<ParsedResourceUsageRecord>();

         for (int i = 0; i < csvTable.Rows.Count; i++)
         {
            usages.Add(new ParsedResourceUsageRecord
            {
               CustomerId = csvTable.Rows[i][1].ToString().Replace("\"", string.Empty),
               InstanceId = csvTable.Rows[i][2].ToString().Replace("\"", string.Empty),
               InstanceType = csvTable.Rows[i][3].ToString().Replace("\"", string.Empty),
               UsedFrom = DateTime.Parse(csvTable.Rows[i][4].ToString().Replace("\"", string.Empty)),
               UsedUntil = DateTime.Parse(csvTable.Rows[i][5].ToString().Replace("\"", string.Empty)),
               Region = csvTable.Rows[i][6].ToString().Replace("\"", string.Empty),
               Os = csvTable.Rows[i][7].ToString().Replace("\"", string.Empty),
               IsOnDemand = IsOnDemand,
            });
         }

         return (from record in usages orderby record.UsedFrom.Year, record.UsedFrom.Month select record).ToList();
      }
   }
}
