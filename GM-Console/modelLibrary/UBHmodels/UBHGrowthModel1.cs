using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.UBHmodels
{
    public class UBHGrowthModel1:IUBHModels3
    {
        /// <summary>
        /// LP Leites (2009) Logistic型
        /// </summary>
        /// <param name="array"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<Tree> InvokeUBHModels(List<Tree> array, List<double> param,double area,DEMOperate dem)
        {
            //BAS 每公顷断面积
            double BA = 0;
            for (int i = 0; i < array.Count; i++)
            {
                BA = BA + Math.PI * array[i].DBH * array[i].DBH / 4;
            }
            BA = BA / area;

            //计算冠高
            for (int i = 0; i < array.Count; i++)
            {
                double CR = 1 / (1 + Math.Exp(param[0] + param[1] * array[i].DBH + param[2] * array[i].Height + param[3] * BA));
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
