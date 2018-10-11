using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.Mortalitymodels
{
    class MortalityModels8 : IMortalityModels1
    {
        /// <summary>
        /// 目标树经营单木生长模型及干扰树采伐模拟(宋玉福，2015)
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
            BA = BA / area;
            //断面积平均胸径
            double Dg = 0;
            for (int i = 0; i < array.Count; i++)
            {
                Dg = Dg + Math.PI * array[i].DBH * array[i].DBH / 4;
            }
            Dg = Math.Sqrt(Dg / array.Count / Math.PI) * 2;
            
            for (int i = 0; i < array.Count; i++)
            {
                double p = 1;
                if (!array[i].isEdge)
                {
                    p = 1 / (1 + Math.Exp(param[0] + param[1] * array[i].DBH + param[2] * BA + param[3] * BAL[i] + param[4] * array[i].WVA + param[5] * Dg));

                    if (Double.IsNaN(p) || Double.IsInfinity(p))
                    {
                        Console.WriteLine("ERROR: NaN or Infinity of probility");
                        return null;
                    }
                }
                probility.Add(p);
            }

            return probility;
        }
    }
}
