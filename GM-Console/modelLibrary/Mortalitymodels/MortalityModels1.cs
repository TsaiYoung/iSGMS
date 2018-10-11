using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.Mortalitymodels
{
    public class MortalityModels1:IMortalityModels
    {
        /// <summary>
        /// 计算枯损率1——邵国凡
        /// </summary>
        /// <param name="units">空间结构单元</param>
        /// <param name="array">林木</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public List<double> InvokeMortalityModels(List<SpatialUnit.unit_tree> units, List<Tree> array, List<double> param)
        {
            List<double> probility = new List<double>();
            for (int i = 0; i < array.Count; i++)
            {
                //计算竞争因子
                double comp = 0;
                for (int j = 0; j < units[i].id.Count; j++)
                {
                    double distance1 = Math.Sqrt((array[units[i].id[j]].X - array[i].X) * (array[units[i].id[j]].X - array[i].X) + (array[units[i].id[j]].Y - array[i].Y) * (array[units[i].id[j]].Y - array[i].Y));
                    double comp1 = array[units[i].id[j]].DBH / array[i].DBH / distance1;
                    comp = comp + comp1;
                }

                //计算枯损率
                double p =param[0] * Math.Pow(comp, param[1]);

                if (Double.IsNaN(p) || Double.IsInfinity(p))
                {
                    Console.WriteLine("ERROR: NaN or Infinity of probility");
                    return null;
                }

                probility.Add(1-p);
            }

            return probility;
        }
    }
}
