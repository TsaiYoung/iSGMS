using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.CWmodels
{
    interface ICWGrowth2
    {
        List<Tree> InvokeCWGrowth(List<Tree> array, List<double> param,double area);
    }
}
