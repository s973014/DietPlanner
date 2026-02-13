using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entitites
{
    public class Allergy : Entity
    {
        public string Name { get; private set; }

        public Allergy(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }
    }
}
