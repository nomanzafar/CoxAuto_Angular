using System;
using System.Globalization;

namespace CoxAuto.Models
{
    public class DateTimeColumnSpec : ColumnSpecBase
    {
        public DateTimeColumnSpec(int number, string name, string notes, bool required, string cultureCode) : base(number, name, CustomFieldType.DateTime, notes, required, cultureCode, string.Empty)
        {
            Example = DateTime.Now.ToString(CultureInfo.GetCultureInfo(cultureCode));
        }
    }
}
