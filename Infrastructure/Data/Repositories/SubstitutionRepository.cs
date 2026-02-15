using Application.Interfaces;
using Domain.Entitites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class SubstitutionRepository : ISubstitutionRepository
    {
        private readonly AppDbContext _context;

        public SubstitutionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SubstitutionListVm>> GetAllAsync()
        {
            return await _context.Substitutions
                .Select(s => new SubstitutionListVm
                {
                    Id = s.Id,
                    OriginalMeal = s.OriginalMeal.Name,  
                    SubstituteMeal = s.SubstituteMeal.Name,
                    Reason = s.Reason
                })
                .ToListAsync();
        }

        public async Task<Substitution?> GetByIdAsync(Guid id)
            => await _context.Substitutions
                .Include(x => x.OriginalMeal)
                .Include(x => x.SubstituteMeal)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task AddAsync(Substitution substitution)
            => await _context.Substitutions.AddAsync(substitution);

        public void Remove(Substitution substitution)
            => _context.Substitutions.Remove(substitution);
    }

}
