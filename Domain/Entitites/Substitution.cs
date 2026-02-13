using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entitites
{
    public class Substitution : Entity
    {
        public Product OriginalProduct { get; private set; }
        public Product SubstituteProduct { get; private set; }
        public string Reason { get; private set; }


        public Substitution(Product original, Product substitute, string reason)
        {
            Id = Guid.NewGuid();
            OriginalProduct = original;
            SubstituteProduct = substitute;
            Reason = reason;
        }
    }
}
