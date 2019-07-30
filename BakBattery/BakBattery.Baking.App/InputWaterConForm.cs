using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TengDa;

namespace BakBattery.Baking.App
{
    public partial class InputWaterConForm : Form
    {
        private Station ovenSamFromStation;
        public InputWaterConForm()
        {
            InitializeComponent();

            this.lbTrayCode.Text = "";
            this.lbFromStation.Text = "";
            this.tbResult.Text = "";
            this.lbTip.Text = "";

            this.lbTrayCode.Text = Current.Transfer.Station.Clamp.Code;
            ovenSamFromStation = Station.StationList.FirstOrDefault(s => s.Id == Current.Transfer.Station.FromStationId);
            if (ovenSamFromStation.GetPutType == GetPutType.烤箱)
            {
                this.lbFromStation.Text = ovenSamFromStation.Name;
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            try
            {


                if (!float.TryParse(this.tbResult.Text.Trim(), out float val))
                {
                    ShowTip("输入有误，请重新输入！", Color.Red);
                    return;
                }

                if (val <= 0f)
                {
                    ShowTip("结果不能小于等于0！", Color.Red);
                }

                if (val < Current.option.WaterContentStandard)
                {
                    Current.Transfer.Station.SampleStatus = SampleStatus.测试OK;
                    Current.Transfer.Station.Clamp.IsOutUploaded = false;
                    Current.Transfer.Station.Clamp.IsOutFinished = true;
                    if (Current.Transfer.Station.FromStationId > 0)
                    {
                        var ovenSamFromStation = Station.StationList.FirstOrDefault(s => s.Id == Current.Transfer.Station.FromStationId);
                        if (ovenSamFromStation.GetPutType == GetPutType.烤箱)
                        {
                            ovenSamFromStation.GetFloor().Stations.ForEach(s =>
                            {
                                s.SampleStatus = SampleStatus.未知;
                                s.Clamp.WaterContent = val;
                            });

                            //该炉腔水分测试OK，下次换该烤箱另一个炉腔测试水分
                            ovenSamFromStation.GetFloor().GetOven().ChangeWaterContentTestFloor();

                            //测试ng次数复位为0
                            ovenSamFromStation.GetFloor().NgTimes = 0;

                            Current.Transfer.Station.Clamp.WaterContent = val;

                            ShowTip("水含量OK结果成功输入系统！", Color.Green);
                        }
                        else
                        {
                            ShowTip("来源工位类型不是烤箱！", Color.Red);
                        }
                    }
                    else
                    {
                        ShowTip(Current.Transfer.Station.Name + "来源工位Id < 1！", Color.Red);
                    }
                }
                else
                {
                    Current.Transfer.Station.SampleStatus = SampleStatus.测试NG;
                    if (Current.Transfer.Station.FromStationId > 0)
                    {
                        var ovenSamFromStation = Station.StationList.FirstOrDefault(s => s.Id == Current.Transfer.Station.FromStationId);
                        ovenSamFromStation.SampleStatus = SampleStatus.测试NG;
                        if (ovenSamFromStation.GetPutType == GetPutType.烤箱)
                        {
                            ovenSamFromStation.GetFloor().Stations.ForEach(s =>
                            {
                                s.Clamp.WaterContent = val;
                            });
                            //水分NG次数增加1
                            ovenSamFromStation.GetFloor().NgTimes++;

                            Current.Transfer.Station.Clamp.WaterContent = val;

                            ShowTip("水含量NG结果成功输入系统！", Color.Green);
                        }
                        else
                        {
                            ShowTip("来源工位类型不是烤箱！", Color.Red);
                        }
                    }
                    else
                    {
                        ShowTip(Current.Transfer.Station.Name + "来源工位Id < 1！", Color.Red);
                    }
                }

            }
            catch (Exception ex)
            {
                Error.Alert(ex);
            }
        }

        private void ShowTip(string msg, Color color)
        {
            this.lbTip.Text = msg;
            this.lbTip.ForeColor = color;
        }

        private void TbResult_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.lbTip.Text = "";
        }
    }
}
