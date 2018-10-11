using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.Biomassmodels
{
    public class BiomassModel2:IBiomass
    {
        /// <summary>
        /// W = a(D^2H)^b
        /// Xiongqing Zhang（2013）Tree Biomass Estimation of Chinese fir (Cunninghamia lanceolata) Based on Bayesian Method
        /// </summary>
        /// <param name="array"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<Tree> InvokeBiomassGrowth(List<Tree> array, List<double> param)
        {
            for (int i = 0; i < array.Count; i++)
            {
                array[i].Biomass = param[0]*Math.Pow(array[i].DBH*array[i].DBH*array[i].Height,param[1]);
            }

            return array;
        }
    }
}
