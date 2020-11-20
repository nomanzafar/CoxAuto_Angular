using CoxAuto.Models;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CoxAuto.Helpers
{
    public class ImportHelpers
    {

        public static void AddImportResultToList(ref IList<ImportResult> ImportResultList, int RowCount, string PerformedAction, string Message, Deal Deal)
        {
            ImportResultList.Add(new ImportResult()
            {
                RowNo = RowCount,
                PerformedAction = PerformedAction,
                Message = Message,
                Deal = Deal
            });
        }

        public static void AddImportErrorToList(ref IList<ImportError> ImportErrorList, int RowCount, string PerformedAction, string Message)
        {
            ImportErrorList.Add(new ImportError()
            {
                RowNo = RowCount,
                PerformedAction = PerformedAction,
                Message = Message,
            });
        }

        public static T ConvertDictionaryTo<T>(IDictionary<string, string> dictionary) where T : new()
        {
            Type type = typeof(T);
            T result = (T)Activator.CreateInstance(type);
            foreach (var item in dictionary)
            {
                PropertyInfo propertyInfo = type.GetProperty(item.Key);
                propertyInfo?.SetValue(result, Convert.ChangeType(item.Value, propertyInfo.PropertyType), null);
            }

            return result;
        }

    }
}
