using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class RoleService
    {
        public readonly List<Tuple<int, DemoRole>> Roles = new List<Tuple<int, DemoRole>>
        {
            new Tuple<int, DemoRole>(1, new DemoRole(Guid.Parse("8C55F3A1-0915-46BD-BB3F-687860AC2144"), "default_role")),
            new Tuple<int, DemoRole>(1, new DemoRole(Guid.Parse("9A3F1D67-F659-4AA9-82DD-69C40154A192"), "power_role")),
            new Tuple<int, DemoRole>(2, new DemoRole(Guid.Parse("66E0E122-58C0-4730-B1D7-A50CC9BD8429"), "admin")),
            new Tuple<int, DemoRole>(3, new DemoRole(Guid.Parse("06BB01D3-D243-4418-9380-868177741973"), "readonly")),
            new Tuple<int, DemoRole>(4, new DemoRole(Guid.Parse("C982037E-2658-46BF-B813-F7D2E0DC85F4"), "admin")),
        };

        public IEnumerable<DemoRole> GetByUserId(int userId)
        {
            return Roles.Where(x => x.Item1 == userId).Select(x => x.Item2);
        }
    }
}