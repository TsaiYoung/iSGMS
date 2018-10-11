using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.UBHmodels
{
    class UBHGrowthModel10:IUBHmodel1
    {
        /// <summary>
        /// 基于冠长模型——浙江省森林资源监测中心（基于抽样固定样地的森林乔木碳储量更新预测模型,2012,邹奕巧）
        /// 基于CAR模型
        /// 杉木类
        /// </summary>
        /// <param name="array"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<Tree> InvokeUBHModels(List<Tree> array, List<double> param)
        {
            for (int i = 0; i < array.Count; i++)
            {
                array[i].UnderBranchHeight = array[i].Height - param[0] * Math.Pow(array[i].DBH, param[1]) * Math.Pow(array[i].Height, param[2]);

                if (Double.IsNaN(array[i].UnderBranchHeight) || Double.IsInfinity(array[i].UnderBranchHeight))
                {
                    Console.WriteLine("ERROR: NaN or Infinity of UnderBranchHeight");
                    return null;
                }
            }
            return array;
        }
    }
}
