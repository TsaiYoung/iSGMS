using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.Heightmodels
{
    public class HeightModel29:IHeightGrowth
    {
        /// <summary>
        /// Harrison et al. (1986)
        /// </summary>
        /// <param name="array"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<Tree> InvokeTreeHeight(List<Tree> array, List<double> param)
        {
            //林分优势木高，降序
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
                array[i].Height = H0 * (1 + param[0] * Math.Exp(param[1] * H0)) * (1 - Math.Exp(-param[2] * array[i].DBH / H0));

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
