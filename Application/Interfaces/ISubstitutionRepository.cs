using Domain.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISubstitutionRepository
    {
        public Task<List<SubstitutionListVm>> GetAllAsync();
        Task<Substitution?> GetByIdAsync(Guid id);
        Task AddAsync(Substitution substitution);
        void Remove(Substitution substitution);
        Task<Substitution?> GetByOriginalMealIdAsync(Guid originalMealId);
    }

    public class SubstitutionListVm
    {
        public Guid Id { get; set; }
        public string OriginalMeal { get; set; }
        public string SubstituteMeal { get; set; }
        public string Reason { get; set; }
    }
}
