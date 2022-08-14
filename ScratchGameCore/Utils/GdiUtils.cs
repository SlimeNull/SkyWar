using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchGameCore.Utils
{
    internal class GdiUtils
    {
        public static void DrawImage(Bitmap canvas, Image originImage, float scale, float rotation)
        {
            PointF targetPoint = (PointF)ImgUtils.UniformOffset(canvas.Size, originImage.Size * scale);
            PointF canvasCenter = (PointF)((SizeF)canvas.Size / 2);

            Matrix matrix = new Matrix();
            matrix.RotateAt(rotation, canvasCenter);

            using (Graphics g = Graphics.FromImage(canvas))
            {
                g.Transform = matrix;
                g.DrawImage(originImage, new RectangleF(targetPoint, originImage.Size * scale));
            }
        }

        /// <summary>
        /// 获取原图像绕中心任意角度旋转后的图像
        /// </summary>
        /// <param name="rawImg"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Image GetRotateImage(Image srcImage, int angle)
        {
            angle %= 360;
            //原图的宽和高
            int srcWidth = srcImage.Width;
            int srcHeight = srcImage.Height;
            //图像旋转之后所占区域宽和高
            Size rotatedSize = ImgUtils.Rotate(srcImage.Size, angle * MathF.PI / 180);
            int rotatedWidth = rotatedSize.Width;
            int rotatedHeight = rotatedSize.Height;
            //目标位图
            Bitmap destImage = new Bitmap(rotatedWidth, rotatedHeight, PixelFormat.Format32bppArgb);
            using (Graphics graphics = Graphics.FromImage(destImage))
            {
                //要让graphics围绕某矩形中心点旋转N度，分三步
                //第一步，将graphics坐标原点移到矩形中心点,假设其中点坐标（x,y）
                //第二步，graphics旋转相应的角度(沿当前原点)
                //第三步，移回（-x,-y）
                //获取画布中心点
                Point centerPoint = new Point(rotatedWidth / 2, rotatedHeight / 2);

                //将graphics坐标原点移到中心点
                graphics.TranslateTransform(centerPoint.X, centerPoint.Y);
                //graphics旋转相应的角度(绕当前原点)
                graphics.RotateTransform(angle);
                //恢复graphics在水平和垂直方向的平移(沿当前原点)
                graphics.TranslateTransform(-centerPoint.X, -centerPoint.Y);
                //此时已经完成了graphics的旋转

                //计算:如果要将源图像画到画布上且中心与画布中心重合，需要的偏移量
                PointF offset = (PointF)ImgUtils.UniformOffset(rotatedSize, srcImage.Size);
                //将源图片画到rect里（rotateRec的中心）
                graphics.DrawImage(srcImage, new RectangleF(offset.X, offset.Y, srcWidth, srcHeight));

                return destImage;
            }
        }
    }
}
