#define ISOFFLINE
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using Tafel.MES.TafelWebService;
using TengDa;

namespace Tafel.MES
{

    public class MES
    {
        public static bool CheckUser(MESUser user, out string name, out string msg)
        {
            name = string.Empty;
            string xml = GetXML("MESUser", new string[] { "UserNumber=" + user.UserNumber, "UserPwd=" + user.UserPwd, "ProcessCode=" + user.ProcessCode, "StationCode=" + user.StationCode, "MachineNo=" + user.MachineNo });
            ServiceAPI ws = new ServiceAPI();

#if ISOFFLINE
            string returnXml = @"<?xml version='1.0' encoding='utf-8'?>
                                <response>
                                    <table>
                                        <rows>
                                            <returncode>1</returncode>
                                            <errormsg>MesUser</errormsg>
                                        </rows>
                                    </table>
                                </response>"; 
#else
            string returnXml = string.Empty;
            try
            {
                returnXml = ws.Check_User(xml);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
#endif
            bool b = CheckXML(returnXml, out name, out msg);
            if (!string.IsNullOrEmpty(msg))
            {
                LogHelper.WriteError(msg);
            }

            if (string.IsNullOrEmpty(name))
            {
                name = user.UserNumber;
            }

            return b;
        }

        public static bool CheckSfc(Sfc cs, out string msg)
        {

            string xml = GetXML("Sfc", new string[] { "BarcodeNo=" + cs.BarcodeNo, "ProcessCode=" + cs.ProcessCode, "StationCode=" + cs.StationCode,
                "OrderNo=" + cs.OrderNo, "IPAddress=" + cs.IPAddress, "MaterialOrderNo=" + cs.MaterialOrderNo, "MachineNo=" + cs.MachineNo, "UserNumber=" + cs.UserNumber });
            ServiceAPI ws = new ServiceAPI();

#if ISOFFLINE
            string returnXml = @"<?xml version='1.0' encoding='utf-8'?>
                                <response>
                                    <table>
                                        <rows>
                                            <returncode>1</returncode>
                                            <errormsg>OK</errormsg>
                                        </rows>
                                    </table>
                                </response>";
#else
            string returnXml = string.Empty;
            try
            {
                returnXml = ws.Check_SFC(xml);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
#endif
            bool b= CheckXML(returnXml, out msg);
            if (!string.IsNullOrEmpty(msg))
            {
                LogHelper.WriteError(msg);
            }
            return b;

        }

        public static bool UploadMachineState(MachineState ms, out string msg)
        {

            string xml = GetXML("MachineState", new string[] { "MachineNo=" + ms.MachineNo, "state=" + ms.state, "UserNumber=" + ms.UserNumber });
            ServiceAPI ws = new ServiceAPI();

#if ISOFFLINE
            string returnXml = @"<?xml version='1.0' encoding='utf-8'?>
                                <response>
                                    <table>
                                        <rows>
                                            <returncode>1</returncode>
                                            <errormsg>OK</errormsg>
                                        </rows>
                                    </table>
                                </response>";
#else

            string returnXml = string.Empty;
            try
            {
                returnXml = ws.Insert_MachineNo(xml);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
#endif
            bool b = CheckXML(returnXml, out msg);
            if (!string.IsNullOrEmpty(msg))
            {
                LogHelper.WriteError(msg);
            }
            return b;

        }

        public static bool UploadBattery(TrayInfo taryInfo, out string msg)
        {

            string xml = GetXML("TrayInfo", new string[] { "BarcodeNo=" + taryInfo.BarcodeNo, "TrayCode=" + taryInfo.TrayCode, "ProcessCode=" + taryInfo.ProcessCode, "UserNumber=" + taryInfo.UserNumber, "InputTime=" + taryInfo.InputTime });
            ServiceAPI ws = new ServiceAPI();

#if ISOFFLINE
            string returnXml = @"<?xml version='1.0' encoding='utf-8'?>
                                <response>
                                    <table>
                                        <rows>
                                            <returncode>1</returncode>
                                            <errormsg>OK</errormsg>
                                        </rows>
                                    </table>
                                </response>";
#else

            string returnXml = string.Empty;
            try
            {
                returnXml = returnXml = ws.SetTrayInfo(xml);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
#endif
            bool b = CheckXML(returnXml, out msg);
            if (!string.IsNullOrEmpty(msg))
            {
                LogHelper.WriteError(msg);
            }

            TengDa.LogHelper.WriteInfo("XML上传：\r\n" + xml);
            TengDa.LogHelper.WriteInfo("XML返回：\r\n" + returnXml);
            return b;
        }

        public static ProcessInfo GetProcessInfo(IP ip, out string msg)
        {

            string xml = GetXML("IP", new string[] { "IPAddress=" + ip.IPAddress });

            ServiceAPI ws = new ServiceAPI();

#if ISOFFLINE
            string returnXml = @"<?xml version='1.0' encoding='utf-8'?>
                                <response>
                                    <table>
                                        <rows>
                                            <Process_Code>A22</Process_Code>
                                            <Process_Name>上料</Process_Name>
                                        </rows>
                                    </table>
                                </response>";
#else
            string returnXml = string.Empty;
            try
            {
                returnXml = ws.Get_ProcessInfo(xml);
            }
            catch(Exception ex)
            {
                msg = ex.Message;
                return new ProcessInfo { ProcessCode = "" , ProcessName = ""};
            }

#endif
            ProcessInfo pi = ConvertXMLtoProcess(returnXml, out msg);

            if (!string.IsNullOrEmpty(msg))
            {
                LogHelper.WriteError(msg);
            }
            return pi;
        }

        public static StationInfo GetStationInfo(IpAndProcess iap, out string msg)
        {

            string xml = GetXML("IpAndProcess", new string[] { "IPAddress=" + iap.IPAddress, "ProcessCode=" + iap.ProcessCode });
            ServiceAPI ws = new ServiceAPI();

#if ISOFFLINE
            string returnXml = @"<?xml version='1.0' encoding='utf-8'?>
                                <response>
                                    <table>
                                        <rows>
                                            <Station_Code>B00</Station_Code>
                                            <Station_Name>上料工位</Station_Name>
                                        </rows>
                                    </table>
                                </response>";
#else

            string returnXml = string.Empty;
            try
            {
                returnXml = ws.Get_StationInfo(xml);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return new StationInfo { StationCode = "", StationName = "" };
            }
#endif
            return ConvertXMLtoStation(returnXml, out msg);

        }

        /// <summary>
        /// 根据类名和属性生成要求的XML
        /// </summary>
        /// <param name="className"></param>
        /// <param name="Properties"></param>
        /// <returns></returns>
        private static string GetXML(string className, string[] Properties)
        {
            Request request = new Request();
            request.table = new Table(className);
            Type type = request.table.rows.GetType();

            string _namespace = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace;
            request.table.rows = (BaseClass)type.Assembly.CreateInstance(_namespace + "." + className);


            System.Reflection.PropertyInfo pi;
            for (int i = 0; i < Properties.Length; i++)
            {
                string[] p = Properties[i].Split('=');
                pi = type.GetProperty(p[0]);

                if (pi.PropertyType == typeof(string))
                {
                    pi.SetValue(request.table.rows, p[1], null);
                }
                else if (pi.PropertyType == typeof(State))
                {
                    pi.SetValue(request.table.rows, (State)Enum.Parse(typeof(State), p[1]), null);
                }

            }

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
           // settings.Indent = false;

            MemoryStream mem = new MemoryStream();

            using (XmlWriter writer = XmlWriter.Create(mem, settings))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Request));
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");//去掉命名空间
                serializer.Serialize(writer, request, ns);
            }

            string xml = Encoding.UTF8.GetString(mem.ToArray());
            return Regex.Replace(xml, @"<rows[^>]*>", "<rows>").Replace('"','\'').Substring(1);
        }

        /// <summary>
        /// 根据返回的XML判断是否成功
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private static bool CheckXML(string xml, out string name, out string msg)
        {
            name = string.Empty;
            msg = string.Empty;
            byte[] array = Encoding.UTF8.GetBytes(xml);
            Response rp;
            using (MemoryStream stream = new MemoryStream(array))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Response));
                rp = (Response)serializer.Deserialize(stream);
            }

            if (rp.table.rows.ReturnCode == "1")
            {
                name = "";
                return true;
            }
            else if (rp.table.rows.ReturnCode == "0")
            {
                msg = rp.table.rows.ErrorMsg.Split(' ')[0];
            }
            return false;
        }

        /// <summary>
        /// 根据返回的XML判断是否成功
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private static bool CheckXML(string xml, out string msg)
        {
            string name = string.Empty;
            return CheckXML(xml, out name, out msg);
        }

        /// <summary>
        /// 根据返回的XML反序列化出对象
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private static ProcessInfo ConvertXMLtoProcess(string xml, out string msg)
        {
            msg = string.Empty;

            try
            {
                byte[] array = Encoding.UTF8.GetBytes(xml);
                ResponseProcess rpp;
                using (MemoryStream stream = new MemoryStream(array))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(ResponseProcess));
                    rpp = (ResponseProcess)serializer.Deserialize(stream);
                }

                return rpp.table.rows;

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return new ProcessInfo {  ProcessCode = "", ProcessName = "" };
            }
        }

        /// <summary>
        /// 根据返回的XML判断是否成功
        /// </summary>
        /// <param name="className"></param>
        /// <param name="Properties"></param>
        /// <returns></returns>
        private static StationInfo ConvertXMLtoStation(string xml, out string msg)
        {
            msg = string.Empty;
            try
            {
                byte[] array = Encoding.UTF8.GetBytes(xml);
                ResponseStation rps;
                using (MemoryStream stream = new MemoryStream(array))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(ResponseStation));
                    rps = (ResponseStation)serializer.Deserialize(stream);
                }

                return rps.table.rows;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return new StationInfo { StationCode = "", StationName = "" };
            }

        }
    }
}
