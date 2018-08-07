using System.Windows.Input;

namespace Zopoise.Scada.App
{
    public static class QueryCommand
    {
        private static RoutedUICommand queryOperationLog;
        public static ICommand QueryOperationLog
        {
            get
            {
                if (queryOperationLog == null)
                {
                    queryOperationLog = new RoutedUICommand("Query Operation Log", "QueryOperationLog", typeof(QueryCommand));
                    queryOperationLog.InputGestures.Add(new KeyGesture(Key.O, ModifierKeys.Alt));
                }
                return queryOperationLog;
            }
        }

        private static RoutedUICommand queryCVLog;
        public static ICommand QueryCVLog
        {
            get
            {
                if (queryCVLog == null)
                {
                    queryCVLog = new RoutedUICommand("Query Current and Voltage Log", "QueryCurrentVoltageLog", typeof(QueryCommand));
                    queryCVLog.InputGestures.Add(new KeyGesture(Key.C, ModifierKeys.Alt));
                }
                return queryCVLog;
            }
        }
    }
}
