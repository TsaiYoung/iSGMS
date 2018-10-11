using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.Mortalitymodels
{
    interface IMortalityModels
    {
        List<double> InvokeMortalityModels(List<SpatialUnit.unit_tree> units, List<Tree> array, List<double> param);
    }
}
