using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace SIImageAnalisys
{
    class Drawing
    {
        public static Bitmap mergeImages(Bitmap image1, Bitmap image2)
        {
            Bitmap result = new Bitmap(image1.Width + image2.Width, image1.Height);
            Graphics graphics = Graphics.FromImage(result);
            graphics.DrawImage(image1, 0, 0, image1.Width, image1.Height);
            graphics.DrawImage(image2, image1.Width, 0, image2.Width, image2.Height);

            return result;
        }

        public static void DrawKeyPoints(Bitmap image, List<Pair> keyPoints, Color penColor, List<Pair> points, List<Pair> narrowedPoints, int offset = 0)
        {
            Graphics graphics = Graphics.FromImage(image);
            foreach (var point in points)
            {
                Point point1 = point.FirstImage;
                Point point2 = point.SecondImage;

                drawPointOnBitmap(image, (int)point1.X, (int)point1.Y, Color.Red);
                drawPointOnBitmap(image, (int)point2.X + offset, (int)point2.Y, Color.Red);
            }

            foreach (var point in narrowedPoints)
            {
                Point point1 = point.FirstImage;
                Point point2 = point.SecondImage;
                Pen pen = new Pen(Color.Green, 2);

                drawPointOnBitmap(image, (int)point1.X, (int)point1.Y, Color.Blue);
                drawPointOnBitmap(image, (int)point2.X + offset, (int)point2.Y, Color.Blue);
                graphics.DrawLine(pen, point1.X, point1.Y, point2.X + offset, point2.Y);
            }

            foreach (var point in keyPoints)
            {
                Point point1 = point.FirstImage;
                Point point2 = point.SecondImage;
                Pen pen = new Pen(penColor, 2);

                drawPointOnBitmap(image, (int)point1.X, (int)point1.Y,Color.Blue);
                drawPointOnBitmap(image, (int)point2.X + offset, (int)point2.Y, Color.Blue);
                graphics.DrawLine(pen, point1.X, point1.Y, point2.X + offset, point2.Y);
            }            
        }

        public static Bitmap generateNewImage(Bitmap image1, Bitmap image2, List<Pair> keyPoints, Color penColor, List<Pair> points, List<Pair> narrowedPoints)
        {
            var other = keyPoints;

            Bitmap newImage = mergeImages(image1, image2);
            DrawKeyPoints(newImage, other, penColor, points, narrowedPoints, image1.Width);
            return newImage;
        }

        public static void drawPointOnBitmap(Bitmap bitmap, int x, int y, Color color)
        {
            for (int mod_x = -3; mod_x <= 3; mod_x++)
            {
                for (int mod_y = -3; mod_y <= 3; mod_y++)
                {
                    int newX = x + mod_x;
                    int newY = y + mod_y;

                    if (newX >= 0 && newX < bitmap.Width && newY >= 0 && newY < bitmap.Height)
                    {
                        bitmap.SetPixel(newX, newY, color);
                    }
                }
            }
        }

        public static BitmapImage imageFromBitmap(Bitmap bitmap)
        {
            BitmapImage bitmapImage = new BitmapImage();

            //bitmap.Save(@"E:\Studia\VI semestr\SI\lab\lab4\result.jpg");

            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
            }

            return bitmapImage;
        }


    }
}
