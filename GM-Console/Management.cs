using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console
{
    class Management
    {
        //每个单元内树木
        public struct indivTree
        {
            //public int centerID;
            public int id;
            //public double distance;
            public double azimuth;
            public int quadrant;
        }

        enum enCuttingtype
        {
            Thinning, Selection_cutting, Shelterwood_cutting, Clear_cutting, Regeneration_cutting
        };

        private List<Tree> forest = new List<Tree>();
        private List<SpatialUnit.unit_tree> units = new List<SpatialUnit.unit_tree>();
        private List<double> sdi = new List<double>();                  //空间密度指数
        private List<double> nc = new List<double>();                   //大小比数
        private List<double> uai = new List<double>();                  //角尺度
        private List<double> mingle = new List<double>();               //混交度
        private List<double> dbhIndex = new List<double>();         //胸径指数
        private List<double> heightIndex = new List<double>();      //树高指数

        private double maxHeight = 0;
        private double maxDBH = 0;

        //输入参数
        private int cuttingType;
        private double intensity = 0; //一般小于0.2（20%）
        private double weightofH = 0;
        private double weightofD = 0;
        private double weightofSDI = 0;
        private double weightofNC = 0;
        private double weightofUAI = 0;
        private double weightofMingle = 0;
        private double weightofHealth = 0;

        public Management(List<Tree> array, List<SpatialUnit.unit_tree> units, int type,double[] weights,double cutIntensity)
        {
            forest = array;
            this.units = units;

            cuttingType = type;
            intensity = cutIntensity;
            weightofD = weights[0];
            weightofH = weights[1];
            weightofSDI = weights[2];
            weightofNC = weights[3];
            weightofUAI = weights[4];
            weightofMingle = weights[5];
            weightofHealth = weights[6];
        }

        // 计算优势木胸径和优势木树高
        public void CalcuMaxHeightDBH()
        {
            for (int i = 0; i < forest.Count; i++)
            {
                if (forest[i].DBH > maxDBH)
                    maxDBH = forest[i].DBH;
                if (forest[i].Height > maxHeight)
                    maxHeight = forest[i].Height;
            }
        }

        //计算胸径、树高指数
        public void CalcuDBH_Height_Index()
        {
            for (int i = 0; i < forest.Count; i++)
            {
                double indexH = 1 - (forest[i].Height / maxHeight);
                double indexD = 1 - (forest[i].DBH / maxDBH);
                dbhIndex.Add(indexD);
                heightIndex.Add(indexH);
            }
        }

        // 计算空间密度指数
        public void Spatial_Density_Index()
        {
            List<double> MaxInUnit = new List<double>();
            double maxDist = 0;
            for (int i = 0; i < forest.Count; i++)
            {
                int maxId = units[i].id[3];
                double distance = Math.Sqrt((forest[maxId].X - forest[i].X) * (forest[maxId].X - forest[i].X) + (forest[maxId].Y - forest[i].Y) * (forest[maxId].Y - forest[i].Y));
                MaxInUnit.Add(distance);

                if (distance > maxDist)
                    maxDist = distance;
            }

            for (int i = 0; i < forest.Count; i++)
            {
                double sdi_value = 1 - (MaxInUnit[i] / maxDist);
                sdi.Add(sdi_value);
            }
        }

        // 计算大小比数
        public void Neighborhood_Comparison()
        {            
            for (int i = 0; i < forest.Count; i++)
            {
                int count = 0;
                for (int j = 0; j < 4; j++)
                {
                    if (forest[i].DBH < forest[units[i].id[j]].DBH)
                        count++;
                }
                nc.Add(count/4.0);
            }
        }

        // 计算角尺度
        public void Uniform_Angle_Index()
        {
            for (int i = 0; i < forest.Count; i++)
            {
                int count = 0;

                //计算周围木方位角
                List<indivTree> trees = new List<indivTree>();
                indivTree iTree = new indivTree();

                for (int j = 0; j < 4; j++)
                {
                    if (units[i].quad[j] == 1)
                    {
                        double angle = Math.Atan((forest[units[i].id[j]].Y - forest[i].Y) / (forest[units[i].id[j]].X - forest[i].X));
                        iTree.azimuth = angle;
                        iTree.id = units[i].id[j];
                        iTree.quadrant = 1;
                        trees.Add(iTree);
                    }
                    else if (units[i].quad[j] == 2)
                    {
                        double angle = Math.Atan((forest[units[i].id[j]].Y - forest[i].Y) / (forest[i].X - forest[units[i].id[j]].X));
                        iTree.azimuth = Math.PI - angle;
                        iTree.id = units[i].id[j];
                        iTree.quadrant = 2;
                        trees.Add(iTree);
                    }
                    else if (units[i].quad[j] == 3)
                    {
                        double angle = Math.Atan((forest[i].Y - forest[units[i].id[j]].Y) / (forest[i].X - forest[units[i].id[j]].X));
                        iTree.azimuth = angle + Math.PI;
                        iTree.id = units[i].id[j];
                        iTree.quadrant = 3;
                        trees.Add(iTree);
                    }
                    else if (units[i].quad[j] == 4)
                    {
                        double angle = Math.Atan((forest[i].Y - forest[units[i].id[j]].Y) / (forest[units[i].id[j]].X - forest[i].X));
                        iTree.azimuth = Math.PI * 2 - angle;
                        iTree.id = units[i].id[j];
                        iTree.quadrant = 4;
                        trees.Add(iTree);
                    }                    
                }

                //按方位角排序,升序
                trees.Sort((left, right) => left.azimuth.CompareTo(right.azimuth));

                //计算夹角
                double inAngle1 = trees[1].azimuth - trees[0].azimuth;
                double inAngle2 = trees[2].azimuth - trees[1].azimuth;
                double inAngle3= trees[3].azimuth - trees[2].azimuth;
                double inAngle4 = trees[0].azimuth + Math.PI * 2 - trees[1].azimuth;

                if (inAngle1 < Math.PI / 2)
                    count++;
                else if (inAngle2 < Math.PI / 2)
                    count++;
                else if (inAngle3 < Math.PI / 2)
                    count++;
                else if (inAngle4 < Math.PI / 2)
                    count++;

                uai.Add(count / 4.0);
            }
        }

        // 计算混交度
        public void Mingling()
        {
            for (int i = 0; i < forest.Count; i++)
            {
                int count = 0;
                for (int j = 0; j < 4; j++)
                {
                    if (forest[i].Species != forest[units[i].id[j]].Species)
                        count++;
                }
                mingle.Add(count / 4.0);
            }
        }

        /// <summary>
        /// 基于权重的采伐决策模型
        /// </summary>
        /// <returns></returns>
        public List<Tree> MarkCuttingTree()
        {
            CalcuMaxHeightDBH();
            CalcuDBH_Height_Index();
            Spatial_Density_Index();
            Neighborhood_Comparison();
            Uniform_Angle_Index();
            Mingling();

            //采伐量
            int cuttingNum = (int)(intensity * forest.Count);

            //计算采伐概率
            if (cuttingType == (int)enCuttingtype.Thinning)
            {
                for (int i = 0; i < forest.Count; i++)
                {
                    double rtm = weightofD * dbhIndex[i] + weightofH * heightIndex[i] + weightofSDI * sdi[i] + weightofNC * nc[i] + weightofUAI * uai[i] + weightofMingle * (1 - mingle[i]) + weightofHealth * (1 - forest[i].Health);
                    forest[i].RTM = rtm;
                }
            }
            else if (cuttingType == (int)enCuttingtype.Selection_cutting)
            {
                for (int i = 0; i < forest.Count; i++)
                {
                    double rtm = weightofD * (1 - dbhIndex[i]) + weightofH * (1 - heightIndex[i]) + weightofSDI * sdi[i] + weightofNC * (1 - nc[i]) + weightofUAI * uai[i] + weightofMingle * (1 - mingle[i]) + weightofHealth * (1 - forest[i].Health);
                    forest[i].RTM = rtm;
                }
            }
            else if (cuttingType == (int)enCuttingtype.Shelterwood_cutting)
            {
                for (int i = 0; i < forest.Count; i++)
                {
                    double rtm = weightofD * (1 - dbhIndex[i]) + weightofH * (1 - heightIndex[i]) + weightofSDI * sdi[i] + weightofNC * (1 - nc[i]) + weightofUAI * uai[i] + weightofMingle * (1 - mingle[i]) + weightofHealth * (1 - forest[i].Health);
                    forest[i].RTM = rtm;
                }
            }
            else if (cuttingType == (int)enCuttingtype.Clear_cutting)
            {
                for (int i = 0; i < forest.Count; i++)
                {
                    forest[i].RTM = 1;
                }
            }
            else if (cuttingType == (int)enCuttingtype.Regeneration_cutting)
            {
                for (int i = 0; i < forest.Count; i++)
                {
                    double rtm = 1 - forest[i].Health;
                    forest[i].RTM = rtm;
                }
            }

            //标记采伐木
            if (cuttingType == (int)enCuttingtype.Clear_cutting)
            {
                for (int i = 0; i < forest.Count; i++)
                {
                    forest[i].isCutting = true;
                }
            }
            else
            {
                //按采伐概率排序，降序
                forest.Sort((left, right) => -left.RTM.CompareTo(right.RTM));

                for (int i = 0; i < cuttingNum; i++)
                {
                    forest[i].isCutting = true;
                }

                //按ID排序，升序
                forest.Sort((left, right) => left.ID.CompareTo(right.ID));
            }

            return forest;
        }
        
    }
}
