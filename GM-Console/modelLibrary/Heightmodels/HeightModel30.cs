using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.Heightmodels
{
    public class HeightModel30:IHeightGrowth
    {
        /// <summary>
        /// Gadow and Hui (1999)
        /// </summary>
        /// <param name="array"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<Tree> InvokeTreeHeight(List<Tree> array, List<double> param)
        {
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
                array[i].Height = 1.3 + param[0] * Math.Pow(H0, param[1]) * Math.Pow(array[i].DBH, param[2] * Math.Pow(H0, param[3]));

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
