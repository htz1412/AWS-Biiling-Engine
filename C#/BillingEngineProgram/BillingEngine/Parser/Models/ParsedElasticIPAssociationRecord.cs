using System;

namespace BillingEngine.Parser.Models
{
   public class ParsedElasticIPAssociationRecord
   {
      public string IPAddress { get; set; }
      public string InstanceId { get; set; }
      public DateTime AssociatedFrom { get; set; }
      public DateTime AssociatedUntil { get; set; }
   }
}
