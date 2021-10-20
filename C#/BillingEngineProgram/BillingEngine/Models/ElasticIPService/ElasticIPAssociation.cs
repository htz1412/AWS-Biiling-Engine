using System.Collections.Generic;

namespace BillingEngine.Models.ElasticIPService
{
   public class ElasticIPAssociation
   {
      public string Ec2InstanceId { get; set; }
      public List<UsageEvent> AssociatedEvents { get; set; }

      public ElasticIPAssociation()
      {
         AssociatedEvents = new();
      }

      public ElasticIPAssociation(string ec2InstanceId, List<UsageEvent> associatedEvents)
      {
         Ec2InstanceId = ec2InstanceId;
         AssociatedEvents = associatedEvents;
      }
   }
}
