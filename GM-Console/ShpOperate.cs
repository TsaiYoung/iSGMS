using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OSGeo.GDAL;
using OSGeo.OGR;
using OSGeo.OSR;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.IO;

namespace GM_Console
{    
    public class ShpOperate
    {
        // 保存SHP属性字段  
        public OSGeo.OGR.Driver oDriver;
        //public OSGeo.OGR.DataSource pDataSource;
        //public OSGeo.OGR.Layer pLayer;
        public List<string> m_FeildList;
        public string sCoordiantes;
        //private string shpType;
        private Layer oLayer;
        
        public ShpOperate()
        {
            m_FeildList = new List<string>();
            oLayer = null;
            sCoordiantes = null;
        }

        /// <summary>  
        /// 初始化Gdal  
        /// </summary>  
        public void InitinalGdal()
        {
            // 为了支持中文路径  
            Gdal.SetConfigOption("GDAL_FILENAME_IS_UTF8", "YES");
            // 为了使属性表字段支持中文  
            Gdal.SetConfigOption("SHAPE_ENCODING", "");
            //Gdal.AllRegister();

            Ogr.RegisterAll();

            oDriver = Ogr.GetDriverByName("ESRI Shapefile");
            if (oDriver == null)
            {
               Console.WriteLine("驱动不可用，请检查");
            }
        }

        /// <summary>  
        /// 获取SHP文件的层  
        /// </summary>  
        /// <param name="sfilename"></param>  
        /// <param name="oLayer"></param>  
        /// <returns></returns>  
        public bool GetShpLayer(string sfilename)
        {
            if (sfilename == null || sfilename.Length <= 3)
            {
                oLayer = null;
                return false;
            }
            //if (oDriver == null)
            //{
            //    MessageBox.Show("文件不能打开，请检查");
            //}

            DataSource ds = oDriver.Open(sfilename, 1);
            if (ds == null)
            {
                oLayer = null;
                return false;
            }
            int iPosition = sfilename.LastIndexOf("\\");
            string sTempName = sfilename.Substring(iPosition + 1, sfilename.Length - iPosition - 4 - 1);
            oLayer = ds.GetLayerByName(sTempName);
            if (oLayer == null)
            {
                ds.Dispose();
                return false;
            }
            return true;
        }
        /// <summary>  
        /// 获取所有的属性字段  
        /// </summary>  
        /// <returns></returns>  
        public bool GetFeilds()
        {
            if (null == oLayer)
            {
                return false;
            }
            m_FeildList.Clear();
            wkbGeometryType oTempGeometryType = oLayer.GetGeomType();
            List<string> TempstringList = new List<string>();

            //
            FeatureDefn oDefn = oLayer.GetLayerDefn();
            int iFieldCount = oDefn.GetFieldCount();
            for (int iAttr = 0; iAttr < iFieldCount; iAttr++)
            {
                FieldDefn oField = oDefn.GetFieldDefn(iAttr);
                if (null != oField)
                {
                    m_FeildList.Add(oField.GetNameRef());
                }
            }
            return true;
        }
        /// <summary>  
        ///  获取某条数据的字段内容  
        /// </summary>  
        /// <param name="iIndex"></param>  
        /// <param name="FeildStringList"></param>  
        /// <returns></returns>  
        public string GetFeildContent(int iAttr, Feature oFeature)
        {
            //string tempType;
            string attribute = "";
            if (oFeature != null)
            {
                FeatureDefn oDefn = oLayer.GetLayerDefn();
                FieldDefn oField = oDefn.GetFieldDefn(iAttr);
                string sFeildName = oField.GetNameRef();

                FieldType Ftype = oFeature.GetFieldType(sFeildName);
                switch (Ftype)
                {
                    case FieldType.OFTString:
                        string sFValue = oFeature.GetFieldAsString(sFeildName);
                        //tempType = "string";
                        attribute = sFValue.ToString();
                        break;
                    case FieldType.OFTReal:
                        double dFValue = oFeature.GetFieldAsDouble(sFeildName);
                        //tempType = "float";
                        attribute = dFValue.ToString();
                        break;
                    case FieldType.OFTInteger:
                        int iFValue = oFeature.GetFieldAsInteger(sFeildName);
                        //tempType = "int";
                        attribute = iFValue.ToString();
                        break;
                    default:
                        //sFValue = oFeature.GetFieldAsString(ChosenFeildIndex[iFeildIndex]);  
                        //tempType = "string";
                        break;
                }
            }
            return attribute;
        }

        ///<summary>
        ///求Voronoi多边形面积
        /// </summary>
        [DllImport("MYDLL.dll", EntryPoint = "CreateThiessen")]
        public static extern void CreateThiessen(double[] treeX, double[] treeY, int t_num, double[] b_pointX, double[] b_pointY, int b_num, double[] area);

        /// <summary>  
        /// 获取数据  
        /// </summary>  
        /// <returns></returns>  
        public List<Tree> GetGeometry()
        {
            List<Tree> trees = new List<Tree>();

            if (oLayer == null)
            {
                return null;
            }

            int iFeatureCout = (int)oLayer.GetFeatureCount(0);
            Feature oFeature = null;
            int treeID = 0;

            for (int i = 0; i < iFeatureCout; i++)
            {
                Tree tree = new Tree();
                oFeature = oLayer.GetFeature(i);
                bool existD = false;
                tree.ForestID = 0;//代表所有树木在同一片树林
                //读取属性
                for (int iField = 0; iField < m_FeildList.Count; iField++)
                {
                   if (m_FeildList[iField].ToUpper() == "H" || m_FeildList[iField].ToUpper() == "HEIGHT")
                    {
                        tree.Height = Convert.ToDouble(GetFeildContent(iField, oFeature));
                    }
                    else if (m_FeildList[iField].ToUpper() == "D" || m_FeildList[iField].ToUpper() == "DBH")
                    {
                        tree.DBH = Convert.ToDouble(GetFeildContent(iField, oFeature));
                        existD = true;
                    }
                    else if (m_FeildList[iField].ToUpper() == "C" || m_FeildList[iField].ToUpper() == "CW")
                    {
                        tree.CrownWidth = Convert.ToDouble(GetFeildContent(iField, oFeature));
                    }
                    else if (m_FeildList[iField].ToUpper() == "HB" || m_FeildList[iField].ToUpper() == "UBH")
                    {
                        tree.UnderBranchHeight = Convert.ToDouble(GetFeildContent(iField, oFeature));
                    }
                    else if (m_FeildList[iField].ToUpper() == "SPECIES") 
                    {
                        tree.Species = GetFeildContent(iField, oFeature);
                    }
                    else if (m_FeildList[iField].ToUpper() == "HEALTH")
                    {
                        tree.Health = Convert.ToDouble(GetFeildContent(iField, oFeature));
                    }
                }
                if (existD)
                {
                    Geometry geom = oFeature.GetGeometryRef();
                    tree.X = geom.GetX(0);
                    tree.Y = geom.GetY(0);

                    tree.ID = treeID;
                    trees.Add(tree);
                    treeID++;
                }
            }//for

            //以下求WVA,WVA表示采用加权Voronoi图得出每株数的多边形面积
            List<double> treeX = new List<double>();
            List<double> treeY = new List<double>();
            List<double> boundX = new List<double>();
            List<double> boundY = new List<double>();
            List<double> Area = new List<double>();
            int t_num = trees.Count;int b_num =4;
            //求外边界，同时将坐标存入数组
            double minX = trees[0].X, maxX = trees[0].X, minY = trees[0].Y, maxY = trees[0].Y;
            for (int i = 0; i < trees.Count; i++)
            {
                treeX.Add(trees[i].X);
                treeY.Add(trees[i].Y);
                Area.Add(0);//初始化
                if (trees[i].X < minX) { minX = trees[i].X; }
                if (trees[i].X > maxX) { maxX = trees[i].X; }
                if (trees[i].Y < minY) { minY = trees[i].Y; }
                if (trees[i].Y > maxY) { maxY = trees[i].Y; }
            }
            boundX.Add(minX);boundX.Add(minX);boundX.Add(maxX);boundX.Add(maxX);
            boundY.Add(minY);boundY.Add(maxY);boundY.Add(maxY);boundY.Add(minY);
            //调用CreateThiessen求WVA
            double[] treex = treeX.ToArray();double[] treey = treeY.ToArray();
            double[] boundx = boundX.ToArray();double[] boundy = boundY.ToArray();
            double[] area = Area.ToArray();
            CreateThiessen(treex, treey, t_num, boundx, boundy, b_num, area);
            for(int i = 0; i < trees.Count; i++)
            {
                trees[i].WVA = area[i];
            }

            return trees;
        }

        /// <summary>  
        /// 获取数据类型
        /// </summary>  
        /// <returns></returns>  
        public string GetGeometryType()
        {
            string shpType;

            if (oLayer == null)
            {
                return "";
            }

            wkbGeometryType oGeometryType = oLayer.GetGeomType();

            if (oGeometryType == wkbGeometryType.wkbPoint ||
                oGeometryType == wkbGeometryType.wkbPoint25D ||
                oGeometryType == wkbGeometryType.wkbPointM ||
                oGeometryType == wkbGeometryType.wkbPointZM)
            {
                shpType = "points";
            }
            else if (oGeometryType == wkbGeometryType.wkbPolygon ||
                oGeometryType == wkbGeometryType.wkbPolygon25D ||
                oGeometryType == wkbGeometryType.wkbPolygonM ||
                oGeometryType == wkbGeometryType.wkbPolygonZM)
            {
                shpType = "polygons";
            }
            else if (oGeometryType == wkbGeometryType.wkbLineString ||
                oGeometryType == wkbGeometryType.wkbLineString25D ||
                oGeometryType == wkbGeometryType.wkbLineStringM ||
                oGeometryType == wkbGeometryType.wkbLineStringZM)
            {
                shpType = "polyline";
            }
            else
            {
                shpType = "other";
            }
            return shpType;
        }

        /// <summary>
        /// 创建树木位置
        /// </summary>
        public List<Tree> createTreePosition(DEMOperate dem, int i)
        {
            Console.WriteLine("Create trees of feature " + i);

            if (oLayer == null)
            {
                return null;
            }

            string forestType = "人工林";
            string distributeType = "";
            double density = 0;
            double standArea = 0;
            double treeNum = 0;
            double D = 0, Dg = 0;
            List<Tree> trees = new List<Tree>();
            Feature oFeature = oLayer.GetFeature(i);            

            // 获得森林类型
            for (int iField = 0; iField < m_FeildList.Count; iField++)
            {
                if (m_FeildList[iField].ToUpper() == "F_QIYUAN" || m_FeildList[iField].ToUpper() == "TYPE" || m_FeildList[iField].ToUpper() == "FOREST_TYPE" || m_FeildList[iField].ToUpper() == "STAND_TYPE")
                {
                    forestType = GetFeildContent(iField, oFeature);
                }
                if (m_FeildList[iField].ToUpper() == "F_DWZS" || m_FeildList[iField].ToUpper() == "DENSITY" || m_FeildList[iField].ToUpper() == "FOREST_DENSITY" || m_FeildList[iField].ToUpper() == "STAND_DENSITY")
                {
                    density = Convert.ToDouble(GetFeildContent(iField, oFeature));
                }
                if (m_FeildList[iField].ToUpper() == "DISTRIBUTE" || m_FeildList[iField].ToUpper() == "F_FBLX")
                {
                    distributeType = GetFeildContent(iField, oFeature);
                }
                if (m_FeildList[iField].ToUpper() == "MEAN_DBH")//算数平均胸径
                {
                    D = Convert.ToDouble(GetFeildContent(iField, oFeature));
                }
                if (m_FeildList[iField].ToUpper() == "QUADRATIC_MEAN_DBH" || m_FeildList[iField].ToUpper() == "Q_MEAN_DBH")//平均胸径（二次平均胸径）
                {
                    Dg = Convert.ToDouble(GetFeildContent(iField, oFeature));
                }
                if (m_FeildList[iField].ToUpper() == "F_ZHUSHU" || m_FeildList[iField].ToUpper() == "TREE_NUMBER" || m_FeildList[iField].ToUpper() == "TREENUMBER")//林木数量
                {
                    treeNum = Convert.ToDouble(GetFeildContent(iField, oFeature));
                }
            }
            if (density == 0)
            {
                if (treeNum != 0)
                {
                    standArea = oFeature.GetGeometryRef().GetArea();
                    density = treeNum / standArea / 10000;
                }
                else
                {
                    Console.WriteLine("DATA ERROR: The Stand density is lacked.");
                    return null;
                }
            }

            // 人工林
            if (forestType == "人工林" || forestType == "Plantation")
            {
                Geometry geom = oFeature.GetGeometryRef();
                Envelope env = new Envelope();
                geom.GetEnvelope(env);

                //分布范围
                double xMax, xMin, yMax, yMin, xCen, yCen;
                xMax = env.MaxX;
                xMin = env.MinX;
                yMax = env.MaxY;
                yMin = env.MinY;
                xCen = (xMax + xMin) / 2;
                yCen = (yMax + yMin) / 2;

                //将值放入泛型当中位置坐标
                List<double> tempX = new List<double>();
                List<double> tempY = new List<double>();

                double sideLength = Math.Sqrt(density / 10000 * ((xMax - xMin) * (yMax - yMin)));//边长
                double offsetX = (xMax - xMin) / sideLength;
                double offsetY = (yMax - yMin) / sideLength;
                double diagonal = Math.Sqrt((xMax - xMin) * (xMax - xMin) + (yMax - yMin) * (yMax - yMin)); //对角线
                double xAdd = (diagonal - (xMax - xMin)) / 2;
                double yAdd = (diagonal - (yMax - yMin)) / 2;

                Random rand = new Random();
                //菱形均匀分布
                if (distributeType.Contains("Diamond") || distributeType.Contains("Interlaced"))
                {
                    Console.WriteLine("Diamond distribution");
                    int row = 0;
                    for (double j = xMin - xAdd; j < xMax + xAdd; j += offsetX)
                    {
                        if (row % 2 == 0)
                        {
                            for (double k = yMin - yAdd; k < yMax + yAdd; k += offsetY)
                            {
                                tempX.Add(j + rand.Next(10) / 10.0 * offsetX / 4);
                                tempY.Add(k + rand.Next(10) / 10.0 * offsetY / 4);
                            }
                        }
                        else
                        {
                            for (double k = yMin - yAdd + offsetY / 2; k < yMax + yAdd; k += offsetY)
                            {
                                tempX.Add(j + rand.Next(10) / 10.0 * offsetX / 4);
                                tempY.Add(k + rand.Next(10) / 10.0 * offsetY / 4);
                            }
                        }
                        row++;
                    }
                }
                // 环形均匀分布   
                else if (distributeType.Contains("Circular"))
                {
                    Console.WriteLine("Circular distribution");

                    //对角线做直径,计算面积，计算株数，计算环数，计算环距，计算极坐标，计算xy
                    double r = diagonal / 2;
                    double A = Math.PI * r * r;
                    double N = density / 10000 * A;
                    int roundNum = (int)((Math.Sqrt(1 + 4.0 / 3 * N) - 1) / 2);
                    double roundDistance = Math.Sqrt(A / Math.PI) / roundNum;

                    for (int j = 0; j < roundNum; j++)
                    {
                        double radius = j * roundDistance;      //极矩

                        for (int k = 0; k < 6 * j; k++)
                        {
                            double angle = k * 2 * Math.PI / 6 / j;         //极径

                            tempX.Add(radius * Math.Cos(angle) + rand.Next(10) / 10.0 * roundDistance / 4 + xCen + rand.Next(10) / 10.0 * roundDistance / 4);
                            tempY.Add(radius * Math.Sin(angle) + rand.Next(10) / 10.0 * roundDistance / 4 + yCen + rand.Next(10) / 10.0 * roundDistance / 4);
                        }
                    }
                }
                // 井型均匀分布
                else if (distributeType.Contains("Rectangular"))
                {
                    Console.WriteLine("Rectangular distribution");

                    for (double j = xMin - xAdd; j < xMax + xAdd; j += offsetX)
                    {
                        for (double k = yMin - yAdd; k < yMax + yAdd; k += offsetY)
                        {
                            tempX.Add(j + rand.Next(10) / 10.0 * offsetX / 4);
                            tempY.Add(k + rand.Next(10) / 10.0 * offsetY / 4);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Rectangular distribution (default)");
                    // 默认井型
                    for (double j = xMin - xAdd; j < xMax + xAdd; j += offsetX)
                    {
                        for (double k = yMin - yAdd; k < yMax + yAdd; k += offsetY)
                        {
                            tempX.Add(j + rand.Next(10) / 10.0 * offsetX / 4);
                            tempY.Add(k + rand.Next(10) / 10.0 * offsetY / 4);
                        }
                    }
                }

                //计算林分坡度
                dem.getColRow(xCen, yCen);
                double aspect = dem.CalcuAspect();
                //坐标系转换，林木沿山坡分布
                for (int j = 0; j < tempX.Count; j++)
                {
                    double x = (tempX[j] - xCen) * Math.Cos(aspect) + (tempY[j] - yCen) * Math.Sin(aspect);
                    double y = (tempY[j] - yCen) * Math.Cos(aspect) - (tempX[j] - xCen) * Math.Sin(aspect);
                    tempX[j] = x + xCen;
                    tempY[j] = y + yCen;
                }

                //用此Driver创建Shape文件
                //创建DataSource
                string temppath = "temp" + i + ".shp";
                DataSource pDataSource  = oDriver.CreateDataSource(temppath, null);                

                if (pDataSource == null)
                    Console.WriteLine("DataSource Creation Error");

                //创建层Layer
                string outname = "point_out" + i;
                Layer tempLayer = pDataSource.CreateLayer(outname, null, wkbGeometryType.wkbPoint, null);
                if (tempLayer == null)
                    Console.WriteLine("Layer Creation Failed");

                //创建属性
                FieldDefn oField1 = new FieldDefn("TREE_ID", FieldType.OFTInteger);
                oField1.SetWidth(16);
                tempLayer.CreateField(oField1, 1);

                //创建一个Feature
                Feature mFeature = new Feature(tempLayer.GetLayerDefn());

                // 创建点
                Geometry mPoint = new Geometry(wkbGeometryType.wkbPoint);
                for (int j = 0; j < tempX.Count; j++)
                {
                    //树木编号
                    mFeature.SetField(0, j);
                    //添加坐标点
                    mPoint.AddPoint(tempX[j], tempY[j], 0);
                    mFeature.SetGeometry(mPoint);
                    //将带有坐标及属性的Feature要素点写入Layer中
                    tempLayer.CreateFeature(mFeature);
                }

                //关闭文件读写
                mFeature.Dispose();
                mPoint.Dispose();

                //创建裁切Layer
                string clipLyrname = "ClipLayer" + i;
                Layer clipLayer = pDataSource.CreateLayer(clipLyrname, null, wkbGeometryType.wkbPolygon, null);
                clipLayer.CreateFeature(oFeature);

                //裁剪
                string treePosition = "treePosition" + i ;
                Layer result = pDataSource.CreateLayer(treePosition, null, wkbGeometryType.wkbPoint, null);
                tempLayer.Clip(clipLayer, result, new string[] { "SKIP_FAILURES=YES" }, null, null);

                // 删除临时temp.shp
                tempLayer.Dispose();
                pDataSource.DeleteLayer(0);

                //以下读取数据并对trees赋值
                int rFeatureCount = (int)result.GetFeatureCount(0);
                Feature rFeature = null;
                for (int k = 0; k < rFeatureCount; k++)
                {
                    Tree tree = new Tree();
                    tree.ForestID = i;//标注所属树林
                    tree.ID = k;//树木编号

                    rFeature = result.GetFeature(k);
                    Geometry geometry = rFeature.GetGeometryRef();
                    tree.X = geometry.GetX(0);
                    tree.Y = geometry.GetY(0);
                    trees.Add(tree);//加入trees
                }
                Console.WriteLine("Get trees' locations");

                //求算WVA,WVA表示采用加权Voronoi图得出每株数的多边形面积
                List<double> boundX = new List<double>();
                List<double> boundY = new List<double>();
                boundX.Add(xMin-offsetX); boundX.Add(xMin- offsetX); boundX.Add(xMax+ offsetX); boundX.Add(xMax+ offsetX);
                boundY.Add(yMin- offsetY); boundY.Add(yMax+ offsetY); boundY.Add(yMax+ offsetY); boundY.Add(yMin- offsetY);
                Geometry oGeometry = new Geometry(wkbGeometryType.wkbPolygon);
                List<double> treeX = new List<double>();
                List<double> treeY = new List<double>();
                List<double> Area = new List<double>();
                for (int t = 0; t < trees.Count; t++)
                {
                    //只在本树林范围计算WVA，此时tree_count为除本树林已加入trees的棵数
                    treeX.Add(trees[t].X);
                    treeY.Add(trees[t].Y);
                    Area.Add(0);//初始化
                }
                int t_num = trees.Count;//此树林树木棵数
                double[] treex = treeX.ToArray(); double[] treey = treeY.ToArray();
                double[] boundx = boundX.ToArray(); double[] boundy = boundY.ToArray();
                double[] area = Area.ToArray();
                CreateThiessen(treex, treey, t_num, boundx, boundy, 4, area);
                for (int t = 0; t < trees.Count; t++)
                {
                    trees[t].WVA = area[t];
                }

                //分配胸径
                WeibullDistribute(trees, trees.Count, D, Dg);

                // 删除临时
                clipLayer.Dispose();
                result.Dispose();
                pDataSource.DeleteLayer(0);
                pDataSource.DeleteLayer(0);
            }

            return trees;
        }

        /// <summary>
        /// 返回面状要素面积
        /// </summary>
        /// <returns></returns>
        public List<double> GetForestArea()
        {
            List<double> forestArea = new List<double>();

            Feature oFeature = null;
            int iFeatureCount = (int)oLayer.GetFeatureCount(0);
            for (int i = 0; i < iFeatureCount; i++)
            {
                oFeature = oLayer.GetFeature(i);
                Geometry oGeometry = new Geometry(wkbGeometryType.wkbPolygon);
                oGeometry = oFeature.GetGeometryRef();

                double area = oGeometry.GetArea();
                forestArea.Add(area);
            }
            return forestArea;
        }

        /// <summary>
        /// 输出shapefile 文件
        /// </summary>
        /// <param name="trees"></param>
        /// <param name="path"></param>
        public void Createshp(List<Tree> trees, string path)
        {
            DataSource outds = oDriver.CreateDataSource(path, null);
            if (outds == null)
            {
                Console.WriteLine("创建矢量文件失败！");
                return;
            }
            SpatialReference sr =oLayer.GetSpatialRef();
            if (sr == null)
            {               
                string wkt = "";
                sr = new SpatialReference(wkt);
            }
            Layer outLayer = outds.CreateLayer("PointLayer", sr, wkbGeometryType.wkbPoint, null);

            //create fields
            FieldDefn oFieldID = new FieldDefn("ID", FieldType.OFTInteger);
            outLayer.CreateField(oFieldID, 1);

            FieldDefn oFieldX = new FieldDefn("X", FieldType.OFTReal);// float型
            oFieldX.SetWidth(18);
            oFieldX.SetPrecision(8);
            outLayer.CreateField(oFieldX, 1);
            
            FieldDefn oFieldY = new FieldDefn("Y", FieldType.OFTReal);
            oFieldY.SetWidth(18);
            oFieldY.SetPrecision(8);
            outLayer.CreateField(oFieldY, 1);

            FieldDefn oDBH = new FieldDefn("DBH", FieldType.OFTReal);
            oDBH.SetWidth(18);
            oDBH.SetPrecision(8);
            outLayer.CreateField(oDBH, 1);

            FieldDefn oHeight= new FieldDefn("Height", FieldType.OFTReal);
            oHeight.SetWidth(18);
            oHeight.SetPrecision(8);
            outLayer.CreateField(oHeight, 1);

            FieldDefn oUBH= new FieldDefn("UBH", FieldType.OFTReal);
            oUBH.SetWidth(18);
            oUBH.SetPrecision(8);
            outLayer.CreateField(oUBH, 1);

            FieldDefn oCH= new FieldDefn("CH", FieldType.OFTReal);
            oCH.SetWidth(18);
            oCH.SetPrecision(8);
            outLayer.CreateField(oCH, 1);

            FieldDefn oCW = new FieldDefn("CW", FieldType.OFTReal);
            oCW.SetWidth(18);
            oCW.SetPrecision(8);
            outLayer.CreateField(oCW, 1);

            FieldDefn eUBH = new FieldDefn("eUBH", FieldType.OFTReal);
            eUBH.SetWidth(18);
            eUBH.SetPrecision(8);
            outLayer.CreateField(eUBH, 1);

            FieldDefn sUBH = new FieldDefn("sUBH", FieldType.OFTReal);
            sUBH.SetWidth(18);
            sUBH.SetPrecision(8);
            outLayer.CreateField(sUBH, 1);

            FieldDefn wUBH = new FieldDefn("wUBH", FieldType.OFTReal);
            wUBH.SetWidth(18);
            wUBH.SetPrecision(8);
            outLayer.CreateField(wUBH, 1);

            FieldDefn nUBH = new FieldDefn("nUBH", FieldType.OFTReal);
            nUBH.SetWidth(18);
            nUBH.SetPrecision(8);
            outLayer.CreateField(nUBH, 1);

            FieldDefn eCH = new FieldDefn("eCH", FieldType.OFTReal);
            eCH.SetWidth(18);
            eCH.SetPrecision(8);
            outLayer.CreateField(eCH, 1);

            FieldDefn sCH = new FieldDefn("sCH", FieldType.OFTReal);
            sCH.SetWidth(18);
            sCH.SetPrecision(8);
            outLayer.CreateField(sCH, 1);

            FieldDefn wCH = new FieldDefn("wCH", FieldType.OFTReal);
            wCH.SetWidth(18);
            wCH.SetPrecision(8);
            outLayer.CreateField(wCH, 1);

            FieldDefn nCH = new FieldDefn("nCH", FieldType.OFTReal);
            nCH.SetWidth(18);
            nCH.SetPrecision(8);
            outLayer.CreateField(nCH, 1);

            FieldDefn eCW = new FieldDefn("eCW", FieldType.OFTReal);
            eCW.SetWidth(18);
            eCW.SetPrecision(8);
            outLayer.CreateField(eCW, 1);

            FieldDefn sCW = new FieldDefn("sCW", FieldType.OFTReal);
            sCW.SetWidth(18);
            sCW.SetPrecision(8);
            outLayer.CreateField(sCW, 1);

            FieldDefn wCW = new FieldDefn("wCW", FieldType.OFTReal);
            wCW.SetWidth(18);
            wCW.SetPrecision(8);
            outLayer.CreateField(wCW, 1);

            FieldDefn nCW = new FieldDefn("nCW", FieldType.OFTReal);
            nCW.SetWidth(18);
            nCW.SetPrecision(8);
            outLayer.CreateField(nCW, 1);

            FieldDefn oBiomass = new FieldDefn("Biomass", FieldType.OFTReal);
            oBiomass.SetWidth(18);
            oBiomass.SetPrecision(8);
            outLayer.CreateField(oBiomass, 1);

            FieldDefn oMortality = new FieldDefn("Mortality", FieldType.OFTString);
            oMortality.SetWidth(50);
            outLayer.CreateField(oMortality, 1);

            FieldDefn oCutting = new FieldDefn("Cutting", FieldType.OFTString);
            oCutting.SetWidth(50);
            outLayer.CreateField(oCutting, 1);

            // create feature
            FeatureDefn oDefn = outLayer.GetLayerDefn();
            for (int i = 0; i < trees.Count; i++)
            {
                Feature oFeature = new Feature(oDefn);
                oFeature.SetField(0, i);
                oFeature.SetField(1, double.Parse(trees[i].X.ToString("0.00")));
                oFeature.SetField(2, double.Parse(trees[i].Y.ToString("0.00"))); 
                oFeature.SetField(3, double.Parse(trees[i].DBH.ToString("0.00"))); 
                oFeature.SetField(4, double.Parse(trees[i].Height.ToString("0.00"))); 
                oFeature.SetField(5, double.Parse(trees[i].UnderBranchHeight.ToString("0.00"))); 
                oFeature.SetField(6, double.Parse(trees[i].CrownHeight.ToString("0.00"))); 
                oFeature.SetField(7, double.Parse(trees[i].CrownWidth.ToString("0.00")));
                if (trees[i].eastUnderBranchHeight > 0)
                {
                    oFeature.SetField(8, double.Parse(trees[i].eastUnderBranchHeight.ToString("0.00")));
                    oFeature.SetField(9, double.Parse(trees[i].southUnderBranchHeight.ToString("0.00")));
                    oFeature.SetField(10, double.Parse(trees[i].westUnderBranchHeight.ToString("0.00")));
                    oFeature.SetField(11, double.Parse(trees[i].northUnderBranchHeight.ToString("0.00")));
                }
                if (trees[i].eastCrownHeight > 0)
                {
                    oFeature.SetField(12, double.Parse(trees[i].eastCrownHeight.ToString("0.00")));
                    oFeature.SetField(13, double.Parse(trees[i].southCrownHeight.ToString("0.00")));
                    oFeature.SetField(14, double.Parse(trees[i].westCrownHeight.ToString("0.00")));
                    oFeature.SetField(15, double.Parse(trees[i].northCrownHeight.ToString("0.00")));
                }
                if (trees[i].eastCrownWidth > 0)
                {
                    oFeature.SetField(16, double.Parse(trees[i].eastCrownWidth.ToString("0.00")));
                    oFeature.SetField(17, double.Parse(trees[i].southCrownWidth.ToString("0.00")));
                    oFeature.SetField(18, double.Parse(trees[i].westCrownWidth.ToString("0.00")));
                    oFeature.SetField(19, double.Parse(trees[i].northCrownWidth.ToString("0.00")));
                }
                oFeature.SetField(20, double.Parse(trees[i].Biomass.ToString("0.00")));
                if (trees[i].isMortality)
                    oFeature.SetField(21, "Mortality");
                if (trees[i].isCutting)
                    oFeature.SetField(22, "Cutting");
                Geometry geoTree = new Geometry(wkbGeometryType.wkbPoint);
                geoTree.AddPoint(double.Parse(trees[i].X.ToString("0.00")), double.Parse(trees[i].Y.ToString("0.00")), 0);
                oFeature.SetGeometry(geoTree);
                outLayer.CreateFeature(oFeature);
            }
        }
        
        /// <summary>
        /// weibul1分布分配胸径
        /// </summary>
        /// <param name="trees"></param>
        /// <param name="tree_count"></param>
        /// <param name="D">算数平均胸径</param>
        /// <param name="Dg">平均胸径</param>
        public void WeibullDistribute(List<Tree> treesinStand,int tree_num, double D,double Dg) 
        {
            //计算Weibull分布的参数
            int M = 1;//径阶距
            double cvd, a, cvx, x, r1, c, b;
            cvd = Math.Pow((Math.Pow((Dg / D), 2) - 1), 0.5);
            a = 0.45 * Dg;//位置参数
            cvx = cvd * (D / (-a));
            x = Math.Round(a)+0.5;//径阶中值
            r1 = 1.002 - 0.4832 * cvx + 0.49688 * Math.Pow(cvx, 2) - 0.057665 * Math.Pow(cvx, 7) + 0.03946 * Math.Pow(cvx, 8);
            c= 0.1342 - 0.8879/cvx+0.10049 / Math.Pow(cvx, 2) - 0.011996 / Math.Pow(cvx, 3) + 0.0005361 / Math.Pow(cvx, 4);//形状指数
            b = (D - a) / r1;//尺度参数

            //计算用于分配的各胸径对应概率           
            double minPro = M * (c / b) * Math.Pow((x - a) / b, c - 1) * Math.Pow(Math.E, -1 * Math.Pow((x - a) / b, c));
            double sumP = 0;
            List<double> DBH_P = new List<double>();//某胸径对应下的概率
            List<double> DBH_X = new List<double>();//各胸径值

            //Weibull分布径阶理论概率
            for(double Pf = minPro;Pf>=minPro;x+=M)
            {
                Pf = M * (c / b) * Math.Pow((x - a) / b, c - 1) * Math.Pow(Math.E, -1 * Math.Pow((x - a) / b, c));
                DBH_X.Add(x);
                DBH_P.Add(Pf);
                sumP += Pf;
            }

            //求各胸径相应下的树木
            double sumtreenum = 0;
            List<double> DBH_N = new List<double>();//所有树木按照胸径顺序排序
            for(int i = 0; i < DBH_P.Count; i++)
            {
                DBH_P[i] = DBH_P[i] / sumP;
                int num = (int)Math.Round(DBH_P[i] * tree_num);
                sumtreenum += num;

                Console.WriteLine("径阶概率：" + DBH_P[i]);
                Console.WriteLine("径阶棵数：" + num);
                Console.WriteLine("总棵数：" + sumtreenum);

                for (int j = 0; j < num; j++)
                {
                    DBH_N.Add(DBH_X[i]);
                }
            }

            if (sumtreenum < tree_num)
            {
                double n = tree_num - sumtreenum;
                for (int i = 0; i < n; i++)
                {
                    DBH_N.Add(DBH_X[DBH_P.Count - 1]);
                }
            }

            //对根据MVA进行排序
            int[] ID = new int[tree_num];
            double[] WVA = new double[tree_num];
            for(int i = 0; i < tree_num; i++)
            {   //初始化排序数组
                ID[i] = i;
                WVA[i] = treesinStand[i].WVA;
            }

            for(int i = 0; i < tree_num - 1; i++)//选择排序
            {
                int min = i;double w;int id;
                for(int j = i + 1; j < tree_num; j++)
                {
                    if (WVA[j] < WVA[min]) { min = j; }
                }
                w = WVA[min];WVA[min] = WVA[i];WVA[i] = w;
                id = ID[min];ID[min] = ID[i];ID[i] = id;
            }

            //根据WVA排序结果分配胸径
            for(int i = 0; i < tree_num; i++)
            {
                treesinStand[ID[i]].DBH = DBH_N[i];
            }

        }

        public int GetFeatureNumber()
        {
            int iFeatureCount = (int)oLayer.GetFeatureCount(0);
            return iFeatureCount;
        }

    }//END class  
  
}
