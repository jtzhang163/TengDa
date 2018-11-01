using System.Windows.Input;

namespace Zopoise.Scada.App
{
    public static class QueryCommands
    {
        private static RoutedUICommand queryOperationLog;
        public static ICommand QueryOperationLog
        {
            get
            {
                if (queryOperationLog == null)
                {
                    queryOperationLog = new RoutedUICommand("Query Operation Log", "QueryOperationLog", typeof(QueryCommands));
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
                    queryIDLog = new RoutedUICommand("Query Insulation Data Log", "QueryInsulationDataLog", typeof(QueryCommands));
                    queryIDLog.InputGestures.Add(new KeyGesture(Key.C, ModifierKeys.Alt));
                }
                return queryIDLog;
            }
        }
    }
}
