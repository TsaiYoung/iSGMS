using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console.modelLibrary.DBHmodels
{
    class DBHGrowthModelSet
    {
        private List<Tree> array = new List<Tree>();
        public DBHGrowthModelSet(List<Tree> trees)
        {
            array = trees;
        }

        /// <summary>
        /// 基于简单竞争指数胸径连年生长量模型(覃阳平)——胸径
        /// param 0~7：模型参数；param 8~10：竞争指数参数
        /// </summary>
        /// <param name="units"></param>
        /// <param name="dem"></param>
        /// <param name="param"></param>
        /// <param name="SI"></param>
        /// <returns></returns>
        public List<Tree> DBHGrowthModel1(List<SpatialUnit.unit_tree> units, DEMOperate dem, List<double> param, double SI)
        {
            //double avgDBH = 0;
            //int count = 0;

            List<double> D = new List<double>();

            for (int i = 0; i < array.Count; i++)
            {
                //胸径增长量
                double DGI = 0;
                //计算x，y对应DEM的行列号
                dem.getColRow(array[i].X, array[i].Y);
                //读取高程
                double altitude = dem.getDEMValue();
                //计算坡度
                double slope = dem.CalcuSlope();
                //计算坡向
                double aspect = dem.CalcuAspect();
                //生长模型
                //计算胸径增长量
                //计算林木大小因子
                double size = param[1] * Math.Log(array[i].DBH) + param[2] * array[i].DBH * array[i].DBH;

                //计算竞争因子
                double distance1 = Math.Sqrt((array[units[i].id[0]].X - array[i].X) * (array[units[i].id[0]].X - array[i].X) + (array[units[i].id[0]].Y - array[i].Y) * (array[units[i].id[0]].Y - array[i].Y));
                double comp1 = (Math.Pow(array[units[i].id[0]].DBH, param[8]) / Math.Pow(array[i].DBH, param[9])) / Math.Pow(distance1, param[10]);

                double distance2 = Math.Sqrt((array[units[i].id[1]].X - array[i].X) * (array[units[i].id[1]].X - array[i].X) + (array[units[i].id[1]].Y - array[i].Y) * (array[units[i].id[1]].Y - array[i].Y));
                double comp2 = (Math.Pow(array[units[i].id[1]].DBH, param[8]) / Math.Pow(array[i].DBH, param[9])) / Math.Pow(distance2, param[10]);

                double distance3 = Math.Sqrt((array[units[i].id[2]].X - array[i].X) * (array[units[i].id[2]].X - array[i].X) + (array[units[i].id[2]].Y - array[i].Y) * (array[units[i].id[2]].Y - array[i].Y));
                double comp3 = (Math.Pow(array[units[i].id[2]].DBH, param[8]) / Math.Pow(array[i].DBH, param[9])) / Math.Pow(distance3, param[10]);

                double distance4 = Math.Sqrt((array[units[i].id[3]].X - array[i].X) * (array[units[i].id[3]].X - array[i].X) + (array[units[i].id[3]].Y - array[i].Y) * (array[units[i].id[3]].Y - array[i].Y));
                double comp4 = (Math.Pow(array[units[i].id[3]].DBH, param[8]) / Math.Pow(array[i].DBH, param[9])) / Math.Pow(distance4, param[10]);
                double comp = comp1 + comp2 + comp3 + comp4;

                //计算立地条件
                double site = param[4] * SI + param[5] * Math.Tan(slope) + param[6] * altitude + param[7] * aspect * 180 / Math.PI;

                //胸径增长量
                DGI = Math.Exp(param[0] + size + param[3] * comp + site);
                double tempD = Math.Sqrt(DGI + array[i].DBH * array[i].DBH);

                array[i].DBH = tempD;

                if (Double.IsNaN(array[i].DBH) || Double.IsInfinity(array[i].DBH))
                {
                    Console.WriteLine("ERROR: NaN or Infinity of DBH");
                    return null;
                }

            }

            ////边界木变化取平均值
            //if (count > 0)
            //{
            //    avgDBH = avgDBH / count;
            //    for (int i = 0; i < array.Count; i++)
            //    {
            //        if (array[i].isEdge)
            //        {
            //            array[i].DBH = avgDBH;
            //        }
            //    }
            //}

            return array;
        }

        /// <summary>
        /// 基于树冠竞争因子的落叶松人工林单木生长模型(刘强)——断面积
        /// param 0~10：模型参数；param 11：林分密度指数参数
        /// </summary>
        /// <param name="units"></param>
        /// <param name="dem"></param>
        /// <param name="param"></param> 
        /// <param name="D0">标准平均直径（中国一般取10）</param>
        /// <returns></returns>
        public List<Tree> DBHGrowthModel2(List<SpatialUnit.unit_tree> units, DEMOperate dem, List<double> param, double D0, double area)
        {
            //林木大小
            double Dg = 0;
            List<double> AREA = new List<double>();

            double avgA = 0;
            int count = 0;

            //林分平均直径
            for (int i = 0; i < array.Count; i++)
            {
                Dg = Dg + Math.PI * array[i].DBH * array[i].DBH / 4;
            }
            Dg = Math.Sqrt(Dg / array.Count / Math.PI) * 2;

            for (int i = 0; i < array.Count; i++)
            {
                if (!array[i].isEdge)
                {
                    double size = param[1] * Math.Log(array[i].DBH) + param[2] * array[i].DBH * array[i].DBH + param[3] * Dg;

                    //Hegyi竞争指数
                    double distance1 = Math.Sqrt((array[units[i].id[0]].X - array[i].X) * (array[units[i].id[0]].X - array[i].X) + (array[units[i].id[0]].Y - array[i].Y) * (array[units[i].id[0]].Y - array[i].Y));
                    double comp1 = (array[units[i].id[0]].DBH / array[i].DBH) / distance1;

                    double distance2 = Math.Sqrt((array[units[i].id[1]].X - array[i].X) * (array[units[i].id[1]].X - array[i].X) + (array[units[i].id[1]].Y - array[i].Y) * (array[units[i].id[1]].Y - array[i].Y));
                    double comp2 = (array[units[i].id[1]].DBH / array[i].DBH) / distance2;

                    double distance3 = Math.Sqrt((array[units[i].id[2]].X - array[i].X) * (array[units[i].id[2]].X - array[i].X) + (array[units[i].id[2]].Y - array[i].Y) * (array[units[i].id[2]].Y - array[i].Y));
                    double comp3 = (array[units[i].id[2]].DBH / array[i].DBH) / distance3;

                    double distance4 = Math.Sqrt((array[units[i].id[3]].X - array[i].X) * (array[units[i].id[3]].X - array[i].X) + (array[units[i].id[3]].Y - array[i].Y) * (array[units[i].id[3]].Y - array[i].Y));
                    double comp4 = (array[units[i].id[3]].DBH / array[i].DBH) / distance4;
                    double CI = comp1 + comp2 + comp3 + comp4;

                    //对象木冠面积
                    double C = CrownArea(i);
                    //树冠竞争指数
                    double CI1 = CrownArea(units[i].id[0]) / CrownArea(i) / distance1 + CrownArea(units[i].id[1]) / CrownArea(i) / distance2 + CrownArea(units[i].id[2]) / CrownArea(i) / distance3 + CrownArea(units[i].id[3]) / CrownArea(i) / distance4;
                    double CI2 = (CrownArea(units[i].id[0]) + CrownArea(i)) / CrownArea(i) / distance1 + (CrownArea(units[i].id[1]) + CrownArea(i)) / CrownArea(i) / distance2 + (CrownArea(units[i].id[2]) + CrownArea(i)) / CrownArea(i) / distance3 + (CrownArea(units[i].id[3]) + CrownArea(i)) / CrownArea(i) / distance4;

                    //林分密度指数
                    double N = array.Count / area * 10000;
                    double SDI = N * Math.Pow(D0 / array[i].DBH, param[11]);

                    double comp = param[4] * array[i].Height + param[5] * array[i].UnderBranchHeight + param[6] * C + param[7] * CI + param[8] * CI1 + param[9] * CI2 + param[10]*SDI;

                    double AGI = Math.Exp(param[0] + size + comp);

                    double tempA = AGI + Math.PI * (array[i].DBH * array[i].DBH / 4);
                    AREA.Add(tempA);

                    if (Double.IsNaN(tempA) || Double.IsInfinity(tempA))
                    {
                        Console.WriteLine("ERROR: NaN or Infinity of DBH");
                        return null;
                    }

                    avgA += tempA;
                    count++;
                }
            }

            //边界木变化取平均值
            if (count > 0)
            {
                avgA = avgA / count;
                for (int i = 0; i < array.Count; i++)
                {
                    if (array[i].isEdge)
                    {
                        AREA[i] = avgA;
                    }
                }
            }

            //更新胸径
            for (int i = 0; i < array.Count; i++)
            {
                array[i].DBH = Math.Sqrt(AREA[i] / Math.PI) * 2;
            }

            return array;
        }
        //计算树冠面积
        private double CrownArea(int id)
        {
            double CA = 0;

            if (array[id].eastCrownWidth > 0 && array[id].southCrownWidth > 0 && array[id].westCrownWidth > 0 && array[id].northCrownWidth > 0)
                CA = Math.PI * Math.Pow((array[id].eastCrownWidth + array[id].southCrownWidth + array[id].westCrownWidth + array[id].northCrownWidth) / 4, 2);
            else
                CA = Math.PI * array[id].CrownWidth * array[id].CrownWidth / 4;

            return CA;
        }

        /// <summary>
        /// 基于气象因子的白桦天然林单木直径生长模型（张海平，2017）
        /// minTg：生长季最低气温； meanPg：生长季平均降水量
        /// </summary>
        /// <param name="units"></param>
        /// <param name="dem"></param>
        /// <param name="param"></param>
        /// <param name="minTg"></param>
        /// <param name="meanPg"></param>
        /// <returns></returns>
        public List<Tree> DBHGrowthModel3(List<SpatialUnit.unit_tree> units, DEMOperate dem, List<double> param, double minTg, double meanPg, double area)
        {

            //BAS 每公顷断面积
            double BA = 0;
            for (int i = 0; i < array.Count; i++)
            {
                BA = BA + Math.PI * array[i].DBH * array[i].DBH / 4;
            }
            double BAS = BA / area;

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

            for (int i = 0; i < array.Count; i++)
            {
                //林木个体
                double size = param[1] * Math.Log(array[i].DBH) + param[2] * array[i].DBH * array[i].DBH;

                //竞争
                double comp = param[3] * BAS + param[4] * BAL[i] / array[i].DBH;

                //气象因子
                double clim = param[5] * minTg + param[6] * meanPg;

                double DGI = Math.Exp(param[0] + size + comp + clim) - 1;

                array[i].DBH = Math.Sqrt(array[i].DBH * array[i].DBH + DGI);


                if (Double.IsNaN(array[i].DBH) || Double.IsInfinity(array[i].DBH))
                {
                    Console.WriteLine("ERROR: NaN or Infinity of DBH");
                    return null;
                }
            }

            return array;
        }

        /// <summary>
        /// 蒙古栎天然林单木生长模型研究(马武，2015)
        /// </summary>
        /// <param name="units"></param>
        /// <param name="dem"></param>
        /// <param name="param">param16：林分密度指数参数</param>
        /// <param name="canopydensity">郁闭度</param>
        /// <param name="SI">地位指数</param>
        /// <param name="D0">标准平均直径（中国一般取10）</param>
        /// <returns></returns>
        public List<Tree> DBHGrowthModel4(List<SpatialUnit.unit_tree> units, DEMOperate dem, List<double> param, double canopydensity, double SI, double D0, double area)
        {
            //林分每公顷断面积
            double BA = 0;
            for (int i = 0; i < array.Count; i++)
            {
                BA = BA + Math.PI * array[i].DBH * array[i].DBH / 4;
            }
            double BAS = BA / area;

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
            //林分中最大直径
            double maxDBH = array[0].DBH;
            for (int i = 0; i < array.Count; i++)
            {
                if (maxDBH < array[i].DBH)
                    maxDBH = array[i].DBH;
            }

            for (int i = 0; i < array.Count; i++)
            {
                //林木个体
                double size = param[1] * Math.Log(array[i].DBH) + param[2] * array[i].DBH * array[i].DBH;

                //林分密度指数
                double N = array.Count / area * 10000;
                double SDI = N * Math.Pow(D0 / array[i].DBH, param[17]);

                //对象木胸径与林分断面积平均胸径之比RD
                double RD = array[i].DBH / (Math.Sqrt((BA / array.Count) / Math.PI) * 2);

                //林分中大于对象木的所有林木直径平方和DL
                double DL = 0;
                for (int j = 0; j < array.Count; j++)
                {
                    if (array[j].DBH > array[i].DBH)
                    {
                        DL = DL + array[j].DBH * array[j].DBH;
                    }
                }

                //对象木直径与林分中最大林木直径之比DDM                
                double DDM = array[i].DBH / maxDBH;

                //竞争
                double comp = param[3] * BAL[i] + param[4] * SDI + param[5] * BAS + param[6] * RD + param[7] * DL + param[8] * canopydensity + param[9] * DDM;

                //立地条件
                //计算x，y对应DEM的行列号
                dem.getColRow(array[i].X, array[i].Y);
                //读取高程
                double altitude = dem.getDEMValue();
                //计算坡度
                double slope = dem.CalcuSlope();
                //计算坡向
                double aspect = dem.CalcuAspect();

                double SL = Math.Tan(slope);
                double E = altitude;
                double SLS = SL * Math.Sin(aspect);
                double SLC = SL * Math.Cos(aspect);
                double site = param[10] * SI + param[11] * SL + param[12] * SL * SL + param[13] * E + param[14] * E * E + param[15] * SLS + param[16] * SLC;

                double DGI =Math.Exp( param[0] + size + comp + site);
                array[i].DBH = Math.Sqrt(array[i].DBH * array[i].DBH + DGI);

                if (Double.IsNaN(array[i].DBH) || Double.IsInfinity(array[i].DBH))
                {
                    Console.WriteLine("ERROR: NaN or Infinity of DBH");
                    return null;
                }
            }
            return array;
        }

        /// <summary>
        /// Sönmez 2009, A distance-independent basal area growth model for oriental spruce...
        /// 针对混合混交林，对于纯林同样适用
        /// </summary>
        /// <param name="units"></param>
        /// <param name="dem"></param>
        /// <param name="param"></param>
        /// <param name="SI"></param>
        /// <param name="age"></param>
        /// <returns></returns>
        public List<Tree> DBHGrowthModel5(List<SpatialUnit.unit_tree> units, DEMOperate dem, List<double> param, double SI, double age, double area,double BP)
        {
            //BAS 每公顷断面积
            double BA = 0;
            for (int i = 0; i < array.Count; i++)
            {
                BA = BA + Math.PI * array[i].DBH * array[i].DBH / 4;
            }
            BA = BA / area;

            double N = array.Count / area * 10000;

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

            for (int i = 0; i < array.Count; i++)
            {
                //个体参数
                double size = param[1] * Math.Log(array[i].DBH) + param[2] * array[i].DBH * array[i].DBH + param[3] / age;

                //竞争
                double comp = param[4] * BAL[i] + param[5] * Math.Log(BA) + param[6] * Math.Log(N);

                //计算x，y对应DEM的行列号
                dem.getColRow(array[i].X, array[i].Y);
                //读取高程
                double altitude = dem.getDEMValue();
                //计算坡度
                double slope = dem.CalcuSlope();
                //计算坡向
                double aspect = dem.CalcuAspect();

                //立地
                double site = param[7] * SI + param[8] * altitude + param[9] * Math.Log(aspect * 180 / Math.PI + 1);

                //混交
                double mix = param[10] * Math.Log(BP + 1);

                double DGI =Math.Exp( param[0] + size + comp + site);

                array[i].DBH = 2 * Math.Sqrt((Math.PI * array[i].DBH * array[i].DBH / 4 + DGI) / Math.PI);

                if (Double.IsNaN(array[i].DBH) || Double.IsInfinity(array[i].DBH))
                {
                    Console.WriteLine("ERROR: NaN or Infinity of DBH");
                    return null;
                }
            }
            return array;
        }

        /// <summary>
        /// Using optimization for fitting individual-tree growth models for uneven-aged stands(Pukkala,2011)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<Tree> DBHGrowthModel6(List<double> param, double area, int MT, int VT, double TS)
        {
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

            //每公顷断面积
            double BA = 0;
            for (int i = 0; i < array.Count; i++)
            {
                BA = BA + Math.PI * array[i].DBH * array[i].DBH / 4;
            }
            double G = BA / area;

            for (int i = 0; i < array.Count; i++)
            {
                double iD = Math.Exp(param[0] + param[1] * BAL[i] + param[2] * Math.Log(G) + param[3] * Math.Sqrt(array[i].DBH) + param[4] * array[i].DBH * array[i].DBH + param[5] * MT + param[6] * VT + param[7] * Math.Log(TS));
                array[i].DBH = array[i].DBH + iD;

                if (Double.IsNaN(array[i].DBH) || Double.IsInfinity(array[i].DBH))
                {
                    Console.WriteLine("ERROR: NaN or Infinity of DBH");
                    return null;
                }
            }

            return array;
        }

        /// <summary>
        /// Modelling annual individual-tree growth and mortality of Scots pine with data obtained at irregular measurement intervals and containing missing observations
        /// Felipe Crecente-Campo,2010
        /// </summary>
        /// <param name="param"></param>
        /// <param name="area"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public List<Tree> DBHGrowthModel7(List<double> param, double area, int t)
        {
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

            //每公顷断面积
            double BA = 0;
            for (int i = 0; i < array.Count; i++)
            {
                BA = BA + Math.PI * array[i].DBH * array[i].DBH / 4;
            }
            double G = BA / area;

            for (int i = 0; i < array.Count; i++)
            {
                double ig = param[0] * Math.Pow(array[i].DBH, param[1]) * Math.Pow(G, param[2]) * Math.Exp(param[3] * t + param[4] * BAL[i]);
                array[i].DBH = Math.Sqrt((array[i].DBH * array[i].DBH * Math.PI / 4 + ig) / Math.PI) * 2;

                if (Double.IsNaN(array[i].DBH) || Double.IsInfinity(array[i].DBH))
                {
                    Console.WriteLine("ERROR: NaN or Infinity of DBH");
                    return null;
                }
            }

            return array;
        }

        /// <summary>
        /// Individual-tree growth and mortality models for Eucalyptus grandis (Hill) Maiden plantations in Zimbabwe
        /// Danaza Mabvurira,2002
        /// 非空间结构模型
        /// </summary>
        /// <param name="param"></param>
        /// <param name="area"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public List<Tree> DBHGrowthModel8(List<double> param, double area, int t)
        {
            //每公顷断面积
            double BA = 0;
            for (int i = 0; i < array.Count; i++)
            {
                BA = BA + Math.PI * array[i].DBH * array[i].DBH / 4;
            }
            double G = BA / area;

            //优势高
            array.Sort((left, right) => -left.Height.CompareTo(right.Height));
            double[] domainHeight = new double[5];
            for (int j = 0; j < 5; j++)
            {
                domainHeight[j] = array[j].Height;
            }
            double Hidom = domainHeight.Average();
            array.Sort((left, right) => left.ID.CompareTo(right.ID));

            for (int i = 0; i < array.Count; i++)
            {
                double id = Math.Exp(param[0] + param[1] * array[i].DBH + param[2] * array[i].DBH * array[i].DBH + param[3] * Math.Log(array[i].DBH / (t + 1)) + param[4] * Math.Log(t) + param[5] * Math.Log(G) + param[6] * Math.Log(Hidom));
                array[i].DBH += id;

                if (Double.IsNaN(array[i].DBH) || Double.IsInfinity(array[i].DBH))
                {
                    Console.WriteLine("ERROR: NaN or Infinity of DBH");
                    return null;
                }
            }

            return array;
        }

        /// <summary>
        /// Individual-tree growth and mortality models for Eucalyptus grandis (Hill) Maiden plantations in Zimbabwe
        /// Danaza Mabvurira,2002
        /// 空间结构模型
        /// </summary>
        /// <param name="param"></param>
        /// <param name="area"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public List<Tree> DBHGrowthModel9(List<double> param, double area, int t)
        {

            //每公顷断面积
            double BA = 0;
            for (int i = 0; i < array.Count; i++)
            {
                BA = BA + Math.PI * array[i].DBH * array[i].DBH / 4;
            }
            double G = BA / area;

            //优势高
            array.Sort((left, right) => -left.Height.CompareTo(right.Height));
            double[] domainHeight = new double[5];
            for (int j = 0; j < 5; j++)
            {
                domainHeight[j] = array[j].Height;
            }
            double Hidom = domainHeight.Average();
            array.Sort((left, right) => left.ID.CompareTo(right.ID));

            //竞争指数
            List<double> CI = new List<double>();
            for (int i = 0; i < array.Count; i++)
            {
                double ci = 0;
                for (int j = 0; j < array.Count; j++)
                {
                    double distance = Math.Sqrt((array[j].X - array[i].X) * (array[j].X - array[i].X) + (array[j].Y - array[i].Y) * (array[j].Y - array[i].Y));
                    if (distance < 5 && array[j].Height > array[i].Height)
                    {
                        double va = Math.Atan((array[j].Height - array[i].Height) / distance);
                        ci = ci + va;
                    }
                }
                CI.Add(ci);
            }

            for (int i = 0; i < array.Count; i++)
            {
                double id = param[0] + param[1] * Math.Log(array[i].DBH / (t + 1)) + param[2] * Math.Log(G) + param[3] * Math.Log(Hidom) + param[4] * Math.Log(CI[i] + 1);
                array[i].DBH += id;

                if (Double.IsNaN(array[i].DBH) || Double.IsInfinity(array[i].DBH))
                {
                    Console.WriteLine("ERROR: NaN or Infinity of DBH");
                    return null;
                }
            }

            return array;
        }
        
        /// <summary>
        /// 以相对直径为竞争指标的单木直径生长模型研究
        /// 邓成，2011
        /// Richard型
        /// </summary>
        /// <param name="param"></param>
        /// <param name="area"></param>
        /// <param name="t"></param>
        /// <param name="SI"></param>
        /// <returns></returns>
        public List<Tree> DBHGrowthModel10(List<double> param, double area, int t, double SI)
        {
            //林分密度
            double N = array.Count / area * 10000;

            //林分平均直径
            double Dg = 0;
            for (int i = 0; i < array.Count; i++)
            {
                Dg = Dg + Math.PI * array[i].DBH * array[i].DBH / 4;
            }
            Dg = Math.Sqrt(Dg / array.Count / Math.PI) * 2;

            for (int i = 0; i < array.Count; i++)
            {
                double K = param[0] * Math.Pow(SI, param[1]) * Math.Pow(N, param[2]) * Math.Pow(array[i].DBH / Dg, param[3]);
                array[i].DBH = K * Math.Pow(1 - Math.Exp(-param[4] * t), param[5]);


                if (Double.IsNaN(array[i].DBH) || Double.IsInfinity(array[i].DBH))
                {
                    Console.WriteLine("ERROR: NaN or Infinity of DBH");
                    return null;
                }
            }

            return array;
        }

        /// <summary>
        /// 以相对直径为竞争指标的单木直径生长模型研究
        /// 邓成，2011
        /// Logistic型
        /// </summary>
        /// <param name="param"></param>
        /// <param name="area"></param>
        /// <param name="t"></param>
        /// <param name="SI"></param>
        /// <returns></returns>
        public List<Tree> DBHGrowthModel11(List<double> param, double area, int t, double SI)
        {
            //林分密度
            double N = array.Count / area * 10000;

            //林分平均直径
            double Dg = 0;
            for (int i = 0; i < array.Count; i++)
            {
                Dg = Dg + Math.PI * array[i].DBH * array[i].DBH / 4;
            }
            Dg = Math.Sqrt(Dg / array.Count / Math.PI) * 2;

            for (int i = 0; i < array.Count; i++)
            {
                double K = param[0] * Math.Pow(SI, param[1]) * Math.Pow(N, param[2]) * Math.Pow(array[i].DBH / Dg, param[3]);
                array[i].DBH = K / (1 + param[4] * Math.Exp(-param[5] * t));

                if (Double.IsNaN(array[i].DBH) || Double.IsInfinity(array[i].DBH))
                {
                    Console.WriteLine("ERROR: NaN or Infinity of DBH");
                    return null;
                }
            }

            return array;
        }

        /// <summary>
        /// 以相对直径为竞争指标的单木直径生长模型研究
        /// 邓成，2011
        /// Mitscherlich型
        /// </summary>
        /// <param name="param"></param>
        /// <param name="area"></param>
        /// <param name="t"></param>
        /// <param name="SI"></param>
        /// <returns></returns>
        public List<Tree> DBHGrowthModel12(List<double> param, double area, int t, double SI)
        {
            //林分密度
            double N = array.Count / area * 10000;

            //林分平均直径
            double Dg = 0;
            for (int i = 0; i < array.Count; i++)
            {
                Dg = Dg + Math.PI * array[i].DBH * array[i].DBH / 4;
            }
            Dg = Math.Sqrt(Dg / array.Count / Math.PI) * 2;

            for (int i = 0; i < array.Count; i++)
            {
                double K = param[0] * Math.Pow(SI, param[1]) * Math.Pow(N, param[2]) * Math.Pow(array[i].DBH / Dg, param[3]);
                array[i].DBH = K * (1 - Math.Exp(-param[4] * t));

                if (Double.IsNaN(array[i].DBH) || Double.IsInfinity(array[i].DBH))
                {
                    Console.WriteLine("ERROR: NaN or Infinity of DBH");
                    return null;
                }
            }

            return array;
        }

        /// <summary>
        /// 以相对直径为竞争指标的单木直径生长模型研究
        /// 邓成，2011
        /// Campertz型
        /// </summary>
        /// <param name="param"></param>
        /// <param name="area"></param>
        /// <param name="t"></param>
        /// <param name="SI"></param>
        /// <returns></returns>
        public List<Tree> DBHGrowthModel13(List<double> param, double area, int t, double SI)
        {
            //林分密度
            double N = array.Count / area * 10000;

            //林分平均直径
            double Dg = 0;
            for (int i = 0; i < array.Count; i++)
            {
                Dg = Dg + Math.PI * array[i].DBH * array[i].DBH / 4;
            }
            Dg = Math.Sqrt(Dg / array.Count / Math.PI) * 2;

            for (int i = 0; i < array.Count; i++)
            {
                double K = param[0] * Math.Pow(SI, param[1]) * Math.Pow(N, param[2]) * Math.Pow(array[i].DBH / Dg, param[3]);
                array[i].DBH = K * Math.Exp(-param[4] * Math.Exp(-param[5] * t));

                if (Double.IsNaN(array[i].DBH) || Double.IsInfinity(array[i].DBH))
                {
                    Console.WriteLine("ERROR: NaN or Infinity of DBH");
                    return null;
                }
            }

            return array;
        }

        /// <summary>
        /// 以相对直径为竞争指标的单木直径生长模型研究
        /// 邓成，2011
        /// Modified-Weibull型
        /// </summary>
        /// <param name="param"></param>
        /// <param name="area"></param>
        /// <param name="t"></param>
        /// <param name="SI"></param>
        /// <returns></returns>
        public List<Tree> DBHGrowthModel14(List<double> param, double area, int t, double SI)
        {
            //林分密度
            double N = array.Count / area * 10000;

            //林分平均直径
            double Dg = 0;
            for (int i = 0; i < array.Count; i++)
            {
                Dg = Dg + Math.PI * array[i].DBH * array[i].DBH / 4;
            }
            Dg = Math.Sqrt(Dg / array.Count / Math.PI) * 2;

            for (int i = 0; i < array.Count; i++)
            {
                double K = param[0] * Math.Pow(SI, param[1]) * Math.Pow(N, param[2]) * Math.Pow(array[i].DBH / Dg, param[3]);
                array[i].DBH = K * (1 - Math.Exp(-param[4] * Math.Pow(t, param[5])));

                if (Double.IsNaN(array[i].DBH) || Double.IsInfinity(array[i].DBH))
                {
                    Console.WriteLine("ERROR: NaN or Infinity of DBH");
                    return null;
                }
            }

            return array;
        }
        
        /// <summary>
        /// 曾思齐,2001（李永亮博士论文）
        /// </summary>
        /// <param name="param"></param>
        /// <param name="area"></param>
        /// <param name="t"></param>
        /// <param name="SI"></param>
        /// <returns></returns>
        public List<Tree> DBHGrowthModel15(List<double> param, double area, int t, double SI)
        {
            //林分密度
            double N = array.Count / area * 10000;
            
            for (int i = 0; i < array.Count; i++)
            {
                double dD = param[0] * (1 / (1 + param[1] * Math.Pow(N, param[2])*Math.Pow(t,param[3]))) * (param[4] * Math.Pow(SI, param[5]) * Math.Pow(array[i].DBH, param[6]) - Math.Pow(t, param[7]) * array[i].DBH);
                array[i].DBH = array[i].DBH + dD;

                if (Double.IsNaN(array[i].DBH) || Double.IsInfinity(array[i].DBH))
                {
                    Console.WriteLine("ERROR: NaN or Infinity of DBH");
                    return null;
                }
            }

            return array;
        }

        /// <summary>
        /// 吕勇,2002
        /// </summary>
        /// <param name="param"></param>
        /// <param name="area"></param>
        /// <param name="SI"></param>
        /// <returns></returns>
        public List<Tree> DBHGrowthModel16(List<double> param, double area, double SI)
        {
            //优势高
            array.Sort((left, right) => -left.Height.CompareTo(right.Height));
            double[] domainHeight = new double[5];
            for (int j = 0; j < 5; j++)
            {
                domainHeight[j] = array[j].Height;
            }
            double StandH = domainHeight.Average();
            array.Sort((left, right) => left.ID.CompareTo(right.ID));

            //优势胸径
            array.Sort((left, right) => -left.DBH.CompareTo(right.DBH));
            double[] domainDBH= new double[5];
            for (int j = 0; j < 5; j++)
            {
                domainDBH[j] = array[j].DBH;
            }
            double StandDBH = domainDBH.Average();
            array.Sort((left, right) => left.ID.CompareTo(right.ID));


            for (int i = 0; i < array.Count; i++)
            {
                double RS = Math.Sqrt(10000 / array.Count) / StandH;
                double RD = array[i].DBH / StandDBH;

                double dD = param[0] * (Math.Pow(RS, param[1])) * (Math.Pow(RD, param[2])) * (param[3] * (Math.Pow(SI, param[4])) * (Math.Pow(array[i].DBH, param[5])) + param[6] * (Math.Pow(SI, param[7])) * array[i].DBH);
                array[i].DBH = array[i].DBH + dD;

                if (Double.IsNaN(array[i].DBH) || Double.IsInfinity(array[i].DBH))
                {
                    Console.WriteLine("ERROR: NaN or Infinity of DBH");
                    return null;
                }
            }
            return array;
        }

    }
}
