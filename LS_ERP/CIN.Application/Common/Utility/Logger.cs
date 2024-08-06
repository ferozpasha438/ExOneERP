namespace CIN.Application
{
    using log4net;
    public class Log
    {
        public static void Info(string message) => LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)?.Info(message);
        public static void Error(string message) => LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)?.Error(message);

    }
}
