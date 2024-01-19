using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain;
using WPFInvoiceSystem.Domain.Modals;

namespace WPFInvoiceSystem.Libraries
{
    public class ReportsGenerator : IReportsGenerator
    {
        public void GenerateInvoicesReport(IList<Invoice> invoices) 
        {
            using var workbook = new XLWorkbook();

            var filePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "invoice-report.xlsx");

            var sheet = workbook.Worksheets.Add("Invoice report");

            //Title
            var titleCell = sheet.Cell("B2");

            titleCell.Value = "Invoice Report";
            titleCell.Style.Font.FontSize = 16;
            titleCell.Style.Font.FontColor = XLColor.BlueViolet;
            titleCell.Style.Font.SetBold();

            //Table header
            sheet.Cell("B4").Value = "Invoice Number";
            sheet.Cell("C4").Value = "Date";
            sheet.Cell("D4").Value = "Identity Card";
            sheet.Cell("E4").Value = "Customer";
            sheet.Cell("F4").Value = "Exempt";
            sheet.Cell("G4").Value = "Tax Base";
            sheet.Cell("H4").Value = "Tax";
            sheet.Cell("I4").Value = "Total";

            sheet.Range("B4:I4").Style
                .Font.SetBold()
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            //Table rows
            var tableInitialRow = 5;

            for (var i = 0; i < invoices.Count; i++)
            {
                var invoiceNumberCell = sheet.Cell($"B{tableInitialRow + i}");
                var dateCell = sheet.Cell($"C{tableInitialRow + i}");
                var identityCardCell = sheet.Cell($"D{tableInitialRow + i}");
                var customerCell = sheet.Cell($"E{tableInitialRow + i}");
                var exemptCell = sheet.Cell($"F{tableInitialRow + i}");
                var taxBaseCell = sheet.Cell($"G{tableInitialRow + i}");
                var taxCell = sheet.Cell($"H{tableInitialRow + i}");
                var totalCell = sheet.Cell($"I{tableInitialRow + i}");

                invoiceNumberCell.Value = invoices[i].InvoiceNumber;
                invoiceNumberCell.Style
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .NumberFormat.NumberFormatId = (int)XLPredefinedFormat.Number.IntegerWithSeparator;

                dateCell.Value = invoices[i].Date.ToString("d");
                dateCell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                identityCardCell.Value = invoices[i].Customer.IdentityCard;
                identityCardCell.Style
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .NumberFormat.NumberFormatId = (int)XLPredefinedFormat.Number.IntegerWithSeparator;

                customerCell.Value = invoices[i].Customer.Name;
                customerCell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                exemptCell.Value = invoices[i].Exempt;
                exemptCell.Style.NumberFormat.Format = "$#,##0.00";

                taxBaseCell.Value = invoices[i].TaxBase;
                taxBaseCell.Style.NumberFormat.Format = "$#,##0.00";

                taxCell.Value = invoices[i].Tax;
                taxCell.Style.NumberFormat.Format = "$#,##0.00";

                totalCell.Value = invoices[i].Total;
                totalCell.Style.NumberFormat.Format = "$#,##0.00";
            }

            //Columns width
            foreach (var colum in sheet.ColumnsUsed())
            {
                colum.Width = 20;
            }

            workbook.SaveAs(filePath);
        }
    }
}
