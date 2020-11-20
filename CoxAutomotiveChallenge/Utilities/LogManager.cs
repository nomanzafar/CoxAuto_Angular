using System;

namespace CoxAutomotiveChallenge.Utilities
{
    public static class LogManager
    {
        public static void WriteLog(string message)
        {
            Console.WriteLine(message);
        }

        public static void WriteLog(string message, object logObject)
        {
            Console.WriteLine(message, logObject);
        }

        public static string FlattenExceptionMessages(Exception e)
        {
            var retVal = e.Message;
            while (e.InnerException != null)
            {
                e = e.InnerException;
                retVal += ": " + e.Message;
            }
            return retVal;
        }
    }
}
