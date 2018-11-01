namespace Zopoise.Scada.Bak
{
    public static class AppContext
    {
        public static CommunicatorContext CommunicatorContext = new CommunicatorContext();
        public static PlcContext PlcContext = new PlcContext();
        public static TesterContext TesterContext = new TesterContext();
    }
}
