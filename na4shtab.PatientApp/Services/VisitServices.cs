using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using na4shtab.PatientApp.Data;
using na4shtab.PatientApp.Models;

namespace na4shtab.PatientApp.Services
{
    public class VisitService : IVisitService
    {
        private readonly PatientDbContext _db = new();

        public async Task<List<Visit>> GetAllAsync(int? patientId = null)
        {
            var q = _db.Visits
                       .Include(v => v.Patient)
                       .Include(v => v.SelectedProcedures)
                       .AsQueryable();

            if (patientId.HasValue)
                q = q.Where(v => v.PatientId == patientId);

            return await q.OrderByDescending(v => v.Date).ToListAsync();
        }

        public async Task<Visit> GetByIdAsync(int id)
        {
            return await _db.Visits
                            .Include(v => v.Patient)
                            .Include(v => v.SelectedProcedures)
                            .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task AddAsync(Visit visit)
        {
            if (visit.TotalCost <= 0)
            {
                visit.TotalCost = visit.SelectedProcedures
                                        .Sum(p => p.Cost);
            }

            _db.Visits.Add(visit);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Visit visit)
        {
            if (visit.TotalCost <= 0)
            {
                visit.TotalCost = visit.SelectedProcedures
                                        .Sum(p => p.Cost);
            }

            _db.Visits.Update(visit);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var v = await _db.Visits.FindAsync(id);
            if (v != null)
            {
                _db.Visits.Remove(v);
                await _db.SaveChangesAsync();
            }
        }
    }
}
