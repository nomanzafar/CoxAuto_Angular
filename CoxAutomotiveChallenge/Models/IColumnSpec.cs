using System;
namespace CoxAuto.Models
{
    public interface IColumnSpec
    {
        ColumnHelpSpec ToHelp();
        bool Validate(ImportDeal csvRow);
        string Name { get; }
    }
}
