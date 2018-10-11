using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.UBHmodels
{
    public class UBHGrowthModel7:IUBHModel2
    {
        /// <summary>
        /// song
        /// </summary>
        /// <param name="array"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<Tree> InvokeUBHModels(List<Tree> array, List<double> param,int age)
        {           
            //计算枝下高
            for (int i = 0; i < array.Count; i++)
            {
                array[i].UnderBranchHeight = array[i].Height - Math.Exp(param[0] + param[1] / (array[i].DBH + 1));
                
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
