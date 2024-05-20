using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace Alarmlines
{
    public class ComChart
    {
        public static void ShowValueAtPoint(GraphPane myPane, LineItem myLine, bool isY2axis, Color color, int offset, int size, string unit, bool showALL)
        {
            //tinh toan scale cho truc Y2
            double scale = 0;
            double scaleOffet = 0;
            if (isY2axis == true)
            {
                scale = myPane.YAxis.Scale.Max / myPane.Y2Axis.Scale.Max;
                scaleOffet = scale * myPane.Y2Axis.Scale.Max * offset / 100;
            }
            else
            {
                scale = 1;
                scaleOffet = myPane.YAxis.Scale.Max * offset / 100;
            }
            int finalDisplayIndex = 0;
            for (int i = myLine.Points.Count - 1; i >= 0; i--)
            {
                if (myLine.Points[i].Y > 0)
                {
                    finalDisplayIndex = i;
                    break;
                }
            }

            //10.3) hiển thị thông tin value của Line
            for (int i = 0; i < myLine.Points.Count; i++)
            {
                // Get the pointpair
                PointPair pt = myLine.Points[i];

                // Create a text label from the Y data value

                //chuyen doi don vi
                double value = 0;
                if (unit == "M") value = (pt.Y / 1000000);
                else if (unit == "K") value = (pt.Y / 1000);
                else value = pt.Y;

                TextObj text = new TextObj(value.ToString("F0") + unit, pt.X, scale * pt.Y + scaleOffet, CoordType.AxisXYScale, AlignH.Left, AlignV.Center);
                text.ZOrder = ZOrder.A_InFront;
                // Hide the border and the fill
                text.FontSpec.Border.IsVisible = false;
                text.FontSpec.Fill.IsVisible = false;
                text.FontSpec.Size = size;
                text.FontSpec.Fill = new Fill(Color.FromArgb(150, color));

                text.FontSpec.FontColor = Color.Black;
                // Rotate the text to 90 degrees
                text.FontSpec.Angle = 0;

                if (showALL == true)
                {
                    if (value > 0) myPane.GraphObjList.Add(text);
                }
                else
                {
                    if (value > 0 && (i % 2 == 0 || i == finalDisplayIndex))
                    {
                        myPane.GraphObjList.Add(text);
                    }
                }
            }
        }

        public static void showValue(GraphPane myPane, LineItem myLine, Color _color, string unit)
        {
            //10.3) hiển thị thông tin value của Line
            const double offset = 0.2;
            for (int i = 0; i < myLine.Points.Count; i++)
            {
                // Get the pointpair
                PointPair pt = myLine.Points[i];

                // Create a text label from the Y data value
                string _unit = "";
                if (unit == "PSC") _unit = "";
                if (unit == "%") _unit = "%";
                if (unit == "PPM") _unit = "P";
                TextObj text = new TextObj(pt.Y.ToString("F1") + _unit, pt.X, pt.Y + offset, CoordType.AxisXYScale, AlignH.Left, AlignV.Center);
                text.ZOrder = ZOrder.A_InFront;
                // Hide the border and the fill
                text.FontSpec.Border.IsVisible = false;
                text.FontSpec.Fill.IsVisible = false;
                text.FontSpec.Size = 8;

                text.FontSpec.Fill = new Fill(Color.FromArgb(150, _color));

                // Rotate the text to 90 degrees
                text.FontSpec.Angle = 0;

                if (myLine.Points[i].Y != 0)
                    myPane.GraphObjList.Add(text);
            }
        }
        //Vẽ biểu đồ dạng line cho một list các khu vực
        public static void Report_LineGraphic(ZedGraphControl zgc, List<Xaxis_YLocation> _LstReport_AllCustomer, ChartProperty p)
        {
            GraphPane myPane = zgc.GraphPane;

            //1) Cài đặt thông tin các trục X,Y (Set the titles and axis labels)
            myPane.Title.Text = p.title;
            //myPane.XAxis.Title.IsVisible = false;
            myPane.XAxis.Title.Text = p.titleX;
            myPane.YAxis.Title.Text = p.titleY;

            //1.1) cài đặt phông nền cho biểu đồ Fill the Axis and Pane backgrounds
            myPane.Chart.Fill = new Fill(Color.White, Color.FromArgb(255, 255, 240), 90F);
            myPane.Fill = new Fill(Color.FromArgb(250, 250, 255));

            //3) Cài đặt Scale trên các trục (cài đặt độ phân giải + min/max các trục)
            myPane.YAxis.Scale.MaxAuto = true;
            //myPane.YAxis.Scale.MinAuto  = true;
            myPane.YAxis.Scale.Min = 0;
            //myPane.YAxis.Scale.Max = 1000;
            //myPane.YAxis.Scale.MinorStep  = 20;
            //myPane.YAxis.Scale.MajorStep  = 100;

            //4) Cài đặt vị trí && thuộc tính của legend (tiêu đề các series)
            myPane.Legend.Position = ZedGraph.LegendPos.TopCenter;
            myPane.Legend.Border.Style = System.Drawing.Drawing2D.DashStyle.Dot;
            myPane.Legend.FontSpec.Size = 10;

            //#region tính toán 
            //5) lấy thông tin giá trị của từng series (thông thường trục X cố định)
            CalLocation res = CalculatorChart.CalculatorLocation(_LstReport_AllCustomer, p.selectObj);
            Color _color = new Color();
            LineItem myLine = new LineItem("");
            if (p.selectObj == "BIVN" || p.selectObj == "ALL")
            {
                #region 1) vẽ biểu đồ Line (BIVN)
                _color = Color.Blue;
                myLine = myPane.AddCurve("BIVN", null, res.BIVN, _color, SymbolType.Circle);
                //7.1) setup thuộc tính của Line
                myLine.Symbol.Fill = new Fill(Color.Red);   //điền mầu cho điểm:SymbolType.Circle tại các điểm
                myLine.Line.Width = 2.0F;                   //độ rộng của đường Line
                showValue(myPane, myLine, _color, "PSC");
                #endregion
            }
            if (p.selectObj == "CVN" || p.selectObj == "ALL")
            {
                #region 2) vẽ biểu đồ Line (CVN)
                _color = Color.Red;
                myLine = myPane.AddCurve("CVN", null, res.CVN, _color, SymbolType.Circle);
                //7.1) setup thuộc tính của Line
                myLine.Symbol.Fill = new Fill(Color.Red);   //điền mầu cho điểm:SymbolType.Circle tại các điểm
                myLine.Line.Width = 2.0F;                   //độ rộng của đường Line
                showValue(myPane, myLine, _color, "PSC");
                #endregion
            }
            if (p.selectObj == "YASKAWA" || p.selectObj == "ALL")
            {
                #region 3) vẽ biểu đồ Line (Yasukawa)
                _color = Color.Purple;
                myLine = myPane.AddCurve("YASKAWA", null, res.YASKAWA, _color, SymbolType.Circle);
                //7.1) setup thuộc tính của Line
                myLine.Symbol.Fill = new Fill(Color.Red);   //điền mầu cho điểm:SymbolType.Circle tại các điểm
                myLine.Line.Width = 2.0F;                   //độ rộng của đường Line
                showValue(myPane, myLine, _color, "PSC");
                #endregion
            }
            if (p.selectObj == "AUTO" || p.selectObj == "ALL")
            {
                #region  vẽ biểu đồ Line (AUTO)
                _color = Color.Yellow;
                myLine = myPane.AddCurve("AUTO", null, res.AUTO, _color, SymbolType.Circle);
                //7.1) setup thuộc tính của Line
                myLine.Symbol.Fill = new Fill(Color.Red);   //điền mầu cho điểm:SymbolType.Circle tại các điểm
                myLine.Line.Width = 2.0F;                   //độ rộng của đường Line
                showValue(myPane, myLine, _color, "PSC");
                #endregion
            }
            //Tính toán tự động Scale (Calculate the Axis Scale Ranges)
            zgc.AxisChange();

            //10) cài đặt hiển thị label value các trục
            //10.1) label value trục X
            myPane.XAxis.Type = ZedGraph.AxisType.Text;
            myPane.XAxis.Scale.TextLabels = res.Xaxis;
            myPane.XAxis.MajorTic.IsBetweenLabels = true;

            //Tính toán tự động Scale (Calculate the Axis Scale Ranges)
            zgc.AxisChange();
        }
        //Vẽ biểu đồ dạng cột cho một list các khách hàng hoặc một khách hàng theo thời gian
        public static void Report_BarGraphic(ZedGraphControl zgc, List<Xaxis_YLocation> _LstReport, ChartProperty p)
        {

            GraphPane myPane = zgc.GraphPane;

            //1) Cài đặt thông tin các trục X,Y (Set the titles and axis labels)
            myPane.Title.Text = p.title;
            //Khong hien thi Title truc X
            //myPane.XAxis.Title.IsVisible = false;
            myPane.XAxis.Title.Text = p.titleX;
            myPane.YAxis.Title.Text = p.titleY;

            //1.1) cài đặt phông nền cho biểu đồ Fill the Axis and Pane backgrounds
            myPane.Chart.Fill = new Fill(Color.White, Color.FromArgb(255, 255, 166), 90F);
            myPane.Fill = new Fill(Color.FromArgb(250, 250, 255));
            //3) Cài đặt Scale trên các trục (cài đặt độ phân giải + min/max các trục)
            myPane.YAxis.Scale.MaxAuto = true;
            //myPane.YAxis.Scale.MinAuto  = true;
            myPane.YAxis.Scale.Min = 0;
            //myPane.YAxis.Scale.Max = 1000;
            //myPane.YAxis.Scale.MinorStep  = 20;
            //myPane.YAxis.Scale.MajorStep  = 100;
            //4) Cài đặt vị trí && thuộc tính của legend (tiêu đề các series)
            myPane.Legend.Position = ZedGraph.LegendPos.TopCenter;
            myPane.Legend.Border.Style = System.Drawing.Drawing2D.DashStyle.Dot;
            myPane.Legend.FontSpec.Size = p.FontSize;

            //#region tính toán 
            CalLocation res = CalculatorChart.CalculatorLocation(_LstReport, p.selectObj);
            ////1) vẽ biểu đồ Line (Total)
            //Color _color = Color.Red;
            //LineItem myLine = myPane.AddCurve("Total", null, res.Total, _color, SymbolType.Circle);
            ////7.1) setup thuộc tính của Line
            //myLine.Symbol.Fill = new Fill(Color.Blue);   //điền mầu cho điểm: SymbolType.Circle tại các điểm
            //myLine.Line.Width = 2.0F;                   //độ rộng của đường Line
            //ShowValueAtPoint(myPane, myLine, false, Color.Lime, 0, p.FontSize, "", true);

            //1) vẽ biểu đồ Line (Total)
            Color _color = Color.Blue;
            LineItem myLine1 = myPane.AddCurve("Total", null, res.Total, _color, SymbolType.Circle);
            //7.1) setup thuộc tính của Line
            myLine1.Symbol.Fill = new Fill(Color.Red);   //điền mầu cho điểm: SymbolType.Circle tại các điểm
            myLine1.Line.Width = 3.0F;                   //độ rộng của đường Line
            ShowValueAtPoint(myPane, myLine1, false, Color.GreenYellow, 0, p.FontSize, "", true);
            //Chon doi tuong Bar
            BarItem myBar = new BarItem("");
            //1) vẽ biểu đồ Line (BIVN)
            if (p.selectObj == "BROTHER"/* || p.selectObj == "ALL"*/)
            {
                _color = Color.Red;
                myBar = myPane.AddBar("BIVN DAY", null, res.BIVN_AM, _color);
                myBar.Bar.Fill = new Fill(_color, _color, _color);
            }

            if (p.selectObj == "BROTHER"/* || p.selectObj == "ALL"*/)
            {
                _color = Color.Pink;
                myBar = myPane.AddBar("BIVN NIGHT", null, res.BIVN_PM, _color);
                myBar.Bar.Fill = new Fill(_color, _color, _color);
            }
            //2) vẽ biểu đồ Line (CVN)
            if (p.selectObj == "CANON" /*|| p.selectObj == "ALL"*/)
            {
                _color = Color.Lime;
                myBar = myPane.AddBar("CANON DAY", null, res.CVN_AM, _color);
                myBar.Bar.Fill = new Fill(_color, _color, _color);
            }

            if (p.selectObj == "CANON" /*|| p.selectObj == "ALL"*/)
            {
                _color = Color.DarkGoldenrod;
                myBar = myPane.AddBar("CANON NIGHT", null, res.CVN_PM, _color);
                myBar.Bar.Fill = new Fill(_color, _color, _color);
            }
            //3) vẽ biểu đồ Line (AUTO)
            if (p.selectObj == "AUTO1" /*|| p.selectObj == "ALL"*/)
            {
                _color = Color.Yellow;
                myBar = myPane.AddBar("AUTO1 DAY", null, res.AUTO1_AM, _color);
                myBar.Bar.Fill = new Fill(_color, _color, _color);
            }

            if (p.selectObj == "AUTO2" /*|| p.selectObj == "ALL"*/)
            {
                _color = Color.DarkGoldenrod;
                myBar = myPane.AddBar("AUTO2 DAY", null, res.AUTO2_AM, _color);
                myBar.Bar.Fill = new Fill(_color, _color, _color);
            }

            if (p.selectObj == "AUTO1" /*|| p.selectObj == "ALL"*/)
            {
                _color = Color.BurlyWood;
                myBar = myPane.AddBar("AUTO1 NIGHT", null, res.AUTO1_PM, _color);
                myBar.Bar.Fill = new Fill(_color, _color, _color);
            }

            if (p.selectObj == "AUTO2" /*|| p.selectObj == "ALL"*/)
            {
                _color = Color.Brown;
                myBar = myPane.AddBar("AUTO2 NIGHT", null, res.AUTO2_PM, _color);
                myBar.Bar.Fill = new Fill(_color, _color, _color);
            }
            //4) vẽ biểu đồ Line (Nichicon)
            if (p.selectObj == "YASKAWA" /*|| p.selectObj == "ALL"*/)
            {
                _color = Color.Blue;
                myBar = myPane.AddBar("YASKAWA DAY", null, res.YASKAWA_AM, _color);
                myBar.Bar.Fill = new Fill(_color, _color, _color);
            }
            if (p.selectObj == "YASKAWA" /*|| p.selectObj == "ALL"*/)
            {
                _color = Color.BlueViolet;
                myBar = myPane.AddBar("YASKAWA NIGHT", null, res.YASKAWA_PM, _color);
                myBar.Bar.Fill = new Fill(_color, _color, _color);
            }
            //5) vẽ biểu đồ tổng các khu vực
            if (p.selectObj == "ALL") {
                _color = Color.Red;
                myBar = myPane.AddBar("BIVN", null, res.BIVN, _color);
                myBar.Bar.Fill = new Fill(_color, _color, _color);

                _color = Color.Lime;
                myBar = myPane.AddBar("CANON", null, res.CVN, _color);
                myBar.Bar.Fill = new Fill(_color, _color, _color);

                _color = Color.Yellow;
                myBar = myPane.AddBar("AUTO1", null, res.AUTO1, _color);
                myBar.Bar.Fill = new Fill(_color, _color, _color);

                _color = Color.Brown;
                myBar = myPane.AddBar("AUTO2", null, res.AUTO2, _color);
                myBar.Bar.Fill = new Fill(_color, _color, _color);

                _color = Color.Blue;
                myBar = myPane.AddBar("YASKAWA", null, res.YASKAWA, _color);
                myBar.Bar.Fill = new Fill(_color, _color, _color);
            }

            // Setting kiểu Bar
                myPane.BarSettings.Type = BarType.Stack; // Các BAR cùng 1 cột
            //Tính toán tự động Scale (Calculate the Axis Scale Ranges)
            zgc.AxisChange();
            //10) cài đặt hiển thị label value các trục
            //10.1) label value trục X
            myPane.XAxis.Type = ZedGraph.AxisType.Text;
            myPane.XAxis.Scale.TextLabels = res.Xaxis;
            myPane.XAxis.MajorTic.IsBetweenLabels = true;

            //Tính toán tự động Scale (Calculate the Axis Scale Ranges)
            zgc.AxisChange();
        }

        //Vẽ biểu đồ dạng cột cho một list các khách hàng hoặc một khách hàng theo thời gian
        public static void Report_BarGraphicAnalysis(ZedGraphControl zgc, Xaxis_YAnalysis report, ChartProperty p)
        {

            GraphPane myPane = zgc.GraphPane;

            //1) Cài đặt thông tin các trục X,Y (Set the titles and axis labels)
            myPane.Title.Text = p.title;
            //Khong hien thi Title truc X
            //myPane.XAxis.Title.IsVisible = false;
            myPane.XAxis.Title.Text = p.titleX;
            myPane.YAxis.Title.Text = p.titleY;

            //1.1) cài đặt phông nền cho biểu đồ Fill the Axis and Pane backgrounds
            myPane.Chart.Fill = new Fill(Color.White, Color.FromArgb(255, 255, 166), 90F);
            myPane.Fill = new Fill(Color.FromArgb(250, 250, 255));
            //3) Cài đặt Scale trên các trục (cài đặt độ phân giải + min/max các trục)
            myPane.YAxis.Scale.MaxAuto = true;
            //myPane.YAxis.Scale.MinAuto  = true;
            myPane.YAxis.Scale.Min = 0;
            //myPane.YAxis.Scale.Max = 1000;
            //myPane.YAxis.Scale.MinorStep  = 20;
            //myPane.YAxis.Scale.MajorStep  = 100;
            //4) Cài đặt vị trí && thuộc tính của legend (tiêu đề các series)
            myPane.Legend.Position = ZedGraph.LegendPos.TopCenter;
            myPane.Legend.Border.Style = System.Drawing.Drawing2D.DashStyle.Dot;
            myPane.Legend.FontSpec.Size = p.FontSize;

            //#region tính toán 
            //CalLocation res = CalculatorChart.CalculatorLocation(_LstReport, p.selectObj);
            //1) vẽ biểu đồ Line (Total)
            Color _color = Color.Red;
            //LineItem myLine = myPane.AddCurve(p.selectObj, null, report.Yaxis, _color, SymbolType.Circle);
            //7.1) setup thuộc tính của Line
            ///myLine.Symbol.Fill = new Fill(Color.Blue);   //điền mầu cho điểm: SymbolType.Circle tại các điểm
            //myLine.Line.Width = 2.0F;                   //độ rộng của đường Line
            //ShowValueAtPoint(myPane, myLine, false, Color.Lime, 0, p.FontSize, "", true);

            //1) vẽ biểu đồ Line (Total)
            //LineItem myLine1 = myPane.AddCurve("Total", null, res.Total, _color, SymbolType.Circle);
            //7.1) setup thuộc tính của Line
            //myLine1.Symbol.Fill = new Fill(Color.Red);   //điền mầu cho điểm: SymbolType.Circle tại các điểm
            //myLine1.Line.Width = 3.0F;                   //độ rộng của đường Line
            //ShowValueAtPoint(myPane, myLine1, false, Color.GreenYellow, 0, p.FontSize, "", true);
            //Chon doi tuong Bar
            BarItem myBar = new BarItem("");
            //1) vẽ biểu đồ Line (BIVN)
            //if (p.selectObj == "BROTHER" || p.selectObj == "ALL")
            //{
                _color = Color.Red;
                myBar = myPane.AddBar(p.selectObj , null, report.Yaxis, _color);
                myBar.Bar.Fill = new Fill(_color, _color, _color);
            //}

            //2) vẽ biểu đồ Line (CVN)
            //if (p.selectObj == "CANON" || p.selectObj == "ALL")
            //{
            //    _color = Color.Lime;
            //    myBar = myPane.AddBar("CANON", null, res.CVN, _color);
            //    myBar.Bar.Fill = new Fill(_color, _color, _color);
            //}
            //3) vẽ biểu đồ Line (KYO)
            //if (p.selectObj == "AUTO" || p.selectObj == "ALL")
            //{
            //    _color = Color.Yellow;
            //    myBar = myPane.AddBar("AUTO", null, res.AUTO, _color);
            //    myBar.Bar.Fill = new Fill(_color, _color, _color);
            //}
            //4) vẽ biểu đồ Line (Nichicon)
            //if (p.selectObj == "YASKAWA" || p.selectObj == "ALL")
            //{
            //    _color = Color.Blue;
            //    myBar = myPane.AddBar("YASKAWA", null, res.YASKAWA, _color);
            //    myBar.Bar.Fill = new Fill(_color, _color, _color);
            //}
            // Setting kiểu Bar
            myPane.BarSettings.Type = BarType.Stack; // Các BAR cùng 1 cột
            //Tính toán tự động Scale (Calculate the Axis Scale Ranges)
            zgc.AxisChange();
            //10) cài đặt hiển thị label value các trục
            //10.1) label value trục X
            myPane.XAxis.Type = ZedGraph.AxisType.Text;            
            myPane.XAxis.Scale.TextLabels = report.X_axis;
            myPane.XAxis.MajorTic.IsBetweenLabels = true;
            // Set the labels at an angle so they don't overlap
            myPane.XAxis.Scale.FontSpec.Angle = 90;
            myPane.XAxis.Scale.FontSpec.Size = p.FontSize;
            //10.2) hiển thị thông tin value của các thanh BAR
            BarItem.CreateBarLabels(myPane, false, "F0");

            //Tính toán tự động Scale (Calculate the Axis Scale Ranges)
            zgc.AxisChange();
        }

        //biểu đồ hình tròn
        public static void Piegraphic(ZedGraphControl zgc, List<Piegraphic> lst, string Title)
        {
            GraphPane myPane = zgc.GraphPane;

            //1) Cài đặt thông tin các trục X,Y (Set the titles and axis labels)
            myPane.Title.Text = Title;

            //1.1) cài đặt phông nền cho biểu đồ Fill the Axis and Pane backgrounds
            myPane.Chart.Fill = new Fill(Color.White,
                  Color.FromArgb(255, 255, 166), 90F);
            myPane.Fill = new Fill(Color.FromArgb(250, 250, 255));

            //4) Cài đặt vị trí && thuộc tính của legend (tiêu đề các series)
            myPane.Legend.Position = ZedGraph.LegendPos.BottomCenter;
            myPane.Legend.Border.Style = System.Drawing.Drawing2D.DashStyle.Dot;

            //5) lấy thông tin giá trị của từng series (thông thường trục X cố định)
            string[] Label = new string[lst.Count];
            double[] NGRate = new double[lst.Count];

            double total = 0;
            foreach (var item in lst) total += item.Qty;

            for (int i = 0; i < lst.Count; i++)
            {
                Label[i] = lst[i].Content;
                NGRate[i] = 100 * lst[i].Qty / total;

                int colorseg = 255 - ((255 / lst.Count) * (i + 1));

                //7) vẽ biểu đồ pie (NG Rate)
                PieItem myPie;
                if (i == 0) myPie = myPane.AddPieSlice(NGRate[i], Color.Red, 0.05, Label[i]);
                else if (i == 1) myPie = myPane.AddPieSlice(NGRate[i], Color.Yellow, 0.05, Label[i]);
                else myPie = myPane.AddPieSlice(NGRate[i], Color.FromArgb(255 - colorseg, colorseg, 255 - colorseg), Color.Lime, 0, 0.05, Label[i]);

                myPie.LabelType = PieLabelType.Name_Percent;
                myPie.LabelDetail.IsVisible = true;            // Show label
                myPie.LabelDetail.FontSpec.Size = 25;          // Adjust he font size
                myPie.LabelDetail.FontSpec.Family = "Arial";   // Adjust the font family
            }


            //Tính toán tự động Scale (Calculate the Axis Scale Ranges)
            zgc.AxisChange();
        }
    }
}
