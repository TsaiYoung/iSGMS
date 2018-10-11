using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.CWmodels
{
    public class CWModel21 : ICWGrowth2
    {
        /// <summary>
        /// 冠幅模型的备选形式(7-10)
        /// </summary>
        /// <param name="array"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<Tree> InvokeCWGrowth(List<Tree> array, List<double> param, double area)
        {
            double Dg = 0, d_sum = 0;//Dg表示林分平均胸径
            for (int i = 0; i < array.Count; i++)
            {
                d_sum += Math.Pow(array[i].DBH, 2);
            }
            Dg = Math.Pow((d_sum / array.Count), 0.5);

            for (int i = 0; i < array.Count; i++)
            {
                double BAL = 0;
                for (int j = 0; j < array.Count; j++)
                {
                    if (array[i].DBH < array[j].DBH)
                    {
                        //BAL表示样地内大于对象木的所有林木断面积和(平方米)
                        BAL += Math.PI * array[j].DBH * array[j].DBH / (4.0 * 10000);
                    }
                }
                array[i].CrownWidth = param[0] + param[1] * array[i].DBH + param[2] * BAL + param[3] * Dg+param[4]*Math.Pow(array.Count / area * 10000, 0.5);

                if (Double.IsNaN(array[i].CrownWidth) || Double.IsInfinity(array[i].CrownWidth))
                {
                    Console.WriteLine("ERROR: NaN or Infinity of CrownWidth");
                    return null;
                }
            }
            return array;
        }
    }
}
