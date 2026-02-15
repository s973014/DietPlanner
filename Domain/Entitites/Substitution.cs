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
        public Meal OriginalMeal { get; private set; }
        public Meal SubstituteMeal { get; private set; }


        public Guid OriginalMealId { get; private set; }
        public Guid SubstituteMealId { get; private set; }

        public string Reason { get; private set; }

        private Substitution() { }

        public Substitution(Meal original, Meal substitute, string reason)
        {
            Id = Guid.NewGuid();

            OriginalMeal = original;
            SubstituteMeal = substitute;

            OriginalMealId = original.Id;
            SubstituteMealId = substitute.Id;

            Reason = reason;
        }

        public void Update(Guid substituteMealId, string reason)
        {
            SubstituteMealId = substituteMealId;
            Reason = reason;
        }
    }
}
