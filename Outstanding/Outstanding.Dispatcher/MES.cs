using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web.Services.Description;
using TengDa;
using TengDa.WF;

namespace Outstanding.Dispatcher
{
    public class MES : TengDa.WF.MES.MES
    {

        #region 字段属性
        protected string webServiceUrl = string.Empty;
        /// <summary>
        /// Web服务地址
        /// </summary>
        [DisplayName("Web服务地址")]
        [Category("基本设置")]
        public string WebServiceUrl
        {
            get
            {
                return webServiceUrl;
            }
            set
            {
                if (webServiceUrl != value)
                {
                    UpdateDbField("WebServiceUrl", value);
                }
                webServiceUrl = value;
            }
        }

        protected string workstationSn = string.Empty;
        /// <summary>
        /// 工作中心SN
        /// </summary>
        [DisplayName("工作中心SN")]
        [Category("参数配置")]
        public string WorkstationSn
        {
            get
            {
                return workstationSn;
            }
            set
            {
                if (workstationSn != value)
                {
                    UpdateDbField("WorkstationSn", value);
                }
                workstationSn = value;
            }
        }

        protected string employeeNo = string.Empty;
        /// <summary>
        /// 员工号
        /// </summary>
        [DisplayName("员工号")]
        [Category("参数配置")]
        public string EmployeeNo
        {
            get
            {
                return employeeNo;
            }
            set
            {
                if (employeeNo != value)
                {
                    UpdateDbField("EmployeeNo", value);
                }
                employeeNo = value;
            }
        }


        protected string manufactureOrder = string.Empty;
        /// <summary>
        /// 制令单号
        /// </summary>
        [DisplayName("制令单号")]
        [Category("参数配置")]
        public string ManufactureOrder
        {
            get
            {
                return manufactureOrder;
            }
            set
            {
                if (manufactureOrder != value)
                {
                    UpdateDbField("ManufactureOrder", value);
                }
                manufactureOrder = value;
            }
        }


        #endregion

        #region 构造方法

        public MES() : this(-1) { }

        public MES(int id)
        {
            if (id < 0)
            {
                this.Id = -1;
                return;
            }

            string msg = string.Empty;

            DataTable dt = Database.Query("SELECT * FROM [dbo].[" + TableName + "] WHERE Id = " + id, out msg);

            if (!string.IsNullOrEmpty(msg))
            {
                Error.Alert(msg);
                return;
            }

            if (dt == null || dt.Rows.Count == 0) { return; }

            Init(dt.Rows[0]);

            //释放资源
            dt.Dispose();
        }


        #endregion

        #region 初始化方法
        protected void Init(DataRow rowInfo)
        {
            if (rowInfo == null)
            {
                this.Id = -1;
                return;
            }

            InitFields(rowInfo);
        }

        protected void InitFields(DataRow rowInfo)
        {
            this.Id = TengDa._Convert.StrToInt(rowInfo["Id"].ToString(), -1);
            this.name = rowInfo["Name"].ToString();
            this.host = rowInfo["Host"].ToString();
            this.isEnable = Convert.ToBoolean(rowInfo["IsEnable"]);
            this.isOffline = Convert.ToBoolean(rowInfo["IsOffline"]);
            this.webServiceUrl = rowInfo["WebServiceUrl"].ToString();
            this.workstationSn = rowInfo["WorkstationSn"].ToString();
            this.employeeNo = rowInfo["EmployeeNo"].ToString();
            this.manufactureOrder = rowInfo["ManufactureOrder"].ToString();
        }
        #endregion

        #region 系统MES列表
        private static List<MES> mesList = new List<MES>();
        private static List<MES> MesList
        {
            get
            {
                string msg = string.Empty;

                DataTable dt = Database.Query("SELECT * FROM [dbo].[" + TableName + "]", out msg);
                if (!string.IsNullOrEmpty(msg))
                {
                    Error.Alert(msg);
                    return null;
                }

                if (dt == null || dt.Rows.Count == 0)
                {
                    mesList = null;
                }
                else
                {
                    mesList.Clear();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        MES mes = new MES();
                        mes.InitFields(dt.Rows[i]);
                        mesList.Add(mes);
                    }
                }
                return mesList;
            }
        }

        #endregion

        #region 获取

        public static MES Mes
        {
            get
            {
                return MesList.First();
            }
        }

        public static MES GetMES(string name, out string msg)
        {
            try
            {
                List<MES> list = (from mes in MesList where mes.Name == name select mes).ToList();
                if (list.Count() > 0)
                {
                    msg = string.Empty;
                    return list[0];
                }
                msg = string.Format("数据库不存在 {0}！", name);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return null;
        }

        #endregion

        #region MES方法

        public bool GetInfo()
        {
            return false;
        }

        public bool UploadCellInfo(Clamp clamp, out string msg)
        {
            msg = string.Empty;
            try
            {
                //WEB服务地址(非正式地址)
                //string url = "http://192.168.72.1:8092/mesframework.asmx";
                string url = this.WebServiceUrl;

                //客户端代理服务命名空间，可以设置成需要的值。  
                string ns = string.Format("ProxyServiceReference");

                //获取WSDL  
                WebClient wc = new WebClient();
                Stream stream = wc.OpenRead(url + "?WSDL");
                ServiceDescription sd = ServiceDescription.Read(stream);//服务的描述信息都可以通过ServiceDescription获取  
                string classname = sd.Services[0].Name;

                ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
                sdi.AddServiceDescription(sd, "", "");
                CodeNamespace cn = new CodeNamespace(ns);

                //生成客户端代理类代码  
                CodeCompileUnit ccu = new CodeCompileUnit();
                ccu.Namespaces.Add(cn);
                sdi.Import(cn, ccu);
                CSharpCodeProvider csc = new CSharpCodeProvider();

                //设定编译参数  
                CompilerParameters cplist = new CompilerParameters();
                cplist.GenerateExecutable = false;
                cplist.GenerateInMemory = true;
                cplist.ReferencedAssemblies.Add("System.dll");
                cplist.ReferencedAssemblies.Add("System.XML.dll");
                cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
                cplist.ReferencedAssemblies.Add("System.Data.dll");

                //编译代理类  
                CompilerResults cr = csc.CompileAssemblyFromDom(cplist, ccu);
                if (cr.Errors.HasErrors == true)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach (System.CodeDom.Compiler.CompilerError ce in cr.Errors)
                    {
                        sb.Append(ce.ToString());
                        sb.Append(System.Environment.NewLine);
                    }
                    throw new Exception(sb.ToString());
                }

                //生成代理实例
                Assembly assembly = cr.CompiledAssembly;
                Type t = assembly.GetType(ns + "." + classname, true, true);
                object obj = Activator.CreateInstance(t);

                //实例化方法
                MethodInfo methodInfo = t.GetMethod("AutoLineDataUpload");


                Station station = Station.StationList.FirstOrDefault(s => s.Id == clamp.OvenStationId);
                Floor floor = Floor.FloorList.FirstOrDefault(f => f.Stations.Contains(station));

                var batteries = clamp.Batteries;
                foreach (Battery battery in batteries)
                {

                    //设置参数
                    object device = floor.Number;
                    object workstation_sn = this.WorkstationSn;
                    object emp_no = this.EmployeeNo;
                    object mo_number = this.ManufactureOrder;
                    object product_sn = "$$$" + battery.Code;

                    //  object contaion_sn = clamp.Code;
                    object contaion_sn = string.Empty;
                    object union_list = string.Empty;
                    object error_code = string.Empty;
                    object error_value = string.Empty;
                    object error_qty = "0";

                    object ec_flg = "OK";
                    object sn_qty = "1";
                    object test_item_list = string.Format("烘烤温度:{0}:℃|烘烤真空度:{1}:Pa|烘烤时间:{2}:min|烘烤开始时间:{3}:|烘烤结束时间:{4}:|出烤箱温度:{5}:℃|极片水分:{6}:ppm|夹具扭力:{7}:N|夹具条码:{8}:",
                      clamp.Temperature.ToString("#0.0"),
                      clamp.Vacuum.ToString("#0"),
                      (clamp.BakingStopTime - clamp.BakingStartTime).TotalMinutes.ToString("#0"),
                      clamp.BakingStartTime.ToString("yyyy-MM-dd HH-mm-ss"),
                      clamp.BakingStopTime.ToString("yyyy-MM-dd HH-mm-ss"),
                      clamp.OutOvenTemp.ToString("#0.0"),
                      clamp.WaterContent.ToString("#0.0"),
                      clamp.ClampTorsion.ToString("#0.0"),
                      clamp.Code
                      );
                    object test_time = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
                    object[] addParams = new object[] { device, workstation_sn, emp_no, mo_number, product_sn, contaion_sn, union_list, error_code, error_value, error_qty, ec_flg, sn_qty, test_item_list, test_time };

                    //参数赋值并调用方法
                    object methodInfoReturn = methodInfo.Invoke(obj, addParams);

                    msg = methodInfoReturn.ToString();

                    if (msg != "OK")
                    {
                        return false;
                    }

                    LogHelper.WriteInfo(string.Format("MES上传完成，{0}", battery.Code));
                }
                clamp.IsUploaded = true;
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return false;
        }

        public static bool GetUserName(string userNumber, out string userName, out string msg)
        {
            userName = "";
            msg = "";
            return false;
        }

        #endregion
    }
}
