using ClosedXML.Excel;
using System.Data;
using WPFInvoiceSystem.Application.Abstractions;
using WPFInvoiceSystem.Domain.Entities;

namespace WPFInvoiceSystem.ReportsGenerator
{
    public class InvoiceReport : IInvoicesReportGenerator
    {
        private readonly int _headerStartRowNumber = 2;
        private readonly int _tableStartRowNumber = 4;
        private IEnumerable<Invoice> _invoices = [];

        public byte[] Generate(
            IEnumerable<Invoice> invoices
            )
        {
            _invoices = invoices;

            using var workbook = new XLWorkbook();

            IXLWorksheet sheet = workbook.Worksheets.Add();

            sheet = AppendHeaderToSheet(sheet);
            sheet = AppendTableToSheet(sheet);
            sheet = ApplyStylesToSheet(sheet);

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        private IXLWorksheet AppendHeaderToSheet(IXLWorksheet sheet)
        {
            IXLCell headerTitleCell = sheet.Cell($"A{_headerStartRowNumber}");
            headerTitleCell.Value = "Invoices Report";
            headerTitleCell.Style.Font.FontSize = 18;
            headerTitleCell.Style.Font.FontColor = XLColor.BlueViolet;
            headerTitleCell.Style.Font.SetBold();

            return sheet;
        }

        private IXLWorksheet AppendTableToSheet(IXLWorksheet sheet)
        {
            var dataTable = new DataTable();

            dataTable = SetTableColumns(dataTable);
            dataTable = FillTableRows(dataTable);
            AppendDataTableToSheet(sheet, dataTable);

            return sheet;
        }

        private IXLWorksheet ApplyStylesToSheet(IXLWorksheet sheet)
        {
            //sheet.ColumnWidth = 25; <==== Not working. Width will be set manually bellow
            foreach (var column in sheet.Columns())
            {
                column.Width = 25;
            }
            int numberOfFirstColumnUsed = sheet.FirstColumnUsed().ColumnNumber();
            int numberOfFirstRowUsed = _tableStartRowNumber;
            int numberOfLastColumnUsed = sheet.LastRowUsed().LastCellUsed().WorksheetColumn().ColumnNumber();
            int numberOfLastRowUsed = sheet.LastRowUsed().RowNumber();

            IXLRange tableHeaderRange = sheet.Range(
                firstCellRow: numberOfFirstRowUsed,
                firstCellColumn: numberOfFirstColumnUsed,
                lastCellRow: numberOfFirstRowUsed,
                lastCellColumn: numberOfLastColumnUsed);

            //Style headers
            tableHeaderRange.Style
                .Font.SetBold()
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            //Style rest of table
            IXLRange rangeOfTextData = sheet.Range(
                firstCellRow: numberOfFirstRowUsed,
                firstCellColumn: numberOfFirstColumnUsed,
                lastCellRow: numberOfLastRowUsed,
                lastCellColumn: 4);

            IXLRange rangeOfNumericData = sheet.Range(
                firstCellRow: numberOfFirstRowUsed,
                firstCellColumn: 5,
                lastCellRow: numberOfLastRowUsed,
                lastCellColumn: numberOfLastColumnUsed
                );

            rangeOfTextData.Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Border.OutsideBorder = XLBorderStyleValues.Thin;

            rangeOfNumericData.Style
                .NumberFormat.Format = "$#,##0.00";

            rangeOfNumericData.Style
                .Border.OutsideBorder = XLBorderStyleValues.Thin;

            //Add border to table headers
            tableHeaderRange.Style
                .Border.OutsideBorder = XLBorderStyleValues.Medium;

            return sheet;
        }

        private DataTable SetTableColumns(DataTable table)
        {
            //Adding columns for invoice data
            table.Columns.Add(new DataColumn("Date", typeof(DateTime)));
            table.Columns.Add(new DataColumn("Invoice Number", typeof(string)));
            table.Columns.Add(new DataColumn("Customer", typeof(string)));
            table.Columns.Add(new DataColumn("Customer Id. Card", typeof(string)));
            table.Columns.Add(new DataColumn("Exempt", typeof(decimal)));
            table.Columns.Add(new DataColumn("Tax Base", typeof(decimal)));
            table.Columns.Add(new DataColumn("Tax", typeof(decimal)));
            table.Columns.Add(new DataColumn("Total", typeof(decimal)));

            return table;
        }

        private DataTable FillTableRows(DataTable table)
        {
            foreach (var invoice in _invoices)
            {
                DataRow tableRow = table.NewRow();

                //Fullfill invoice info cells:
                tableRow["Date"] = invoice.Date.ToString("d");
                tableRow["Invoice Number"] = invoice.InvoiceNumber;
                tableRow["Customer"] = invoice.Customer.Name;
                tableRow["Customer Id. Card"] = invoice.Customer.IdentityCard;
                tableRow["Exempt"] = invoice.Exempt;
                tableRow["Tax Base"] = invoice.TaxBase;
                tableRow["Tax"] = invoice.Tax;
                tableRow["Total"] = invoice.Total;

                table.Rows.Add(tableRow);
            }

            return table;
        }

        private IXLWorksheet AppendDataTableToSheet(IXLWorksheet sheet, DataTable table)
        {
            IXLRow tableHeaderRow = sheet.Row(_tableStartRowNumber);
            //Append table headers
            for (var i = 0; i < table.Columns.Count; i++)
            {
                IXLCell currentCell = tableHeaderRow.Cell(i + 1);
                currentCell.Value = table.Columns[i].ColumnName;
            }

            //Append rest of table
            tableHeaderRow.RowBelow().FirstCell().InsertData(table);

            return sheet;
        }
    }
}
