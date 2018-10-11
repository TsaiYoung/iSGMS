using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.Heightmodels
{
    public class HeightModel27 : IHeightGrowth2
    {
        /// <summary>
        /// Sharma and Zhang (2004)
        /// </summary>
        /// <param name="array"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<Tree> InvokeTreeHeight(List<Tree> array, List<double> param,double area)
        {            
            //计算单位面积断面积
            double sumBA = 0;
            for (int i = 0; i < array.Count; i++)
            {
                sumBA = Math.PI * array[i].DBH * array[i].DBH / 4.0 + sumBA;
            }
            double BA = sumBA / area;

            for (int i = 0; i < array.Count; i++)
            {
                array[i].Height = 1.3 + param[0] * Math.Pow(BA, param[1]) * (1 - Math.Exp(-param[2] * array[i].DBH));

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
