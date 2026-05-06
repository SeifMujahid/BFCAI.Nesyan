using BFCAI.Nesyan.Application.Abstraction.Models.IoT;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Services.IoT
{
    public interface ITelemetryNotifier
    {
        Task NotifyTelemetryAsync(TelemetryRequestDto dto);
    }
}
