using System;
using System.Collections.Generic;
using System.Linq;
namespace CoxAuto.Models
{
    public class ImportData
    {
        public IList<ImportDeal> DealData { get; set; } = new List<ImportDeal>();
        public ImportDealStatus Status { get; set; }
        public string Disposition { get; set; }

        public ImportData()
        {
            DealData = new List<ImportDeal>();
            Status = ImportDealStatus.Success;
            Disposition = string.Empty;
        }
    }
}
