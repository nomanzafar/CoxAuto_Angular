using System.Collections.Generic;

namespace CoxAuto.Models
{
    public class ImportSummary
    {
        public IList<ImportError> ImportErrors { get; set; } = new List<ImportError>();
        public IList<ImportResult> ImportResults { get; set; } = new List<ImportResult>();
        public IList<SummaryItem> Summary { get; set; } = new List<SummaryItem>();
    }

}
