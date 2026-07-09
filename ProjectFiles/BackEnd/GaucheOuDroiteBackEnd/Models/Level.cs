namespace GaucheOuDroiteBackEnd.Models
{
    public class Level
    {
        public int Id { get; set; } // The Id is not required because the DataBase will generate it, when creating a new Level.

        public required string Name { get; set; }

        public required int Difficulty { get; set; }

        public required bool IsInfinite { get; set; }

        public required string ResponseSequence { get; set; }

        public required int Star1MinimumScore { get; set; }

        public required int Star2MinimumScore { get; set; }

        public required int Star3MinimumScore { get; set; }
    }
}