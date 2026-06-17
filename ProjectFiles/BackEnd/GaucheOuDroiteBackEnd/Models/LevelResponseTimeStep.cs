namespace GaucheOuDroiteBackEnd.Models
{
    public class LevelResponseTimeStep
    {
        public int Id { get; set; } // The Id is not required because the DataBase will generate it, when creating a new LevelResponseTimeStep.

        public required int LevelId { get; set; }

        public required float TriggerTimeInSeconds { get; set; }

        public required float MaxResponseTimeInSeconds { get; set; }
    }
}