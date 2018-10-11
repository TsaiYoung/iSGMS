using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.Mortalitymodels
{
    public class MortalityModels2 : IMortalityModels1
    {
        /// <summary>
        /// 蒙古栎天然林单木生长模型研究—单木枯死模型(马武，2015)
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
            //断面积平均胸径
            double Dg = 0;
            for (int i = 0; i < array.Count; i++)
            {
                Dg = Dg + Math.PI * array[i].DBH * array[i].DBH / 4;
            }
            Dg = Math.Sqrt(Dg / array.Count / Math.PI) * 2;

            //每公顷断面积
            double BA = 0;
            for (int i = 0; i < array.Count; i++)
            {
                BA = BA + Math.PI * array[i].DBH * array[i].DBH / 4;
            }
            double G = BA / area;

            //林分密度指数
            double N = array.Count / area * 10000;

            for (int i = 0; i < array.Count; i++)
            {
                double BAP = Math.PI * array[i].DBH * array[i].DBH / 4 / G;
                double p =1/ (1 + Math.Exp(param[0] + param[1] / array[i].DBH + param[2] * BAL[i] + param[3] * array[i].DBH / Dg + param[4] * G + param[5] * N + param[6] * BAP));

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
