using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.UBHmodels
{
    public class UBHGrowthModel6:IUBHModels3
    {
        /// <summary>
        /// Hynynen, 1995
        /// </summary>
        /// <param name="array"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<Tree> InvokeUBHModels(List<Tree> array, List<double> param,double area, DEMOperate dem)
        {
            //BAS 每公顷断面积
            double BA = 0;
            for (int i = 0; i < array.Count; i++)
            {
                BA = BA + Math.PI * array[i].DBH * array[i].DBH / 4;
            }
            BA = BA / area;

            //BAL 大于对象木全部树木胸高断面积之和
            List<double> BAL = new List<double>();
            for (int i = 0; i < array.Count; i++)
            {
                double bal = 0;
                for (int j = 0; j < array.Count; j++)
                {
                    if (array[j].DBH > array[i].DBH)
                    {
                        bal = bal + Math.PI * array[j].DBH * array[j].DBH / 4;
                    }
                }
                BAL.Add(bal / 10000);
            }

            //优势高
            array.Sort((left, right) => -left.Height.CompareTo(right.Height));
            double[] domainHeight = new double[5];
            for (int j = 0; j < 5; j++)
            {
                domainHeight[j] = array[j].Height;
            }
            double DH = domainHeight.Average();
            //按ID排序，升序
            array.Sort((left, right) => left.ID.CompareTo(right.ID));

            //计算枝下高
            for (int i = 0; i < array.Count; i++)
            { 
                //计算x，y对应DEM的行列号
                dem.getColRow(array[i].X, array[i].Y);
                //读取高程
                double altitude = dem.getDEMValue();
                //计算坡度
                double slope = dem.CalcuSlope();
                //计算坡向
                double aspect = dem.CalcuAspect();

                double X = param[0] + param[1] * array[i].Height / array[i].DBH + param[2] * array[i].Height + param[3] * array[i].DBH * array[i].DBH + param[4] * BAL[i] + param[5] * altitude / 100
                    + param[6] * altitude * altitude / 10000 + param[7] * Math.Tan(slope) + param[8] * Math.Tan(slope) * Math.Tan(slope) + param[9] * slope * Math.Sin(aspect) + param[10] * slope * Math.Cos(aspect);

                double CR = 1/(1 + Math.Exp(-X));
                array[i].UnderBranchHeight = (1 - CR) * array[i].Height;

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
