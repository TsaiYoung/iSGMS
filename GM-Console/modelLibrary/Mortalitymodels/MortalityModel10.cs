using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.Mortalitymodels
{
    class MortalityModel10:IMortalityModels1
    {
        /// <summary>
        /// Modelling annual individual-tree growth and mortality of Scots pine with data obtained at irregular measurement intervals and containing missing observations
        /// Felipe Crecente-Campo,2010
        /// 间隔期5年
        /// </summary>
        /// <param name="array"></param>
        /// <param name="param"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        public List<double> InvokeMortalityModels(List<Tree> array, List<double> param, double area)
        {
            List<double> probility = new List<double>();

            //BAL 大于对象木全部树木胸高断面积之和
            List<double> BAL = new List<double>();
            for (int i = 0; i < array.Count; i++)
            {
                double bal = 0;
                for (int j = 0; j < array.Count; j++)
                {
                    if (array[j].DBH > array[i].DBH)
                    {
                        bal = bal + Math.PI * array[j].DBH * array[j].DBH / 4;
                    }
                }
                BAL.Add(bal / 10000);
            }

            //每公顷断面积
            double BA = 0;
            for (int i = 0; i < array.Count; i++)
            {
                BA = BA + Math.PI * array[i].DBH * array[i].DBH / 4;
            }
            double G = BA / area;

            double N = array.Count / area * 10000;

            //计算平方平均胸径
            double D2 = 0;
            for (int i = 0; i < array.Count; i++)
            {
                D2 += array[i].DBH * array[i].DBH;
            }
            double Dq = Math.Sqrt(D2 / array.Count);

            //优势高
            array.Sort((left, right) => -left.Height.CompareTo(right.Height));
            double[] domainHeight = new double[5];
            for (int j = 0; j < 5; j++)
            {
                domainHeight[j] = array[j].Height;
            }
            double hdom = domainHeight.Average();
            array.Sort((left, right) => left.ID.CompareTo(right.ID));

            double IH = 100 / (hdom * Math.Sqrt(N));

            //计算存活率
            for (int i = 0; i < array.Count; i++)
            {
                double p = 1 / (1 + Math.Exp(param[0] + param[1] * array[i].DBH + param[2] * Dq + param[3] * BAL[i] / G / IH + param[4] * BAL[i] / G));

                if (Double.IsNaN(p) || Double.IsInfinity(p))
                {
                    Console.WriteLine("ERROR: NaN or Infinity of probility");
                    return null;
                }

                probility.Add(p);               
            }

            return probility;
        }
    }
}
