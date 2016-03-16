using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using Filter;

namespace Swim_Algorithm
{
    public partial class Swim_Algorithm : Form
    {
        DataTable Original_ChartLine_DataTable, MovingAverage_DataTable, MedianFilter_DataTable,
                SVM_DataTable, SVM_MA_DataTable, PeakValeey_DataTable;
        string FileName;

        #region Best algorithm global parameter
        //Best algorithm global parameter
        const int MovingAverage_Length = 16;
        const int MovingAverage_Buffer_Size = 480;
        const int MedianFilter_Length = 9;
        const int PreSearch_Length = 48;
        const int Slope_Width = 16;
        const int Peaks_Array_Size = 100;
        const int SampleRate = 16;  // Hz

        bool IsReady = false;

        int Buffer_Ptr = 0;
        int StrokeCnt = 0, StrokeLap = 0;

        int pos_slope_cnt = 0, neg_slope_cnt = 0;

        int PeakCnt = 0;
        int Peaks_Cnt = 0;

        float[] MovingAverage_x = new float[MovingAverage_Buffer_Size], MovingAverage_y = new float[MovingAverage_Buffer_Size], MovingAverage_z = new float[MovingAverage_Buffer_Size];

        struct Peaks
        {
            public int peak_time, valley_time;
            public float peak, valley;
        }

        Peaks[] FoundPeaks = new Peaks[Peaks_Array_Size];

        //Moving average buffer
        float[] Buffer_MA_x, Buffer_MA_y, Buffer_MA_z;
        #endregion


        public Swim_Algorithm()
        {
            InitializeComponent();
        }

        private void OpenFileDialog_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog choofdlog = new OpenFileDialog();
            choofdlog.Filter = "CSV Files (*.csv)|*.csv";
            choofdlog.FilterIndex = 1;
            choofdlog.Multiselect = true;

            if (choofdlog.ShowDialog() == DialogResult.OK)
            {
                //string sFileName = choofdlog.FileName;
                string[] arrAllFiles = choofdlog.FileNames; //used when Multiselect = true    

                for (int i = 0; i < arrAllFiles.Length; i++)
                {
                    //FilePath_textBox.Text = sFileName;
                    FilePath_textBox.Text = arrAllFiles[i];
                    //FileName = choofdlog.SafeFileName;
                    FileName = choofdlog.SafeFileNames[i];

                    //Raw data process to useful data
                    Original_ChartLine_DataTable = ConvertCSVtoDataTable(FilePath_textBox.Text);
                    //MovingAverage_DataTable = Filter.Filter.MovingAverage(Original_ChartLine_DataTable, MovingAverage_Length);
                    MedianFilter_DataTable = Filter.Filter.MedianFilter(Original_ChartLine_DataTable, MedianFilter_Length);
                    MovingAverage_DataTable = Filter.Filter.MovingAverage(MedianFilter_DataTable, MovingAverage_Length);

                    //Original measured vale to Chart Line
                    ChartLine.ChartLine Original_NewChartLine = new ChartLine.ChartLine();
                    Original_NewChartLine.ConvertDataTabletoChartLine(Original_ChartLine_DataTable, FileName);
                    Original_NewChartLine.Text = FileName;
                    if (RawData_checkBox.Checked == true)
                        Original_NewChartLine.Show();
                    //Moving average Chart Line
                    ChartLine.ChartLine DataProcess_ChartLine = new ChartLine.ChartLine();
                    DataProcess_ChartLine.ConvertDataTabletoChartLine(MovingAverage_DataTable, FileName + "    with Moving average");
                    DataProcess_ChartLine.Text = FileName + "    with Moving average";
                    if (MA_checkBox.Checked == true)
                        DataProcess_ChartLine.Show();

                    //Median filter Chart Line
                    //ChartLine.ChartLine MedianFilter_ChartLine = new ChartLine.ChartLine();
                    //MedianFilter_ChartLine.ConvertDataTabletoChartLine(MedianFilter_DataTable, FileName + "    with Median filter");
                    //MedianFilter_ChartLine.Text = FileName + "    with Median filter";
                    //if (MA_checkBox.Checked == true)
                    //    MedianFilter_ChartLine.Show();

                    /*
                    //SVM Chart Line
                    ChartLine.ChartLine SVM_ChartLine = new ChartLine.ChartLine();
                    SVM_ChartLine.ConvertDataTabletoChartLine(SVM_DataTable, FileName + "    with SVM");
                    SVM_ChartLine.Text = FileName + "    with SVM";
                    if (SVM_checkBox.Checked == true)
                        SVM_ChartLine.Show();

                    //SVM + Moving average Chart Line
                    ChartLine.ChartLine SVM_MA_ChartLine = new ChartLine.ChartLine();
                    SVM_MA_ChartLine.ConvertDataTabletoChartLine(SVM_MA_DataTable, FileName + "    with SVM + MA");
                    SVM_MA_ChartLine.Text = FileName + "    with SVM + MA";
                    if (SVM_MA_checkBox.Checked == true)
                        SVM_MA_ChartLine.Show();
                    */
                }
            }
        }

        public DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            string[] ColumnsName = { "X", "Y", "Z" };

            string[] rows = File.ReadAllLines(strFilePath, Encoding.UTF8);
            string[] columns;

            for (int i = 0; i < ColumnsName.Length; i++)
            {
                dt.Columns.Add(ColumnsName[i]);
            }

            for (int i = 0; i < rows.Length; i++)
            {
                DataRow dr = dt.NewRow();
                columns = rows[i].Split(',');
                for (int j = 0; j < columns.Length; j++)
                {
                    dr[j] = columns[j];
                }
                dt.Rows.Add(dr);
            }
            
            return dt;
        }

        public void BestAlgorithm_X(float GSensor_x, float GSensor_y, float GSensor_z, int MA_Length)
        {
            float MA_Buffer_Sum_x = 0, MA_Buffer_Sum_y = 0, MA_Buffer_Sum_z = 0;
            float peak = 0, valley = 0;
            int peak_time = 0, valley_time = 0;

            // 將GSensor資料丟入Buffer_MA
            Buffer_MA_x[Buffer_Ptr % MA_Length] = GSensor_x;
            Buffer_MA_y[Buffer_Ptr % MA_Length] = GSensor_y;
            Buffer_MA_z[Buffer_Ptr % MA_Length] = GSensor_z;

            if (Buffer_Ptr >= MA_Length - 1)
            {
                // 取得MA Buffaer 的總和
                for (int i = 0; i < MA_Length; i++)
                {
                    MA_Buffer_Sum_x = MA_Buffer_Sum_x + Buffer_MA_x[i];
                    MA_Buffer_Sum_y = MA_Buffer_Sum_y + Buffer_MA_y[i];
                    MA_Buffer_Sum_z = MA_Buffer_Sum_z + Buffer_MA_z[i];
                }

                // 取得MovingAverage Buffer 的 Ptr
                int MovingAverage_Buffer_Ptr = (Buffer_Ptr - (MA_Length - 1)) % MovingAverage_Buffer_Size;

                // 取得移動平均數值
                MovingAverage_x[MovingAverage_Buffer_Ptr] = MA_Buffer_Sum_x / MA_Length;
                MovingAverage_y[MovingAverage_Buffer_Ptr] = MA_Buffer_Sum_y / MA_Length;
                MovingAverage_z[MovingAverage_Buffer_Ptr] = MA_Buffer_Sum_z / MA_Length;

                //File.AppendAllText("Algorithm_MA.txt", string.Format("Time:{0}      peak:{1}\r\n", Buffer_Ptr, MovingAverage_y[MovingAverage_Buffer_Ptr]));
            }

            Buffer_Ptr++;

            // 判斷資料是否準備完成可以開始進行演算法判斷
            if (Buffer_Ptr > 48 && IsReady == false)
                IsReady = true;

            // BestAlogorithm開始
            float Slope = 0;

            if (IsReady == true)
            {
                int Slope_r, Slope_l;

                // 取得斜率 right 與 left 點
                Slope_r = (Buffer_Ptr - MA_Length) % MovingAverage_Buffer_Size;
                if ((Slope_r - Slope_Width + 1) >= 0)
                    Slope_l = Slope_r - Slope_Width + 1;
                else
                    Slope_l = MovingAverage_Buffer_Size + (Slope_r - Slope_Width + 1);

                // 取得斜率
                Slope = (MovingAverage_x[Slope_r] - MovingAverage_x[Slope_l]) / Slope_Width;

                // 判斷是否斜率為負->開始找波峰與波谷
                if (pos_slope_cnt > 5 && Slope < 0)
                {
                    // Peak Valley Initial
                    peak = MovingAverage_x[Slope_r];
                    peak_time = Buffer_Ptr - MA_Length;
                    valley = MovingAverage_x[Slope_r];
                    valley_time = 0;

                    for (int i = 0; i < PreSearch_Length; i++)
                    {
                        if (Buffer_Ptr - MA_Length - i > 48)
                        {
                            // 如果往左邊找 PreSearch_Length 個點時,已經遇到上個 Peak 點時離開
                            if (Peaks_Cnt > 0)
                            {
                                if (Buffer_Ptr - MA_Length - i <= FoundPeaks[(Peaks_Cnt - 1) % Peaks_Array_Size].peak_time)
                                    break;
                            }

                            if ((Slope_r - i) >= 0)
                            {
                                // 當 Slope_r 往左邊找 PreSearch_Length 個點時, > 0

                                // Peak Valley find
                                if (MovingAverage_x[Slope_r - i] > peak)
                                {
                                    if (Buffer_Ptr - MA_Length - i >= valley_time || i < Slope_Width)
                                    {
                                        peak = MovingAverage_x[Slope_r - i];
                                        peak_time = Buffer_Ptr - MA_Length - i;

                                        // 觸發時間往左超過 Slope_Width 個Sample 且 valley 尚未找到任一個的話, 將valley_time移至'當前移動時間'
                                        if (Buffer_Ptr - MA_Length - peak_time > Slope_Width && valley_time == 0)
                                            valley_time = Buffer_Ptr - MA_Length - i;
                                    }
                                }
                                if (MovingAverage_x[Slope_r - i] < valley)
                                {
                                    valley = MovingAverage_x[Slope_r - i];
                                    valley_time = Buffer_Ptr - MA_Length - i;
                                }
                            }
                            else
                            {
                                // 當 Slope_r 往左邊找 PreSearch_Length 個點時, < 0

                                // Peak Valley find
                                if (MovingAverage_x[MovingAverage_Buffer_Size + Slope_r - i] > peak)
                                {
                                    if (Buffer_Ptr - MA_Length - i >= valley_time || i < Slope_Width)
                                    {
                                        peak = MovingAverage_x[MovingAverage_Buffer_Size + Slope_r - i];
                                        peak_time = Buffer_Ptr - MA_Length - i;

                                        // 觸發時間往左超過 Slope_Width 個Sample 且 valley 尚未找到任一個的話, 將valley_time移至'當前移動時間'
                                        if (Buffer_Ptr - MA_Length - peak_time > Slope_Width && valley_time == 0)
                                            valley_time = Buffer_Ptr - MA_Length - i;
                                    }
                                }
                                if (MovingAverage_x[MovingAverage_Buffer_Size + Slope_r - i] < valley)
                                {
                                    valley = MovingAverage_x[MovingAverage_Buffer_Size + Slope_r - i];
                                    valley_time = Buffer_Ptr - MA_Length - i;
                                }
                            }
                        }
                    }

                    // 波峰波谷 > 0.4 (Threshold) 進行 次數判斷
                    if (Math.Abs(peak - valley) > 0.4)
                    {
                        FoundPeaks[Peaks_Cnt % Peaks_Array_Size].peak = peak;
                        FoundPeaks[Peaks_Cnt % Peaks_Array_Size].peak_time = peak_time;
                        FoundPeaks[Peaks_Cnt % Peaks_Array_Size].valley = valley;
                        FoundPeaks[Peaks_Cnt % Peaks_Array_Size].valley_time = valley_time;

                        //Lap_Alogorithm();
                        File.AppendAllText("Lap_X.csv", string.Format("{0},{1},{2},{3}\r\n", peak, peak_time, valley, valley_time));

                        if (Peaks_Cnt > 0)
                        {
                            // 偵測到連續兩個有效的波峰才進行Stroke++, 並且兩個波峰的時間必須在閥值內.
                            if (((Peaks_Cnt % Peaks_Array_Size) - 1) >= 0)
                            {
                                if (FoundPeaks[Peaks_Cnt % Peaks_Array_Size].peak_time - FoundPeaks[Peaks_Cnt % Peaks_Array_Size - 1].peak_time < SampleRate * 5 &&
                                    FoundPeaks[Peaks_Cnt % Peaks_Array_Size].peak_time - FoundPeaks[Peaks_Cnt % Peaks_Array_Size - 1].peak_time >= SampleRate * 1)
                                {
                                    StrokeCnt++;
                                    File.AppendAllText("PeakPoint.txt", string.Format("Stroke Detected! "));
                                }
                            }
                            else
                            {
                                if (FoundPeaks[Peaks_Cnt % Peaks_Array_Size].peak_time - FoundPeaks[Peaks_Array_Size - 1].peak_time < SampleRate * 5 &&
                                    FoundPeaks[Peaks_Cnt % Peaks_Array_Size].peak_time - FoundPeaks[Peaks_Array_Size - 1].peak_time >= SampleRate * 1)
                                {
                                    StrokeCnt++;
                                    File.AppendAllText("PeakPoint.txt", string.Format("Stroke Detected! "));
                                }
                            }

                            // 將不符合有效次數的StrokeCnt減去
                            //if (((Peaks_Cnt % Peaks_Array_Size) - 1) >= 0)
                            //{
                            //    if (FoundPeaks[Peaks_Cnt % Peaks_Array_Size].peak_time - FoundPeaks[Peaks_Cnt % Peaks_Array_Size - 1].peak_time > SampleRate * 10 ||
                            //        FoundPeaks[Peaks_Cnt % Peaks_Array_Size].peak_time - FoundPeaks[Peaks_Cnt % Peaks_Array_Size - 1].peak_time < SampleRate * 1)
                            //    {
                            //        StrokeCnt--;
                            //        File.AppendAllText("PeakPoint.txt", string.Format("========== Stroke-- ==========\r\n"));
                            //    }
                            //}
                            //else
                            //{
                            //    if (FoundPeaks[Peaks_Cnt % Peaks_Array_Size].peak_time - FoundPeaks[Peaks_Array_Size - 1].peak_time > SampleRate * 10 ||
                            //        FoundPeaks[Peaks_Cnt % Peaks_Array_Size].peak_time - FoundPeaks[Peaks_Array_Size - 1].peak_time < SampleRate * 1)
                            //    {
                            //        StrokeCnt--;
                            //        File.AppendAllText("PeakPoint.txt", string.Format("========== Stroke-- ==========\r\n"));
                            //    }
                            //}
                        }

                        Peaks_Cnt++;
                        //StrokeCnt++;

                        File.AppendAllText("PeakPoint.txt", string.Format("Peak time:{0}     peak:{1}       Valley time:{2}     valley:{3}       Occur time:{4}\r\n", peak_time, peak, valley_time, valley, Buffer_Ptr - MA_Length));
                    }
                }

                if (Slope >= 0)
                    pos_slope_cnt++;
                else
                {
                    pos_slope_cnt = 0;
                    neg_slope_cnt++;
                }

                File.AppendAllText("Slope.txt", string.Format("Buffer_Ptr:{0}   Time:{1}    Slope:{2}   MovingAverage_x[{3}]:{4}    MovingAverage_x[{5}]:{6}\r\n", Buffer_Ptr, Slope_r, Slope, Buffer_Ptr - MA_Length, MovingAverage_x[Slope_r], Buffer_Ptr - MA_Length - Slope_Width, MovingAverage_x[Slope_l]));
                //File.AppendAllText("Slope.txt", string.Format("Time:{0}      peak:{1}\r\n", Buffer_Ptr - MA_Length, MovingAverage_x[Slope_r]));
            }
        }

        public void BestAlgorithm_Y(float GSensor_x, float GSensor_y, float GSensor_z, int MA_Length)
        {
            float MA_Buffer_Sum_x = 0, MA_Buffer_Sum_y = 0, MA_Buffer_Sum_z = 0;
            float peak = 0, valley = 0;
            int peak_time = 0, valley_time = 0;

            // 將GSensor資料丟入Buffer_MA
            Buffer_MA_x[Buffer_Ptr % MA_Length] = GSensor_x;
            Buffer_MA_y[Buffer_Ptr % MA_Length] = GSensor_y;
            Buffer_MA_z[Buffer_Ptr % MA_Length] = GSensor_z;

            if (Buffer_Ptr >= MA_Length - 1)
            {
                // 取得MA Buffaer 的總和
                for (int i = 0; i < MA_Length; i++)
                {
                    MA_Buffer_Sum_x = MA_Buffer_Sum_x + Buffer_MA_x[i];
                    MA_Buffer_Sum_y = MA_Buffer_Sum_y + Buffer_MA_y[i];
                    MA_Buffer_Sum_z = MA_Buffer_Sum_z + Buffer_MA_z[i];
                }

                // 取得MovingAverage Buffer 的 Ptr
                int MovingAverage_Buffer_Ptr = (Buffer_Ptr - (MA_Length - 1)) % MovingAverage_Buffer_Size;

                // 取得移動平均數值
                MovingAverage_x[MovingAverage_Buffer_Ptr] = MA_Buffer_Sum_x / MA_Length;
                MovingAverage_y[MovingAverage_Buffer_Ptr] = MA_Buffer_Sum_y / MA_Length;
                MovingAverage_z[MovingAverage_Buffer_Ptr] = MA_Buffer_Sum_z / MA_Length;

                //File.AppendAllText("Algorithm_MA.txt", string.Format("Time:{0}      peak:{1}\r\n", Buffer_Ptr, MovingAverage_y[MovingAverage_Buffer_Ptr]));
            }

            Buffer_Ptr++;

            // 判斷資料是否準備完成可以開始進行演算法判斷
            if (Buffer_Ptr > 48 && IsReady == false)
                IsReady = true;

            // BestAlogorithm開始
            float Slope = 0;

            if (IsReady == true)
            {
                int Slope_r, Slope_l;

                // 取得斜率 right 與 left 點
                Slope_r = (Buffer_Ptr - MA_Length) % MovingAverage_Buffer_Size;
                if ((Slope_r - Slope_Width + 1) >= 0)
                    Slope_l = Slope_r - Slope_Width + 1;
                else
                    Slope_l = MovingAverage_Buffer_Size + (Slope_r - Slope_Width + 1);

                // 取得斜率
                Slope = (MovingAverage_y[Slope_r] - MovingAverage_y[Slope_l]) / Slope_Width;

                // 判斷是否斜率為負->開始找波峰與波谷
                if (pos_slope_cnt > 5 && Slope < 0)
                {
                    // Peak Valley Initial
                    peak = MovingAverage_y[Slope_r];
                    peak_time = Buffer_Ptr - MA_Length;
                    valley = MovingAverage_y[Slope_r];
                    valley_time = 0;

                    for (int i = 0; i < PreSearch_Length; i++)
                    {
                        if (Buffer_Ptr - MA_Length - i > 48)
                        {
                            // 如果往左邊找 PreSearch_Length 個點時,已經遇到上個 Peak 點時離開
                            if (Peaks_Cnt > 0)
                            {
                                if (Buffer_Ptr - MA_Length - i <= FoundPeaks[(Peaks_Cnt - 1) % Peaks_Array_Size].peak_time)
                                    break;
                            }

                            if ((Slope_r - i) >= 0)
                            {
                                // 當 Slope_r 往左邊找 PreSearch_Length 個點時, > 0

                                // Peak Valley find
                                if (MovingAverage_y[Slope_r - i] > peak)
                                {
                                    if (Buffer_Ptr - MA_Length - i >= valley_time || i < Slope_Width)
                                    {
                                        peak = MovingAverage_y[Slope_r - i];
                                        peak_time = Buffer_Ptr - MA_Length - i;

                                        // 觸發時間往左超過 Slope_Width 個Sample 且 valley 尚未找到任一個的話, 將valley_time移至'當前移動時間'
                                        if (Buffer_Ptr - MA_Length - peak_time > Slope_Width && valley_time == 0)
                                            valley_time = Buffer_Ptr - MA_Length - i;
                                    }
                                }
                                if (MovingAverage_y[Slope_r - i] < valley)
                                {
                                    valley = MovingAverage_y[Slope_r - i];
                                    valley_time = Buffer_Ptr - MA_Length - i;
                                }
                            }
                            else
                            {
                                // 當 Slope_r 往左邊找 PreSearch_Length 個點時, < 0

                                // Peak Valley find
                                if (MovingAverage_y[MovingAverage_Buffer_Size + Slope_r - i] > peak)
                                {
                                    if (Buffer_Ptr - MA_Length - i >= valley_time || i < Slope_Width)
                                    {
                                        peak = MovingAverage_y[MovingAverage_Buffer_Size + Slope_r - i];
                                        peak_time = Buffer_Ptr - MA_Length - i;

                                        // 觸發時間往左超過 Slope_Width 個Sample 且 valley 尚未找到任一個的話, 將valley_time移至'當前移動時間'
                                        if (Buffer_Ptr - MA_Length - peak_time > Slope_Width && valley_time == 0)
                                            valley_time = Buffer_Ptr - MA_Length - i;
                                    }
                                }
                                if (MovingAverage_y[MovingAverage_Buffer_Size + Slope_r - i] < valley)
                                {
                                    valley = MovingAverage_y[MovingAverage_Buffer_Size + Slope_r - i];
                                    valley_time = Buffer_Ptr - MA_Length - i;
                                }
                            }
                        }
                    }

                    // 波峰波谷 > 0.4 (Threshold) 進行 次數判斷
                    if (Math.Abs(peak - valley) > 0.4)
                    {
                        FoundPeaks[Peaks_Cnt % Peaks_Array_Size].peak = peak;
                        FoundPeaks[Peaks_Cnt % Peaks_Array_Size].peak_time = peak_time;
                        FoundPeaks[Peaks_Cnt % Peaks_Array_Size].valley = valley;
                        FoundPeaks[Peaks_Cnt % Peaks_Array_Size].valley_time = valley_time;

                        File.AppendAllText("Lap_Y.csv", string.Format("{0},{1},{2},{3}\r\n", peak, peak_time, valley, valley_time));

                        if (Peaks_Cnt > 0)
                        {
                            // 偵測到連續兩個有效的波峰才進行Stroke++, 並且兩個波峰的時間必須在閥值內.
                            if (((Peaks_Cnt % Peaks_Array_Size) - 1) >= 0)
                            {
                                if (FoundPeaks[Peaks_Cnt % Peaks_Array_Size].peak_time - FoundPeaks[Peaks_Cnt % Peaks_Array_Size - 1].peak_time < SampleRate * 5 &&
                                    FoundPeaks[Peaks_Cnt % Peaks_Array_Size].peak_time - FoundPeaks[Peaks_Cnt % Peaks_Array_Size - 1].peak_time >= SampleRate * 1)
                                {
                                    StrokeCnt++;
                                    File.AppendAllText("PeakPoint.txt", string.Format("Stroke Detected! "));
                                }
                            }
                            else
                            {
                                if (FoundPeaks[Peaks_Cnt % Peaks_Array_Size].peak_time - FoundPeaks[Peaks_Array_Size - 1].peak_time < SampleRate * 5 &&
                                    FoundPeaks[Peaks_Cnt % Peaks_Array_Size].peak_time - FoundPeaks[Peaks_Array_Size - 1].peak_time >= SampleRate * 1)
                                {
                                    StrokeCnt++;
                                    File.AppendAllText("PeakPoint.txt", string.Format("Stroke Detected! "));
                                }
                            }

                            // 將不符合有效次數的StrokeCnt減去
                            //if (((Peaks_Cnt % Peaks_Array_Size) - 1) >= 0)
                            //{
                            //    if (FoundPeaks[Peaks_Cnt % Peaks_Array_Size].peak_time - FoundPeaks[Peaks_Cnt % Peaks_Array_Size - 1].peak_time > SampleRate * 10 ||
                            //        FoundPeaks[Peaks_Cnt % Peaks_Array_Size].peak_time - FoundPeaks[Peaks_Cnt % Peaks_Array_Size - 1].peak_time < SampleRate * 1)
                            //    {
                            //        StrokeCnt--;
                            //        File.AppendAllText("PeakPoint.txt", string.Format("========== Stroke-- ==========\r\n"));
                            //    }
                            //}
                            //else
                            //{
                            //    if (FoundPeaks[Peaks_Cnt % Peaks_Array_Size].peak_time - FoundPeaks[Peaks_Array_Size - 1].peak_time > SampleRate * 10 ||
                            //        FoundPeaks[Peaks_Cnt % Peaks_Array_Size].peak_time - FoundPeaks[Peaks_Array_Size - 1].peak_time < SampleRate * 1)
                            //    {
                            //        StrokeCnt--;
                            //        File.AppendAllText("PeakPoint.txt", string.Format("========== Stroke-- ==========\r\n"));
                            //    }
                            //}
                        }

                        Peaks_Cnt++;
                        //StrokeCnt++;

                        File.AppendAllText("PeakPoint.txt", string.Format("Peak time:{0}     peak:{1}       Valley time:{2}     valley:{3}       Occur time:{4}\r\n", peak_time, peak, valley_time, valley, Buffer_Ptr - MA_Length));
                    }
                }

                if (Slope >= 0)
                    pos_slope_cnt++;
                else
                {
                    pos_slope_cnt = 0;
                    neg_slope_cnt++;
                }

                File.AppendAllText("Slope.txt", string.Format("Buffer_Ptr:{0}   Time:{1}    Slope:{2}   MovingAverage_y[{3}]:{4}    MovingAverage_y[{5}]:{6}\r\n", Buffer_Ptr, Slope_r, Slope, Buffer_Ptr - MA_Length, MovingAverage_y[Slope_r], Buffer_Ptr - MA_Length - Slope_Width, MovingAverage_y[Slope_l]));
                //File.AppendAllText("Slope.txt", string.Format("Time:{0}      peak:{1}\r\n", Buffer_Ptr - MA_Length, MovingAverage_y[Slope_r]));
            }
        }

        public void BestAlgorithm_Z(float GSensor_x, float GSensor_y, float GSensor_z, int MA_Length)
        {
            float MA_Buffer_Sum_x = 0, MA_Buffer_Sum_y = 0, MA_Buffer_Sum_z = 0;
            float peak = 0, valley = 0;
            int peak_time = 0, valley_time = 0;

            // 將GSensor資料丟入Buffer_MA
            Buffer_MA_x[Buffer_Ptr % MA_Length] = GSensor_x;
            Buffer_MA_y[Buffer_Ptr % MA_Length] = GSensor_y;
            Buffer_MA_z[Buffer_Ptr % MA_Length] = GSensor_z;

            if (Buffer_Ptr >= MA_Length - 1)
            {
                // 取得MA Buffaer 的總和
                for (int i = 0; i < MA_Length; i++)
                {
                    MA_Buffer_Sum_x = MA_Buffer_Sum_x + Buffer_MA_x[i];
                    MA_Buffer_Sum_y = MA_Buffer_Sum_y + Buffer_MA_y[i];
                    MA_Buffer_Sum_z = MA_Buffer_Sum_z + Buffer_MA_z[i];
                }

                // 取得MovingAverage Buffer 的 Ptr
                int MovingAverage_Buffer_Ptr = (Buffer_Ptr - (MA_Length - 1)) % MovingAverage_Buffer_Size;

                // 取得移動平均數值
                MovingAverage_x[MovingAverage_Buffer_Ptr] = MA_Buffer_Sum_x / MA_Length;
                MovingAverage_y[MovingAverage_Buffer_Ptr] = MA_Buffer_Sum_y / MA_Length;
                MovingAverage_z[MovingAverage_Buffer_Ptr] = MA_Buffer_Sum_z / MA_Length;

                //File.AppendAllText("Algorithm_MA.txt", string.Format("Time:{0}      peak:{1}\r\n", Buffer_Ptr, MovingAverage_y[MovingAverage_Buffer_Ptr]));
            }

            Buffer_Ptr++;

            // 判斷資料是否準備完成可以開始進行演算法判斷
            if (Buffer_Ptr > 48 && IsReady == false)
                IsReady = true;

            // BestAlogorithm開始
            float Slope = 0;

            if (IsReady == true)
            {
                int Slope_r, Slope_l;

                // 取得斜率 right 與 left 點
                Slope_r = (Buffer_Ptr - MA_Length) % MovingAverage_Buffer_Size;
                if ((Slope_r - Slope_Width + 1) >= 0)
                    Slope_l = Slope_r - Slope_Width + 1;
                else
                    Slope_l = MovingAverage_Buffer_Size + (Slope_r - Slope_Width + 1);

                // 取得斜率
                Slope = (MovingAverage_z[Slope_r] - MovingAverage_z[Slope_l]) / Slope_Width;

                // 判斷是否斜率為負->開始找波峰與波谷
                if (pos_slope_cnt > 5 && Slope < 0)
                {
                    // Peak Valley Initial
                    peak = MovingAverage_z[Slope_r];
                    peak_time = Buffer_Ptr - MA_Length;
                    valley = MovingAverage_z[Slope_r];
                    valley_time = 0;

                    for (int i = 0; i < PreSearch_Length; i++)
                    {
                        if (Buffer_Ptr - MA_Length - i > 48)
                        {
                            // 如果往左邊找 PreSearch_Length 個點時,已經遇到上個 Peak 點時離開
                            if (Peaks_Cnt > 0)
                            {
                                if (Buffer_Ptr - MA_Length - i <= FoundPeaks[(Peaks_Cnt - 1) % Peaks_Array_Size].peak_time)
                                    break;
                            }

                            if ((Slope_r - i) >= 0)
                            {
                                // 當 Slope_r 往左邊找 PreSearch_Length 個點時, > 0

                                // Peak Valley find
                                if (MovingAverage_z[Slope_r - i] > peak)
                                {
                                    if (Buffer_Ptr - MA_Length - i >= valley_time || i < Slope_Width)
                                    {
                                        peak = MovingAverage_z[Slope_r - i];
                                        peak_time = Buffer_Ptr - MA_Length - i;

                                        // 觸發時間往左超過 Slope_Width 個Sample 且 valley 尚未找到任一個的話, 將valley_time移至'當前移動時間'
                                        if (Buffer_Ptr - MA_Length - peak_time > Slope_Width && valley_time == 0)
                                            valley_time = Buffer_Ptr - MA_Length - i;
                                    }
                                }
                                if (MovingAverage_z[Slope_r - i] < valley)
                                {
                                    valley = MovingAverage_z[Slope_r - i];
                                    valley_time = Buffer_Ptr - MA_Length - i;
                                }
                            }
                            else
                            {
                                // 當 Slope_r 往左邊找 PreSearch_Length 個點時, < 0

                                // Peak Valley find
                                if (MovingAverage_z[MovingAverage_Buffer_Size + Slope_r - i] > peak)
                                {
                                    if (Buffer_Ptr - MA_Length - i >= valley_time || i < Slope_Width)
                                    {
                                        peak = MovingAverage_z[MovingAverage_Buffer_Size + Slope_r - i];
                                        peak_time = Buffer_Ptr - MA_Length - i;

                                        // 觸發時間往左超過 Slope_Width 個Sample 且 valley 尚未找到任一個的話, 將valley_time移至'當前移動時間'
                                        if (Buffer_Ptr - MA_Length - peak_time > Slope_Width && valley_time == 0)
                                            valley_time = Buffer_Ptr - MA_Length - i;
                                    }
                                }
                                if (MovingAverage_z[MovingAverage_Buffer_Size + Slope_r - i] < valley)
                                {
                                    valley = MovingAverage_z[MovingAverage_Buffer_Size + Slope_r - i];
                                    valley_time = Buffer_Ptr - MA_Length - i;
                                }
                            }
                        }
                    }

                    // 波峰波谷 > 0.4 (Threshold) 進行 次數判斷
                    if (Math.Abs(peak - valley) > 0.4)
                    {
                        FoundPeaks[Peaks_Cnt % Peaks_Array_Size].peak = peak;
                        FoundPeaks[Peaks_Cnt % Peaks_Array_Size].peak_time = peak_time;
                        FoundPeaks[Peaks_Cnt % Peaks_Array_Size].valley = valley;
                        FoundPeaks[Peaks_Cnt % Peaks_Array_Size].valley_time = valley_time;

                        File.AppendAllText("Lap_Z.csv", string.Format("{0},{1},{2},{3}\r\n", peak, peak_time, valley, valley_time));

                        if (Peaks_Cnt > 0)
                        {
                            // 偵測到連續兩個有效的波峰才進行Stroke++, 並且兩個波峰的時間必須在閥值內.
                            if (((Peaks_Cnt % Peaks_Array_Size) - 1) >= 0)
                            {
                                if (FoundPeaks[Peaks_Cnt % Peaks_Array_Size].peak_time - FoundPeaks[Peaks_Cnt % Peaks_Array_Size - 1].peak_time < SampleRate * 5 &&
                                    FoundPeaks[Peaks_Cnt % Peaks_Array_Size].peak_time - FoundPeaks[Peaks_Cnt % Peaks_Array_Size - 1].peak_time >= SampleRate * 1)
                                {
                                    StrokeCnt++;
                                    File.AppendAllText("PeakPoint.txt", string.Format("Stroke Detected! "));
                                }
                            }
                            else
                            {
                                if (FoundPeaks[Peaks_Cnt % Peaks_Array_Size].peak_time - FoundPeaks[Peaks_Array_Size - 1].peak_time < SampleRate * 5 &&
                                    FoundPeaks[Peaks_Cnt % Peaks_Array_Size].peak_time - FoundPeaks[Peaks_Array_Size - 1].peak_time >= SampleRate * 1)
                                {
                                    StrokeCnt++;
                                    File.AppendAllText("PeakPoint.txt", string.Format("Stroke Detected! "));
                                }
                            }

                            // 將不符合有效次數的StrokeCnt減去
                            //if (((Peaks_Cnt % Peaks_Array_Size) - 1) >= 0)
                            //{
                            //    if (FoundPeaks[Peaks_Cnt % Peaks_Array_Size].peak_time - FoundPeaks[Peaks_Cnt % Peaks_Array_Size - 1].peak_time > SampleRate * 10 ||
                            //        FoundPeaks[Peaks_Cnt % Peaks_Array_Size].peak_time - FoundPeaks[Peaks_Cnt % Peaks_Array_Size - 1].peak_time < SampleRate * 1)
                            //    {
                            //        StrokeCnt--;
                            //        File.AppendAllText("PeakPoint.txt", string.Format("========== Stroke-- ==========\r\n"));
                            //    }
                            //}
                            //else
                            //{
                            //    if (FoundPeaks[Peaks_Cnt % Peaks_Array_Size].peak_time - FoundPeaks[Peaks_Array_Size - 1].peak_time > SampleRate * 10 ||
                            //        FoundPeaks[Peaks_Cnt % Peaks_Array_Size].peak_time - FoundPeaks[Peaks_Array_Size - 1].peak_time < SampleRate * 1)
                            //    {
                            //        StrokeCnt--;
                            //        File.AppendAllText("PeakPoint.txt", string.Format("========== Stroke-- ==========\r\n"));
                            //    }
                            //}
                        }

                        Peaks_Cnt++;
                        //StrokeCnt++;

                        File.AppendAllText("PeakPoint.txt", string.Format("Peak time:{0}     peak:{1}       Valley time:{2}     valley:{3}       Occur time:{4}\r\n", peak_time, peak, valley_time, valley, Buffer_Ptr - MA_Length));
                    }
                }

                if (Slope >= 0)
                    pos_slope_cnt++;
                else
                {
                    pos_slope_cnt = 0;
                    neg_slope_cnt++;
                }

                File.AppendAllText("Slope.txt", string.Format("Buffer_Ptr:{0}   Time:{1}    Slope:{2}   MovingAverage_z[{3}]:{4}    MovingAverage_z[{5}]:{6}\r\n", Buffer_Ptr, Slope_r, Slope, Buffer_Ptr - MA_Length, MovingAverage_z[Slope_r], Buffer_Ptr - MA_Length - Slope_Width, MovingAverage_z[Slope_l]));
                //File.AppendAllText("Slope.txt", string.Format("Time:{0}      peak:{1}\r\n", Buffer_Ptr - MA_Length, MovingAverage_z[Slope_r]));
            }
        }

        private void Test_button_Click(object sender, EventArgs e)
        {
            //File.WriteAllText("Algorithm_MA.txt", "");
            File.WriteAllText("Lap_Y.csv", "Peak,Peak Time,Valley,Valley Time\r\n");
            File.WriteAllText("Lap_X.csv", "Peak,Peak Time,Valley,Valley Time\r\n");
            File.WriteAllText("Lap_Z.csv", "Peak,Peak Time,Valley,Valley Time\r\n");
            File.WriteAllText("Slope.txt", "");
            File.WriteAllText("PeakPoint.txt", "                檔案名稱:" + FileName + "\r\n" +
                        "============================================================\r\n");

            // 初始演算法參數
            StrokeCnt = 0; StrokeLap = 0;
            Buffer_Ptr = 0; Peaks_Cnt = 0;
            pos_slope_cnt = 0; neg_slope_cnt = 0;
            IsReady = false;

            // 初始化MovingAverage_x, MovingAverage_y, MovingAverage_z
            MovingAverage_x = new float[MovingAverage_Buffer_Size];
            MovingAverage_y = new float[MovingAverage_Buffer_Size];
            MovingAverage_z = new float[MovingAverage_Buffer_Size];

            // 初始化Moving average buffer
            Buffer_MA_x = new float[MovingAverage_Length];
            Buffer_MA_y = new float[MovingAverage_Length];
            Buffer_MA_z = new float[MovingAverage_Length];

            for (int i = 0; i < Original_ChartLine_DataTable.Rows.Count; i++)
            {
                BestAlgorithm_Y(float.Parse(Original_ChartLine_DataTable.Rows[i][0].ToString()), float.Parse(Original_ChartLine_DataTable.Rows[i][1].ToString()), float.Parse(Original_ChartLine_DataTable.Rows[i][2].ToString())
                    , MovingAverage_Length);
            }

            MessageBox.Show(StrokeCnt.ToString());



            //FoundPeaks = new Peaks[Peaks_Array_Size];
            // 初始演算法參數
            StrokeCnt = 0; StrokeLap = 0;
            Buffer_Ptr = 0; Peaks_Cnt = 0;
            pos_slope_cnt = 0; neg_slope_cnt = 0;
            IsReady = false;

            // 初始化MovingAverage_x, MovingAverage_y, MovingAverage_z
            MovingAverage_x = new float[MovingAverage_Buffer_Size];
            MovingAverage_y = new float[MovingAverage_Buffer_Size];
            MovingAverage_z = new float[MovingAverage_Buffer_Size];

            // 初始化Moving average buffer
            Buffer_MA_x = new float[MovingAverage_Length];
            Buffer_MA_y = new float[MovingAverage_Length];
            Buffer_MA_z = new float[MovingAverage_Length];

            for (int i = 0; i < Original_ChartLine_DataTable.Rows.Count; i++)
            {
                BestAlgorithm_X(float.Parse(Original_ChartLine_DataTable.Rows[i][0].ToString()), float.Parse(Original_ChartLine_DataTable.Rows[i][1].ToString()), float.Parse(Original_ChartLine_DataTable.Rows[i][2].ToString())
                    , MovingAverage_Length);
            }

            //MessageBox.Show(StrokeCnt.ToString());



            //FoundPeaks = new Peaks[Peaks_Array_Size];
            // 初始演算法參數
            StrokeCnt = 0; StrokeLap = 0;
            Buffer_Ptr = 0; Peaks_Cnt = 0;
            pos_slope_cnt = 0; neg_slope_cnt = 0;
            IsReady = false;

            // 初始化MovingAverage_x, MovingAverage_y, MovingAverage_z
            MovingAverage_x = new float[MovingAverage_Buffer_Size];
            MovingAverage_y = new float[MovingAverage_Buffer_Size];
            MovingAverage_z = new float[MovingAverage_Buffer_Size];

            // 初始化Moving average buffer
            Buffer_MA_x = new float[MovingAverage_Length];
            Buffer_MA_y = new float[MovingAverage_Length];
            Buffer_MA_z = new float[MovingAverage_Length];

            for (int i = 0; i < Original_ChartLine_DataTable.Rows.Count; i++)
            {
                BestAlgorithm_Z(float.Parse(Original_ChartLine_DataTable.Rows[i][0].ToString()), float.Parse(Original_ChartLine_DataTable.Rows[i][1].ToString()), float.Parse(Original_ChartLine_DataTable.Rows[i][2].ToString())
                    , MovingAverage_Length);
            }

            //MessageBox.Show(StrokeCnt.ToString());
        }
    }
}
