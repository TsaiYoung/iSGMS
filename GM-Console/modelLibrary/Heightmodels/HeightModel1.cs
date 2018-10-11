using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.Heightmodels
{
    /// <summary>
    /// Lei Xiangdong
    /// </summary>
    public class HeightModel1:IHeightGrowth
    {
        /// <summary>
        /// 
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

            //不同树高
            for (int i = 0; i < array.Count; i++)
            {
                double height = 1.3 + Math.Exp(param[0] + param[1] * Math.Log(array[i].DBH / Dq) + param[2] * Math.Log(Hm));
                array[i].Height = height;

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
