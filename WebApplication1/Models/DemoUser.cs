namespace WebApplication1.Models
{
    public class DemoUser
    {
        public int Id { get; private set; }

        public string Username { get; private set; }

        public string Email { get; private set; }

        public string Password { get; private set; }

        public string Name { get; private set; }

        public AuthType AuthType { get; private set; }

        public DemoUser()
        {

        }

        public DemoUser(int id, string username, string email, string password, string name, AuthType authType)
        {
            Id = id;
            Username = username;
            Email = email;
            Password = password;
            Name = name;
            AuthType = authType;
        }
    }
}