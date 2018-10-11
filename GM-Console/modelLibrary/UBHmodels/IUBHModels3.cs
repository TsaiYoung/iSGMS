using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.UBHmodels
{
    interface IUBHModels3
    {
        List<Tree> InvokeUBHModels(List<Tree> array, List<double> param,double area, DEMOperate dem);
    }
}
