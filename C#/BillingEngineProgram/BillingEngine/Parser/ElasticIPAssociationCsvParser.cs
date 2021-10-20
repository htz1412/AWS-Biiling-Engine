using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillingEngine.Parser.Models;
using LumenWorks.Framework.IO.Csv;

namespace BillingEngine.Parser
{
   public class ElasticIPAssociationCsvParser
   {
      public List<ParsedElasticIPAssociationRecord> Parse(string filePath)
      {
         var csvTable = new DataTable();
         using (var csvReader = new CsvReader(new StreamReader(File.OpenRead(filePath)), true))
         {
            csvTable.Load(csvReader);
         }

         var elasticIPAssociationRecords = new List<ParsedElasticIPAssociationRecord>();

         for (int i = 0; i < csvTable.Rows.Count; i++)
         {
            var col = 0;
            elasticIPAssociationRecords.Add(new ParsedElasticIPAssociationRecord
            {
               IPAddress = csvTable.Rows[i][col++].ToString().Replace("\"", string.Empty),
               InstanceId = csvTable.Rows[i][col++].ToString().Replace("\"", string.Empty),
               AssociatedFrom = DateTime.Parse(csvTable.Rows[i][col++].ToString().Replace("\"", string.Empty)),
               AssociatedUntil = DateTime.Parse(csvTable.Rows[i][col++].ToString().Replace("\"", string.Empty)),
            });
         }

         return elasticIPAssociationRecords;
      }
   }
}
