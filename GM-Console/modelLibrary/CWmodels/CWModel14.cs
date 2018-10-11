using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.CWmodels
{
    public class CWModel14 : ICWGrowth2
    {
        /// <summary>
        /// 冠幅模型的备选形式(7-3)
        /// </summary>
        /// <param name="array"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<Tree> InvokeCWGrowth(List<Tree> array, List<double> param, double area)
        {
            //计算单位面积断面积
            double sumBA = 0;
            for (int i = 0; i < array.Count; i++)
            {
                sumBA = Math.PI * array[i].DBH * array[i].DBH / 4.0 + sumBA;
            }
            double BA = sumBA / area;
            //求冠幅
            for (int i = 0; i < array.Count; i++)
            {
                array[i].CrownWidth = param[0]+param[1]*array[i].DBH+param[2]*BA;

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
