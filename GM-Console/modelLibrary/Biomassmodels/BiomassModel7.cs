using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.Biomassmodels
{
    public class BiomassModel7:IBiomass
    {
        /// <summary>
        /// W = a(D^2H)^b(Cw^2Cl)^c
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
                array[i].Biomass = param[0] * Math.Pow(array[i].DBH * array[i].DBH * array[i].Height, param[1]) * Math.Pow(Cw * Cw * Cl, param[2]);
            }

            return array;
        }
    }
}
