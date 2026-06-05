namespace GaucheOuDroiteBackEnd.Models
{
    public class UserProgression
    {
        public required int Id { get; set; }

        public required int UserId { get; set; }

        public required int LevelId { get; set; }

        public required bool IsUnlocked { get; set; }

        public required int BestScore { get; set; }
    }
}