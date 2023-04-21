namespace CrossTeamTestSuite.Data.Repository.Users;

public record User(string Email, string Password, string FirstName, string LastName, DateOnly BirthDate);
