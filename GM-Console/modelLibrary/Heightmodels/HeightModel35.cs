using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.Heightmodels
{
    public class HeightModel35:IHeightGrowthfromAge
    {
        /// <summary>
        /// Curtis RO (1970) 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<Tree> InvokeTreeHeight(List<Tree> array, List<double> param, int t)
        {
            //计算平方平均胸径
            double D2 = 0;
            for (int i = 0; i < array.Count; i++)
            {
                D2 += array[i].DBH * array[i].DBH;
            }
            double Dq = Math.Sqrt(D2 / array.Count);

            for (int i = 0; i < array.Count; i++)
            {
                array[i].Height = Math.Pow(10, param[0] + param[1] / array[i].DBH + param[2] / t + param[3] / Dq / t);

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
