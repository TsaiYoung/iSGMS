using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.CWmodels
{
    public class CWModel17 : ICWGrowth2
    {
        /// <summary>
        /// 冠幅模型的备选形式(7-6)
        /// </summary>
        /// <param name="array"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<Tree> InvokeCWGrowth(List<Tree> array, List<double> param, double area)
        {
            double Dg=0,d_sum=0;//Dg表示林分平均胸径
            for (int i = 0; i < array.Count; i++)
            {
                d_sum += Math.Pow(array[i].DBH,2);
            }
            Dg = Math.Pow((d_sum / array.Count), 0.5);

            for(int i = 0; i < array.Count; i++)
            {
                array[i].CrownWidth = param[0] + param[1] * array[i].DBH + param[2] * Dg + param[3] * Math.Pow(array.Count / area * 10000, 0.5);

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
