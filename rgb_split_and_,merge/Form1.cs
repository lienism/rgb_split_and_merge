using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace rgb_split_and__merge
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        static string file = "WIN_20181214_14_59_56_Pro";
        static string filetype = ".jpg";
        string filepath = @"C:\Users\ft20180723\Desktop\qwer\wareware\" + file + filetype;
        private void button1_Click(object sender, EventArgs e)
        {
            
            OpenCvSharp.Point[][] Dummy;
            HierarchyIndex[] blue_hi;
            pictureBox1.Image = new Bitmap(filepath);
            Mat src = BitmapConverter.ToMat(new Bitmap(pictureBox1.Image));
            Mat rgb = split_rgb_merge(src);

            Moments mm;
            OpenCvSharp.Point heavy;
            double a;
            double b;
            Cv2.FindContours(rgb, out Dummy, out blue_hi, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
            for(int j = 0; j < Dummy.Length; j++)
            {
                mm = Cv2.Moments(Dummy[j]);
                heavy = new OpenCvSharp.Point((mm.M10 / mm.M00), (mm.M01 / mm.M00));
                a = Cv2.ContourArea(Dummy[j]);
                b = Cv2.ArcLength(Dummy[j], false);
                if (b > (src.Height * src.Width) / 4000)
                {
                    Cv2.PutText(src, "Area : " + a, new OpenCvSharp.Point(heavy.X + 10,heavy.Y + 10), HersheyFonts.HersheyTriplex, 2, new Scalar(255, 165, 0,255), 5);
                    Cv2.PutText(src, "length : " + b, new OpenCvSharp.Point(heavy.X + 10, heavy.Y + 10 + (src.Height) / 10), HersheyFonts.HersheyTriplex, 2, new Scalar(255, 165, 0, 255), 5);
                    Cv2.Line(src, heavy, heavy, new Scalar(255, 255, 0, 255), 40);
                    RotatedRect rcc = Cv2.MinAreaRect(Dummy[j]);
                    Rect rc = rcc.BoundingRect();
                    Cv2.DrawContours(src, Dummy, j, new Scalar(255,0,0,255), 5);
                    Cv2.Rectangle(src, rc, new Scalar(0, 0, 255, 255), 5);
                    
                    //TODO ERROR!!

                    //Mat one = new Mat(src,rc);

                    //Scalar color = Cv2.Mean(one);
                    //Debug.WriteLine("R : " + color.Val0);
                    //Debug.WriteLine("G : " + color.Val1);
                    //Debug.WriteLine("B : " + color.Val2);
                    //Debug.WriteLine("A : " + color.Val3);

                    //one.Dispose();

                    
                }
            }
            pictureBox12.Image = BitmapConverter.ToBitmap(src);
            Bitmap Final_result = BitmapConverter.ToBitmap(src);
            Final_result.Save(@"C:\Users\ft20180723\Desktop\qwer\wareware\power\" + file + @"\" + file + "_Final_result" + filetype);
            
            
           
        }
        public Mat split_rgb_merge(Mat src)
        {
            OpenCvSharp.Point[][][] concontours;
            Mat Dummy_mat = new Mat();
            Mat original_blue = new Mat();
            Mat original_green = new Mat();
            Mat original_red = new Mat();
            Mat result = new Mat();

            src.CopyTo(original_blue);
            src.CopyTo(original_green);
            src.CopyTo(original_red);

            OpenCvSharp.Point[][] Blue;
            OpenCvSharp.Point[][] Green;
            OpenCvSharp.Point[][] Red;
            OpenCvSharp.Point[][] Dummy;
            HierarchyIndex[] blue_hi;
            HierarchyIndex[] green_hi;
            HierarchyIndex[] red_hi;

            Mat[] BGR;
            BGR = Cv2.Split(src);
            Cv2.CvtColor(BGR[0], BGR[0], ColorConversionCodes.GRAY2BGR);
            Cv2.CvtColor(BGR[1], BGR[1], ColorConversionCodes.GRAY2BGR);
            Cv2.CvtColor(BGR[2], BGR[2], ColorConversionCodes.GRAY2BGR);
            pictureBox2.Image = BitmapConverter.ToBitmap(BGR[0]);
            pictureBox3.Image = BitmapConverter.ToBitmap(BGR[1]);
            pictureBox4.Image = BitmapConverter.ToBitmap(BGR[2]);

            Bitmap bmp_B = BitmapConverter.ToBitmap(BGR[0]);
            bmp_B.Save(@"C:\Users\ft20180723\Desktop\qwer\wareware\power\" + file + @"\" + file + "_B" + filetype);
            Bitmap bmp_G = BitmapConverter.ToBitmap(BGR[1]);
            bmp_G.Save(@"C:\Users\ft20180723\Desktop\qwer\wareware\power\" + file + @"\" + file + "_G" + filetype);
            Bitmap bmp_R = BitmapConverter.ToBitmap(BGR[2]);
            bmp_R.Save(@"C:\Users\ft20180723\Desktop\qwer\wareware\power\" + file + @"\" + file + "_R" + filetype);


            Cv2.CvtColor(BGR[0], BGR[0], ColorConversionCodes.BGR2GRAY);
            Cv2.CvtColor(BGR[1], BGR[1], ColorConversionCodes.BGR2GRAY);
            Cv2.CvtColor(BGR[2], BGR[2], ColorConversionCodes.BGR2GRAY);

            //Cv2.AdaptiveThreshold(BGR[0], BGR[0], 127, AdaptiveThresholdTypes.GaussianC, ThresholdTypes.BinaryInv, 3 ,0);
            //Cv2.AdaptiveThreshold(BGR[1], BGR[1], 127, AdaptiveThresholdTypes.GaussianC, ThresholdTypes.BinaryInv, 3 , 0);
            //Cv2.AdaptiveThreshold(BGR[2], BGR[2], 127, AdaptiveThresholdTypes.GaussianC, ThresholdTypes.BinaryInv, 3 , 0);


            Cv2.Threshold(BGR[0], BGR[0], 0, 255, ThresholdTypes.Otsu | ThresholdTypes.BinaryInv);
            Cv2.Threshold(BGR[1], BGR[1], 0, 255, ThresholdTypes.Otsu | ThresholdTypes.BinaryInv);
            Cv2.Threshold(BGR[2], BGR[2], 0, 255, ThresholdTypes.Otsu | ThresholdTypes.BinaryInv);
            Cv2.Erode(BGR[0], BGR[0], new Mat());
            Cv2.Dilate(BGR[0], BGR[0], new Mat());
            Cv2.Erode(BGR[1], BGR[1], new Mat());
            Cv2.Dilate(BGR[1], BGR[1], new Mat());
            Cv2.Erode(BGR[2], BGR[2], new Mat());
            Cv2.Dilate(BGR[2], BGR[2], new Mat());
            Cv2.Erode(BGR[0], BGR[0], new Mat());
            Cv2.Dilate(BGR[0], BGR[0], new Mat());
            Cv2.Erode(BGR[1], BGR[1], new Mat());
            Cv2.Dilate(BGR[1], BGR[1], new Mat());
            Cv2.Erode(BGR[2], BGR[2], new Mat());
            Cv2.Dilate(BGR[2], BGR[2], new Mat());
            pictureBox5.Image = BitmapConverter.ToBitmap(BGR[0]);
            pictureBox6.Image = BitmapConverter.ToBitmap(BGR[1]);
            pictureBox7.Image = BitmapConverter.ToBitmap(BGR[2]);

            Bitmap bmp_B_th = BitmapConverter.ToBitmap(BGR[0]);
            bmp_B_th.Save(@"C:\Users\ft20180723\Desktop\qwer\wareware\power\" + file + @"\" + file + "_B_th" + filetype);
            Bitmap bmp_G_th = BitmapConverter.ToBitmap(BGR[1]);
            bmp_G_th.Save(@"C:\Users\ft20180723\Desktop\qwer\wareware\power\" + file + @"\" + file + "_G_th" + filetype);
            Bitmap bmp_R_th = BitmapConverter.ToBitmap(BGR[2]);
            bmp_R_th.Save(@"C:\Users\ft20180723\Desktop\qwer\wareware\power\" + file + @"\" + file + "_R_th" + filetype);

            Cv2.FindContours(BGR[0], out Blue, out blue_hi, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
            Cv2.FindContours(BGR[1], out Green, out green_hi, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
            Cv2.FindContours(BGR[2], out Red, out red_hi, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
            

            int line_hutosa = 10;


            for (int i = 0; i < Blue.Length; i++)
            {
                RotatedRect rcc = Cv2.MinAreaRect(Blue[i]);
                Rect rc = rcc.BoundingRect();

                Cv2.DrawContours(original_blue, Blue, i, new Scalar(255, 0, 0, 255), line_hutosa, LineTypes.AntiAlias);
                Cv2.Rectangle(original_blue, rc, new Scalar(0, 0, 255, 255), line_hutosa, LineTypes.AntiAlias);
            }
            pictureBox8.Image = BitmapConverter.ToBitmap(original_blue);

            Bitmap Check_original_blue = BitmapConverter.ToBitmap(original_blue);
            Check_original_blue.Save(@"C:\Users\ft20180723\Desktop\qwer\wareware\power\" + file + @"\" + file + "_B_th_check" + filetype);




            for (int i = 0; i < Green.Length; i++)
            {
                RotatedRect rcc = Cv2.MinAreaRect(Green[i]);
                Rect rc = rcc.BoundingRect();

                Cv2.DrawContours(original_green, Green, i, new Scalar(255, 0, 0, 255), line_hutosa, LineTypes.AntiAlias);
                Cv2.Rectangle(original_green, rc, new Scalar(0, 0, 255, 255), line_hutosa, LineTypes.AntiAlias);
            }
            pictureBox9.Image = BitmapConverter.ToBitmap(original_green);

            Bitmap Check_original_green = BitmapConverter.ToBitmap(original_green);
            Check_original_green.Save(@"C:\Users\ft20180723\Desktop\qwer\wareware\power\" + file + @"\" + file + "_G_th_check" + filetype);


            for (int i = 0; i < Red.Length; i++)
            {
                RotatedRect rcc = Cv2.MinAreaRect(Red[i]);
                Rect rc = rcc.BoundingRect();

                Cv2.DrawContours(original_red, Red, i, new Scalar(255, 0, 0, 255), line_hutosa, LineTypes.AntiAlias);
                Cv2.Rectangle(original_red, rc, new Scalar(0, 0, 255, 255), line_hutosa, LineTypes.AntiAlias);
            }
            pictureBox10.Image = BitmapConverter.ToBitmap(original_red);

            Bitmap Check_original_red = BitmapConverter.ToBitmap(original_red);
            Check_original_red.Save(@"C:\Users\ft20180723\Desktop\qwer\wareware\power\" + file + @"\" + file + "_R_th_check" + filetype);


            Debug.WriteLine("Blue.length : " + Blue.Length);
            Debug.WriteLine("Red.length : " + Red.Length);
            Debug.WriteLine("Green.length : " + Green.Length);

            double Blue_sum = 0;
            for(int i = 0; i < Blue.Length; i++)
            {
                Blue_sum = Blue_sum + Cv2.ContourArea(Blue[i]);
            }
            double Green_sum = 0;
            for (int i = 0; i < Green.Length; i++)
            {
                Green_sum = Green_sum + Cv2.ContourArea(Green[i]);
            }
            double Red_sum = 0;
            for (int i = 0; i < Red.Length; i++)
            {
                Red_sum = Red_sum + Cv2.ContourArea(Red[i]);
            }
            


            if (Blue_sum < Red_sum)
            {
                Dummy = Blue;
                Dummy_mat = BGR[0];
                Blue = Red;
                BGR[0] = BGR[2];
                Red = Dummy;
                BGR[2] = Dummy_mat;
            }
           if(Blue_sum < Green_sum)
            {
                Dummy = Blue;
                Dummy_mat = BGR[0];
                Blue = Green;
                Dummy_mat = BGR[1];
                Green = Dummy;
                BGR[1] = Dummy_mat;
            }
           if(Red_sum < Green_sum)
            {
                Dummy = Green;
                Dummy_mat = BGR[1];
                Green = Red;
                BGR[1] = BGR[2];
                Red = Dummy;
                BGR[1] = Dummy_mat;
            }
           
            Cv2.BitwiseAnd(BGR[0], BGR[1], result);
            Cv2.BitwiseAnd(result, BGR[2], result);


            pictureBox11.Image = BitmapConverter.ToBitmap(result);
            return result;
        }
    }
}
