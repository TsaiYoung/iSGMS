using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.UBHmodels
{
    public class UBHGrowthModel5:IUBHModels
    {
        /// <summary>
        /// Paula Soares(2001) A tree crown ratio prediction equation for eucalypt plantations
        /// Weibull型
        /// </summary>
        /// <param name="array"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<Tree> InvokeUBHModels(List<Tree> array, List<double> param, int age,double area)
        {
            //林分密度
            double SD = array.Count / area * 10000;

            //优势高
            array.Sort((left, right) => -left.Height.CompareTo(right.Height));
            double[] domainHeight = new double[5];
            for (int j = 0; j < 5; j++)
            {
                domainHeight[j] = array[j].Height;
            }
            double DH = domainHeight.Average();
            //按ID排序，升序
            array.Sort((left, right) => left.ID.CompareTo(right.ID));

            //计算冠高
            for (int i = 0; i < array.Count; i++)
            {
                double CR = param[0] * (1 - param[1] * Math.Exp(-Math.Pow( param[2] + param[3] / age + param[4] * array.Count / 1000 + param[5] * DH + param[6] * array[i].DBH, param[7])));
                array[i].UnderBranchHeight = (1 - CR) * array[i].Height;

                if (Double.IsNaN(array[i].UnderBranchHeight) || Double.IsInfinity(array[i].UnderBranchHeight))
                {
                    Console.WriteLine("ERROR: NaN or Infinity of UnderBranchHeight");
                    return null;
                }
            }

            return array;
        }
    }
}
