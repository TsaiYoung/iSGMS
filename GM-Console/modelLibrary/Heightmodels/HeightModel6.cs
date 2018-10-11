using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.Heightmodels
{
    class HeightModel6:IHeightGrowth
    {
        /// <summary>
        /// Lei Xiangdong
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
            double Hm = param[3] * Math.Log(Dq) + param[4]; //平均树高与平方平均胸径的模型

            for (int i = 0; i < array.Count; i++)
            {
                array[i].Height = param[0] + param[1] * Math.Log(array[i].DBH / Dq) + param[2] * Hm;

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
