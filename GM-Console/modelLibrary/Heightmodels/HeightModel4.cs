﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.Heightmodels
{
    public class HeightModel4:IHeightGrowth
    {
        /// <summary>
        /// Prodan (1965), Curtis (1967)
        /// </summary>
        /// <param name="array"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<Tree> InvokeTreeHeight(List<Tree> array, List<double> param)
        {
            for (int i = 0; i < array.Count; i++)
            {
                array[i].Height = 1.3 + Math.Exp(param[0] + param[1] * Math.Log(array[i].DBH));

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
