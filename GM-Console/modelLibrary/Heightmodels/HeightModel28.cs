using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.Heightmodels
{
    public class HeightModel28:IHeightGrowth
    {
        /// <summary>
        /// Sloboda et al. (1993), Sáncheza et al. (2003)
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
            double Hm = param[2] * Math.Log(Dq) + param[3]; //平均树高与平方平均胸径的模型

            for (int i = 0; i < array.Count; i++)
            {
                array[i].Height = 1.3 + (Hm - 1.3) * Math.Exp(param[0] * (1 - array[i].DBH / Dq) + param[1] * (array[i].DBH / Dq - 1 / array[i].DBH));

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
