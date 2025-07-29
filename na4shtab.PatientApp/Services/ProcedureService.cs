using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using na4shtab.PatientApp.Data;
using na4shtab.PatientApp.Models;

namespace na4shtab.PatientApp.Services
{
    public class ProcedureService : IProcedureService
    {
        private readonly PatientDbContext _db = new();

        public async Task<List<Procedure>> GetAllAsync(string filter = null)
        {
            var query = _db.Procedures.AsQueryable();
            if (!string.IsNullOrWhiteSpace(filter))
                query = query.Where(p => p.Name.Contains(filter));
            return await query.ToListAsync();
        }

        public async Task<Procedure> GetByIdAsync(int id)
            => await _db.Procedures.FindAsync(id);

        public async Task AddAsync(Procedure procedure)
        {
            _db.Procedures.Add(procedure);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Procedure procedure)
        {
            _db.Procedures.Update(procedure);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var proc = await _db.Procedures.FindAsync(id);
            if (proc != null)
            {
                _db.Procedures.Remove(proc);
                await _db.SaveChangesAsync();
            }
        }
    }
}