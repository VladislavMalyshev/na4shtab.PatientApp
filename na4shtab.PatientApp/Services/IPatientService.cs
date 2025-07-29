using System.Collections.Generic;
using System.Threading.Tasks;
using na4shtab.PatientApp.Models;

namespace na4shtab.PatientApp.Services
{
    public interface IPatientService
    {
        Task<List<Patient>> GetAllAsync(string filter = null);
        Task<Patient> GetByIdAsync(int id);
        Task AddAsync(Patient patient);
        Task UpdateAsync(Patient patient);
        Task DeleteAsync(int id);
    }
}