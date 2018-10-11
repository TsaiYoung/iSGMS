using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.Mortalitymodels
{
    class MortalityModels4 : IMortalityModels2
    {
        /// <summary>
        /// A Generalized Logistic Model of Individual Tree Mortality for Aspen, White Spruce, and Lodgepole Pine in Alberta Mixedwood Forests(Xiaohong Yao,2001)
        /// Survival model
        /// (5年)
        /// </summary>
        /// <param name="array"></param>
        /// <param name="param"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        public List<double> InvokeMortalityModels(List<Tree> array, List<double> param,double area, double SI,List<double> DIN)
        {
            List<double> probility = new List<double>();
            //每公顷断面积
            double BA = 0;
            for (int i = 0; i < array.Count; i++)
            {
                BA = BA + Math.PI * array[i].DBH * array[i].DBH / 4;
            }
            BA = BA / area;

            //存活率
            for (int i = 0; i < array.Count; i++)
            {
                double p = Math.Pow(1 + Math.Exp(-(param[0] + param[1] * array[i].DBH + param[2] * array[i].DBH * array[i].DBH + param[3] * DIN[i] + param[4] * BA  + param[5] * SI + param[6] * array[i].DBH * array[i].DBH / BA + param[7] * SI / BA)),-5);

                if (Double.IsNaN(p) || Double.IsInfinity(p))
                {
                    Console.WriteLine("ERROR: NaN or Infinity of probility");
                    return null;
                }

                probility.Add(p);
            }
            return probility;
        }
    }
}
