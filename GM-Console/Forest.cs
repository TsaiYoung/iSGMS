using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console
{
    public class Forest
    {
        private List<Tree> trees = new List<Tree>();
        public ShpOperate forestShp = new ShpOperate();
        //public double forestArea = 0;
        public List<double> forestArea = new List<double>();
        public string type;

        public int Readshp(string shppath)
        {
            // 初始化GDAL和OGR
            forestShp.InitinalGdal();

            forestShp.GetShpLayer(shppath);
            // 获取所有属性字段名称,存放在m_FeildList中  
            forestShp.GetFeilds();
            
            // 获取数据类型
            type = forestShp.GetGeometryType();
            Console.WriteLine("The type of input shapefile data is " + type);

            if (type == "points")
            {
                return 1;
            }
            else if (type == "polygons")
            {
                int num = getForestsNum();
                return num;
            }
            else
            {
                Console.WriteLine("Wrroy Data format");
                return 0;
            }
        }

        public List<Tree> getTrees()
        {
            trees = forestShp.GetGeometry();
            Console.WriteLine("Get trees succeed");

            double maxX = trees[0].X;
            double maxY = trees[0].Y; 
            double minX = trees[0].X;
            double minY = trees[0].Y;
            for (int i = 0; i < trees.Count; i++)
            {
                if (trees[i].X > maxX)
                    maxX = trees[i].X;
                else if (trees[i].X < minX)
                    minX = trees[i].X;

                if (trees[i].Y > maxY)
                    maxY = trees[i].Y;
                else if (trees[i].Y < minY)
                    minY = trees[i].Y;
            }
            double area = (maxX - minX) * (maxY - minY);
            forestArea.Add(area);

            return trees;
        }

        public int  getForestsNum()
        {
            int forestnum = forestShp.GetFeatureNumber();
            return forestnum;
        }

        public List<Tree> createTrees(DEMOperate dem, int i)
        {
            trees = forestShp.createTreePosition(dem,i);
            forestArea = forestShp.GetForestArea();

            return trees;
        }
        
        public void WriteShp(string outpath, List<Tree> standTrees)
        {
            forestShp.Createshp(standTrees, outpath);
        }
    }
}
