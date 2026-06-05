namespace GaucheOuDroiteBackEnd.Models
{
    public class LevelResponseTimeStep
    {
        public required int Id { get; set; }

        public required int LevelId { get; set; }

        public required float TriggerTimeInSeconds { get; set; }

        public required float MaxResponseTimeInSeconds { get; set; }
    }
}