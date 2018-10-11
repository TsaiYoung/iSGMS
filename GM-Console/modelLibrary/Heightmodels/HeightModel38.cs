using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.Heightmodels
{
    public class HeightModel38:IHeightGrowthfromAge2
    {
        /// <summary>
        /// Burkhart HE, Strub MR (1974)
        /// </summary>
        /// <param name="array"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<Tree> InvokeTreeHeight(List<Tree> array, List<double> param, int t,double area)
        {
            double N = array.Count / area * 10000;

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

            for (int i = 0; i < array.Count; i++)
            {
                array[i].Height = Math.Exp(param[0] + param[1] * Math.Log(H0) + param[2] / t + param[3] * Math.Log(N) / array[i].DBH + param[4] / array[i].DBH / t + param[5] / array[i].DBH);

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
