using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.Biomassmodels
{
    public class BiomassModel1:IBiomass
    {
        /// <summary>
        /// 生物量模型1（地上生物量）——傅煜
        /// José Návar（2013）Regional aboveground biomass equations for North American arid and semi-arid forests
        /// </summary>
        /// <param name="array"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<Tree> InvokeBiomassGrowth(List<Tree> array, List<double> param)
        {
            for (int i = 0; i < array.Count; i++)
            {
                array[i].Biomass = param[0] * Math.Pow(array[i].DBH, param[1]);
            }

            return array;
        }
    }
}
