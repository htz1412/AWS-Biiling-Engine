using System;
using System.Globalization;
using System.IO;
using BillingEngine.Models.Billing;

namespace BillingEngine.Printer
{
   public class BillPrinter
   {
      public string PrintCustomerBill(MonthlyBill monthlyBill, string pathToOutputDir)
      {
         //var filepath = @"C:\Users\HarshGohel\source\repos\harsh.gohel\C#\BillingEngineProgram\BillingEngine\GeneratedBills\";
         //filepath += monthlyBill.CustomerId + "_" + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthlyBill.MonthYear.Month).Substring(0, 3).ToUpper() + "-" + monthlyBill.MonthYear.Year + ".csv";

         //if (File.Exists(filepath))
         //{
         //   File.Delete(filepath);
         //}
         //using StreamWriter writer = new(new FileStream(filepath,
         //FileMode.Create, FileAccess.Write));

         var aggregatedMonthlyEc2Usages = monthlyBill.GetAggregatedMonthlyEc2Usages();

         string billFormat = "";
         billFormat += monthlyBill.CustomerName + "\n" +
            "Bill for month of : " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthlyBill.MonthYear.Month).Substring(0, 3).ToUpper() + " " + monthlyBill.MonthYear.Year + "\n" +
            "Total Amount : $" + Math.Round(monthlyBill.GetTotalAmount(aggregatedMonthlyEc2Usages), 4) + "\n" +
            "Total Discount : $" + Math.Round(monthlyBill.GetTotalDiscount(aggregatedMonthlyEc2Usages), 4) + "\n" +
            "Actual Amount : $" + Math.Round(monthlyBill.GetAmountToBePaid(aggregatedMonthlyEc2Usages), 4) + "\n" +
            "Region | Resource Type | Total Resources | Total Used Time (HH:mm:ss) | Total Billed Time (HH:mm:ss) | Total Amount | Discount | Actual Amount\n";

         aggregatedMonthlyEc2Usages.ForEach(record => billFormat += PrintBillItem(record));

         //writer.WriteLine(billFormat);
         Console.WriteLine(billFormat);
         return null;
      }

      private string PrintBillItem(AggregatedMonthlyEc2Usage aggregatedMonthlyEc2Usage)
      {
         var billFormat = aggregatedMonthlyEc2Usage.Region + "   " +
            aggregatedMonthlyEc2Usage.ResourceType + "   " +
            aggregatedMonthlyEc2Usage.TotalResources + "   " +
            (aggregatedMonthlyEc2Usage.TotalUsedTime.Days * 24 + aggregatedMonthlyEc2Usage.TotalUsedTime.Hours) + ":" + aggregatedMonthlyEc2Usage.TotalUsedTime.Minutes + ":" + aggregatedMonthlyEc2Usage.TotalUsedTime.Seconds + "   " +
            (aggregatedMonthlyEc2Usage.TotalBilledTime.Days * 24 + aggregatedMonthlyEc2Usage.TotalBilledTime.Hours) + ":00:00   " +
            "$" + Math.Round(aggregatedMonthlyEc2Usage.TotalAmount, 4) + "   " +
            "$" + Math.Round(aggregatedMonthlyEc2Usage.TotalDiscount, 4) + "   " +
            "$" + Math.Round(aggregatedMonthlyEc2Usage.GetActualAmount(), 4) + "\n";

         return billFormat;
      }
   }
}
