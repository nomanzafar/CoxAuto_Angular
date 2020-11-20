using System.Collections.Generic;

namespace CoxAuto.Models
{
    public enum ImportDealStatus
    {
        Success,
        Exception,
        Validation,
        ParseError
    }

    public class ImportDeal
    {
        public Dictionary<string, string> ColumnData { get; set; }
        public int Line { get; set; }
        public ImportDealStatus Status { get; set; }
        public IList<string> Disposition { get; set; } = new List<string>();

        public ImportDeal()
        {
            ColumnData = new Dictionary<string, string>();
            Disposition = new List<string>();
            Status = ImportDealStatus.Success; 
        }
    }
}
