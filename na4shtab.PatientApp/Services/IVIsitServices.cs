using System.Collections.Generic;
using System.Threading.Tasks;
using na4shtab.PatientApp.Models;

namespace na4shtab.PatientApp.Services
{
    public interface IVisitService
    {
        Task<List<Visit>> GetAllAsync(int? patientId = null);
        Task<Visit> GetByIdAsync(int id);
        Task AddAsync(Visit visit);
        Task UpdateAsync(Visit visit);
        Task DeleteAsync(int id);
    }
}