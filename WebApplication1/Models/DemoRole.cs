using System;

namespace WebApplication1.Models
{
    public class DemoRole
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public DemoRole(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}