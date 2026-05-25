using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Services.TreatmentRequests
{
    public interface IDoctorRemovalBackgroundService
    {
        Task RemoveExpiredDoctors();
    }
}
