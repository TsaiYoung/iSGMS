using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.Mortalitymodels
{
    interface IMortalityModels2
    {
        List<double> InvokeMortalityModels( List<Tree> array, List<double> param,double area, double SI, List<double> DIN);
    }
}
