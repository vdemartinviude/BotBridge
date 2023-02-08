using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office.CustomUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TheFileMapper;

public class WooCommerceFileMapper : IFileMapper
{
    private readonly string _folderPath;

    public WooCommerceFileMapper(string FolderPath)
    {
        _folderPath = FolderPath;
    }

    public Task GenerateJsonFile(string Phase, string OutputJsonFilePath)
    {
        switch (Phase.ToLower())
        {
            case "setup":
            case "compost":
                return GenerateSetupJson();

            case "fertilize":
            case "products":
                return GenerateProductsJson();

            default:
                throw (new Exception("This method was not implemented on the WC Robot"));
        }
    }

    private Task GenerateProductsJson()
    {
        throw new NotImplementedException();
    }

    private Task GenerateSetupJson()
    {
        var worksheetsetupfile = Directory.GetFiles(_folderPath)
            .Where(x => Regex.IsMatch(x.ToLower(), @".*setup.*\.xlsx")).FirstOrDefault();

        if (String.IsNullOrEmpty(worksheetsetupfile))
            throw new Exception("Setup file not found");

        var workbook = new XLWorkbook(worksheetsetupfile);
        var worksheet = workbook.Worksheet(1);

        var columnParameter = worksheet.Row(1).CellsUsed()
            .Where(x => x.Value.GetText() == "Parameter")
            .Select<IXLCell, int>(x => x.WorksheetColumn().ColumnNumber())
            .FirstOrDefault();

        var columnValue = worksheet.Row(1).CellsUsed()
            .Where(x => x.Value.GetText() == "Value")
            .Select(x => x.WorksheetColumn().ColumnNumber())
            .FirstOrDefault();

        var totalRows = worksheet.RowsUsed().Count();

        //TODO: Someday I need to fix this type processing.....TRAGICO!!!

        var parameters = worksheet.Rows(2, totalRows)
            .ToDictionary(x => x.Cell(columnParameter).GetFormattedString().ToLower(),
                          x => x.Cell(columnValue).GetFormattedString());

        //var parameters = worksheet.Rows(2, totalRows)
        //    .ToDictionary(x => x.Cell(columnParameter).Value.GetText(),
        //                  x =>
        //                  {
        //                      switch (x.Cell(columnValue).Value.Type)
        //                      {
        //                          case XLDataType.Text:
        //                              return x.Cell(columnValue).Value.GetText();

        //                          case XLDataType.DateTime:
        //                              var cell = x.Cell(columnValue).Value;
        //                              var dt = cell.GetDateTime();
        //                              return dt.ToString("dd/MM/yyyy HH:mm:ss");

        //                          case XLDataType.Number:
        //                              var cell2 = x.Cell(columnValue);
        //                              var format = cell2.Style.NumberFormat.Format;
        //                              var formatId = cell2.Style.DateFormat.NumberFormatId;
        //                              if (format.Contains("dd") || format.Contains("H") || formatId == 14)
        //                              {
        //                                  var cellvalue = cell2.GetDouble();
        //                                  var dt2 = DateTime.FromOADate(cellvalue);
        //                                  return dt2.ToString("dd/MM/yyyy HH:mm:ss");
        //                              }
        //                              return x.Cell(columnValue).GetDouble().ToString();

        //                          default:
        //                              return x.Cell(columnValue).Value.ToString();
        //                      }
        //                  });

        return Task.CompletedTask;
    }
}