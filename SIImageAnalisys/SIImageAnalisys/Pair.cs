namespace SIImageAnalisys
{
    public class Pair
    {
        public Point FirstImage { get; set; }
        public Point SecondImage { get; set; }
        public float D { get; set; }

        public bool MatchPair(Pair pair)
        {
            return FirstImage.Equals(pair.FirstImage) && SecondImage.Equals(pair.SecondImage);
        }
    }
}