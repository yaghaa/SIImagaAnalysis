using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SIImageAnalisys
{
    public partial class Form1 : Form
    {
        private BitmapImage firstImage;
        private Bitmap firstBitmap;
        private Bitmap firstOriginal;

        private BitmapImage secondImage;
        private Bitmap secondBitmap;
        private Bitmap secondOriginal;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var im = new ImageModel();
            //string[] lines = System.IO.File.ReadAllLines(@"C:\1\cat8.PNG.harhes.sift");
            //string[] lines = System.IO.File.ReadAllLines(@"C:\1\las1.PNG.harhes.sift");
            string[] lines = System.IO.File.ReadAllLines(@"C:\1\4.PNG.harhes.sift");
            im.ImageA = PointReader.ReadPoints(lines);
            //string[] lines2 = System.IO.File.ReadAllLines(@"C:\1\cat9.PNG.harhes.sift");
            //string[] lines2 = System.IO.File.ReadAllLines(@"C:\1\las2.PNG.harhes.sift");
            string[] lines2 = System.IO.File.ReadAllLines(@"C:\1\7.PNG.harhes.sift");
            im.ImageB = PointReader.ReadPoints(lines2);

            var pairs = im.MatchPairs();

            var keyPairs = NarrowDownPairs(pairs);

            var ransac = new RANSAC();
            //loadImage(out firstImage, out firstBitmap, out firstOriginal, @"C:\1\cat8.png");
            //loadImage(out secondImage, out secondBitmap, out secondOriginal, @"C:\1\cat9.png");
            //loadImage(out firstImage, out firstBitmap, out firstOriginal, @"C:\1\las1.png");
            //loadImage(out secondImage, out secondBitmap, out secondOriginal, @"C:\1\las2.png");
            loadImage(out firstImage, out firstBitmap, out firstOriginal, @"C:\1\1.png");
            loadImage(out secondImage, out secondBitmap, out secondOriginal, @"C:\1\2.png");
            var reducedKeyPointsPairs = ransac.transform(keyPairs, firstImage.PixelWidth, 10, 50);
            Bitmap result = Drawing.generateNewImage(firstOriginal, secondOriginal, reducedKeyPointsPairs, System.Drawing.Color.Yellow , pairs, keyPairs);
            var resultImage =  Drawing.imageFromBitmap(result);
            pictureBox1.Image = GetBitmap(resultImage);

           
        }

        private List<Pair> NarrowDownPairs(List<Pair> pairs)
        {
            var coherentPairs = new List<Pair>();
            var nhSize = (int)(pairs.Count * 0.02);
            var pointHelper = new PointHelper();


            for (int i = 0; i < pairs.Count; i++)
            {
                List<Tuple<Pair, float>> allNeibhoursA = new List<Tuple<Pair, float>>();
                List<Tuple<Pair, float>> allNeibhoursB = new List<Tuple<Pair, float>>();
                for (int j = 0; j < pairs.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    var distanceA = pointHelper.GetDistance(pairs[i].FirstImage, pairs[j].FirstImage);
                    allNeibhoursA.Add(new Tuple<Pair, float>(pairs[j], distanceA));

                    var distanceB = pointHelper.GetDistance(pairs[i].SecondImage, pairs[j].SecondImage);
                    allNeibhoursB.Add(new Tuple<Pair, float>(pairs[j], distanceB));
                }
                var closesNeibhoursA = allNeibhoursA.OrderBy(x => x.Item2).Take(nhSize);
                var closesNeibhoursB = allNeibhoursB.OrderBy(x => x.Item2).Take(nhSize);

                if ((float)pointHelper.GetCohesion(closesNeibhoursA, closesNeibhoursB) / (float)nhSize >= 0.5)
                {
                    coherentPairs.Add(pairs[i]);
                }
            }

            return coherentPairs;
        }

        private ImageSource loadImage(out BitmapImage image, out Bitmap bitmap, out Bitmap bitmapOriginal,string url)
        {            
            image = new BitmapImage(new Uri(url));            
            bitmap = GetBitmap(image);
            bitmapOriginal = GetBitmap(image);
            return image;           
        }

        private Bitmap GetBitmap(BitmapImage image)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(image));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return bitmap;
            }
        }
    }
}
