namespace GaucheOuDroiteBackEnd.Models
{
    // TODO: Rename LevelProgression
    // TODO: Create a real UserProgression (like UserProgressionDTO)
    public class UserProgression
    {
        public int Id { get; set; } // The Id is not required because the DataBase will generate it, when creating a new UserProgression.

        public required int UserId { get; set; }

        public required int LevelId { get; set; }

        public required bool IsUnlocked { get; set; }

        public required int BestScore { get; set; }
    }
}