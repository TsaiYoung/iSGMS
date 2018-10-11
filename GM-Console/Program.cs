using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GM_Console
{
    enum enDBHmodels
    {
        QinModel, LiuModel, ZhangModel, MaModel, SonmezModel, PukkalaModel, CampoModel, MabvuriraModel1, MabvuriraModel2, DengModel1, DengModel2, DengModel3, DengModel4, DengModel5,ZengModel, LvModel
    };
    enum enHeightmodels
    {
        Model1, Model2, Model3, Model4, Model5, Model6, Model7, Model8, Model9, Model10, Model11, Model12, Model13,
        Model14, Model15, Model16, Model17, Model18, Model19, Model20, Model21, Model22, Model23, Model24, Model25, Model26,
        Model27, Model28, Model29, Model30, Model31, Model32, Model33, Model34, Model35, Model36, Model37
    };
    enum enUBHmodels
    {
        LeitesLogisticModel, SoaresLogisticModel, SoaresExponentialModel, SoaresRichardModel, SoaresWeibullModel, HynynenModel, DyerModel, MaModel, ZouModel1, ZouModel2, ZouModel3
    };
    enum enCWmodels
    {
        Model1, Model2, Model3, Model4, Model5, Model6, Model7, Model8, Model9, Model10, Model11, Model12, Model13,
        Model14, Model15, Model16, Model17, Model18, Model19, Model20, Model21, Model22, Model23, Model24, Model25
    };
    enum enBiomassmodels
    {
        Model1, Model2, Model3, Model4, Model5, Model6, Model7
    };
    enum enMortalitymodels
    {
        ShaoModel, MaModel, PukkalaModel, YaoModel, SongModel1, SongModel2, SongModel3, SongModel4, SongModel5, CampoModel
    };

    public struct ManagementActives
    {
        public int cuttingAge;
        public int cuttingType;
        public double intensity;
        public double[] cutWeights;
    }

    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            
            //参数赋值
            try
            {
                //时间参数
                XDocument doc = XDocument.Load(p.ageparam);
                XElement root = doc.Root;
                if (root.Element("Start_Age").Value != "")
                    p.startage = Convert.ToInt32(root.Element("Start_Age").Value);
                if (root.Element("End_Age").Value != "")
                    p.endage = Convert.ToInt32(root.Element("End_Age").Value);
                if (root.Element("Interval_Age").Value != "")
                    p.interval = Convert.ToInt32(root.Element("Interval_Age").Value);
            }
            catch (Exception ex)
            {
                Console.WriteLine("时间参数读取异常");
                return;
            }
            try
            {
                //模型选项
                XDocument doc = XDocument.Load(p.modeltypeparam);
                XElement root = doc.Root;
                if (root.Element("DBH_model").Value != "")
                    p.DBHmodeltype = Convert.ToInt32(root.Element("DBH_model").Value);
                if (root.Element("Height_model").Value != "")
                    p.Heightmodeltype = Convert.ToInt32(root.Element("Height_model").Value);
                if (root.Element("UBH_model").Value != "")
                    p.UBHmodeltype = Convert.ToInt32(root.Element("UBH_model").Value);
                if (root.Element("CW_model").Value != "")
                    p.CWmodeltype = Convert.ToInt32(root.Element("CW_model").Value);
                if (root.Element("Biomass_model").Value != "")
                    p.Biomassmodeltype = Convert.ToInt32(root.Element("Biomass_model").Value);
                if (root.Element("Mortality_model").Value != "")
                    p.Mortalitymodeltype = Convert.ToInt32(root.Element("Mortality_model").Value);
            }
            catch(Exception ex)
            {
                Console.WriteLine("模型选项读取异常");
                return;
            }
            try
            {
                //林分参数
                XDocument doc = XDocument.Load(p.standparams);
                XElement root = doc.Root;
                if (root.Element("Canopy_Density").Value != "")
                    p.canopydensity = Convert.ToDouble(root.Element("Canopy_Density").Value);
                if (root.Element("Site_Index").Value != "")
                    p.SI = Convert.ToDouble(root.Element("Site_Index").Value);
                if (root.Element("Standard_Mean_Diameter").Value != "")
                    p.D0 = Convert.ToDouble(root.Element("Standard_Mean_Diameter").Value);
                if (root.Element("Min_Temperature").Value != "")
                    p.minTg = Convert.ToDouble(root.Element("Min_Temperature").Value);
                if (root.Element("Mean_Precipitation").Value != "")
                    p.meanPg = Convert.ToDouble(root.Element("Mean_Precipitation").Value);
                if (root.Element("MT").Value != "")
                    p.MT = Convert.ToInt32(root.Element("MT").Value);
                if (root.Element("VT").Value != "")
                    p.VT = Convert.ToInt32(root.Element("VT").Value);
                if (root.Element("TS").Value != "")
                    p.TS = Convert.ToDouble(root.Element("TS").Value);
                if (root.Element("BP").Value != "")
                    p.BP = Convert.ToDouble(root.Element("BP").Value);
            }
            catch (Exception ex)
            {
                Console.WriteLine("林分参数读取异常");
                return;
            }
            try
            {
                //模型参数
                XDocument doc = XDocument.Load(p.modelparams);
                XElement root = doc.Root;
                XElement dbhmodel = root.Element("DBH_Paramters");
                foreach (XElement param in dbhmodel.Elements())
                {
                    if (param.Value != "")
                        p.dbhModelParams.Add(Convert.ToDouble(param.Value));
                }
                XElement heightmodel = root.Element("Height_Paramters");
                foreach (XElement param in heightmodel.Elements())
                {
                    if (param.Value != "")
                        p.heightModelParams.Add(Convert.ToDouble(param.Value));
                }
                XElement ubhmodel = root.Element("UBH_Paramters");
                foreach(XElement param in ubhmodel.Elements())
                {
                    if (param.Value != "")
                        p.ubhModelParams.Add(Convert.ToDouble(param.Value));
                }
                XElement chmodel = root.Element("CH_Paramters");
                foreach (XElement param in chmodel.Elements())
                {
                    if (param.Value != "")
                        p.chModelParams.Add(Convert.ToDouble(param.Value));
                }
                XElement cwmodel = root.Element("CW_Paramters");
                foreach (XElement param in cwmodel.Elements())
                {
                    if (param.Value != "")
                        p.cwModelParams.Add(Convert.ToDouble(param.Value));
                }
                XElement biomassmodel = root.Element("Biomass_Paramters");
                foreach (XElement param in biomassmodel.Elements())
                {
                    if (param.Value != "")
                        p.biomassParams.Add(Convert.ToDouble(param.Value));
                }
                XElement mortalitymodel = root.Element("Mortality_Paramters");
                foreach(XElement param in mortalitymodel.Elements())
                {
                    if (param.Value != "")
                        p.mortalityParams.Add(Convert.ToDouble(param.Value));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("模型参数读取异常");
                return;
            }
            try
            {
                //经营活动
                XDocument doc = XDocument.Load(p.managparams);
                XElement root = doc.Root;
                IEnumerable<XElement> activities = root.Elements();
                foreach (XElement activity in activities)
                {
                    ManagementActives act = new ManagementActives();
                    act.cutWeights = new double[7];

                    XElement cutage = activity.Element("Cutting_Age");
                    if (cutage.Value != "")
                        act.cuttingAge = Convert.ToInt32(cutage.Value);
                    XElement cuttype = activity.Element("Cutting_Type");
                    if (cuttype.Value != "")
                        act.cuttingType = Convert.ToInt32(cuttype.Value);
                    XElement intensity = activity.Element("Intensity");
                    if (intensity.Value != "")
                        act.intensity = Convert.ToDouble(intensity.Value);
                    XElement weights = activity.Element("Cutting_Weights");

                    int i = 0;
                    foreach (XElement weight in weights.Elements())
                    {
                        if (weight.Value != "")
                        {
                            act.cutWeights[i] = Convert.ToDouble(weight.Value);
                            i++;
                        }
                    }

                    p.MActives.Add(act);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("经营活动读取异常");
                return;
            }
            
            p.StartGrowth();
        }

        //PARAMETERS
        //age
        int startage = 10;
        int endage = 25;
        int interval = 5;                       //枯损率预测间隔期
        //files path
        string Dempath = @".\dem.img";
        string Shppath = @".\forest.shp";
        string ageparam = @".\Age_Parameters.xml";
        string modeltypeparam = @".\Model_Type.xml";
        string standparams = @".\Stand_Parameters.xml";
        string modelparams = @".\Model_Parameters.xml";
        string managparams = @".\Management_Parameters.xml";
        string outputpath = @"..\output\Result";
        //model type
        int DBHmodeltype = 0;
        int Heightmodeltype = 5;
        int CWmodeltype = 24;
        int UBHmodeltype = 7;
        int Biomassmodeltype = 0;
        int Mortalitymodeltype = 0;
        //models parameters
        List<double> dbhModelParams = new List<double>();
        List<double> heightModelParams = new List<double>();
        List<double> ubhModelParams = new List<double>();
        List<double> chModelParams = new List<double>();
        List<double> cwModelParams = new List<double>();
        List<double> mortalityParams = new List<double>();
        List<double> biomassParams = new List<double>();
        //management parameters
        List<ManagementActives> MActives = new List<ManagementActives>();
        //other models parameters
        double canopydensity = 0;            //郁闭度
        double SI = 14;                              //地位指数
        double D0 = 10;                          //标准平均直径（中国一般取10）
        double minTg = 0;                       //生长季最低温
        double meanPg = 0;                  //生长季平均降水
        int MT = 1;                                 //MT equals one if the forest site type is MT(medium fertility) and otherwise MT = 0
        int VT = 0;                                 //VT equals 1 if the site type is VT(rather low fertility)
        double TS = 0;                          // temperature sum of the location (degree days)
        double BP = 1;                      //树种所占百分数
        double threshold = 0.2;  //枯损阈值
        //END

        DEMOperate dem = new DEMOperate();
        Forest forest = new Forest();
        SpatialUnit unit = new SpatialUnit();                   //计算空间结构单元
        List<double> suvProbility = new List<double>();     //间隔期后枯损概率

        private void StartGrowth()
        {
            //读取DEM
            dem.ReadDEM(Dempath);
            Console.WriteLine("Read DEM succeed!");
            //读取样地shp，或林场shp
            int standNum = forest.Readshp(Shppath);
            Console.WriteLine("Read shapefile succeed");
            //对活动按时间顺序排序
            MActives.Sort((left, right) => left.cuttingAge.CompareTo(right.cuttingAge));

            //ParallelLoopResult parallelresult = Parallel.For(0, standNum, forestId =>
            for (int forestId = 0; forestId < standNum; forestId++)
            {
                List<Tree> arrayTrees = new List<Tree>();

                if (forest.type == "points")
                {
                    arrayTrees = forest.getTrees();
                    Console.WriteLine("Get trees informations!");
                }
                else if (forest.type == "polygons")
                {
                    arrayTrees = forest.createTrees(dem, forestId);
                    Console.WriteLine("Get trees informations!");
                }
                else
                {
                    return;
                }
                
                //如果没有树高值，先计算
                if (arrayTrees[0].Height <= 0)
                    arrayTrees = CalcuHeight(arrayTrees, heightModelParams, startage, forest.forestArea[forestId]);

                List<Tree> initTrees = new List<Tree>();
                for (int i = 0; i < arrayTrees.Count; i++)
                {
                    initTrees.Add(arrayTrees[i]);
                }

                int cutCount = 0; //第几次砍伐
                bool isNumChange = true;
                for (int iage = startage + 1; iage <= endage; iage++)
                {
                    Console.WriteLine("Start to calculate the growth in " + iage + " years.");

                    //标记边界木
                    if (isNumChange)
                    {
                        //划分空间结构单元
                        unit.DivideUnit(arrayTrees);
                        for (int i = 0; i < unit.edgeTree.Count; i++)
                        {
                            int iTree = unit.edgeTree[i];
                            arrayTrees[iTree].isEdge = true;
                        }
                        isNumChange = false;

                        //空间结构单元按中心木排序
                        unit.near4Units.Sort((left, right) => left.centerID.CompareTo(right.centerID));
                        unit.allUnits.Sort((left, right) => left.centerID.CompareTo(right.centerID));
                    }

                    //计算胸径
                    arrayTrees = CalcuDBH(arrayTrees, unit.near4Units, dem, dbhModelParams, iage, forest.forestArea[forestId], SI, D0, minTg, meanPg, canopydensity, MT, VT, TS,BP);
                    Console.WriteLine("Calculate the DBH of stand " + forestId + " in year " + iage);

                    //计算树高
                    arrayTrees = CalcuHeight(arrayTrees, heightModelParams, iage, forest.forestArea[forestId]);
                    Console.WriteLine("Calculate the Height of stand " + forestId + " in year " + iage);

                    //计算枝下高
                    arrayTrees = CalcuUBH(arrayTrees, ubhModelParams, iage, forest.forestArea[forestId]);
                    Console.WriteLine("Calculate the UBH of stand " + forestId + " in year " + iage);

                    // 计算冠高
                    if (UBHmodeltype == (int)enUBHmodels.MaModel)
                    {
                        modelLibrary.CHmodels.CHGrowthModel ch = new modelLibrary.CHmodels.CHGrowthModel();
                        arrayTrees = ch.CalcuCrownHeight(unit, arrayTrees, chModelParams, iage);
                        Console.WriteLine("Calculate the CH of stand " + forestId + " in year " + iage);
                    }

                    // 计算冠幅
                    arrayTrees = CalcuCW(arrayTrees, cwModelParams, iage, forest.forestArea[forestId]);
                    Console.WriteLine("Calculate the CW of stand " + forestId + " in year " + iage);

                    // 计算生物量
                    arrayTrees = CalcuBiomass(arrayTrees, biomassParams);
                    Console.WriteLine("Calculate the Biomass of stand " + forestId + " in year " + iage);

                    //枯损（估算枯损情况）
                    if ((iage - startage) % interval == 0)
                    {
                        List<double> DIN = new List<double>();
                        for (int j = 0; j < arrayTrees.Count; j++)
                        {
                            DIN.Add(arrayTrees[j].DBH - initTrees[j].DBH);      //间隔期胸径增长量
                            initTrees[j] = arrayTrees[j];                       //更新
                        }
                        suvProbility = CalcuMortality(arrayTrees, mortalityParams, unit.allUnits, iage, forest.forestArea[forestId], SI, DIN);
                        for (int j = 0; j < arrayTrees.Count; j++)
                        {
                            if (suvProbility[j] < threshold)
                            {
                                arrayTrees[j].isMortality = true;
                                isNumChange = true;
                            }
                        }
                        Console.WriteLine("Predict " + "the mortality probility of " + forestId + " stand " + " in " + iage + " year.");
                    }

                    //砍伐
                    if (MActives.Count > 0 && cutCount < MActives.Count && iage == MActives[cutCount].cuttingAge)
                    {
                        Management manager = new Management(arrayTrees, unit.near4Units, MActives[cutCount].cuttingType, MActives[cutCount].cutWeights, MActives[cutCount].intensity);     //添加经营方面的各种参数
                        arrayTrees = manager.MarkCuttingTree();
                        cutCount++;
                        isNumChange = true;

                        Console.WriteLine("Calculate the Cutting of stand " + forestId + " in year " + iage);

                        //output 不同小班数据
                        string path = outputpath + "\\Age" + iage + "Stand" + forestId +"Cutting"+cutCount+ ".shp";
                        Console.WriteLine("Output: " + path);

                        forest.WriteShp(path, arrayTrees);
                    }

                    //到间隔期
                    if ((iage - startage) % interval == 0)
                    {
                        //output 不同小班数据
                        string path = outputpath + "\\Age" + iage + "Stand" + forestId + ".shp";
                        Console.WriteLine("Output: " + path);

                        forest.WriteShp(path, arrayTrees);
                    }

                    //林木数量更新(倒序删除，防止删除后序号变化)
                    if (isNumChange)
                    {
                        Console.WriteLine("The number of forest trees have changed!");
                        arrayTrees = treesUpdate(arrayTrees);
                    }
                    else
                    {
                        Console.WriteLine("The number of forest trees no changes!");
                    }
                }
            }
        }
        
        ///<summary>
        ///求Voronoi多边形面积
        /// </summary>
        [DllImport("MYDLL.dll", EntryPoint = "CreateThiessen")]
        public static extern void CreateThiessen(double[] treeX, double[] treeY, int t_num, double[] b_pointX, double[] b_pointY, int b_num, double[] area);
        /// <summary>
        /// 更新林木，还原边界木，更新泰森多边形面积
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        List<Tree> treesUpdate(List<Tree> array)
        {
            for (int i = array.Count - 1; i >= 0; i--)
            {
                if (array[i].isCutting)
                {
                    array.RemoveAt(i);
                }
                else if (array[i].isMortality)
                {
                    array.RemoveAt(i);
                }
            }

            //边界木初始化
            for (int i = 0; i < array.Count; i++)
            {
                array[i].isEdge = false;
            }


            //计算新的voronoi多边形面积
            List<double> treeX = new List<double>();
            List<double> treeY = new List<double>();
            List<double> boundX = new List<double>();
            List<double> boundY = new List<double>();
            List<double> Area = new List<double>();
            int t_num = array.Count; int b_num = 4;
            //求外边界，同时将坐标存入数组
            double minX = array[0].X, maxX = array[0].X, minY = array[0].Y, maxY = array[0].Y;
            for (int i = 0; i < array.Count; i++)
            {
                treeX.Add(array[i].X);
                treeY.Add(array[i].Y);
                Area.Add(0);//初始化
                if (array[i].X < minX) { minX = array[i].X; }
                if (array[i].X > maxX) { maxX = array[i].X; }
                if (array[i].Y < minY) { minY = array[i].Y; }
                if (array[i].Y > maxY) { maxY = array[i].Y; }
            }
            boundX.Add(minX); boundX.Add(minX); boundX.Add(maxX); boundX.Add(maxX);
            boundY.Add(minY); boundY.Add(maxY); boundY.Add(maxY); boundY.Add(minY);
            //调用CreateThiessen求WVA
            double[] treex = treeX.ToArray(); double[] treey = treeY.ToArray();
            double[] boundx = boundX.ToArray(); double[] boundy = boundY.ToArray();
            double[] area = Area.ToArray();
            CreateThiessen(treex, treey, t_num, boundx, boundy, b_num, area);
            for (int i = 0; i < array.Count; i++)
            {
                array[i].WVA = area[i];
            }

            return array;
        }

        /// <summary>
        /// 计算胸径
        /// </summary>
        /// <param name="array"></param>
        /// <param name="units"></param>
        /// <param name="dem"></param>
        /// <param name="forest"></param>
        /// <param name="param"></param>
        /// <param name="age"></param>
        /// <param name="SI"></param>
        /// <param name="D0"></param>
        /// <param name="minTg"></param>
        /// <param name="meanPg"></param>
        /// <param name="canopydensity"></param>
        /// <returns></returns>
        List<Tree> CalcuDBH(List<Tree> array, List<SpatialUnit.unit_tree> units, DEMOperate dem, List<double> param, int age, double forestArea, double SI, double D0, double minTg, double meanPg, double canopydensity, int MT, int VT, double TS,double BP)
        {
            modelLibrary.DBHmodels.DBHGrowthModelSet dbh = new modelLibrary.DBHmodels.DBHGrowthModelSet(array);

            switch (DBHmodeltype)
            {
                case (int)enDBHmodels.QinModel:
                    array = dbh.DBHGrowthModel1(units, dem, param, SI);
                    break;
                case (int)enDBHmodels.LiuModel:
                    array = dbh.DBHGrowthModel2(units, dem, param, D0, forestArea);
                    break;
                case (int)enDBHmodels.ZhangModel:
                    array = dbh.DBHGrowthModel3(units, dem, param, minTg, meanPg, forestArea);
                    break;
                case (int)enDBHmodels.MaModel:
                    array = dbh.DBHGrowthModel4(units, dem, param, canopydensity, SI, D0, forestArea);
                    break;
                case (int)enDBHmodels.SonmezModel:
                    array = dbh.DBHGrowthModel5(units, dem, param, SI, age, forestArea, BP);
                    break;
                case (int)enDBHmodels.PukkalaModel:
                    array = dbh.DBHGrowthModel6(param, forestArea, MT, VT, TS);
                    break;
                case (int)enDBHmodels.CampoModel:
                    array = dbh.DBHGrowthModel7(param, forestArea, age);
                    break;
                case (int)enDBHmodels.MabvuriraModel1:
                    array = dbh.DBHGrowthModel8(param, forestArea, age);
                    break;
                case (int)enDBHmodels.MabvuriraModel2:
                    array = dbh.DBHGrowthModel9(param, forestArea, age);
                    break;
                case (int)enDBHmodels.DengModel1:
                    array = dbh.DBHGrowthModel10(param, forestArea, age, SI);
                    break;
                case (int)enDBHmodels.DengModel2:
                    array = dbh.DBHGrowthModel11(param, forestArea, age, SI);
                    break;
                case (int)enDBHmodels.DengModel3:
                    array = dbh.DBHGrowthModel12(param, forestArea, age, SI);
                    break;
                case (int)enDBHmodels.DengModel4:
                    array = dbh.DBHGrowthModel13(param, forestArea, age, SI);
                    break;
                case (int)enDBHmodels.DengModel5:
                    array = dbh.DBHGrowthModel14(param, forestArea, age, SI);
                    break;
                case (int)enDBHmodels.ZengModel:
                    array = dbh.DBHGrowthModel15(param, forestArea, age, SI);
                    break;
                case (int)enDBHmodels.LvModel:
                    array = dbh.DBHGrowthModel16(param, forestArea, SI);
                    break;
            }

            return array;
        }

        // 计算树高
        List<Tree> CalcuHeight(List<Tree> array, List<double> param, int age, double forestArea)
        {
            modelLibrary.Heightmodels.IHeightGrowth height1;
            modelLibrary.Heightmodels.IHeightGrowthfromAge height2;
            modelLibrary.Heightmodels.IHeightGrowth2 height3;
            modelLibrary.Heightmodels.IHeightGrowthfromAge2 height4;
            switch (Heightmodeltype)
            {
                case (int)enHeightmodels.Model1:
                case (int)enHeightmodels.Model4:
                    height1 = new modelLibrary.Heightmodels.HeightModel5();
                    array = height1.InvokeTreeHeight(array, param);
                    break;
                case (int)enHeightmodels.Model2:
                    height1 = new modelLibrary.Heightmodels.HeightModel3();
                    array = height1.InvokeTreeHeight(array, param);
                    break;
                case (int)enHeightmodels.Model3:
                    height1 = new modelLibrary.Heightmodels.HeightModel4();
                    array = height1.InvokeTreeHeight(array, param);
                    break;
                case (int)enHeightmodels.Model5:
                    height1 = new modelLibrary.Heightmodels.HeightModel6();
                    array = height1.InvokeTreeHeight(array, param);
                    break;
                case (int)enHeightmodels.Model6:
                    height1 = new modelLibrary.Heightmodels.HeightModel1();
                    array = height1.InvokeTreeHeight(array, param);
                    break;
                case (int)enHeightmodels.Model7:
                    height1 = new modelLibrary.Heightmodels.HeightModel7();
                    array = height1.InvokeTreeHeight(array, param);
                    break;
                case (int)enHeightmodels.Model8:
                    height1 = new modelLibrary.Heightmodels.HeightModel8();
                    array = height1.InvokeTreeHeight(array, param);
                    break;
                case (int)enHeightmodels.Model9:
                    height1 = new modelLibrary.Heightmodels.HeightModel9();
                    array = height1.InvokeTreeHeight(array, param);
                    break;
                case (int)enHeightmodels.Model10:
                case (int)enHeightmodels.Model14:
                case (int)enHeightmodels.Model15:
                case (int)enHeightmodels.Model16:
                    height1 = new modelLibrary.Heightmodels.HeightModel16();
                    array = height1.InvokeTreeHeight(array, param);
                    break;
                case (int)enHeightmodels.Model11:
                    height1 = new modelLibrary.Heightmodels.HeightModel11();
                    array = height1.InvokeTreeHeight(array, param);
                    break;
                case (int)enHeightmodels.Model12:
                    height1 = new modelLibrary.Heightmodels.HeightModel12();
                    array = height1.InvokeTreeHeight(array, param);
                    break;
                case (int)enHeightmodels.Model13:
                    height1 = new modelLibrary.Heightmodels.HeightModel13();
                    array = height1.InvokeTreeHeight(array, param);
                    break;
                case (int)enHeightmodels.Model17:
                    height1 = new modelLibrary.Heightmodels.HeightModel17();
                    array = height1.InvokeTreeHeight(array, param);
                    break;
                case (int)enHeightmodels.Model18:
                    height1 = new modelLibrary.Heightmodels.HeightModel18();
                    array = height1.InvokeTreeHeight(array, param);
                    break;
                case (int)enHeightmodels.Model19:
                    height1 = new modelLibrary.Heightmodels.HeightModel19();
                    array = height1.InvokeTreeHeight(array, param);
                    break;
                case (int)enHeightmodels.Model20:
                    height1 = new modelLibrary.Heightmodels.HeightModel20();
                    array = height1.InvokeTreeHeight(array, param);
                    break;
                case (int)enHeightmodels.Model21:
                    height1 = new modelLibrary.Heightmodels.HeightModel21();
                    array = height1.InvokeTreeHeight(array, param);
                    break;
                case (int)enHeightmodels.Model22:
                    height1 = new modelLibrary.Heightmodels.HeightModel22();
                    array = height1.InvokeTreeHeight(array, param);
                    break;
                case (int)enHeightmodels.Model23:
                    height1 = new modelLibrary.Heightmodels.HeightModel23();
                    array = height1.InvokeTreeHeight(array, param);
                    break;
                case (int)enHeightmodels.Model24:
                    height1 = new modelLibrary.Heightmodels.HeightModel24();
                    array = height1.InvokeTreeHeight(array, param);
                    break;
                case (int)enHeightmodels.Model25:
                    height3 = new modelLibrary.Heightmodels.HeightModel27();
                    array = height3.InvokeTreeHeight(array, param, forestArea);
                    break;
                case (int)enHeightmodels.Model26:
                    height1 = new modelLibrary.Heightmodels.HeightModel28();
                    array = height1.InvokeTreeHeight(array, param);
                    break;
                case (int)enHeightmodels.Model27:
                    height1 = new modelLibrary.Heightmodels.HeightModel29();
                    array = height1.InvokeTreeHeight(array, param);
                    break;
                case (int)enHeightmodels.Model28:
                    height1 = new modelLibrary.Heightmodels.HeightModel30();
                    array = height1.InvokeTreeHeight(array, param);
                    break;
                case (int)enHeightmodels.Model29:
                case (int)enHeightmodels.Model30:
                case (int)enHeightmodels.Model31:
                    height3 = new modelLibrary.Heightmodels.HeightModel33();
                    array = height3.InvokeTreeHeight(array, param, forestArea);
                    break;
                case (int)enHeightmodels.Model32:
                    height1 = new modelLibrary.Heightmodels.HeightModel34();
                    array = height1.InvokeTreeHeight(array, param);
                    break;
                case (int)enHeightmodels.Model33:
                    height2 = new modelLibrary.Heightmodels.HeightModel35();
                    array = height2.InvokeTreeHeight(array, param, age);
                    break;
                case (int)enHeightmodels.Model34:
                    height3 = new modelLibrary.Heightmodels.HeightModel36();
                    array = height3.InvokeTreeHeight(array, param, forestArea);
                    break;
                case (int)enHeightmodels.Model35:
                    height1 = new modelLibrary.Heightmodels.HeightModel37();
                    array = height1.InvokeTreeHeight(array, param);
                    break;
                case (int)enHeightmodels.Model36:
                    height4 = new modelLibrary.Heightmodels.HeightModel38();
                    array = height4.InvokeTreeHeight(array, param, age, forestArea);
                    break;
                case (int)enHeightmodels.Model37:
                    height4 = new modelLibrary.Heightmodels.HeightModel39();
                    array = height4.InvokeTreeHeight(array, param, age, forestArea);
                    break;
            }
            return array;
        }

        // 计算枝下高
        List<Tree> CalcuUBH(List<Tree> array, List<double> param, int age, double forestArea)
        {
            modelLibrary.UBHmodels.IUBHModels ubh1;
            modelLibrary.UBHmodels.IUBHModel2 ubh2;
            modelLibrary.UBHmodels.IUBHModels3 ubh3;
            modelLibrary.UBHmodels.IUBHmodel1 ubh4;

            switch (UBHmodeltype)
            {
                case (int)enUBHmodels.LeitesLogisticModel:
                    ubh3 = new modelLibrary.UBHmodels.UBHGrowthModel1();
                    array = ubh3.InvokeUBHModels(array, param, forestArea,dem);
                    break;
                case (int)enUBHmodels.SoaresLogisticModel:
                    ubh1 = new modelLibrary.UBHmodels.UBHGrowthModel2();
                    array = ubh1.InvokeUBHModels(array, param, age, forestArea);
                    break;
                case (int)enUBHmodels.SoaresExponentialModel:
                    ubh1 = new modelLibrary.UBHmodels.UBHGrowthModel3();
                    array = ubh1.InvokeUBHModels(array, param, age, forestArea);
                    break;
                case (int)enUBHmodels.SoaresRichardModel:
                    ubh1 = new modelLibrary.UBHmodels.UBHGrowthModel4();
                    array = ubh1.InvokeUBHModels(array, param, age, forestArea);
                    break;
                case (int)enUBHmodels.SoaresWeibullModel:
                    ubh1 = new modelLibrary.UBHmodels.UBHGrowthModel5();
                    array = ubh1.InvokeUBHModels(array, param, age, forestArea);
                    break;
                case (int)enUBHmodels.HynynenModel:
                    ubh3 = new modelLibrary.UBHmodels.UBHGrowthModel6();
                    array = ubh3.InvokeUBHModels(array, param, forestArea,dem);
                    break;
                case (int)enUBHmodels.DyerModel:
                    ubh2 = new modelLibrary.UBHmodels.UBHGrowthModel7();
                    array = ubh2.InvokeUBHModels(array, param, age);
                    break;
                case (int)enUBHmodels.MaModel:
                    modelLibrary.UBHmodels.UBHGrowthModel8 ubh5 = new modelLibrary.UBHmodels.UBHGrowthModel8();
                    array = ubh5.CalcuUBH(unit, array, param, age);
                    break;
                case (int)enUBHmodels.ZouModel1:
                    ubh4 = new modelLibrary.UBHmodels.UBHGrowthModel9();
                    array = ubh4.InvokeUBHModels(array, param);
                    break;
                case (int)enUBHmodels.ZouModel2:
                    ubh4 = new modelLibrary.UBHmodels.UBHGrowthModel10();
                    array = ubh4.InvokeUBHModels(array, param);
                    break;
                case (int)enUBHmodels.ZouModel3:
                    ubh4 = new modelLibrary.UBHmodels.UBHGrowthModel11();
                    array = ubh4.InvokeUBHModels(array, param);
                    break;
            }

            return array;
        }

        // 计算冠幅
        List<Tree> CalcuCW(List<Tree> array, List<double> param, int age, double forestArea)
        {
            modelLibrary.CWmodels.ICWGrowth cw1;
            modelLibrary.CWmodels.ICWGrowth2 cw2;
            switch (CWmodeltype)
            {
                case (int)enCWmodels.Model1:
                    cw1 = new modelLibrary.CWmodels.CWModel1();
                    array = cw1.InvokeCWGrowth(array, param);
                    break;
                case (int)enCWmodels.Model2:
                    cw1 = new modelLibrary.CWmodels.CWModel2();
                    array = cw1.InvokeCWGrowth(array, param);
                    break;
                case (int)enCWmodels.Model3:
                    cw1 = new modelLibrary.CWmodels.CWModel3();
                    array = cw1.InvokeCWGrowth(array, param);
                    break;
                case (int)enCWmodels.Model4:
                    cw1 = new modelLibrary.CWmodels.CWModel4();
                    array = cw1.InvokeCWGrowth(array, param);
                    break;
                case (int)enCWmodels.Model5:
                    cw1 = new modelLibrary.CWmodels.CWModel5();
                    array = cw1.InvokeCWGrowth(array, param);
                    break;
                case (int)enCWmodels.Model6:
                    cw1 = new modelLibrary.CWmodels.CWModel6();
                    array = cw1.InvokeCWGrowth(array, param);
                    break;
                case (int)enCWmodels.Model7:
                    cw1 = new modelLibrary.CWmodels.CWModel7();
                    array = cw1.InvokeCWGrowth(array, param);
                    break;
                case (int)enCWmodels.Model8:
                    cw1 = new modelLibrary.CWmodels.CWModel8();
                    array = cw1.InvokeCWGrowth(array, param);
                    break;
                case (int)enCWmodels.Model9:
                    cw1 = new modelLibrary.CWmodels.CWModel9();
                    array = cw1.InvokeCWGrowth(array, param);
                    break;
                case (int)enCWmodels.Model10:
                    cw1 = new modelLibrary.CWmodels.CWModel10();
                    array = cw1.InvokeCWGrowth(array, param);
                    break;
                case (int)enCWmodels.Model11:
                    cw1 = new modelLibrary.CWmodels.CWModel11();
                    array = cw1.InvokeCWGrowth(array, param);
                    break;
                case (int)enCWmodels.Model12:
                    cw1 = new modelLibrary.CWmodels.CWModel12();
                    array = cw1.InvokeCWGrowth(array, param);
                    break;
                case (int)enCWmodels.Model13:
                    cw1 = new modelLibrary.CWmodels.CWModel13();
                    array = cw1.InvokeCWGrowth(array, param);
                    break;
                case (int)enCWmodels.Model14:
                    cw2 = new modelLibrary.CWmodels.CWModel14();
                    array = cw2.InvokeCWGrowth(array, param, forestArea);
                    break;
                case (int)enCWmodels.Model15:
                    cw1 = new modelLibrary.CWmodels.CWModel15();
                    array = cw1.InvokeCWGrowth(array, param);
                    break;
                case (int)enCWmodels.Model16:
                    cw1 = new modelLibrary.CWmodels.CWModel16();
                    array = cw1.InvokeCWGrowth(array, param);
                    break;
                case (int)enCWmodels.Model17:
                    cw2 = new modelLibrary.CWmodels.CWModel17();
                    array = cw2.InvokeCWGrowth(array, param, forestArea);
                    break;
                case (int)enCWmodels.Model18:
                    cw2 = new modelLibrary.CWmodels.CWModel18();
                    array = cw2.InvokeCWGrowth(array, param, forestArea);
                    break;
                case (int)enCWmodels.Model19:
                    cw2 = new modelLibrary.CWmodels.CWModel19();
                    array = cw2.InvokeCWGrowth(array, param, forestArea);
                    break;
                case (int)enCWmodels.Model20:
                    cw1 = new modelLibrary.CWmodels.CWModel20();
                    array = cw1.InvokeCWGrowth(array, param);
                    break;
                case (int)enCWmodels.Model21:
                    cw2 = new modelLibrary.CWmodels.CWModel21();
                    array = cw2.InvokeCWGrowth(array, param, forestArea);
                    break;
                case (int)enCWmodels.Model22:
                    cw2 = new modelLibrary.CWmodels.CWModel22();
                    array = cw2.InvokeCWGrowth(array, param, forestArea);
                    break;
                case (int)enCWmodels.Model23:
                    cw2 = new modelLibrary.CWmodels.CWModel23();
                    array = cw2.InvokeCWGrowth(array, param, forestArea);
                    break;
                case (int)enCWmodels.Model24:
                    cw2 = new modelLibrary.CWmodels.CWModel24();
                    array = cw2.InvokeCWGrowth(array, param,forestArea);
                    break;
                case (int)enCWmodels.Model25:
                    modelLibrary.CWmodels.CWModel25 cw3 = new modelLibrary.CWmodels.CWModel25();
                    array = cw3.CalcuCrown1(unit, array, param, age);
                    break;
            }
            return array;
        }

        // 计算枯损
        List<double> CalcuMortality(List<Tree> array, List<double> param, List<SpatialUnit.unit_tree> units, int age, double forestArea, double SI, List<double> DIN)
        {
            List<double> probility = new List<double>();
            modelLibrary.Mortalitymodels.IMortalityModels mortality1;
            modelLibrary.Mortalitymodels.IMortalityModels1 mortality2;
            modelLibrary.Mortalitymodels.IMortalityModels2 mortality3;

            switch (Mortalitymodeltype)
            {
                case (int)enMortalitymodels.ShaoModel:
                    mortality1 = new modelLibrary.Mortalitymodels.MortalityModels1();
                    probility = mortality1.InvokeMortalityModels(units, array, param);
                    break;
                case (int)enMortalitymodels.MaModel:
                    mortality2 = new modelLibrary.Mortalitymodels.MortalityModels2();
                    probility = mortality2.InvokeMortalityModels(array, param, forestArea);
                    break;
                case (int)enMortalitymodels.PukkalaModel:
                    mortality2 = new modelLibrary.Mortalitymodels.MortalityModels3();
                    probility = mortality2.InvokeMortalityModels(array, param, forestArea);
                    break;
                case (int)enMortalitymodels.YaoModel:
                    mortality3 = new modelLibrary.Mortalitymodels.MortalityModels4();
                    probility = mortality3.InvokeMortalityModels(array, param, forestArea, SI, DIN);
                    break;
                case (int)enMortalitymodels.SongModel1:
                    mortality2 = new modelLibrary.Mortalitymodels.MortalityModels5();
                    probility = mortality2.InvokeMortalityModels(array, param, forestArea);
                    break;
                case (int)enMortalitymodels.SongModel2:
                    mortality2 = new modelLibrary.Mortalitymodels.MortalityModels6();
                    probility = mortality2.InvokeMortalityModels(array, param, forestArea);
                    break;
                case (int)enMortalitymodels.SongModel3:
                    mortality2 = new modelLibrary.Mortalitymodels.MortalityModels7();
                    probility = mortality2.InvokeMortalityModels(array, param, forestArea);
                    break;
                case (int)enMortalitymodels.SongModel4:
                    mortality2 = new modelLibrary.Mortalitymodels.MortalityModels8();
                    probility = mortality2.InvokeMortalityModels(array, param, forestArea);
                    break;
                case (int)enMortalitymodels.SongModel5:
                    mortality2 = new modelLibrary.Mortalitymodels.MortalityModels9();
                    probility = mortality2.InvokeMortalityModels(array, param, forestArea);
                    break;
                case (int)enMortalitymodels.CampoModel:
                    mortality2 = new modelLibrary.Mortalitymodels.MortalityModel10();
                    probility = mortality2.InvokeMortalityModels(array, param, forestArea);
                    break;
            }
            return probility;
        }

        // 计算生物量
        List<Tree> CalcuBiomass(List<Tree> array, List<double> param)
        {
            modelLibrary.Biomassmodels.IBiomass biomass;
            switch (Biomassmodeltype)
            {
                case (int)enBiomassmodels.Model1:
                    biomass = new modelLibrary.Biomassmodels.BiomassModel1();
                    array = biomass.InvokeBiomassGrowth(array, biomassParams);
                    break;
                case (int)enBiomassmodels.Model2:
                    biomass = new modelLibrary.Biomassmodels.BiomassModel2();
                    array = biomass.InvokeBiomassGrowth(array, biomassParams);
                    break;
                case (int)enBiomassmodels.Model3:
                    biomass = new modelLibrary.Biomassmodels.BiomassModel3();
                    array = biomass.InvokeBiomassGrowth(array, biomassParams);
                    break;
                case (int)enBiomassmodels.Model4:
                    biomass = new modelLibrary.Biomassmodels.BiomassModel4();
                    array = biomass.InvokeBiomassGrowth(array, biomassParams);
                    break;
                case (int)enBiomassmodels.Model5:
                    biomass = new modelLibrary.Biomassmodels.BiomassModel5();
                    array = biomass.InvokeBiomassGrowth(array, biomassParams);
                    break;
                case (int)enBiomassmodels.Model6:
                    biomass = new modelLibrary.Biomassmodels.BiomassModel6();
                    array = biomass.InvokeBiomassGrowth(array, biomassParams);
                    break;
                case (int)enBiomassmodels.Model7:
                    biomass = new modelLibrary.Biomassmodels.BiomassModel7();
                    array = biomass.InvokeBiomassGrowth(array, biomassParams);
                    break;
            }
            return array;
        }
    }
}

