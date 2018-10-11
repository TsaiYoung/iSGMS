using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.CWmodels
{
    class CWModel25
    {
        /// <summary>
        /// 基于水平与垂直空间结构参数的冠幅模型
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="array"></param>
        /// <param name="age"></param>
        /// <returns></returns>
        public List<Tree> CalcuCrown1(SpatialUnit unit, List<Tree> array, List<double>param, int age)
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
                    double ePh = unit.CalcuPh(1, i, array);
                    double sPh = unit.CalcuPh(2, i, array);
                    double wPh = unit.CalcuPh(3, i, array);
                    double nPh = unit.CalcuPh(4, i, array);

                    //计算冠幅
                    array[i].eastCrownWidth = param[0] + param[1] * age + param[2] * ePh;avgE += array[i].eastCrownWidth;
                    array[i].southCrownWidth = param[0] + param[1] * age + param[2] * sPh;avgS += array[i].southCrownWidth;
                    array[i].westCrownWidth = param[0] + param[1] * age + param[2] * wPh;avgW += array[i].westCrownWidth;
                    array[i].northCrownWidth = param[0] + param[1] * age + param[2] * nPh;avgN += array[i].northCrownWidth;
                    array[i].CrownWidth = (array[i].eastCrownWidth + array[i].southCrownWidth + array[i].westCrownWidth + array[i].northCrownWidth) / 2;
                    count++;

                    if (Double.IsNaN(array[i].CrownWidth) || Double.IsInfinity(array[i].CrownWidth))
                    {
                        Console.WriteLine("ERROR: NaN or Infinity of CrownWidth");
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
                        //计算冠幅
                        array[i].eastCrownWidth = avgE / count;
                        array[i].southCrownWidth = avgS / count;
                        array[i].westCrownWidth = avgW / count;
                        array[i].northCrownWidth = avgN / count;
                        array[i].CrownWidth = (array[i].eastCrownWidth + array[i].southCrownWidth + array[i].westCrownWidth + array[i].northCrownWidth) / 2;
                    }
                }
            }
            return array;
        }
    }
}
