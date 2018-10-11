using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OSGeo.GDAL;
using OSGeo.OGR;
using OSGeo.OSR;

namespace GM_Console
{
    public class DEMOperate
    {
        private Dataset ds;
        private Band demBand;
        private int iCol = 0;
        private int iRow = 0;
        private int XSize = 0;
        private int YSize = 0;
        private double cellsize = 0;
        private double[] geoTransform;

        /// <summary>
        /// 读取DEM
        /// </summary>
        /// <param name="dempath"></param>
        public void ReadDEM(string dempath)
        {
            // 为了支持中文路径  
            Gdal.SetConfigOption("GDAL_FILENAME_IS_UTF8", "YES");
            // 为了使属性表字段支持中文  
            Gdal.SetConfigOption("SHAPE_ENCODING", "");

            Gdal.AllRegister();
            ds = Gdal.Open(dempath, Access.GA_ReadOnly);

            //栅格数据长宽
            XSize = ds.RasterXSize;
            YSize = ds.RasterYSize;

            //像元大小
            geoTransform = new double[6];
            ds.GetGeoTransform(geoTransform);
            cellsize = geoTransform[1];

            demBand = ds.GetRasterBand(1); // 获取第一个band
        }

        ///<smmary>
        ///读取高程
        ///</smmary>
        public double getDEMValue()
        {
            double value = 0;

            //获取DEM数值到一维数组  
            float[] data = new float[1 * 1];
            CPLErr err = demBand.ReadRaster(iCol, iRow, 1, 1, data, 1, 1, 0, 0);
            value = data[0];

            return value;
        }

        public double getDEMValue(int iCol, int iRow)
        {
            double value = 0;

            //获取DEM数值到一维数组  
            float[] data = new float[1 * 1];
            CPLErr err = demBand.ReadRaster(iCol, iRow, 1, 1, data, 1, 1, 0, 0);
            value = data[0];

            return value;
        }

        /// <summary>
        /// 计算坡度坡向
        /// radian
        /// </summary>
        public double CalcuSlope()
        {
            //三阶反距离平方权差分，求坡度
            double value = 0;
            double fcol = 0, frow = 0;

            //DEM边缘处理
            if (iCol > 0 && iRow > 0 && iCol < XSize && iRow < YSize)
            {
                frow = (getDEMValue(iCol - 1, iRow + 1) + 2 * getDEMValue(iCol, iRow + 1) + getDEMValue(iCol + 1, iRow + 1) -
                            getDEMValue(iCol - 1, iRow - 1) - 2 * getDEMValue(iCol, iRow - 1) - getDEMValue(iCol + 1, iRow - 1)) /
                            (8.0 * cellsize);
                fcol = (getDEMValue(iCol + 1, iRow + 1) + 2 * getDEMValue(iCol + 1, iRow) + getDEMValue(iCol + 1, iRow - 1) -
                            getDEMValue(iCol - 1, iRow + 1) - 2 * getDEMValue(iCol - 1, iRow) - getDEMValue(iCol - 1, iRow - 1)) /
                            (8.0 * cellsize);
            }
            else if (iCol > 0 && iRow == 0 && iCol < XSize && iRow < YSize)
            {
                frow = (getDEMValue(iCol - 1, iRow + 1) + 2 * getDEMValue(iCol, iRow + 1) + getDEMValue(iCol + 1, iRow + 1) -
                            getDEMValue(iCol - 1, iRow) - 2 * getDEMValue(iCol, iRow) - getDEMValue(iCol + 1, iRow)) /
                            (8.0 * cellsize);
                fcol = (getDEMValue(iCol + 1, iRow + 1) + 2 * getDEMValue(iCol + 1, iRow) + getDEMValue(iCol + 1, iRow) -
                            getDEMValue(iCol - 1, iRow + 1) - 2 * getDEMValue(iCol - 1, iRow) - getDEMValue(iCol - 1, iRow)) /
                            (8.0 * cellsize);
            }
            else if (iCol == 0 && iRow > 0 && iCol < XSize && iRow < YSize)
            {
                frow = (getDEMValue(iCol, iRow + 1) + 2 * getDEMValue(iCol, iRow + 1) + getDEMValue(iCol + 1, iRow + 1) -
                            getDEMValue(iCol, iRow - 1) - 2 * getDEMValue(iCol, iRow - 1) - getDEMValue(iCol + 1, iRow - 1)) /
                            (8.0 * cellsize);
                fcol = (getDEMValue(iCol + 1, iRow + 1) + 2 * getDEMValue(iCol + 1, iRow) + getDEMValue(iCol + 1, iRow - 1) -
                            getDEMValue(iCol, iRow + 1) - 2 * getDEMValue(iCol, iRow) - getDEMValue(iCol - 1, iRow - 1)) /
                            (8.0 * cellsize);
            }
            else if (iCol == 0 && iRow == 0 && iCol < XSize && iRow < YSize)
            {
                frow = (getDEMValue(iCol, iRow + 1) + 2 * getDEMValue(iCol, iRow + 1) + getDEMValue(iCol + 1, iRow + 1) -
                            getDEMValue(iCol, iRow) - 2 * getDEMValue(iCol, iRow) - getDEMValue(iCol + 1, iRow)) /
                            (8.0 * cellsize);
                fcol = (getDEMValue(iCol + 1, iRow + 1) + 2 * getDEMValue(iCol + 1, iRow) + getDEMValue(iCol + 1, iRow) -
                            getDEMValue(iCol, iRow + 1) - 2 * getDEMValue(iCol, iRow) - getDEMValue(iCol, iRow)) /
                            (8.0 * cellsize);
            }
            else if (iCol > 0 && iRow > 0 && iCol == XSize && iRow < YSize)
            {
                frow = (getDEMValue(iCol - 1, iRow + 1) + 2 * getDEMValue(iCol, iRow + 1) + getDEMValue(iCol, iRow + 1) -
                            getDEMValue(iCol - 1, iRow - 1) - 2 * getDEMValue(iCol, iRow - 1) - getDEMValue(iCol, iRow - 1)) /
                            (8.0 * cellsize);
                fcol = (getDEMValue(iCol, iRow + 1) + 2 * getDEMValue(iCol, iRow) + getDEMValue(iCol, iRow - 1) -
                            getDEMValue(iCol - 1, iRow + 1) - 2 * getDEMValue(iCol - 1, iRow) - getDEMValue(iCol, iRow - 1)) /
                            (8.0 * cellsize);
            }
            else if (iCol > 0 && iRow > 0 && iCol < XSize && iRow == YSize)
            {
                frow = (getDEMValue(iCol - 1, iRow) + 2 * getDEMValue(iCol, iRow) + getDEMValue(iCol + 1, iRow) -
                            getDEMValue(iCol - 1, iRow - 1) - 2 * getDEMValue(iCol, iRow - 1) - getDEMValue(iCol + 1, iRow - 1)) /
                            (8.0 * cellsize);
                fcol = (getDEMValue(iCol + 1, iRow) + 2 * getDEMValue(iCol + 1, iRow) + getDEMValue(iCol + 1, iRow - 1) -
                            getDEMValue(iCol - 1, iRow) - 2 * getDEMValue(iCol - 1, iRow) - getDEMValue(iCol - 1, iRow - 1)) /
                            (8.0 * cellsize);
            }
            else if (iCol > 0 && iRow > 0 && iCol == XSize && iRow == YSize)
            {
                frow = (getDEMValue(iCol - 1, iRow) + 2 * getDEMValue(iCol, iRow) + getDEMValue(iCol, iRow) -
                            getDEMValue(iCol - 1, iRow - 1) - 2 * getDEMValue(iCol, iRow - 1) - getDEMValue(iCol, iRow - 1)) /
                            (8.0 * cellsize);
                fcol = (getDEMValue(iCol, iRow) + 2 * getDEMValue(iCol, iRow) + getDEMValue(iCol, iRow - 1) -
                            getDEMValue(iCol - 1, iRow) - 2 * getDEMValue(iCol - 1, iRow) - getDEMValue(iCol - 1, iRow - 1)) /
                            (8.0 * cellsize);
            }
            else if (iCol == 0 && iRow > 0 && iCol < XSize && iRow == YSize)
            {
                frow = (getDEMValue(iCol, iRow) + 2 * getDEMValue(iCol, iRow) + getDEMValue(iCol + 1, iRow) -
                            getDEMValue(iCol, iRow - 1) - 2 * getDEMValue(iCol, iRow - 1) - getDEMValue(iCol + 1, iRow - 1)) /
                            (8.0 * cellsize);
                fcol = (getDEMValue(iCol + 1, iRow) + 2 * getDEMValue(iCol + 1, iRow) + getDEMValue(iCol + 1, iRow - 1) -
                            getDEMValue(iCol, iRow) - 2 * getDEMValue(iCol, iRow) - getDEMValue(iCol - 1, iRow - 1)) /
                            (8.0 * cellsize);
            }
            else if (iCol > 0 && iRow == 0 && iCol == XSize && iRow < YSize)
            {
                frow = (getDEMValue(iCol - 1, iRow + 1) + 2 * getDEMValue(iCol, iRow + 1) + getDEMValue(iCol, iRow + 1) -
                            getDEMValue(iCol - 1, iRow) - 2 * getDEMValue(iCol, iRow) - getDEMValue(iCol, iRow)) /
                            (8.0 * cellsize);
                fcol = (getDEMValue(iCol, iRow + 1) + 2 * getDEMValue(iCol, iRow) + getDEMValue(iCol, iRow) -
                            getDEMValue(iCol - 1, iRow + 1) - 2 * getDEMValue(iCol - 1, iRow) - getDEMValue(iCol - 1, iRow)) /
                            (8.0 * cellsize);
            }
            else
            {
                Console.WriteLine("行列号错误");
                return 999;
            }

            value = Math.Atan(Math.Sqrt(fcol * fcol + frow * frow));

            return value;
        }

        /// <summary>
        /// 计算坡向
        /// radian
        /// </summary>
        public double CalcuAspect()
        {
            double value = 0;
            double fcol = 0, frow = 0;

            //求坡向
            if (iCol > 0 && iRow > 0 && iCol < XSize && iRow < YSize)
            {
                frow = (getDEMValue(iCol - 1, iRow + 1) + 2 * getDEMValue(iCol, iRow + 1) + getDEMValue(iCol + 1, iRow + 1) -
                            getDEMValue(iCol - 1, iRow - 1) - 2 * getDEMValue(iCol, iRow - 1) - getDEMValue(iCol + 1, iRow - 1)) /
                            (8.0 * cellsize);
                fcol = (getDEMValue(iCol + 1, iRow + 1) + 2 * getDEMValue(iCol + 1, iRow) + getDEMValue(iCol + 1, iRow - 1) -
                            getDEMValue(iCol - 1, iRow + 1) - 2 * getDEMValue(iCol - 1, iRow) - getDEMValue(iCol - 1, iRow - 1)) /
                            (8.0 * cellsize);
            }
            else if (iCol > 0 && iRow == 0 && iCol < XSize && iRow < YSize)
            {
                frow = (getDEMValue(iCol - 1, iRow + 1) + 2 * getDEMValue(iCol, iRow + 1) + getDEMValue(iCol + 1, iRow + 1) -
                            getDEMValue(iCol - 1, iRow) - 2 * getDEMValue(iCol, iRow) - getDEMValue(iCol + 1, iRow)) /
                            (8.0 * cellsize);
                fcol = (getDEMValue(iCol + 1, iRow + 1) + 2 * getDEMValue(iCol + 1, iRow) + getDEMValue(iCol + 1, iRow) -
                            getDEMValue(iCol - 1, iRow + 1) - 2 * getDEMValue(iCol - 1, iRow) - getDEMValue(iCol - 1, iRow)) /
                            (8.0 * cellsize);
            }
            else if (iCol == 0 && iRow > 0 && iCol < XSize && iRow < YSize)
            {
                frow = (getDEMValue(iCol, iRow + 1) + 2 * getDEMValue(iCol, iRow + 1) + getDEMValue(iCol + 1, iRow + 1) -
                            getDEMValue(iCol, iRow - 1) - 2 * getDEMValue(iCol, iRow - 1) - getDEMValue(iCol + 1, iRow - 1)) /
                            (8.0 * cellsize);
                fcol = (getDEMValue(iCol + 1, iRow + 1) + 2 * getDEMValue(iCol + 1, iRow) + getDEMValue(iCol + 1, iRow - 1) -
                            getDEMValue(iCol, iRow + 1) - 2 * getDEMValue(iCol, iRow) - getDEMValue(iCol - 1, iRow - 1)) /
                            (8.0 * cellsize);
            }
            else if (iCol == 0 && iRow == 0 && iCol < XSize && iRow < YSize)
            {
                frow = (getDEMValue(iCol, iRow + 1) + 2 * getDEMValue(iCol, iRow + 1) + getDEMValue(iCol + 1, iRow + 1) -
                            getDEMValue(iCol, iRow) - 2 * getDEMValue(iCol, iRow) - getDEMValue(iCol + 1, iRow)) /
                            (8.0 * cellsize);
                fcol = (getDEMValue(iCol + 1, iRow + 1) + 2 * getDEMValue(iCol + 1, iRow) + getDEMValue(iCol + 1, iRow) -
                            getDEMValue(iCol, iRow + 1) - 2 * getDEMValue(iCol, iRow) - getDEMValue(iCol, iRow)) /
                            (8.0 * cellsize);
            }
            else if (iCol > 0 && iRow > 0 && iCol == XSize && iRow < YSize)
            {
                frow = (getDEMValue(iCol - 1, iRow + 1) + 2 * getDEMValue(iCol, iRow + 1) + getDEMValue(iCol, iRow + 1) -
                            getDEMValue(iCol - 1, iRow - 1) - 2 * getDEMValue(iCol, iRow - 1) - getDEMValue(iCol, iRow - 1)) /
                            (8.0 * cellsize);
                fcol = (getDEMValue(iCol, iRow + 1) + 2 * getDEMValue(iCol, iRow) + getDEMValue(iCol, iRow - 1) -
                            getDEMValue(iCol - 1, iRow + 1) - 2 * getDEMValue(iCol - 1, iRow) - getDEMValue(iCol, iRow - 1)) /
                            (8.0 * cellsize);
            }
            else if (iCol > 0 && iRow > 0 && iCol < XSize && iRow == YSize)
            {
                frow = (getDEMValue(iCol - 1, iRow) + 2 * getDEMValue(iCol, iRow) + getDEMValue(iCol + 1, iRow) -
                            getDEMValue(iCol - 1, iRow - 1) - 2 * getDEMValue(iCol, iRow - 1) - getDEMValue(iCol + 1, iRow - 1)) /
                            (8.0 * cellsize);
                fcol = (getDEMValue(iCol + 1, iRow) + 2 * getDEMValue(iCol + 1, iRow) + getDEMValue(iCol + 1, iRow - 1) -
                            getDEMValue(iCol - 1, iRow) - 2 * getDEMValue(iCol - 1, iRow) - getDEMValue(iCol - 1, iRow - 1)) /
                            (8.0 * cellsize);
            }
            else if (iCol > 0 && iRow > 0 && iCol == XSize && iRow == YSize)
            {
                frow = (getDEMValue(iCol - 1, iRow) + 2 * getDEMValue(iCol, iRow) + getDEMValue(iCol, iRow) -
                            getDEMValue(iCol - 1, iRow - 1) - 2 * getDEMValue(iCol, iRow - 1) - getDEMValue(iCol, iRow - 1)) /
                            (8.0 * cellsize);
                fcol = (getDEMValue(iCol, iRow) + 2 * getDEMValue(iCol, iRow) + getDEMValue(iCol, iRow - 1) -
                            getDEMValue(iCol - 1, iRow) - 2 * getDEMValue(iCol - 1, iRow) - getDEMValue(iCol - 1, iRow - 1)) /
                            (8.0 * cellsize);
            }
            else if (iCol == 0 && iRow > 0 && iCol < XSize && iRow == YSize)
            {
                frow = (getDEMValue(iCol, iRow) + 2 * getDEMValue(iCol, iRow) + getDEMValue(iCol + 1, iRow) -
                            getDEMValue(iCol, iRow - 1) - 2 * getDEMValue(iCol, iRow - 1) - getDEMValue(iCol + 1, iRow - 1)) /
                            (8.0 * cellsize);
                fcol = (getDEMValue(iCol + 1, iRow) + 2 * getDEMValue(iCol + 1, iRow) + getDEMValue(iCol + 1, iRow - 1) -
                            getDEMValue(iCol, iRow) - 2 * getDEMValue(iCol, iRow) - getDEMValue(iCol - 1, iRow - 1)) /
                            (8.0 * cellsize);
            }
            else if (iCol > 0 && iRow == 0 && iCol == XSize && iRow < YSize)
            {
                frow = (getDEMValue(iCol - 1, iRow + 1) + 2 * getDEMValue(iCol, iRow + 1) + getDEMValue(iCol, iRow + 1) -
                            getDEMValue(iCol - 1, iRow) - 2 * getDEMValue(iCol, iRow) - getDEMValue(iCol, iRow)) /
                            (8.0 * cellsize);
                fcol = (getDEMValue(iCol, iRow + 1) + 2 * getDEMValue(iCol, iRow) + getDEMValue(iCol, iRow) -
                            getDEMValue(iCol - 1, iRow + 1) - 2 * getDEMValue(iCol - 1, iRow) - getDEMValue(iCol - 1, iRow)) /
                            (8.0 * cellsize);
            }
            else
            {
                Console.WriteLine("行列号错误");
                return 999;
            }

            if (fcol != 0)
            {
                value = 270 + Math.Atan(frow / fcol) * 180 / Math.PI - 90 * (fcol / Math.Abs(fcol));
                if (value >= 360)
                    value -= 360;
            }
            else if (frow > 0)
                value = 0;
            else if (frow < 0)
                value = 180;
            else
                value = 0; //平地

            return value / 180 *Math.PI;
        }

        public void getColRow(double x, double y)
        {
            //获取行列号  
            double dTemp = geoTransform[1] * geoTransform[5] - geoTransform[2] * geoTransform[4];
            double dCol = 0.0, dRow = 0.0;
            dCol = (geoTransform[5] * (x - geoTransform[0]) - geoTransform[2] * (y - geoTransform[3])) / dTemp + 0.5;
            dRow = (geoTransform[1] * (y - geoTransform[3]) - geoTransform[4] * (x - geoTransform[0])) / dTemp + 0.5;
            iCol = Convert.ToInt32(dCol);
            iRow = Convert.ToInt32(dRow);
        }
    }
}
