using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Filter
{
    class Filter
    {

        public static DataTable MovingAverage(DataTable InputData, int AveragCount)
        {
            DataTable dt = new DataTable();
            string[] ColumnsName = { "X", "Y", "Z" };
            double[] X, Y, Z;
            X = new double[InputData.Rows.Count]; Y = new double[InputData.Rows.Count]; Z = new double[InputData.Rows.Count];

            for (int i = 0; i < ColumnsName.Length; i++)
            {
                dt.Columns.Add(ColumnsName[i]);
            }

            for (int i = 0; i < InputData.Rows.Count; i++)
            {
                DataRow dr = dt.NewRow();
                if (InputData.Rows.Count - AveragCount >= i)
                {
                    for (int j = 0; j < AveragCount; j++)
                    {
                        X[i] = double.Parse(InputData.Rows[i + j][0].ToString()) + X[i];
                        Y[i] = double.Parse(InputData.Rows[i + j][1].ToString()) + Y[i];
                        Z[i] = double.Parse(InputData.Rows[i + j][2].ToString()) + Z[i];
                    }
                    X[i] = X[i] / AveragCount; Y[i] = Y[i] / AveragCount; Z[i] = Z[i] / AveragCount;
                }
                else
                {
                    for (int j = 0; j < InputData.Rows.Count - i; j++)
                    {
                        X[i] = double.Parse(InputData.Rows[i + j][0].ToString()) + X[i];
                        Y[i] = double.Parse(InputData.Rows[i + j][1].ToString()) + Y[i];
                        Z[i] = double.Parse(InputData.Rows[i + j][2].ToString()) + Z[i];
                    }
                    X[i] = X[i] / (InputData.Rows.Count - i);
                    Y[i] = Y[i] / (InputData.Rows.Count - i);
                    Z[i] = Z[i] / (InputData.Rows.Count - i);
                }

                dr[0] = X[i];
                dr[1] = Y[i];
                dr[2] = Z[i];

                //File.AppendAllText("MovingAverage.txt", string.Format("Time:{0}     Value:{1}\r\n", i, Y[i]));

                dt.Rows.Add(dr);
            }

            return dt;
        }

        public static DataTable SVM(DataTable InputData)
        {
            DataTable dt = new DataTable();
            string[] ColumnsName = { "X", "Y", "Z" };

            for (int i = 0; i < ColumnsName.Length; i++)
            {
                dt.Columns.Add(ColumnsName[i]);
            }

            for (int i = 0; i < InputData.Rows.Count; i++)
            {
                DataRow dr = dt.NewRow();

                dr[0] = Math.Sqrt(double.Parse(InputData.Rows[i][0].ToString()) * double.Parse(InputData.Rows[i][0].ToString())
                                + double.Parse(InputData.Rows[i][1].ToString()) * double.Parse(InputData.Rows[i][1].ToString())
                                + double.Parse(InputData.Rows[i][2].ToString()) * double.Parse(InputData.Rows[i][2].ToString()));
                dr[1] = 0;
                dr[2] = 0;

                dt.Rows.Add(dr);
            }

            return dt;
        }

        public static DataTable MedianFilter(DataTable InputData, int WindowsSize)
        {
            DataTable dt = new DataTable();
            string[] ColumnsName = { "X", "Y", "Z" };
            double[] X, Y, Z;
            X = new double[InputData.Rows.Count]; Y = new double[InputData.Rows.Count]; Z = new double[InputData.Rows.Count];
            double[] X_MedFil_Ary, Y_MedFil_Ary, Z_MedFil_Ary;
            X_MedFil_Ary = new double[WindowsSize]; Y_MedFil_Ary = new double[WindowsSize]; Z_MedFil_Ary = new double[WindowsSize];

            for (int i = 0; i < ColumnsName.Length; i++)
            {
                dt.Columns.Add(ColumnsName[i]);
            }

            for (int i = 0; i < InputData.Rows.Count; i++)
            {
                DataRow dr = dt.NewRow();

                //below the start point
                if (i - ((WindowsSize - 1) / 2) < 0)
                {
                    for (int j = 0; j < WindowsSize - ((WindowsSize - 1) / 2); j++)
                    {
                        X_MedFil_Ary[j] = double.Parse(InputData.Rows[i][0].ToString());
                        Y_MedFil_Ary[j] = double.Parse(InputData.Rows[i][1].ToString());
                        Z_MedFil_Ary[j] = double.Parse(InputData.Rows[i][2].ToString());
                    }

                    for (int j = WindowsSize - ((WindowsSize - 1) / 2); j < WindowsSize; j++)
                    {
                        X_MedFil_Ary[j] = double.Parse(InputData.Rows[i - ((WindowsSize - 1) / 2) + j][0].ToString());
                        Y_MedFil_Ary[j] = double.Parse(InputData.Rows[i - ((WindowsSize - 1) / 2) + j][1].ToString());
                        Z_MedFil_Ary[j] = double.Parse(InputData.Rows[i - ((WindowsSize - 1) / 2) + j][2].ToString());
                    }
                }
                //beyond the end point
                else if (i + ((WindowsSize - 1) / 2) > InputData.Rows.Count - 1)
                {
                    for (int j = 0; j < WindowsSize - ((WindowsSize - 1) / 2); j++)
                    {
                        X_MedFil_Ary[j] = double.Parse(InputData.Rows[i - ((WindowsSize - 1) / 2) + j][0].ToString());
                        Y_MedFil_Ary[j] = double.Parse(InputData.Rows[i - ((WindowsSize - 1) / 2) + j][1].ToString());
                        Z_MedFil_Ary[j] = double.Parse(InputData.Rows[i - ((WindowsSize - 1) / 2) + j][2].ToString());
                    }

                    for (int j = WindowsSize - ((WindowsSize - 1) / 2); j <WindowsSize; j++)
                    {
                        X_MedFil_Ary[j] = double.Parse(InputData.Rows[i][0].ToString());
                        Y_MedFil_Ary[j] = double.Parse(InputData.Rows[i][1].ToString());
                        Z_MedFil_Ary[j] = double.Parse(InputData.Rows[i][2].ToString());
                    }
                }
                //normal situation
                else
                {
                    for (int j = 0; j < WindowsSize; j++)
                    {
                        X_MedFil_Ary[j] = double.Parse(InputData.Rows[i - ((WindowsSize - 1) / 2) + j][0].ToString());
                        Y_MedFil_Ary[j] = double.Parse(InputData.Rows[i - ((WindowsSize - 1) / 2) + j][1].ToString());
                        Z_MedFil_Ary[j] = double.Parse(InputData.Rows[i - ((WindowsSize - 1) / 2) + j][2].ToString());
                    }
                }

                //======================================================================================================================
                //Console.WriteLine("=========================== New Data:" + i.ToString() + " ===========================");
                //Console.WriteLine("Original X Data:");
                //for (int j = 0; j < WindowsSize; j++)
                //{
                //    Console.Write(X_MedFil_Ary[j] + "\t");
                //}
                //Console.WriteLine();

                Quicksort(X_MedFil_Ary, 0, X_MedFil_Ary.Length - 1);
                Quicksort(Y_MedFil_Ary, 0, Y_MedFil_Ary.Length - 1);
                Quicksort(Z_MedFil_Ary, 0, Z_MedFil_Ary.Length - 1);

                dr[0] = X_MedFil_Ary[(WindowsSize - 1) / 2];
                dr[1] = Y_MedFil_Ary[(WindowsSize - 1) / 2];
                dr[2] = Z_MedFil_Ary[(WindowsSize - 1) / 2];

                //Console.WriteLine("After sort X Data:");
                //for (int j = 0; j < WindowsSize; j++)
                //{
                //    Console.Write(X_MedFil_Ary[j] + "\t");
                //}
                //Console.WriteLine();
                //======================================================================================================================

                dt.Rows.Add(dr);
            }

            return dt;
        }

        public static void Quicksort(double[] elements, int left, int right)
        {
            int i = left, j = right;
            double pivot = elements[(left + right) / 2];

            while (i <= j)
            {
                while (elements[i].CompareTo(pivot) < 0)
                {
                    i++;
                }

                while (elements[j].CompareTo(pivot) > 0)
                {
                    j--;
                }

                if (i <= j)
                {
                    // Swap
                    double tmp = elements[i];
                    elements[i] = elements[j];
                    elements[j] = tmp;

                    i++;
                    j--;
                }
            }

            // Recursive calls
            if (left < j)
            {
                Quicksort(elements, left, j);
            }

            if (i < right)
            {
                Quicksort(elements, i, right);
            }
        }
 
    }
}
