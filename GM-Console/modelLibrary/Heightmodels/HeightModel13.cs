using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.Heightmodels
{
    public class HeightModel13 : IHeightGrowth
    {
        /// <summary>
        /// Curtis (1967), Prodan(1968)
        /// </summary>
        /// <param name="array"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<Tree> InvokeTreeHeight(List<Tree> array, List<double> param)
        {
            for (int i = 0; i < array.Count; i++)
            {
                array[i].Height = 1.3 + param[0] * Math.Pow(array[i].DBH / (1 + array[i].DBH), param[1]);

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
