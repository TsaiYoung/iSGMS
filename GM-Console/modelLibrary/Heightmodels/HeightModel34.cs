using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.Heightmodels
{
    public class HeightModel34:IHeightGrowth
    {
        /// <summary>
        /// Cox (1994), Sáncheza et al. (2003)
        /// </summary>
        /// <param name="array"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<Tree> InvokeTreeHeight(List<Tree> array, List<double> param)
        {
            //计算平方平均胸径
            double D2 = 0;
            for (int i = 0; i < array.Count; i++)
            {
                D2 += array[i].DBH * array[i].DBH;
            }
            double Dq = Math.Sqrt(D2 / array.Count);

            //平均树高
            double Hm = param[5] * Math.Log(Dq) + param[7]; //平均树高与平方平均胸径的模型

            for (int i = 0; i < array.Count; i++)
            {
                array[i].Height = param[0] + param[1] * Hm + param[2] * Math.Pow(Dq, 0.95) + param[3] * Math.Exp(-0.08 * array[i].DBH) + param[4] * Math.Pow(Hm, 3) * Math.Exp(-0.08 * array[i].DBH) + param[5] * Math.Pow(Dq, 3) * Math.Exp(-0.08 * array[i].DBH);

                if (Double.IsNaN(array[i].Height) || Double.IsInfinity(array[i].Height))
                {
                    Console.WriteLine("ERROR: NaN or Infinity of Height");
                    return null;
                }
            }

            return array;
        }
    }
}
