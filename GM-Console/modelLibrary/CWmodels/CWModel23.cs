using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.CWmodels
{
    public class CWModel23 : ICWGrowth2
    {
        /// <summary>
        /// 冠幅模型的备选形式(7-12)
        /// </summary>
        /// <param name="array"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<Tree> InvokeCWGrowth(List<Tree> array, List<double> param, double area)
        {
            double avgCW = 0;
            int nonEageCount = 0;

            //计算单位面积断面积
            double sumBA = 0;
            for (int i = 0; i < array.Count; i++)
            {
                sumBA = Math.PI * array[i].DBH * array[i].DBH / 4.0 + sumBA;
            }
            double BA = sumBA / area;

            //求冠幅
            for (int i = 0; i < array.Count; i++)
            {
                double BAL = 0;
                for (int j = 0; j < array.Count; j++)
                {
                    if (array[i].DBH < array[j].DBH)
                    {
                        //BAL表示样地内大于对象木的所有林木断面积和(平方米)
                        BAL += Math.PI * array[j].DBH * array[j].DBH / (4.0 * 10000);

                        if (Double.IsNaN(array[i].CrownWidth) || Double.IsInfinity(array[i].CrownWidth))
                        {
                            Console.WriteLine("ERROR: NaN or Infinity of CrownWidth");
                            return null;
                        }
                    }
                }

                if (!array[i].isEdge)
                {
                    array[i].CrownWidth = param[0] + param[1] * array[i].DBH + param[2] * BA + param[3] * BAL + param[4] * array[i].WVA;

                    avgCW += array[i].CrownWidth;
                    nonEageCount++;
                }
            }

            if (nonEageCount > 0)
            {
                //边界木处理，平均值
                for (int i = 0; i < array.Count; i++)
                {
                    if (array[i].isEdge)
                    {
                        array[i].CrownWidth = avgCW / nonEageCount;
                    }
                }
            }

            return array;
        }
    }
}
