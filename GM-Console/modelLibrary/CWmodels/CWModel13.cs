using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.CWmodels
{
    public class CWModel13 : ICWGrowth
    {
        /// <summary>
        /// 冠幅模型的备选形式(7-1)
        /// </summary>
        /// <param name="array"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<Tree> InvokeCWGrowth(List<Tree> array, List<double> param)
        {
            for (int i = 0; i < array.Count; i++)
            {
                array[i].CrownWidth = Math.Exp(param[0]+param[1]/(array[i].DBH+1));

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
