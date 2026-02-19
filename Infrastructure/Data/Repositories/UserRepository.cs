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
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Users
            .Include(u => u.Allergies)
            .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        {
            await _context.Users.AddAsync(user, cancellationToken);
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public void Remove(User user)
        {
            _context.Users.Remove(user);
        }

        public async Task<User?> GetByIdWithAllergiesAsync(Guid id)
        {
            return await _context.Users
                .Include(u => u.Allergies)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByIdWithWeeklyPlanAsync(Guid id)
        {
            return await _context.Users
                .Include(u => u.WeeklyPlan)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task ClearWeeklyPlanAsync(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return;

            var progresses = await _context.Progresses
                .Where(p => p.UserId == userId)
                .ToListAsync();

            if (progresses.Any())
                _context.Progresses.RemoveRange(progresses);


            user.WeeklyPlanId = null;
            user.CurrentPlanDay = 0;

            
            await _context.SaveChangesAsync();
        }
    }
}
