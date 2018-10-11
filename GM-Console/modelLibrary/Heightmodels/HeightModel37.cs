using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.Heightmodels
{
    public class HeightModel37:IHeightGrowth
    {
        /// <summary>
        /// Cox F (1994)Sánchez(2003)
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
            double Hm = param[9] * Math.Log(Dq) + param[10]; //平均树高与平方平均胸径的模型

            for (int i = 0; i < array.Count; i++)
            {
                array[i].Height = param[0] + param[1] * Hm + param[2] * Dq + param[3] * Math.Exp(param[4] * array[i].DBH) + param[5] * Math.Pow(Hm, param[6]) * Math.Exp(param[4] * array[i].DBH) + param[7] * Math.Pow(Dq, param[8]) * Math.Exp(param[4] * array[i].DBH);

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
