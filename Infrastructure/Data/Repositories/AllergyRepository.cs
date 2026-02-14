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
    public class AllergyRepository : IAllergyRepository
    {
        private readonly AppDbContext _context;

        public AllergyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Allergy>> GetAllAsync()
        {
            return await _context.Allergies.ToListAsync();
        }

        public async Task<Allergy?> GetByIdAsync(Guid id)
        {
            return await _context.Allergies.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddAsync(Allergy allergy)
        {
            await _context.Allergies.AddAsync(allergy);
        }

        public void Update(Allergy allergy)
        {
            _context.Allergies.Update(allergy);
        }

        public void Delete(Allergy allergy)
        {
            _context.Allergies.Remove(allergy);
        }
    }
}
