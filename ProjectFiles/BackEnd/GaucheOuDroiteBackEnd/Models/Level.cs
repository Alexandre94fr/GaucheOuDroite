namespace GaucheOuDroiteBackEnd.Models
{
    public class Level
    {
        public required int Id { get; set; }

        public required string Name { get; set; }

        public required int Difficulty { get; set; }

        public required bool IsInfinite { get; set; }

        public required string ResponseSequence { get; set; }

        public required int Star1MinimumScore { get; set; }

        public required int Star2MinimumScore { get; set; }

        public required int Star3MinimumScore { get; set; }
    }
}