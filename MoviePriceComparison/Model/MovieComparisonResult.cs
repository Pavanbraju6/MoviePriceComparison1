namespace MoviePriceComparison.Model
{
    public class MovieComparisonResult
    {
        public string Title { get; set; }
        public string Poster { get; set; }
        public string CheapestProvider { get; set; }
        public decimal CheapestPrice { get; set; }
    }

}
