using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class UserService
    {
        public readonly RoleService _roleService = new RoleService();

        public readonly List<DemoUser> Users = new List<DemoUser>
        {
            new DemoUser(1, "example", "example@localhost", "example", "Curly Fries", AuthType.Database),
            new DemoUser(2, "admin", "admin@localhost", "admin", "Mr. Admin", AuthType.Database),
            new DemoUser(3, "readonly", "readonly@localhost", "readonly", "Mrs. Read Only", AuthType.Database),
            new DemoUser(4, "r.rodemoyer", "r.rodemoyer@gmail.com", null, "Rodey", AuthType.SingleSignOn),
            new DemoUser(4, "r.rodemoyer@gmail.com", "r.rodemoyer@gmail.com", null, "Rodey", AuthType.SingleSignOn),
        };

        public DemoUser GetByEmail(string email)
        {
            return Users.FirstOrDefault(x => x.Email == email);
        }

        public DemoUser GetByUsername(string username)
        {
            return Users.FirstOrDefault(x => x.Username == username);
        }

        public DemoUser FindByCredentials(string contextUserName, string contextPassword)
        {
            return Users.FirstOrDefault(x => x.Username == contextUserName && x.Password == contextPassword);
        }

        public ClaimsIdentity GenerateUserIdentityAsync(DemoUser user, string authenticationType)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("authentication_type", authenticationType),
            };

            var identity = new ClaimsIdentity(claims, authenticationType);

            // Add roles into claims
            var roles = _roleService.GetByUserId(user.Id);
            if (roles.Any())
            {
                var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r.Name));
                identity.AddClaims(roleClaims);
            }

            return identity;
        }
    }
}