using System.Collections.Generic;
using System.IO;
using CoxAuto.Models;

namespace CoxAutomotiveChallenge.IServices
{
    public interface IImportService
    {
        public IList<IColumnSpec> GetColumnSpec(string cultureCode);
        public ImportSummary ImportFileData(Stream inputStream);
        public bool PreValidate(IList<IColumnSpec> columns, ImportData importData);
        public IList<SummaryItem> GetImportDBSummary(IList<ImportResult> importResults);
    }
}
