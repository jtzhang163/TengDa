using System.Windows.Input;

namespace Tafel.Hipot.App
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

        private static RoutedUICommand queryIDLog;
        public static ICommand QueryIDLog
        {
            get
            {
                if (queryIDLog == null)
                {
                    queryIDLog = new RoutedUICommand("Query Insulation Data Log", "QueryInsulationDataLog", typeof(QueryCommand));
                    queryIDLog.InputGestures.Add(new KeyGesture(Key.C, ModifierKeys.Alt));
                }
                return queryIDLog;
            }
        }
    }
}
