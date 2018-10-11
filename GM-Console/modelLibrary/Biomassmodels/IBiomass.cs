using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.Biomassmodels
{
    interface IBiomass
    {
        List<Tree> InvokeBiomassGrowth(List<Tree> array, List<double> param);
    }
}
