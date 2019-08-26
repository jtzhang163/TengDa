using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMEL.RGV.Touchscreen
{
    public class Parameter
    {
        public string Name { get; set; }

        public string Addr { get; set; }

        public string Type { get; set; }

        public int MaxValue { set; get; }

        public int MinValue { set; get; }

        private static List<Parameter> parameters;
        public static List<Parameter> Parameters
        {
            get
            {
                if (parameters == null)
                {
                    parameters = new List<Parameter>() {
                        new Parameter{ Name= "行走位", Addr = "D1007", Type = "一般参数", MinValue=0, MaxValue=10000 },
                        new Parameter{ Name= "取货位号", Addr = "D1009" ,Type = "一般参数", MinValue=0, MaxValue=10000 },
                        new Parameter{ Name= "放货位号", Addr = "D1008" ,Type = "一般参数", MinValue=0, MaxValue=10000 },
                        new Parameter{ Name= "货叉进", Addr = "D1010" ,Type = "一般参数", MinValue=0, MaxValue=10000 },
                        new Parameter{ Name= "运行时间", Addr = "D2004" ,Type = "一般参数" },
                        new Parameter{ Name= "行走定位时间", Addr = "D2005" ,Type = "一般参数"},
                        new Parameter{ Name= "升降定位时间", Addr = "D2006" ,Type = "一般参数"},
                        new Parameter{ Name= "货叉定位时间", Addr = "D2007" ,Type = "一般参数"},
                        new Parameter{ Name= "行走当前速度", Addr = "D2008" ,Type = "一般参数"},
                        new Parameter{ Name= "升降当前速度", Addr = "D2009" ,Type = "一般参数"},
                        new Parameter{ Name= "货叉当前速度", Addr = "D2010" ,Type = "一般参数"},
                        new Parameter{ Name= "行走加速时间", Addr = "D2020" ,Type = "一般参数", MinValue= 2000, MaxValue= 10000 },
                        new Parameter{ Name= "行走减速时间", Addr = "D2021" ,Type = "一般参数", MinValue= 2000, MaxValue= 10000 },
                        new Parameter{ Name= "行走目标速度", Addr = "D2022" ,Type = "一般参数", MinValue= 0, MaxValue= 2500 },
                        new Parameter{ Name= "升降加速时间", Addr = "D2023" ,Type = "一般参数", MinValue= 2000, MaxValue= 10000 },
                        new Parameter{ Name= "升降减速时间", Addr = "D2024" ,Type = "一般参数", MinValue= 2000, MaxValue= 10000 },
                        new Parameter{ Name= "升降目标速度", Addr = "D2025" ,Type = "一般参数", MinValue= 0, MaxValue= 500 },
                        new Parameter{ Name= "货叉加速时间", Addr = "D2026" ,Type = "一般参数", MinValue= 2000, MaxValue= 10000 },
                        new Parameter{ Name= "货叉减速时间", Addr = "D2027" ,Type = "一般参数", MinValue= 2000, MaxValue= 10000 },
                        new Parameter{ Name= "货叉目标速度", Addr = "D2028" ,Type = "一般参数", MinValue= 0, MaxValue= 500 },
                        new Parameter{ Name= "货位号", Addr = "D2029" ,Type = "一般参数" ,MinValue=0, MaxValue=10000 },
                        new Parameter{ Name= "行走电机", Addr = "D2036" ,Type = "一般参数"},
                        new Parameter{ Name= "升降电机", Addr = "D2038" ,Type = "一般参数"},
                        new Parameter{ Name= "货叉电机", Addr = "D2040" ,Type = "一般参数"},
                        new Parameter{ Name= "行走位置参数", Addr = "D2042" ,Type = "一般参数",MinValue=0, MaxValue=10000 },
                        new Parameter{ Name= "升降1位置参数", Addr = "D2044" ,Type = "一般参数",MinValue=0, MaxValue=10000 },
                        new Parameter{ Name= "升降2位置参数", Addr = "D2046" ,Type = "一般参数",MinValue=0, MaxValue=10000 },
                        new Parameter{ Name= "货叉位置参数", Addr = "D2048" ,Type = "一般参数",MinValue=0, MaxValue=10000 },
                        new Parameter{ Name= "步号", Addr = "D2061" ,Type = "一般参数",MinValue=0 ,MaxValue=5},

                        new Parameter{ Name= "行走JOG正转", Addr = "D2063" ,Type= "点动" },
                        new Parameter{ Name= "行走JOG反转", Addr = "D2064" ,Type= "点动" },
                        new Parameter{ Name= "升降JOG上升", Addr = "D2065" ,Type= "点动" },
                        new Parameter{ Name= "升降JOG降下", Addr = "D2066" ,Type= "点动" },
                        new Parameter{ Name= "货叉JOG正转", Addr = "D2067" ,Type= "点动" },
                        new Parameter{ Name= "货叉JOG反转", Addr = "D2069" ,Type= "点动" },

                        new Parameter{ Name= "参数写入", Addr = "D2062" ,Type = "使能" },
                        new Parameter{ Name= "行走测试", Addr = "D2070" ,Type = "使能" },
                        new Parameter{ Name= "升降1测试", Addr = "D2071" ,Type = "使能" },
                        new Parameter{ Name= "升降2测试", Addr = "D2072" ,Type = "使能"},
                        new Parameter{ Name= "货叉测试", Addr = "D2073" ,Type = "使能" },
                        new Parameter{ Name= "货叉原点", Addr = "D2068" ,Type = "使能" },
                        new Parameter{ Name= "蜂鸣停止", Addr = "D2060" ,Type = "使能" },

                        new Parameter{ Name= "调度无效", Addr = "D2075" ,Type = "状态使能"},
                        new Parameter{ Name= "手动状态", Addr = "D1005" ,Type = "状态使能"},

                        new Parameter{ Name= "启动", Addr = "D1001" ,Type = "单次写入" },
                        new Parameter{ Name= "停止", Addr = "D1002" ,Type = "单次写入" },
                        new Parameter{ Name= "复位", Addr = "D1003" ,Type = "单次写入" },
                        new Parameter{ Name= "急停", Addr = "D1004" ,Type = "单次写入" },
                    };
                }
                return parameters;
            }
        }

        public static string GetAddr(string name)
        {
            return Parameters.FirstOrDefault(o => o.Name == name)?.Addr;
        }

        public static string GetType(string name)
        {
            return Parameters.FirstOrDefault(o => o.Name == name)?.Type;
        }
        public static int GetMinvalue(string name)
        {
            return Parameters.FirstOrDefault(o => o.Name == name).MinValue;
        }
        public static int GetMaxvalue(string name)
        {
            return Parameters.FirstOrDefault(o => o.Name == name).MaxValue;
        }
    }
}
