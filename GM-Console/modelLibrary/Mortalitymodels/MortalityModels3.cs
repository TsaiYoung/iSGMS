using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.Mortalitymodels
{
    class MortalityModels3 : IMortalityModels1
    {
        /// <summary>
        /// Survival model
        /// Growth and yield models for uneven-sized forest stands in Finland(Pukkala,2009)（5年）
        /// Using optimization for fitting individual-tree growth models for uneven-aged stands(Pukkala,2011)（5年）
        /// 初始胸径大于5cm
        /// </summary>
        /// <param name="array"></param>
        /// <param name="param"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        public List<double> InvokeMortalityModels(List<Tree> array, List<double> param, double area)
        {
            List<double> probility = new List<double>();
            //每公顷断面积
            double BA = 0;
            for (int i = 0; i < array.Count; i++)
            {
                if (array[i].DBH > 5)
                    BA = BA + Math.PI * array[i].DBH * array[i].DBH / 4;
            }
            double G = BA / area;

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

            //存活率
            for (int i = 0; i < array.Count; i++)
            {
                double p = 1 / (1 + Math.Exp(-(param[0] + param[1] * Math.Sqrt(array[i].DBH) + param[2] * Math.Log(G) + param[3] * BAL[i])));

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
