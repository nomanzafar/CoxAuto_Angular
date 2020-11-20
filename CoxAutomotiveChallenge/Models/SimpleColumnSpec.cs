using System;
namespace CoxAuto.Models
{
  public class SimpleColumnSpec : ColumnSpecBase
    {
        public SimpleColumnSpec(int number, string name, CustomFieldType dataType, string notes, bool required, string cultureCode, string example) : base(number, name, dataType, notes, required, cultureCode, example)
        {
        }
    }
}
