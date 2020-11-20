using NUnit.Framework;
using System.IO;
using CoxAutomotiveChallenge.IServices;
using CoxAutomotiveChallenge.Services;
using CoxAuto.Models;

namespace NUnitTest
{
    public class ImportTests
    {

        [Test]
        public void CSVImportTest()
        {
            Stream _stream = new MemoryStream();
            var writer = new StreamWriter(_stream);
            writer.WriteLine("DealNumber,CustomerName,DealershipName,Vehicle,Price,Date");
            writer.WriteLine("5469,Milli Fulton,Sun of Saskatoon,2017 Ferrari 488 Spider,\"429,987\",6/19/2018");
            writer.WriteLine("5132,Rahima Skinner,Seven Star Dealership,2009 Lamborghini Gallardo Carbon Fiber LP-560,\"169,900\",1/14/2018");
            writer.WriteLine("5795,Aroush Knapp,Maxwell & Junior,2016 Porsche 911 2dr Cpe GT3 RS,\"289,900\",6/7/2018");
            writer.WriteLine("5455,Mr. Milli Fulton,Sun of Saskatoon,2017 Ferrari 488 Spider,\"429,000\",6/19/2018");
            writer.Flush();
            _stream.Position = 0;

            IImportService _importService = new ImportService();

            ImportSummary _importSummary = _importService.ImportFileData(_stream);

            Assert.IsTrue(_importSummary.ImportErrors.Count <= 0);
            Assert.IsTrue(_importSummary.ImportResults.Count == 4);
            Assert.IsTrue(_importSummary.Summary[0].ItemDescription == "2017 Ferrari 488 Spider"); //Top Vehicle
            Assert.IsTrue(_importSummary.Summary[1].ItemDescription == "Sun of Saskatoon"); //Top Dealer
            Assert.IsTrue(_importSummary.Summary[2].ItemValue == "CAD$1,318,787"); //Total Sales
            Assert.IsTrue(_importSummary.Summary[3].ItemValue == "4"); //Total Deals

        }
    }
}