using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using na4shtab.PatientApp.Data;
using na4shtab.PatientApp.Models;

namespace na4shtab.PatientApp.Services
{
    public class PatientService : IPatientService
    {
        private readonly PatientDbContext _db = new();

        public async Task<List<Patient>> GetAllAsync(string filter = null)
        {
            var q = _db.Patients.AsQueryable();
            if (!string.IsNullOrWhiteSpace(filter))
                q = q.Where(p => p.FullName.Contains(filter));
            return await q.ToListAsync();
        }

        public async Task<Patient> GetByIdAsync(int id)
            => await _db.Patients
                .Include(p => p.Visits)
                .FirstOrDefaultAsync(p => p.Id == id);

        public async Task AddAsync(Patient patient)
        {
            _db.Patients.Add(patient);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Patient patient)
        {
            _db.Patients.Update(patient);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var p = await _db.Patients.FindAsync(id);
            if (p != null)
            {
                _db.Patients.Remove(p);
                await _db.SaveChangesAsync();
            }
        }
    }
}