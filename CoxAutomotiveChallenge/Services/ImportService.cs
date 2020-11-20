using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CoxAuto.Helpers;
using CoxAuto.Models;
using CoxAutomotiveChallenge.ImportMethods;
using CoxAutomotiveChallenge.IServices;
using CoxAutomotiveChallenge.Utilities;

namespace CoxAutomotiveChallenge.Services
{
    public class ImportService : IImportService
    {
        public IList<IColumnSpec> GetColumnSpec(string cultureCode)
        {
            var colIndex = 1;
            IList<IColumnSpec> columnSpec = new List<IColumnSpec>();
            columnSpec.Add(new SimpleColumnSpec(colIndex++, "DealNumber", CustomFieldType.Int, "Deal Number", true,
                cultureCode, "Numbers only e.g. 4567"));
            columnSpec.Add(new SimpleColumnSpec(colIndex++, "CustomerName", CustomFieldType.String, "Customer Name",
                false, cultureCode, "e.g. XYZ Customer"));
            columnSpec.Add(new SimpleColumnSpec(colIndex++, "DealershipName", CustomFieldType.String, "Dealership Name",
                true, cultureCode, "e.g. XYZ Dealership"));
            columnSpec.Add(new SimpleColumnSpec(colIndex++, "Vehicle", CustomFieldType.String, "Vehicle", true,
                cultureCode, "e.g. Ford 2018"));
            columnSpec.Add(new SimpleColumnSpec(colIndex++, "Price", CustomFieldType.Decimal, "Sale Price", false,
                cultureCode, 9.95M.ToString(cultureCode)));
            columnSpec.Add(new DateTimeColumnSpec(colIndex++, "Date", "Sale Date", false, cultureCode));
            return columnSpec;
        }

        public ImportSummary ImportFileData(Stream inputStream)
        {
            var fileData = ImportCSV.Parse(inputStream);

            var csvColumns = GetColumnSpec(CultureInfo.CurrentCulture.Name);

            //if (!PreValidate(csvColumns, fileData))
            if (!PreValidate(csvColumns, fileData))
            {
                IList<ImportError> importErrors = new List<ImportError>();
                try
                {
                    var rowCount = 0;
                    var strErrors = "";
                    foreach (var deal in fileData.DealData.Where(l => l.Status == ImportDealStatus.Validation))
                    {
                        rowCount++;
                        strErrors = deal.Disposition.Aggregate(strErrors,
                            (current, disposition) => current + ". " + disposition);
                    }

                    ImportHelpers.AddImportErrorToList(ref importErrors, rowCount, "Import", strErrors);
                }
                catch (Exception e)
                {
                    LogManager.WriteLog("Error adding import validation erros", e);
                    ImportHelpers.AddImportErrorToList(ref importErrors, 0, "Import", e.Message);
                }

                var importSummary = new ImportSummary {ImportErrors = importErrors};
                return importSummary;
            }
            else
            {
                var importSummary = ImportCSV.ImportFileData(fileData);
                importSummary.Summary = GetImportDBSummary(importSummary.ImportResults);
                return importSummary;
            }
        }

        public bool PreValidate(IList<IColumnSpec> columns, ImportData importData)
        {
            var retVal = true;

            foreach (var dealRow in importData.DealData)
                try
                {
                    foreach (var columnSpec in columns)
                    {
                        var thisCheck = columnSpec.Validate(dealRow);
                        if (!thisCheck) dealRow.Status = ImportDealStatus.Validation;
                        retVal = thisCheck && retVal;
                    }
                }
                catch (Exception e)
                {
                    dealRow.Disposition.Add(LogManager.FlattenExceptionMessages(e));
                    dealRow.Status = ImportDealStatus.Exception;
                }

            if (retVal) return true;
            importData.Status = ImportDealStatus.Validation;
            importData.Disposition = "CSV Pre-Validation Error";

            return false;
        }

        public IList<SummaryItem> GetImportDBSummary(IList<ImportResult> importResults)
        {
            IList<SummaryItem> importDBSummary = new List<SummaryItem>();
            try
            {
                if (importResults.Count > 0)
                {
                    var topVehicle = importResults.GroupBy(x => x.Deal.Vehicle).Select(group => new
                    {
                        Vehicle = group.Key,
                        Count = group.Count()
                    }).OrderByDescending(x => x.Count).FirstOrDefault();

                    importDBSummary.Add(new SummaryItem
                    {
                        ItemName = "TopVehicle",
                        ItemDescription = topVehicle?.Vehicle,
                        ItemValue = topVehicle?.Count.ToString()
                    });

                    var topDealer = importResults.GroupBy(x => x.Deal.DealershipName).Select(group => new
                    {
                        Dealer = group.Key,
                        Count = group.Count()
                    }).OrderByDescending(x => x.Count).FirstOrDefault();

                    importDBSummary.Add(new SummaryItem
                    {
                        ItemName = "TopDealer",
                        ItemDescription = topDealer?.Dealer,
                        ItemValue = topDealer?.Count.ToString()
                    });

                    importDBSummary.Add(new SummaryItem
                    {
                        ItemName = "TotalSales",
                        ItemDescription = "Total Sales",
                        ItemValue = importResults.Sum(x => x.Deal.Price).ToString("CAD$#,##,###")
                    });

                    importDBSummary.Add(new SummaryItem
                    {
                        ItemName = "NoOfDeals",
                        ItemDescription = "Number Of Deals",
                        ItemValue = importResults.Count().ToString()
                    });
                }
            }
            catch (Exception e)
            {
                LogManager.WriteLog("Error generating import DB Summary", e);
            }

            return importDBSummary;
        }
    }
}