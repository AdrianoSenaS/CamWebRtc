using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

public class SignalingHub : Hub
{
    // Renomeado para evitar conflito com SignalR Clients
    private static readonly ConcurrentDictionary<string, string> ClientConnections = new();

    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"Cliente conectado: {Context.ConnectionId}");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine($"Cliente desconectado: {Context.ConnectionId}");
        if (ClientConnections.TryRemove(Context.ConnectionId, out var cameraId))
        {
            // Notifica os outros clientes sobre a desconexão
            await Clients.AllExcept(Context.ConnectionId).SendAsync("ClientDisconnected", new { ClientId = Context.ConnectionId });
        }

        await base.OnDisconnectedAsync(exception);
    }

    // Quando um receptor solicita uma câmera
    public async Task RequestCamera(string cameraId)
    {
        ClientConnections[Context.ConnectionId] = cameraId;
        Console.WriteLine($"Cliente {Context.ConnectionId} solicitou câmera {cameraId}");

        // Notifica os outros clientes (por exemplo, transmissores)
        await Clients.Others.SendAsync("NewClient", new { ClientId = Context.ConnectionId, CameraId = cameraId });
    }

    // Envia oferta do transmissor para o receptor
    public async Task SendOffer(string clientId, object offer)
    {
        Console.WriteLine("Enviar offer para" + clientId);
        await Clients.Clients(clientId).SendAsync("ReceiveOffer", new { From = Context.ConnectionId, Offer = offer });
    }


    // Envia resposta do receptor para o transmissor
    public async Task SendAnswer(string clientId, object answer)
    {
        await Clients.Client(clientId).SendAsync("Answer", new { From = Context.ConnectionId, Answer = answer });
    }

    // Reencaminha candidatos ICE
    public async Task SendIceCandidate(string clientId, object candidate)
    {
        await Clients.Client(clientId).SendAsync("IceCandidate", new { From = Context.ConnectionId, Candidate = candidate });
    }
}
