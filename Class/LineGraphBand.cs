using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;

namespace Alarmlines.Class
{
    public class LineGraphBandDemo : GraphBase
    {
        public LineGraphBandDemo() : base("A demo of a bar graph with a region highlighted.",
                                            "Line Graph Band Demo", DemoType.Line)
        {
            GraphPane myPane = base.GraphPane;

            // Set the title and axis labels
            myPane.Title.Text = "Line Graph with Band Demo";
            myPane.XAxis.Title.Text = "Sequence";
            myPane.YAxis.Title.Text = "Temperature, C";

            // Enter some random data values
            double[] y = { 100, 115, 75, 22, 98, 40, 10 };
            double[] y2 = { 90, 100, 95, 35, 80, 35, 35 };
            double[] y3 = { 80, 110, 65, 15, 54, 67, 18 };
            double[] x = { 100, 200, 300, 400, 500, 600, 700 };

            // Fill the axis background with a color gradient
            myPane.Chart.Fill = new Fill(Color.FromArgb(255, 255, 245), Color.FromArgb(255, 255, 190), 90F);

            // Generate a red curve with "Curve 1" in the legend
            LineItem myCurve = myPane.AddCurve("Curve 1", x, y, Color.Red);
            // Make the symbols opaque by filling them with white
            myCurve.Symbol.Fill = new Fill(Color.White);

            // Generate a blue curve with "Curve 2" in the legend
            myCurve = myPane.AddCurve("Curve 2", x, y2, Color.Blue);
            // Make the symbols opaque by filling them with white
            myCurve.Symbol.Fill = new Fill(Color.White);

            // Generate a green curve with "Curve 3" in the legend
            myCurve = myPane.AddCurve("Curve 3", x, y3, Color.Green);
            // Make the symbols opaque by filling them with white
            myCurve.Symbol.Fill = new Fill(Color.White);

            // Manually set the x axis range
            myPane.XAxis.Scale.Min = 0;
            myPane.XAxis.Scale.Max = 800;
            // Display the Y axis grid lines
            myPane.YAxis.MajorGrid.IsVisible = true;
            myPane.YAxis.MinorGrid.IsVisible = true;

            // Draw a box item to highlight a value range
            BoxObj box = new BoxObj(0, 100, 800, 30, Color.Empty,
                    Color.FromArgb(150, Color.LightGreen));
            box.Fill = new Fill(Color.White, Color.FromArgb(200, Color.LightGreen), 45.0F);
            // Use the BehindGrid zorder to draw the highlight beneath the grid lines
            box.ZOrder = ZOrder.F_BehindGrid;
            myPane.GraphObjList.Add(box);

            // Add a text item to label the highlighted range
            TextObj text = new TextObj("Optimal\nRange", 750, 85, CoordType.AxisXYScale,
                                    AlignH.Right, AlignV.Center);
            text.FontSpec.Fill.IsVisible = false;
            text.FontSpec.Border.IsVisible = false;
            text.FontSpec.IsBold = true;
            text.FontSpec.IsItalic = true;
            myPane.GraphObjList.Add(text);
        }
    }
}
