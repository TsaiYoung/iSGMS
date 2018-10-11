using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.Heightmodels
{
    public class HeightModel33:IHeightGrowth2
    {
        /// <summary>
        /// Sáncheza et al. (2003)(模型31、32和33合并)，param[5] =1或1/2
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

            //林分优势木高
            array.Sort((left, right) => -left.Height.CompareTo(right.Height));
            double[] domainHeight = new double[5];
            for (int j = 0; j < 5; j++)
            {
                domainHeight[j] = array[j].Height;
            }
            double H0 = domainHeight.Average();
            //按ID排序，升序
            array.Sort((left, right) => left.ID.CompareTo(right.ID));

            //计算平方平均胸径
            double D2 = 0;
            for (int i = 0; i < array.Count; i++)
            {
                D2 += array[i].DBH * array[i].DBH;
            }
            double Dq = Math.Sqrt(D2 / array.Count);

            for (int i = 0; i < array.Count; i++)
            {
                array[i].Height = 1.3 + (param[0] + param[1] * H0 - param[2] * Dq+param[3]*BA) * Math.Exp(-param[4] / (Math.Pow(array[i].DBH, param[5])));

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
