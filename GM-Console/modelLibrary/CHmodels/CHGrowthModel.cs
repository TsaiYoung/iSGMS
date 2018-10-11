using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.CHmodels
{
    class CHGrowthModel
    {
        /// <summary>
        /// 基于空间结构单元的冠高生长模型
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="array"></param>
        /// <param name="age"></param>
        /// <returns></returns>
        public List<Tree> CalcuCrownHeight(SpatialUnit unit, List<Tree> array, List<double>param, int age)
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

                    //计算冠高
                    array[i].eastCrownHeight = param[0] + param[1] * age + param[2] * ePv; avgE += array[i].eastCrownHeight;
                    array[i].southCrownHeight = param[0] + param[1] * age + param[2] * sPv; avgS += array[i].southCrownHeight;
                    array[i].westCrownHeight = param[0] + param[1] * age + param[2] * wPv; avgW += array[i].westCrownHeight;
                    array[i].northCrownHeight = param[0] + param[1] * age + param[2] * nPv; avgN += array[i].northCrownHeight;
                    array[i].CrownHeight = (array[i].eastCrownHeight + array[i].southCrownHeight + array[i].westCrownHeight + array[i].northCrownHeight) / 4;
                    count++;

                    if (Double.IsNaN(array[i].CrownHeight) || Double.IsInfinity(array[i].CrownHeight))
                    {
                        Console.WriteLine("ERROR: NaN or Infinity of CrownHeight");
                        return null;
                    }
                }
            }

            for (int i = 0; i < array.Count; i++)
            {
                if (array[i].isEdge && count > 0)
                {
                    //计算冠高
                    array[i].eastCrownHeight = avgE / count;
                    array[i].southCrownHeight = avgS / count;
                    array[i].westCrownHeight = avgW / count;
                    array[i].northCrownHeight = avgN / count;
                    array[i].CrownHeight = (array[i].eastCrownHeight + array[i].southCrownHeight + array[i].westCrownHeight + array[i].northCrownHeight) / 4;
                }
            }

            return array;
        }
    }
}
