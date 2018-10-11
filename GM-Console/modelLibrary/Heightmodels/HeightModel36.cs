using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.Heightmodels
{
    public class HeightModel36 : IHeightGrowth2
    {
        /// <summary>
        /// Cox F (1994)
        /// </summary>
        /// <param name="array"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<Tree> InvokeTreeHeight(List<Tree> array, List<double> param,double area)
        {
            double N = array.Count / area * 10000;

            //计算平方平均胸径
            double D2 = 0;
            for (int i = 0; i < array.Count; i++)
            {
                D2 += array[i].DBH * array[i].DBH;
            }
            double Dq = Math.Sqrt(D2 / array.Count);

            for (int i = 0; i < array.Count; i++)
            {
                array[i].Height = Math.Exp(param[0] + param[1] * Math.Log(Dq) + param[2] * Math.Log(N) + param[3] * Math.Sqrt(array[i].DBH));

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
