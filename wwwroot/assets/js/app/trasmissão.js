const url = "/api/IceServes";
const addPlayerBtn = document.getElementById("btnTransmissao");
const videoPlayer = document.getElementById("PlayerWeb");
const selecionarcameras = document.getElementById("selecionarcameras");
let peerConnections = {};
const token = Cookies();
let username;
let credential;
let urlStun = [];
let urlTurn = [];
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/signaling") // URL do servidor SignalR
    .build();

// Captura e ativa a webcam
navigator.mediaDevices.getUserMedia({ video: true, audio: true });

GetApi(url, "GET", token).then(response => {
    response.forEach(e => {
        credential = e.credential;
        username = e.username;
        e.urlsStun.forEach(i => {
            urlStun.push(i.urls);
        });
        e.urlsTurn.forEach(a => {
            urlTurn.push(a.urls);
        });
    });
})
// Inicia a transmissão
async function startStreaming(clientId, deviceID) {
    const constraints = {
        video: {
            deviceId: { exact: deviceID },
        },
        audio: true,
    };
    // Adicionar tracks ao peerConnection
    const stream = await navigator.mediaDevices.getUserMedia(constraints);
    window.stream = stream;
    videoPlayer.srcObject = stream;
    const response = await fetch("https://webrtcadriano.metered.live/api/v1/turn/credentials?apiKey=02bb2417b9bfdf3ac91c6dedca16b17e17e5");
    const iceServers = await response.json();
    const peerConnection =  new RTCPeerConnection({iceServers: iceServers});

    stream.getTracks().forEach((track) => {
        peerConnection.addTrack(track, stream);
    });

    // Gerenciar candidatos ICE
    peerConnection.onicecandidate = (event) => {
        if (event.candidate) {
            connection.invoke("SendIceCandidate", clientId, event.candidate);
            console.log("SendIceCandidate " + JSON.stringify(event.candidate))
        }
    };

    // Criar e enviar oferta
    const offer = await peerConnection.createOffer();
    await peerConnection.setLocalDescription(offer);

    console.log("SendOffer ClientId " + clientId)
    console.log("SendOffer " + JSON.stringify(offer))
    connection.invoke("SendOffer", clientId, offer);
    // Armazene a conexão
    peerConnections[clientId] = peerConnection;
}

// Conexão ao SignalR
connection.on("NewClient", async ({ clientId, cameraId }) => {
    console.log("Novo cliente conectado: " + clientId);

    navigator.mediaDevices.enumerateDevices()
        .then(devices => {
            devices.forEach(device => {
                if (device.kind === "videoinput") {
                    if (cameraId === device.label) {
                        console.log(`Câmera encontrada: ${device.label}, ID: ${device.deviceId}`);
                        startStreaming(clientId, device.deviceId);
                    } else {
                        console.log("Câmera não encontrada");
                    }
                }
            });
        })
        .catch(error => console.error("Erro ao listar dispositivos:", error));
});

// Adicionar respostas do cliente ao transmissor
connection.on("Answer", ({ from, answer }) => {
    console.log("Resposta recebida de: " + from);
    console.log("Answer " + JSON.stringify(answer))
    peerConnections[from]?.setRemoteDescription(new RTCSessionDescription(answer));
});

// Adicionar candidatos ICE do cliente
connection.on("IceCandidate", ({ from, candidate }) => {
    console.log("Candidato ICE recebido de: " + from);
    console.log("IceCandidate " + JSON.stringify(candidate))
    peerConnections[from]?.addIceCandidate(new RTCIceCandidate(candidate));
});

// Gerenciar desconexões
connection.on("ClientDisconnected", ({ clientId }) => {
    console.log(`Cliente desconectado: ${clientId}`);
    delete peerConnections[clientId];
});

// Inicia a conexão com o SignalR
connection.start()
    .then(() => {
        console.log("Conectado ao servidor SignalR.");
    })
    .catch(error => {
        console.error("Erro ao conectar ao SignalR:", error);
    });
