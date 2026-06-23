using BFCAI.Nesyan.Application.Abstraction.Models.IoT;
using BFCAI.Nesyan.Application.Abstraction.Services.IoT;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.APIs.Hubs
{
    public class SignalRTelemetryNotifier : ITelemetryNotifier
    {
        private readonly IHubContext<TelemetryHub> _hubContext;

        public SignalRTelemetryNotifier(IHubContext<TelemetryHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyTelemetryAsync(TelemetryRequestDto dto)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveTelemetry", dto);
        }
    }
}
