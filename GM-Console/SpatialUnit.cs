using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_Console
{
    public class SpatialUnit
    {
        //每个单元内树木
        public struct indivTree
        {
            public int centerID;
            public int id;
            public double distance;
            public double azimuth;
            public int quadrant;
        }

        //空间结构单元结构
        public struct unit_tree
        {
            public int centerID;
            public List<int> id;
            public List<int> quad;
        }

        public List<unit_tree> near4Units = new List<unit_tree>();
        public List<unit_tree> allUnits = new List<unit_tree>();
        public List<int> edgeTree = new List<int>(); //边界木

        /// <summary>
        /// 划分空间结构单元
        /// </summary>
        public void DivideUnit(List<Tree> array)
        {
            //清理空间结构单元
            near4Units.Clear();
            allUnits.Clear();
            edgeTree.Clear();

            //for (int i = 0; i < array.Count; i++)
            ParallelLoopResult parallelresult = Parallel.For(0, array.Count, i =>
            {
                List<indivTree> TreeInForest = new List<indivTree>();
                List<indivTree> near4trees = new List<indivTree>();          //最近四株木
                List<indivTree> ssUnit = new List<indivTree>();                  //空间结构单元

                for (int j = 0; j < array.Count; j++)
                {
                    indivTree iTree = new indivTree();

                    if (i != j)
                    {
                        //id
                        iTree.centerID = i;
                        iTree.id = j;

                        //计算距离
                        double distance = Math.Sqrt((array[i].X - array[j].X) * (array[i].X - array[j].X) + (array[i].Y - array[j].Y) * (array[i].Y - array[j].Y));
                        iTree.distance = distance;

                        //确定象限
                        double x_ = array[j].X - array[i].X;
                        double y_ = array[j].Y - array[i].Y;

                        if (x_ > 0 && y_ >= 0)
                        {
                            iTree.quadrant = 1;
                        }
                        else if (x_ >= 0 && y_ < 0)
                        {
                            iTree.quadrant = 4;
                        }
                        else if (x_ < 0 && y_ <= 0)
                        {
                            iTree.quadrant = 3;
                        }
                        else if (x_ <= 0 && y_ > 0)
                        {
                            iTree.quadrant = 2;
                        }
                        else
                        {
                            Console.WriteLine("Abnormality of tree position... Tree ID: " + i);
                            return;
                        }

                        TreeInForest.Add(iTree);
                    }
                }

                //对树木按距离进行排序
                TreeInForest.Sort((left, right) => left.distance.CompareTo(right.distance));
                //{
                //    if (left.distance > right.distance)
                //        return 1;
                //    else if (left.distance == right.distance)
                //        return 0;
                //    else
                //        return -1;
                //});

                //最近四株木
                for (int j = 0; j < 4; j++)
                {
                    near4trees.Add(TreeInForest[j]);
                }

                //全象限空间结构单元
                int lackQuad;
                ssUnit = near4trees;
                if (!quadContained(1, ssUnit))
                {
                    int treeCount;
                    lackQuad = 1;
                    for (treeCount = 4; treeCount < TreeInForest.Count; treeCount++)
                    {
                        if (TreeInForest[treeCount].quadrant == lackQuad)
                        {
                            ssUnit.Add(TreeInForest[treeCount]);
                            break;
                        }
                    }
                    if (treeCount >= TreeInForest.Count)
                        edgeTree.Add(i);
                }
                if (!quadContained(2, ssUnit))
                {
                    int treeCount;
                    lackQuad = 2;
                    for (treeCount = 4; treeCount < TreeInForest.Count; treeCount++)
                    {
                        if (TreeInForest[treeCount].quadrant == lackQuad)
                        {
                            ssUnit.Add(TreeInForest[treeCount]);
                            break;
                        }
                    }

                    if (treeCount >= TreeInForest.Count)
                        edgeTree.Add(i);
                }
                if (!quadContained(3, ssUnit))
                {
                    int treeCount;
                    lackQuad = 3;
                    for (treeCount = 4; treeCount < TreeInForest.Count; treeCount++)
                    {
                        if (TreeInForest[treeCount].quadrant == lackQuad)
                        {
                            ssUnit.Add(TreeInForest[treeCount]);
                            break;
                        }
                    }

                    if (treeCount >= TreeInForest.Count)
                        edgeTree.Add(i);
                }
                if (!quadContained(4, ssUnit))
                {
                    int treeCount;
                    lackQuad = 4;
                    for (treeCount = 4; treeCount < TreeInForest.Count; treeCount++)
                    {
                        if (TreeInForest[treeCount].quadrant == lackQuad)
                        {
                            ssUnit.Add(TreeInForest[treeCount]);
                            break;
                        }
                    }

                    if (treeCount >= TreeInForest.Count)
                        edgeTree.Add(i);
                }

                //保存所有惠氏空间结构单元
                unit_tree ut;
                ut.centerID = i;
                ut.id = new List<int>();
                ut.quad = new List<int>();

                for (int j = 0; j < 4; j++)
                {
                    ut.id.Add(ssUnit[j].id);
                    ut.quad.Add(ssUnit[j].quadrant);
                }
                lock (near4Units)
                {
                    near4Units.Add(ut);
                }

                //保存所有全象限空间结构单元
                ut.id.Clear();
                ut.quad.Clear();
                for (int j = 0; j < ssUnit.Count; j++)
                {
                    ut.id.Add(ssUnit[j].id);
                    ut.quad.Add(ssUnit[j].quadrant);
                }
                lock (allUnits)
                {
                    allUnits.Add(ut);
                }
            });
        }

        /// <summary>
        /// 计算垂直空间结构参数
        /// </summary>
        public double CalcuPv(int orient, int id, List<Tree> array)
        {
            double pv = 0;
            int count = 0;

            if (orient == 1 || orient == 2)
            {
                for (int i = 0; i < allUnits[id].id.Count; i++)
                {
                    if (allUnits[id].quad[i] == 1)
                    {
                        count++;
                        pv += (array[allUnits[id].id[i]].Height) / array[id].Height;
                    }
                }
            }
            if (orient == 2 || orient == 3)
            {
                for (int i = 0; i < allUnits[id].id.Count; i++)
                {
                    if (allUnits[id].quad[i] == 2)
                    {
                        count++;
                        pv += (array[allUnits[id].id[i]].Height) / array[id].Height;
                    }
                }
            }
            if (orient == 3 || orient == 4)
            {
                for (int i = 0; i < allUnits[id].id.Count; i++)
                {
                    if (allUnits[id].quad[i] == 3)
                    {
                        count++;
                        pv += (array[allUnits[id].id[i]].Height) / array[id].Height;
                    }
                }
            }
            if (orient == 4 || orient == 1)
            {
                for (int i = 0; i < allUnits[id].id.Count; i++)
                {
                    if (allUnits[id].quad[i] == 4)
                    {
                        count++;
                        pv += (array[allUnits[id].id[i]].Height) / array[id].Height;
                    }
                }
            }
            if (count > 0)
            {
                pv = pv / count;
            }
            else
            {
                Console.WriteLine("ERROR：计算垂直空间结构单元异常");
            }

            return pv;
        }

        /// <summary>
        /// 计算水平空间结构参数 
        /// </summary>
        public double CalcuPh(int orient, int id, List<Tree> array)
        {
            double ph = 0;
            double Dr = 0;
            double direction = 0;

            if (orient == 1 || orient == 2)
            {
                for (int i = 0; i < allUnits[id].id.Count; i++)
                {
                    if (allUnits[id].quad[i] == 1)
                    {
                        direction = Math.Sqrt((array[allUnits[id].id[i]].X - array[id].X) * (array[allUnits[id].id[i]].X - array[id].X)
                                                               + (array[allUnits[id].id[i]].Y - array[id].Y) * (array[allUnits[id].id[i]].Y - array[id].Y));

                        if (Dr == 0)
                            Dr = direction * direction / Math.Abs(array[allUnits[id].id[i]].X - array[id].X);
                        else
                        {
                            double Dr2 = direction * direction / Math.Abs(array[allUnits[id].id[i]].X - array[id].X);
                            Dr = Dr2 < Dr ? Dr2 : Dr;
                        }
                    }
                }
            }
            if (orient == 2 || orient == 3)
            {
                for (int i = 0; i < allUnits[id].id.Count; i++)
                {
                    if (allUnits[id].quad[i] == 2)
                    {
                        direction = Math.Sqrt((array[allUnits[id].id[i]].X - array[id].X) * (array[allUnits[id].id[i]].X - array[id].X)
                                                               + (array[allUnits[id].id[i]].Y - array[id].Y) * (array[allUnits[id].id[i]].Y - array[id].Y));

                        if (Dr == 0)
                            Dr = direction * direction / Math.Abs(array[allUnits[id].id[i]].X - array[id].X);
                        else
                        {
                            double Dr2 = direction * direction / Math.Abs(array[allUnits[id].id[i]].X - array[id].X);
                            Dr = Dr2 < Dr ? Dr2 : Dr;
                        }
                    }
                }
            }
            if (orient == 3 || orient == 4)
            {
                for (int i = 0; i < allUnits[id].id.Count; i++)
                {
                    if (allUnits[id].quad[i] == 3)
                    {
                        direction = Math.Sqrt((array[allUnits[id].id[i]].X - array[id].X) * (array[allUnits[id].id[i]].X - array[id].X)
                                                               + (array[allUnits[id].id[i]].Y - array[id].Y) * (array[allUnits[id].id[i]].Y - array[id].Y));

                        if (Dr == 0)
                            Dr = direction * direction / Math.Abs(array[allUnits[id].id[i]].X - array[id].X);
                        else
                        {
                            double Dr2 = direction * direction / Math.Abs(array[allUnits[id].id[i]].X - array[id].X);
                            Dr = Dr2 < Dr ? Dr2 : Dr;
                        }
                    }
                }
            }
            if (orient == 4 || orient == 1)
            {
                for (int i = 0; i < allUnits[id].id.Count; i++)
                {
                    if (allUnits[id].quad[i] == 4)
                    {
                        direction = Math.Sqrt((array[allUnits[id].id[i]].X - array[id].X) * (array[allUnits[id].id[i]].X - array[id].X)
                                                               + (array[allUnits[id].id[i]].Y - array[id].Y) * (array[allUnits[id].id[i]].Y - array[id].Y));

                        if (Dr == 0)
                            Dr = direction * direction / Math.Abs(array[allUnits[id].id[i]].X - array[id].X);
                        else
                        {
                            double Dr2 = direction * direction / Math.Abs(array[allUnits[id].id[i]].X - array[id].X);
                            Dr = Dr2 < Dr ? Dr2 : Dr;
                        }
                    }
                }
            }

            ph = Dr;

            if (ph > 7)
                ph = 7;
            return ph;
        }

        //象限是否存在
        private bool quadContained(int quad, List<indivTree> Trees)
        {
            int i = 0;
            for (i = 0; i < Trees.Count; i++)
            {
                if (Trees[i].quadrant == quad)
                    break;
            }
            if (i < Trees.Count)
                return true;
            else
                return false;
        }
    }
}
