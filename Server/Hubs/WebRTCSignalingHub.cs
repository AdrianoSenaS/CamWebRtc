using Microsoft.AspNetCore.SignalR;

namespace CamWebRtc.Server.Hubs
{
    public class WebRTCSignalingHub : Hub
    {
        public async Task SendOffer(string offer, string cameraId) => await Clients.Client(cameraId).SendAsync("ReceiveOffer", offer);

        public async Task SendAnswer(string answer, string cameraId) => await Clients.Client(cameraId).SendAsync("ReceiveAnswer", answer);

        public async Task SendIceCandidate(string candidate, string cameraId) => await Clients.Client(cameraId).SendAsync("ReceiveIceCandidate", candidate);

        
    }
}
