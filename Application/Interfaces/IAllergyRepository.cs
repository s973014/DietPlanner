using Domain.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAllergyRepository
    {
        Task<List<Allergy>> GetAllAsync();
        Task<Allergy?> GetByIdAsync(Guid id);
        Task AddAsync(Allergy allergy);
        void Update(Allergy allergy);
        void Delete(Allergy allergy);
    }
}
