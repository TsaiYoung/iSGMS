using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM_Console
{
    public class Tree
    {
        public Tree()
        {
        }

        /// <summary>
        /// 树木编号
        /// </summary>
        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// 是否边界木
        /// </summary>
        public bool isEdge = false;
        /// <summary>
        /// 是否砍伐
        /// </summary>
        public bool isCutting = false;
        /// <summary>
        /// 是否枯损
        /// </summary>
        public bool isMortality = false;
        /// <summary>
        /// 树种
        /// </summary>
        private string species;
        public string Species
        {
            get { return species; }
            set { species = value; }
        }
        /// <summary>
        /// X坐标
        /// </summary>
        private double x, y, z;
        public double X
        {
            get { return x; }
            set { x = value; }
        }
        /// <summary>
        /// Y坐标
        /// </summary>
        public double Y
        {
            get { return y; }
            set { y = value; }
        }
        /// <summary>
        /// Z坐标
        /// </summary>
        public double Z
        {
            get { return z; }
            set { z = value; }
        }
        /// <summary>
        /// 胸径
        /// </summary>
        private double d;
        public double DBH
        {
            get { return d; }
            set { d = value; }
        }
        /// <summary>
        /// 树高
        /// </summary>
        private double height;
        public double Height
        {
            get { return height; }
            set { height = value; }
        }

        /// <summary>
        /// 枝下高
        /// </summary>
        private double UBH;
        public double UnderBranchHeight
        {
            get { return UBH; }
            set { UBH = value; }
        }
        /// <summary>
        /// 冠高
        /// </summary>
        private double ch;
        public double CrownHeight
        {
            get { return ch; }
            set { ch = value; }
        }
        /// <summary>
        /// 冠幅
        /// </summary>
        private double cw;
        public double CrownWidth
        {
            get { return cw; }
            set { cw = value; }
        }

        /// <summary>
        /// 东枝下高
        /// </summary>
        private double eUBH = 0;
        public double eastUnderBranchHeight
        {
            get { return eUBH; }
            set { eUBH = value; }
        }
        /// <summary>
        /// 东冠高
        /// </summary>
        private double ech = 0;
        public double eastCrownHeight
        {
            get { return ech; }
            set { ech = value; }
        }
        /// <summary>
        /// 东冠幅
        /// </summary>
        private double ecw = 0;
        public double eastCrownWidth
        {
            get { return ecw; }
            set { ecw = value; }
        }

        /// <summary>
        /// 西枝下高
        /// </summary>
        private double wUBH = 0;
        public double westUnderBranchHeight
        {
            get { return wUBH; }
            set { wUBH = value; }
        }
        /// <summary>
        /// 西冠高
        /// </summary>
        private double wch = 0;
        public double westCrownHeight
        {
            get { return wch; }
            set { wch = value; }
        }
        /// <summary>
        /// 西冠幅
        /// </summary>
        private double wcw = 0;
        public double westCrownWidth
        {
            get { return wcw; }
            set { wcw = value; }
        }

        /// <summary>
        /// 南枝下高
        /// </summary>
        private double sUBH = 0;
        public double southUnderBranchHeight
        {
            get { return sUBH; }
            set { sUBH = value; }
        }
        /// <summary>
        /// 南冠高
        /// </summary>
        private double sch = 0;
        public double southCrownHeight
        {
            get { return sch; }
            set { sch = value; }
        }
        /// <summary>
        /// 南冠幅
        /// </summary>
        private double scw = 0;
        public double southCrownWidth
        {
            get { return scw; }
            set { scw = value; }
        }

        /// <summary>
        /// 北枝下高
        /// </summary>
        private double nUBH = 0;
        public double northUnderBranchHeight
        {
            get { return nUBH; }
            set { nUBH = value; }
        }
        /// <summary>
        /// 北冠高
        /// </summary>
        private double nch = 0;
        public double northCrownHeight
        {
            get { return nch; }
            set { nch = value; }
        }
        /// <summary>
        /// 北冠幅
        /// </summary>
        private double ncw = 0;
        public double northCrownWidth
        {
            get { return ncw; }
            set { ncw = value; }
        }
        /// <summary>
        /// 健康状况
        /// </summary>
        private double health = 1;
        public double Health
        {
            get { return health; }
            set { health = value; }
        }
        /// <summary>
        /// 采伐概率
        /// </summary>
        private double rtm;
        public double RTM
        {
            get { return rtm; }
            set { rtm = value; }
        }
        /// <summary>
        /// 单木生物量
        /// </summary>
        private double biomass;
        public double Biomass
        {
            get { return biomass; }
            set { biomass = value; }
        }
        /// <summary>
        /// WVA表示样木加权Voronoi图竞争指数(平方米)
        /// </summary>
        private double wva;
        public double WVA
        {
            get { return wva; }
            set { wva = value; }
        }
        /// <summary>
        /// 树林识别编号
        /// </summary>
        private int forestID;
        public int ForestID
        {
            get { return forestID; }
            set { forestID = value; }
        }
    }
}
