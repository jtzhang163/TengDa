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

namespace BakBattery.Baking
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

        //protected string workstationSn = string.Empty;
        ///// <summary>
        ///// 工作中心SN
        ///// </summary>
        //[DisplayName("工作中心SN")]
        //[Category("参数配置")]
        //public string WorkstationSn
        //{
        //    get
        //    {
        //        return workstationSn;
        //    }
        //    set
        //    {
        //        if (workstationSn != value)
        //        {
        //            UpdateDbField("WorkstationSn", value);
        //        }
        //        workstationSn = value;
        //    }
        //}

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
           //this.workstationSn = rowInfo["WorkstationSn"].ToString();
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


        public bool UploadCellInfo(Clamp clamp, out string msg)
        {
            msg = string.Empty;
            return false;
        }

        #endregion
    }

    /// <summary>
    /// 调用MES后返回数据的对象类型
    /// </summary>
    public class MesResponse
    {
        /// <summary>
        /// 状态
        /// 0：成功
        /// </summary>
        public int status { get; set; }
        public string msg { get; set; }
        public string errorMsg { get; set; }
        public string map { get; set; }
        public string data { get; set; }
    }
}
