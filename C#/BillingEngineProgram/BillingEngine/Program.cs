using BillingEngine.Billing;
using BillingEngine.Printer;

namespace BillingEngine
{
   public class Program
   {
      static void Main()
      {
         BillingService billingService = new();
         BillPrinter billPrinter = new();

         var monthlyBills = billingService.GenerateMonthlyBills(
            @"C:\Users\HarshGohel\source\repos\harsh.gohel\C#\BillingEngineProgram\BillingEngine\Csv\Customer.csv",
            @"C:\Users\HarshGohel\source\repos\harsh.gohel\C#\BillingEngineProgram\BillingEngine\Csv\AWSResourceTypes.csv",
            @"C:\Users\HarshGohel\source\repos\harsh.gohel\C#\BillingEngineProgram\BillingEngine\Csv\AWSOnDemandResourceUsage.csv",
            @"C:\Users\HarshGohel\source\repos\harsh.gohel\C#\BillingEngineProgram\BillingEngine\Csv\AWSReservedInstanceUsage.csv",
            @"C:\Users\HarshGohel\source\repos\harsh.gohel\C#\BillingEngineProgram\BillingEngine\Csv\Region.csv",
            @"C:\Users\HarshGohel\source\repos\harsh.gohel\C#\BillingEngineProgram\BillingEngine\Csv\ElasticIPAllocation.csv",
            @"C:\Users\HarshGohel\source\repos\harsh.gohel\C#\BillingEngineProgram\BillingEngine\Csv\ElasticIPAssociation.csv",
            @"C:\Users\HarshGohel\source\repos\harsh.gohel\C#\BillingEngineProgram\BillingEngine\Csv\ElasticIPRates.csv"
            );

         monthlyBills.ForEach(bill =>
           billPrinter.PrintCustomerBill(bill, @"C:\Users\HarshGohel\source\repos\harsh.gohel\C#\BillingEngineProgram\BillingEngine\GeneratedBills\"));
      }
   }
}
