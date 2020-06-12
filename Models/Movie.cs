namespace CovidMovieMadness___Tenta.Models
{
    public class Movie
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public int Year { get; set; }

        public virtual Post Post { get; set; }
    }
}