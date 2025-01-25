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


// Configurar servidor TURN para funcionar com firewalls
const IceServesConfiguration = async () => {
    const response = await GetApi(url, "GET", token);
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
    const configuration = {
        iceServers: [
            { urls: urlStun },
            { username: username, credential: credential, urls: urlTurn }
        ]
    };

    return configuration;
};

// Captura e ativa a webcam
navigator.mediaDevices.getUserMedia({ video: true, audio: true });

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

    const configuration = IceServesConfiguration();
    const peerConnection = new RTCPeerConnection(configuration);

    stream.getTracks().forEach((track) => {
        peerConnection.addTrack(track, stream);
    });

    // Gerenciar candidatos ICE
    peerConnection.onicecandidate = (event) => {
        if (event.candidate) {

            const cadidateJson = JSON.stringify(event.candidate)
            connection.invoke("SendIceCandidate", clientId, cadidateJson);
            console.log("SendIceCandidate " + cadidateJson)
        }
    };

    // Criar e enviar oferta
    const offer = await peerConnection.createOffer();
    await peerConnection.setLocalDescription(offer);

    const json = JSON.stringify(offer);
    console.log("SendOffer ClientId " + clientId)
    console.log("SendOffer " + json)
    connection.invoke("SendOffer", clientId, json);
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
    const answerJson = JSON.parse(answer)
    peerConnections[from]?.setRemoteDescription(new RTCSessionDescription(answerJson));
});

// Adicionar candidatos ICE do cliente
connection.on("IceCandidate", ({ from, candidate }) => {
    console.log("Candidato ICE recebido de: " + from);
    const candidateJson = JSON.parse(candidate)
    peerConnections[from]?.addIceCandidate(new RTCIceCandidate(candidateJson));
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
