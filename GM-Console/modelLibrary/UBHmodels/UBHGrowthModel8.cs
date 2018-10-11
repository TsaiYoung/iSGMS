using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.UBHmodels
{
    class UBHGrowthModel8
    {
        /// <summary>
        /// 基于水平与垂直空间结构参数的枝下高模型
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="array"></param>
        /// <param name="age"></param>
        /// <returns></returns>
        public List<Tree> CalcuUBH(SpatialUnit unit, List<Tree> array,List<double>param, int age)
        {
            int count = 0;
            double avgE = 0;
            double avgS = 0;
            double avgW = 0;
            double avgN = 0;

            for (int i = 0; i < array.Count; i++)
            {
                if (!array[i].isEdge)
                {
                    //垂直、水平空间结构参数
                    double ePv = unit.CalcuPv(1, i, array);
                    double sPv = unit.CalcuPv(2, i, array);
                    double wPv = unit.CalcuPv(3, i, array);
                    double nPv = unit.CalcuPv(4, i, array);

                    //计算树冠枝下高
                    array[i].eastUnderBranchHeight = param[0] + param[1] * age + param[2] * ePv; avgE += array[i].eastUnderBranchHeight;
                    array[i].southUnderBranchHeight = param[0] + param[1] * age + param[2] * sPv; avgS += array[i].southUnderBranchHeight;
                    array[i].westUnderBranchHeight = param[0] + param[1] * age + param[2] * wPv; avgW += array[i].westUnderBranchHeight;
                    array[i].northUnderBranchHeight = param[0] + param[1] * age + param[2] * nPv; avgN += array[i].northUnderBranchHeight;
                    array[i].UnderBranchHeight = (array[i].eastUnderBranchHeight + array[i].southUnderBranchHeight + array[i].westUnderBranchHeight + array[i].northUnderBranchHeight) / 4;
                    count++;

                    if (Double.IsNaN(array[i].UnderBranchHeight) || Double.IsInfinity(array[i].UnderBranchHeight))
                    {
                        Console.WriteLine("ERROR: NaN or Infinity of UnderBranchHeight");
                        return null;
                    }
                }
                
            }

            if (count > 0)
            {
                for (int i = 0; i < array.Count; i++)
                {
                    if (array[i].isEdge)
                    {
                        //计算树冠枝下高
                        array[i].eastUnderBranchHeight = avgE / count;
                        array[i].southUnderBranchHeight = avgS / count;
                        array[i].westUnderBranchHeight = avgW / count;
                        array[i].northUnderBranchHeight = avgN / count;
                        array[i].UnderBranchHeight = (array[i].eastUnderBranchHeight + array[i].southUnderBranchHeight + array[i].westUnderBranchHeight + array[i].northUnderBranchHeight) / 4;
                    }
                }
            }

            return array;
        }
    }
}
