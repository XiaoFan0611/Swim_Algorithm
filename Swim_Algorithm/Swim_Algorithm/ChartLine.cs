using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

namespace ChartLine
{
    public partial class ChartLine : Form
    {
        public ChartLine()
        {
            InitializeComponent();
        }

        public void ConvertDataTabletoChartLine(DataTable dt, string FileName)
        {
            GSensor_chart.ChartAreas.Clear();
            GSensor_chart.Series.Clear();
            GSensor_chart.Titles.Clear();

            GSensor_chart.ChartAreas.Add("chart");
            GSensor_chart.ChartAreas[0].AxisX.Minimum = 0;
            GSensor_chart.ChartAreas[0].AxisY.Maximum = 2f;
            GSensor_chart.ChartAreas[0].AxisY.Minimum = -2f;

            if (FileName.Contains("SVM"))
            {
                GSensor_chart.ChartAreas[0].AxisY.Maximum = 2.5f;
                GSensor_chart.ChartAreas[0].AxisY.Minimum = 0f;
            }

            Series seriesX = new Series("GSensor X");
            Series seriesY = new Series("GSensor Y");
            Series seriesZ = new Series("GSensor Z");

            seriesX.Color = Color.Blue;
            seriesY.Color = Color.Red;
            seriesZ.Color = Color.Green;

            seriesX.Font = new System.Drawing.Font("標楷體", 12);
            seriesY.Font = new System.Drawing.Font("標楷體", 12);
            seriesZ.Font = new System.Drawing.Font("標楷體", 12);

            seriesX.ChartType = SeriesChartType.Line;
            seriesY.ChartType = SeriesChartType.Line;
            seriesZ.ChartType = SeriesChartType.Line;

            //seriesX.IsValueShownAsLabel = true;
            //seriesZ.IsValueShownAsLabel = true;

            for (int index = 0; index < dt.Rows.Count; index++)
            {
                seriesX.Points.AddXY(index, double.Parse(dt.Rows[index][0].ToString()));
                seriesY.Points.AddXY(index, double.Parse(dt.Rows[index][1].ToString()));
                seriesZ.Points.AddXY(index, double.Parse(dt.Rows[index][2].ToString()));
            }

            GSensor_chart.Series.Add(seriesX);
            GSensor_chart.Series.Add(seriesY);
            GSensor_chart.Series.Add(seriesZ);

            GSensor_chart.ChartAreas[0].AxisX.Interval = 16;
            GSensor_chart.ChartAreas[0].AxisY.Interval = 0.2;
            GSensor_chart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            GSensor_chart.ChartAreas[0].CursorX.AutoScroll = true;
            GSensor_chart.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            GSensor_chart.Series[0].IsXValueIndexed = true;

            GSensor_chart.Titles.Add(FileName);
        }

        Point? prevPosition = null;
        ToolTip tooltip = new ToolTip();
        private void GSensor_chart_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = e.Location;
            if (prevPosition.HasValue && pos == prevPosition.Value)
                return;
            tooltip.RemoveAll();
            prevPosition = pos;
            var results = GSensor_chart.HitTest(pos.X, pos.Y, false,
                                            ChartElementType.DataPoint);
            foreach (var result in results)
            {
                if (result.ChartElementType == ChartElementType.DataPoint)
                {
                    var prop = result.Object as DataPoint;
                    if (prop != null)
                    {
                        var pointXPixel = result.ChartArea.AxisX.ValueToPixelPosition(prop.XValue);
                        var pointYPixel = result.ChartArea.AxisY.ValueToPixelPosition(prop.YValues[0]);

                        // check if the cursor is really close to the point (2 pixels around the point)
                        if (Math.Abs(pos.X - pointXPixel) < 2) //&&
                            //Math.Abs(pos.Y - pointYPixel) < 2)
                        {
                            tooltip.Show("X=" + prop.XValue + ", Y=" + prop.YValues[0], this.GSensor_chart,
                                            pos.X, pos.Y - 15);
                        }
                    }
                }
            }
        }

        private void X_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (X_checkBox.Checked == true)
                GSensor_chart.Series[0].Enabled = true;
            else
                GSensor_chart.Series[0].Enabled = false;
        }

        private void Y_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (Y_checkBox.Checked == true)
                GSensor_chart.Series[1].Enabled = true;
            else
                GSensor_chart.Series[1].Enabled = false;
        }

        private void Z_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (Z_checkBox.Checked == true)
                GSensor_chart.Series[2].Enabled = true;
            else
                GSensor_chart.Series[2].Enabled = false;
        }

    }
}
