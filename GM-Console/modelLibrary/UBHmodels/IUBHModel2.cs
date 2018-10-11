using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.UBHmodels
{
    interface IUBHModel2
    {
        /// <summary>
        /// 基于冠长率模型计算枝下高
        /// </summary>
        /// <param name="array"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        List<Tree> InvokeUBHModels(List<Tree> array, List<double> param, int age);
    }
}
