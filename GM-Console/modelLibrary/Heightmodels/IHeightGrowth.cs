using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.Heightmodels
{
    interface IHeightGrowth
    {
        List<Tree> InvokeTreeHeight(List<Tree> array, List<double> param);
    }
}
