using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIImageAnalisys
{
    public partial class Form1 : Form
    {
        public Form1()
        {

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var im = new ImageModel();
            string[] lines = System.IO.File.ReadAllLines(@"C:\extract_features\7.PNG.harhes.sift");
            im.ImageA = PointReader.ReadPoints(lines);
            string[] lines2 = System.IO.File.ReadAllLines(@"C:\extract_features\4.PNG.harhes.sift");
            im.ImageB = PointReader.ReadPoints(lines);

            var pairs = im.MatchPairs();


            var nhSize = (int) (pairs.Count * 0.02);
            var pointHelper = new PointHelper();

           var coherentPairs = new List<Pair>();
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
                var closesNeibhoursA = allNeibhoursA.OrderBy(x=> x.Item2).Take(nhSize);
                var closesNeibhoursB = allNeibhoursB.OrderBy(x=> x.Item2).Take(nhSize);

                if ((float)pointHelper.GetCohesion(closesNeibhoursA, closesNeibhoursB)/(float)nhSize >= 0.5)
                {
                    coherentPairs.Add(pairs[i]);
                }
            }
        }
    }
}
