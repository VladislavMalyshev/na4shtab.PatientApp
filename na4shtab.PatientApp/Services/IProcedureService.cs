using System.Collections.Generic;
using System.Threading.Tasks;
using na4shtab.PatientApp.Models;

namespace na4shtab.PatientApp.Services
{
    public interface IProcedureService
    {
        Task<List<Procedure>> GetAllAsync(string filter = null);

        Task<Procedure> GetByIdAsync(int id);

        Task AddAsync(Procedure procedure);

        Task UpdateAsync(Procedure procedure);

        Task DeleteAsync(int id);
    }
}