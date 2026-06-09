namespace GaucheOuDroiteBackEnd.Models
{
    public class User
    {
        public int Id { get; set; } // The Id is not required because the DataBase will generate it, when creating a new user.

        public required string Username { get; set; }

        public required string PasswordHash { get; set; }
    }
}