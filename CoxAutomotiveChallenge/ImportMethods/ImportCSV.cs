using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CoxAuto.Helpers;
using CoxAuto.Models;
using CoxAutomotiveChallenge.Utilities;
using Microsoft.VisualBasic.FileIO;

namespace CoxAutomotiveChallenge.ImportMethods
{
    public static class ImportCSV
    {
        public static ImportData Parse(Stream inputStream)
        {
            var retVal = new ImportData();
            var encoding = Encoding.GetEncoding("iso-8859-1");
            using var parser = new TextFieldParser(inputStream, encoding) {TextFieldType = FieldType.Delimited};
            parser.SetDelimiters(",");

            string[] headerRow = null;
            try
            {
                headerRow = parser.ReadFields();
            }
            catch (MalformedLineException)
            {
                //malformed CSV line                                            
                retVal.Status = ImportDealStatus.ParseError;
                retVal.Disposition = "Header row malformed";
            }
            catch (Exception e)
            {
                //other exception
                retVal.Status = ImportDealStatus.Exception;
                retVal.Disposition = "Exception: " + e.Message;
            }

            var count = 1;
            while (!parser.EndOfData)
            {
                var newdeal = new ImportDeal();
                try
                {
                    retVal.DealData.Add(newdeal);
                    newdeal.Line = count++;
                    var currentRow = parser.ReadFields();
                    for (var i = 0; i < headerRow.Count(); i++)
                    {
                        if (currentRow[i].Equals("true", StringComparison.OrdinalIgnoreCase))
                        {
                            currentRow[i] = "True";
                        }
                        else if (currentRow[i].Equals("false", StringComparison.OrdinalIgnoreCase))
                        {
                            currentRow[i] = "False";
                        }
                        newdeal.ColumnData.Add(headerRow[i], currentRow[i]);
                    }
                }
                catch (MalformedLineException)
                {
                    //malformed CSV line                                                
                    newdeal.Status = ImportDealStatus.ParseError;
                    newdeal.Disposition.Add("Malformed Line: \"" + parser.ErrorLine + "\"");
                    retVal.Status = ImportDealStatus.ParseError;
                    retVal.Disposition = "Data row(s) malformed";
                }
                catch (Exception e)
                {
                    //other exception
                    newdeal.Status = ImportDealStatus.Exception;
                    newdeal.Disposition.Add("Exception: " + e.Message);
                    retVal.Status = ImportDealStatus.Exception;
                    retVal.Disposition = "Data row exception(s)";
                }
            }

            return retVal;
        }

        /// <summary>
        /// Processes request to upload a CSV file of deals to be imported
        /// </summary>
        public static ImportSummary ImportFileData(ImportData fileData)
        {

            var jobStartTime = DateTime.Now;
            var importSummary = new ImportSummary();
            IList<ImportError> importErrors = new List<ImportError>();
            IList<ImportResult> importResults = new List<ImportResult>();

            var importProperties = new Dictionary<string, object>
            {
                {"NoOfDeals", fileData.DealData.Count},
                {"TotalSales", fileData.DealData.Count},
                {"TopCar", fileData.DealData.Count},
                {"TopDealership", fileData.DealData.Count}
            };
            LogManager.WriteLog("Starting CSV Import", importProperties);

            var rowCount = 0;

            foreach (var deal in fileData.DealData)
            {
                rowCount++;

                try
                {
                    //We can save Deal in DB here   
                    var properties = new Dictionary<string, object>(deal.ColumnData.ToDictionary(k => k.Key, k => (object)k.Value));
                    LogManager.WriteLog("Created deal from CSV", deal);
                    deal.Status = ImportDealStatus.Success;
                    var newDeal = ImportHelpers.ConvertDictionaryTo<Deal>(deal.ColumnData);
                    ImportHelpers.AddImportResultToList(ref importResults, rowCount, "Import", "Deal Added", newDeal);
                }
                catch (Exception e)
                {
                    deal.Status = ImportDealStatus.Exception;
                    deal.Disposition.Add(e.Message);
                    ImportHelpers.AddImportErrorToList(ref importErrors, 0, "Import", e.Message);
                }
            }
            LogManager.WriteLog("Stopping CSV Import", importProperties);


            var jobEndTime = DateTime.Now;
            importSummary.ImportErrors = importErrors;
            importSummary.ImportResults = importResults;
            return importSummary;
        }
    }
}
