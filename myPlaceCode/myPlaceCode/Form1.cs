using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OSGeo.GDAL;
using OSGeo.OGR;
using OSGeo.OSR;
using System.IO;
using ThoughtWorks.QRCode.Codec;

namespace myPlaceCode
{
    public partial class Form1 : Form
    {
        // 秒以下的单位值的倒数，将秒以下数值转换为二进制，需要先乘以该值
        // 2048=2^11，对应11位二进制；
        long subsec = 2048;
        //度（9位），分（6位），秒（6位），秒以下（11位）
        static int lde = 9, lmin = lde + 6, lsec = lmin+6;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnGenCode_Click(object sender, EventArgs e)
        {
            //地名地址时空编码，输入为球面坐标（度小数），经度与纬度用半角逗号隔开
            string[] xys = txtCoor.Text.Split(',');
            int n = xys.GetLength(0);
            if (n != 2)
            {
                MessageBox.Show("输入坐标格式有问题，请顺序输入坐标的xy值，用,连接");
                return;
            }

            double x = double.Parse(xys[0]), y = double.Parse(xys[1]),h=8848;
            string code = trandxy2code(x,y,h);
            lblCodeExp.Text = code;
            QRCodeGenerator(code);

        }

        //将坐标转换为时空编码，
        //具体编码方法参照文献：《地球空间参考网格系统建设初探》程承旗
        //该函数实现的是基于单点坐标生成定位码，高程值用5位0表示
        private string trandxy2code(double x,double y,double h)
        {
            //编码共32级，64位，每一级纬向和经向编码各占一位，
            //因此，在一个方向上位数分布为，度（9位），分（6位），秒（6位），秒以下（11位）；
            //经度和纬度分别转为度分秒，以及秒以下的
            string xcd,ycd,code,xbu,ybu;
            xcd = du2bin(x);
            ycd = du2bin(y);
            if (x >= 0)
                xbu = "1";
            else
                xbu = "0";
            if (y >= 0)
                ybu = "1";
            else
                ybu = "0";

            xcd = xbu + xcd;
            ycd = ybu + ycd;
            code = CrossString(xcd,ycd);
            //convert binary to hex
            code = Convert.ToInt64(code, 2).ToString("X2");
            string sh = Convert.ToString(Math.Round(h));
            sh=sh.PadLeft(5, '0');
            code = code + sh;

            //Todo 
            

            return code;
        }
        //字符串交叉，纬向在前，经向在后，每一位交叉组合
        private string CrossString(string xcd, string ycd)
        {
            int nx, ny;
            nx = xcd.Length;
            ny = ycd.Length;
            if (nx != ny)
            {
                MessageBox.Show("经向和纬向编码长度不一致，请检查程序问题");
                return "";
            }
            string code="";
            for (int i = 0; i < nx; ++i)
            {
                code = code + ycd.Substring(i, 1) + xcd.Substring(i, 1);
            }
            return code;
        }

        private string du2bin(double x)
        {
            int d=0, m=0, s=0,subs=0;
            string d2, m2, s2, subs2;
            //将度小数，转换为度，分，秒，和秒以下
            du2dms(x, ref d, ref m, ref s, ref subs);
            //分别将度分秒秒下转换为二进制的字符串
            d2=Convert.ToString(d, 2);
            m2 = Convert.ToString(m, 2);
            s2 = Convert.ToString(s, 2);
            subs2 = Convert.ToString(subs, 2);
            //按照实际位数补位，在一个方向上位数分布为，度（9位），分（6位），秒（6位），秒以下（11位）；
            //注意，此处度先补够8位，剩余一位根据经纬度的正负号确定
            d2 = d2.PadLeft(8, '0');
            m2 = m2.PadLeft(6, '0');
            s2 = s2.PadLeft(6, '0');
            subs2 = subs2.PadRight(11, '0');
            string code = d2 + m2 + s2 + subs2;
            return code;
        }

        private void du2dms(double x, ref int d, ref int m, ref int s, ref int subs)
        {
//             d = Convert.ToInt32(x);
//             m = Convert.ToInt32((x - d)*60);
//             s = Convert.ToInt32(((x - d) * 60 - m) * 60);
//             subs = Convert.ToInt32((((x - d) * 60 - m) * 60 - s) * subsec);


             d = Convert.ToInt32(Math.Floor(x));
             m = Convert.ToInt32(Math.Floor((x - d) * 60));
             s = Convert.ToInt32(Math.Floor(((x - d) * 60 - m) * 60));
             subs = Convert.ToInt32(Math.Floor((((x - d) * 60 - m) * 60 - s) * subsec));
            return;
        }

        //生成范围码，即，根据坐上角点和右下角点，生成表征范围的编码
        //编码构成规则：级别（2位）+纬向半跨度码（2位）+经向半跨度码（2位）
        private void btnGenRangeCode_Click(object sender, EventArgs e)
        {
            string[] sXyul = txtUL.Text.Split(',');
            string[] sXybr = txtBR.Text.Split(',');
            int nul = sXyul.GetLength(0),nbr=sXybr.GetLength(0);
            if (nul != 2 || nbr !=2)
            {
                MessageBox.Show("输入坐标格式有问题，请顺序输入坐标的xy值，用,连接");
                return;
            }

            double xul = double.Parse(sXyul[0]), yul = double.Parse(sXyul[1]);
            double xbr = double.Parse(sXybr[0]), ybr = double.Parse(sXybr[1]);
            string code = trandxy2rangecode(xul,yul,xbr,ybr);
            lblRangeExp.Text = code;
        }

        //根据左上角点和右下角点坐标，生成范围码
        private string trandxy2rangecode(double xul, double yul, double xbr, double ybr)
        {
            long level = 32;  //当前范围编码级别
            double dbXdet = xbr - xul;
            double dbYdet = yul - ybr;
            if (dbXdet < 0 || dbYdet < 0)
            {
                MessageBox.Show("左上角点和右下角点输入有问题，xul < xbr && yul > ybr");
                return "";
            }

            //将度小数转换为秒
            int d=0, m=0, s=0, subs=0;
            du2dms(dbXdet, ref d, ref m, ref s, ref subs);
            double dbd = Convert.ToDouble(d), dbm = Convert.ToDouble(m), dbs = Convert.ToDouble(s), dbss = Convert.ToDouble(subs) / subsec;
            double dbXsec = dbd * 3600 + dbm * 60 + dbs + dbss;
            double dbXmin = dbd * 60 + dbm + (dbs+dbss)/60;
            double dbXde = dbd + dbm/60 + (dbs+dbss)/ 3600;
            du2dms(dbYdet, ref d, ref m, ref s, ref subs);
            dbd = Convert.ToDouble(d); dbm = Convert.ToDouble(m); dbs = Convert.ToDouble(s); dbss = Convert.ToDouble(subs) / subsec;
            double dbYsec = dbd * 3600 + dbm * 60 + dbs + dbss;
            double dbYmin = dbd * 60 + dbm + (dbs + dbss) / 60;
            double dbYde = dbd + dbm / 60 + (dbs + dbss) / 3600;
            int nX=0, nY=0,ie=0,i;
            //从32级开始生成范围码，如果范围码任何一个方向上的半跨度码超过两位数（99），那么需要提升一级，重新开始编码，直至符合条件
            for (i = 32; i > 0;--i )
            {
                if (i>lsec)
                {
                    //秒以下级别
                    ie = i - lsec;
                    nX = Convert.ToInt32(Math.Ceiling(dbXsec * Math.Pow(2, ie)));
                    nY = Convert.ToInt32(Math.Ceiling(dbYsec * Math.Pow(2, ie)));
                    if (nX<100 && nY<100)
                        break;
                }
                else if (i>lmin)
                {
                    //秒级
                    ie = i - lmin;
                    nX = Convert.ToInt32(Math.Ceiling(dbXsec / Math.Pow(2, ie)));
                    nY = Convert.ToInt32(Math.Ceiling(dbYsec / Math.Pow(2, ie)));
                    if (nX < 100 && nY < 100)
                        break;
                }
                else if (i>lde)
                {
                    //分级
                    ie = i - lde;
                    nX = Convert.ToInt32(Math.Ceiling(dbXmin / Math.Pow(2, ie)));
                    nY = Convert.ToInt32(Math.Ceiling(dbYmin / Math.Pow(2, ie)));
                    if (nX < 100 && nY < 100)
                        break;
                }
                else
                {
                    //度级
                    ie = i - lde;
                    nX = Convert.ToInt32(Math.Ceiling(dbXde / Math.Pow(2, ie)));
                    nY = Convert.ToInt32(Math.Ceiling(dbXde / Math.Pow(2, ie)));
                    if (nX < 100 && nY < 100)
                        break;
                }
            }

            string code = "";
            code = Convert.ToString(i).PadLeft(2, '0') + Convert.ToString(nY).PadLeft(2, '0') + Convert.ToString(nX).PadLeft(2, '0');
            return code;
        }

        //生成日期码
        //modified by zhangxun
        private string GenDateCode(bool iscreattime=false)
        {
           // 标志位，0采集日期，1实体创建日期
            string code = DateTime.Now.ToString("yyyyMMdd");
            if (iscreattime)
                code += "0";
            else
                code += "1";
            return code;
        }

        //从shp文件中生成
        private void btnGenCodeShp_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;//该值确定是否可以选择多个文件
            dialog.Title = "请选择shp文件";
            dialog.Filter = "SHP文件(*.shp)|*.shp";
            if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            string file = dialog.FileName;
            InitinalGdal();
            if (!GetShpLayer(file))
            {
                MessageBox.Show("error");
                return;
            }
            DateTime t0 = DateTime.Now;
            DateTime t1;

            int iIndex = 0;
            //读取当前图层中对象数量，并便利所有对象，对每个对象进行编码，然后输出成txt
            int iFeatureCout = Convert.ToInt32(oLayer.GetFeatureCount(0));
            //创建输出的编码文件
            int iPosition = file.LastIndexOf("\\");
            string sTxtName = file.Substring(iPosition + 1, file.Length - iPosition - 4 - 1) + ".txt";
            FileStream fs = new FileStream("D:\\"+sTxtName, FileMode.Create);
            string[] sXY;  //每次读取的POI的坐标对，只有一个坐标的X和Y
            int nul; //上述字符串数组要素数量
            string codepos, coderange, codedate, code;
            byte[] data;
            for (iIndex = 0; iIndex < iFeatureCout;++iIndex )
            {
                if (!GetGeometry(iIndex))
                {
                    MessageBox.Show("error");
                    break;
                }

                sXY = sCoordiantes.Split(' ');
                nul = sXY.GetLength(0);
                if (nul<2)
                    continue;
                codepos = trandxy2code(double.Parse(sXY[0]), double.Parse(sXY[1]), 0);
                coderange = trandxy2rangecode(double.Parse(sXY[0]), double.Parse(sXY[1]), double.Parse(sXY[0]), double.Parse(sXY[1]));
                codedate = GenDateCode();

                //code = codepos + "-" + coderange + "-" + codedate + "\r\n";
                code = codepos + coderange + codedate + "\r\n";

                //获得字节数组
                data = System.Text.Encoding.Default.GetBytes(code);
                //开始写入
                fs.Write(data, 0, data.Length);
            }

            t1 = DateTime.Now;
            // 求时间差
            TimeSpan ts = t1 - t0;
            string sTmp = String.Format("完成时间：{0:F} \r\n要素数量：{1:D1} \r\n编码耗时（以秒计）：{2} \r\n", t1, iFeatureCout, ts.TotalSeconds);
            data = System.Text.Encoding.Default.GetBytes(sTmp);
            fs.Seek(0, SeekOrigin.Begin);
            fs.Write(data, 0, data.Length);
            //清空缓冲区、关闭流
            fs.Flush();
            fs.Close();

            MessageBox.Show("ok");
        }


        //从shp读取polygon，然后计算中心，然后编码
        private void butchoosePolygonShp_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;//该值确定是否可以选择多个文件
            dialog.Title = "请选择shp文件";
            dialog.Filter = "SHP文件(*.shp)|*.shp";
            if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            string file = dialog.FileName;
            InitinalGdal();
            if (!GetShpLayer(file))
            {
                MessageBox.Show("error");
                return;
            }


            DateTime t0 = DateTime.Now;
            DateTime t1;
            int iIndex = 0;
            //读取当前图层中对象数量，并便利所有对象，对每个对象进行编码，然后输出成txt
            int iFeatureCout = Convert.ToInt32(oLayer.GetFeatureCount(0));
            int iPosition = file.LastIndexOf("\\");
            string sTxtName = file.Substring(iPosition + 1, file.Length - iPosition - 4 - 1) + ".txt";
            FileStream fs = new FileStream("D:\\" + sTxtName, FileMode.Create);
            string[] PolygonXYs; //每个polygon的多个点集
            string[] sXY;  //每次读取的POI的坐标对，只有一个坐标的X和Y
            int nul; //上述字符串数组要素数量
            string codepos, coderange, codedate, code;
            byte[] data;



            //for (iIndex = 0; iIndex < iFeatureCout; ++iIndex)
            //{
            //    Feature oFeature = null;
            //    oFeature = oLayer.GetFeature(iIndex);
            //    //  Geometry  
            //    Geometry oGeometry = oFeature.GetGeometryRef();
            //    wkbGeometryType oGeometryType = oGeometry.GetGeometryType();

            //    Console.WriteLine(oGeometry.GetGeometryName());
            //    Console.WriteLine(iIndex);
            //}

            for (iIndex = 0; iIndex < iFeatureCout; ++iIndex)
            {
                if (!GetGeometry(iIndex))
                {
                    MessageBox.Show("error");
                    break;
                }

                PolygonXYs = sCoordiantes.Split(',');
                Console.WriteLine(PolygonXYs[0]);

                double sumx = 0, sumy = 0;
                int cout = 0;
                for (int i = 0; i < PolygonXYs.Length; i++)
                {
                    sXY = PolygonXYs[i].Split(' ');
                    nul = sXY.GetLength(0);
                    if (nul < 2)
                        continue;
                    try
                    {
                        sumx += double.Parse(sXY[0]);
                        sumy += double.Parse(sXY[1]);
                    }
                    catch (Exception)
                    {

                        continue;
                    }
                   
                   
                    cout += 1;
                   
                }
                double averagex = sumx / cout;
                double averagey = sumy / cout;

                codepos = trandxy2code(averagex, averagey, 0);
                Console.WriteLine("codepos", codepos);
                coderange = trandxy2rangecode(averagex, averagey, averagex, averagey);
                codedate = GenDateCode();

                code = codepos + "-" + coderange + "-" + codedate + "\r\n";

                //string temp = codepos + coderange + codedate;
                //string frontpartcode = temp.Substring(0, 63);
                //frontpartcode = Convert.ToInt64(frontpartcode, 2).ToString("X2");
                //compress binary to hex

                //string backpartcode = temp.Remove(0, temp.Length - 32);
                //backpartcode = Convert.ToInt32(backpartcode, 2).ToString("X2");
                //Console.WriteLine("backpartcode", backpartcode);

                //code = frontpartcode + "\r\n";
                //获得字节数组
                data = System.Text.Encoding.Default.GetBytes(code);
                //开始写入
                fs.Write(data, 0, data.Length);

            }


            t1 = DateTime.Now;
            TimeSpan ts = t1 - t0;
            string sTmp = String.Format("完成时间：{0:F} \r\n要素数量Polygon：{1:D1} \r\n编码耗时（以秒计）：{2} \r\n", t1, iFeatureCout, ts.TotalSeconds);
            data = System.Text.Encoding.Default.GetBytes(sTmp);
            fs.Seek(0, SeekOrigin.Begin);
            fs.Write(data, 0, data.Length);
            //清空缓冲区、关闭流
            fs.Flush();
            fs.Close();

            MessageBox.Show("ok");

        }


        public void QRCodeGenerator(string s)
        {
            ThoughtWorks.QRCode.Codec.QRCodeEncoder encoder = new QRCodeEncoder();
            encoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;//编码方式(注意：BYTE能支持中文，ALPHA_NUMERIC扫描出来的都是数字)
            encoder.QRCodeScale = 8;//大小(值越大生成的二维码图片像素越高)
            encoder.QRCodeVersion = 0;//版本(注意：设置为0主要是防止编码的字符串太长时发生错误)
            encoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;//错误效验、错误更正(有4个等级)
            System.Drawing.Bitmap bp = encoder.Encode(s.ToString(), Encoding.GetEncoding("GB2312"));
            Image image = bp;
            pictureBox2.Image = bp;
            //保存二维码图片：
            SaveFileDialog sf = new SaveFileDialog();
            sf.Title = "选择保存文件位置";
            sf.Filter = "保存图片(*.jpg) |*.jpg|所有文件(*.*) |*.*";
            //设置默认文件类型显示顺序
            sf.FilterIndex = 1;
            //保存对话框是否记忆上次打开的目录
            sf.RestoreDirectory = true;
            if (sf.ShowDialog() == DialogResult.OK)
            {
                Image im = this.pictureBox2.Image;
                //获得文件路径
                string localFilePath = sf.FileName.ToString();
                if (sf.FileName != "")
                {
                    string fileNameExt = localFilePath.Substring(localFilePath.LastIndexOf("\\") + 1);//获取文件名，不带路径
                                                                                                      // newFileName = fileNameExt+DateTime.Now.ToString("yyyyMMdd")  ;//给文件名后加上时间
                    string FilePath = localFilePath.Substring(0, localFilePath.LastIndexOf("."));  //获取文件路径，带文件名,不带后缀
                    string fn = sf.FileName;
                    pictureBox2.Image.Save(FilePath + "-" + DateTime.Now.ToString("yyyyMMdd") + ".jpg");

                }
            }
        }

        public OSGeo.OGR.Driver oDerive;
        private Layer oLayer;
        public string sCoordiantes; 
        // 初始化Gdal  
        public void InitinalGdal()  
        {
            oLayer = null;
            sCoordiantes = null;
            SharpMap.GdalConfiguration.ConfigureGdal();
            SharpMap.GdalConfiguration.ConfigureOgr();
            // 为了支持中文路径  
            Gdal.SetConfigOption("GDAL_FILENAME_IS_UTF8", "YES");  
            // 为了使属性表字段支持中文  
            Gdal.SetConfigOption("SHAPE_ENCODING", "");  
            Gdal.AllRegister();  
            Ogr.RegisterAll();  
              
            oDerive = Ogr.GetDriverByName("ESRI Shapefile");  
            if (oDerive == null)  
            {  
                MessageBox.Show("文件不能打开，请检查");  
            }  
        }


        // 获取SHP文件的层  
        public bool GetShpLayer(string sfilename)
        {
            if (null == sfilename || sfilename.Length <= 3)
            {
                oLayer = null;
                return false;
            }
            if (oDerive == null)
            {
                MessageBox.Show("文件不能打开，请检查");
            }
            DataSource ds = oDerive.Open(sfilename, 1);
            if (null == ds)
            {
                oLayer = null;
                return false;
            }
            int iPosition = sfilename.LastIndexOf("\\");
            string sTempName = sfilename.Substring(iPosition + 1, sfilename.Length - iPosition - 4 - 1);
            //oLayer = ds.GetLayerByName(sTempName);
            int nly=ds.GetLayerCount();
            if (nly <= 0)
            {
                ds.Dispose();
                return false;
            }
            oLayer = ds.GetLayerByIndex(0);
            if (oLayer == null)
            {
                ds.Dispose();
                return false;
            }
            return true;
        }

        // 获取数据  
        public bool GetGeometry(int iIndex)
        {
            Console.Write(iIndex);
            if (null == oLayer)
            {
                return false;
            }
            
            Feature oFeature = null;
            oFeature = oLayer.GetFeature(iIndex);
            //  Geometry  
            Geometry oGeometry = oFeature.GetGeometryRef();
            wkbGeometryType oGeometryType = oGeometry.GetGeometryType();
            switch (oGeometryType)
            {
                case wkbGeometryType.wkbPoint:
                    oGeometry.ExportToWkt(out sCoordiantes);
                    sCoordiantes = sCoordiantes.ToUpper().Replace("POINT (", "").Replace(")", "");
                    break;
                case wkbGeometryType.wkbLineString:
                case wkbGeometryType.wkbLinearRing:
                    oGeometry.ExportToWkt(out sCoordiantes);
                    sCoordiantes = sCoordiantes.ToUpper().Replace("LINESTRING (", "").Replace(")", "");
                    break;
                case wkbGeometryType.wkbPolygon:
                    oGeometry.ExportToWkt(out sCoordiantes);
                    //Console.WriteLine(sCoordiantes);
                    //POLYGON ((106.497253433332 29.5029544986433,106.497901901985 29.5035991546911,106.497360214679 29.5030326859769,106.497291558329 29.5029544986433,106.497253433332 29.5029544986433))
                    sCoordiantes = sCoordiantes.ToUpper().Replace("POLYGON ((", "").Replace("))", "");
                    break;
                default:
                    break;
            }
            return true;
        }  
    }
}
