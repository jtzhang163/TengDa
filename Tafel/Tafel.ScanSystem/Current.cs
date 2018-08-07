using TengDa;

namespace Tafel.ScanSystem
{
    /// <summary>
    /// 系统当前运行时公用类
    /// </summary>
    public static class Current
    {
        public static RunStatus runStstus = RunStatus.未知;

        public static Feeder feeder = new Feeder(1);
        public static Scaner scaner = new Scaner(1);
        public static MES mes = new MES();
        public static Option option = new Option();

        public static int clampId = -1;
        public static int batteryId = -1;

        public const int ClampCount = 2;
        public const int ClampRowCount = 12;
        public const int ClampColCount = 3;
    }
}
