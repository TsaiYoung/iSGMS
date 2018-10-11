using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.Biomassmodels
{
    public class BiomassModel6:IBiomass
    {
        /// <summary>
        /// W = a(D^2H)^bCw^cCl^d
        /// </summary>
        /// <param name="array"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<Tree> InvokeBiomassGrowth(List<Tree> array, List<double> param)
        {
            for (int i = 0; i < array.Count; i++)
            {
                double Cw = array[i].CrownWidth;
                double Cl = array[i].Height - array[i].UnderBranchHeight;
                array[i].Biomass = param[0] * Math.Pow(array[i].DBH * array[i].DBH * array[i].Height, param[1]) * Math.Pow(Cw, param[2]) * Math.Pow(Cl, param[3]);
            }

            return array;
        }
    }
}
