using System.Runtime.Serialization;

namespace CoxAuto.Models
{
    [DataContract]
    public enum CustomFieldType
    {
        String = 1,
        Int = 2,
        Boolean = 3,
        Decimal = 4,
        DateTime = 5,
        Enum = 6
    }
    public class ColumnHelpSpec
        {
            public int ColumnNumber { get; private set; }
            public string ColumnString { get; private set; }
            public string Name { get; private set; }
            public string DataType { get; private set; }
            public string Notes { get; private set; }
            public string Example { get; private set; }
            public bool Required { get; private set; }

            public ColumnHelpSpec(int colNum, string colStr, string name, string dataType, string notes, bool required, string example)
            {
                ColumnNumber = colNum;
                ColumnString = colStr;
                Name = name;
                DataType = dataType;
                Notes = notes;
                Required = required;
                Example = example;
            }
        }

    } 
