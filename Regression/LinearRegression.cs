using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Regression
{
    public class LinearRegression
    {
        public double[] valX, valY, ost;
        public double standardXdev, standardYdev, correlation, slope, interception;

        public LinearRegression(double[] valX, double[] valY)
        {
            this.valX = valX;
            this.valY = valY;
            standardXdev = GetStandDev(valX);
            standardYdev = GetStandDev(valY);
            correlation = GetCorrelation(valX, valY);
            slope = correlation * standardYdev/ standardXdev;
            interception = GetMean(valY) - slope * GetMean(valX);
        }

        public static double GetMean(double[] array)
        {
            return array.Sum() / array.Length;
        }

        public static double GetStandDev(double[] array)
        {
            double mean = GetMean(array);
            double[] meanDev = new double[array.Length];
            for (int i = 0; i < array.Length; i++)
                meanDev[i] = Math.Pow(array[i] - mean, 2);
            return Math.Sqrt(meanDev.Sum()/(meanDev.Length - 1));
        }
        private static double GetCorrelation(double[] X, double[] Y)
        {
            double XMean = X.Sum() / X.Length;
            double YMean = Y.Sum() / Y.Length;
            double[] x = new double[X.Length];
            double[] y = new double[Y.Length];

            for (int i = 0; i < X.Length; i++)
            {
                x[i] = X[i] - XMean;
                y[i] = Y[i] - YMean;
            }

            double[] xy = new double[X.Length];
            for (int i = 0; i < X.Length; i++)
            {
                xy[i] = x[i] * y[i];
            }

            double[] xPowed = new double[X.Length];
            double[] yPowed = new double[Y.Length];

            for (int i = 0; i < X.Length; i++)
            {
                xPowed[i] = Math.Pow(x[i], 2);
                yPowed[i] = Math.Pow(y[i], 2);
            }

            return xy.Sum() / Math.Sqrt(xPowed.Sum() * yPowed.Sum());
        }

        public double Predict(double x)
        {
            return interception + slope * x;
        }

        public string GetEquation()
        {
            return Math.Round(this.slope,6) + "*x + " + Math.Round(this.interception, 6);
        }
        public void GetOstatok(){
            ost = new double[valY.Length];
            for (int i = 0; i < valY.Length; i++)
                this.ost[i] = valY[i] - (interception + slope * valX[i]);

        }
        public double GetDetermination()
        {
            GetOstatok();
            double Qe = 0;
            for (int i=0; i<ost.Length; i++)
                 Qe+=Math.Pow(ost[i],2);
            double Qy = 0;
            for (int i = 0; i < valY.Length; i++)
                Qy += Math.Pow(valY[i] - GetMean(valY), 2);    
            return Math.Round(1 - (Qe/Qy), 6);
        }
    }
}