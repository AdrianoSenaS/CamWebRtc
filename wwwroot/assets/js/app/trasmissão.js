const addPlayerBtn = document.getElementById("btnTransmissao")
const videoPlayer = document.getElementById("PlayerWeb")
const selecionarcameras = document.getElementById("selecionarcameras")
const socket = io('https://node-media-server-servernode.fpumgv.easypanel.host');
let peerConnections = {};

//Configurar servidor TURN, para funcionar onde o firewall do roteador ou provedores bloqueiam conexões diretas
const configuration = {
    iceServers: [
        { urls: "stun:stun.l.google.com:19302" }//usando iceServers do google
    ]
};

//Captura e ativa a webcam
navigator.mediaDevices.getUserMedia({ video: true, audio: true });

//Inicia a transmissão
async function startStreaming(clientId, deviceID) {

    //pega o id da camera 
    const constraints = {
        video: {
            deviceId: { exact: deviceID },
        },
        audio: true
    };

    //Adicionar tracks ao peerConnection
    const stream = await navigator.mediaDevices.getUserMedia(constraints);
    window.stream = stream;
    videoPlayer.srcObject = stream;
    console.log(deviceID)
    const peerConnection = new RTCPeerConnection(configuration);


    stream.getTracks().forEach((track) => {
        peerConnection.addTrack(track, stream);
    });

    //Gerenciar candidatos ICE
    peerConnection.onicecandidate = (event) => {
        if (event.candidate) {
            socket.emit('ice-candidate', { to: clientId, candidate: event.candidate });
        }
    };

    //Criar e enviar oferta
    const offer = await peerConnection.createOffer();
    await peerConnection.setLocalDescription(offer);

    //Verificar se o cliente ainda está conectado antes de emitir a oferta
    if (socket.connected) {
        socket.emit('offer', { to: clientId, offer });
    } else {
        console.error(`Cliente ${clientId} não está mais conectado.`);
    }

    //Armazene a conexão
    peerConnections[clientId] = peerConnection;

    
}

//Ao receber uma nova solicitação de câmera
socket.on('new-client', async ({ clientId, cameraId }) => {
    console.log("novo cliente " + clientId)
    navigator.mediaDevices.enumerateDevices()
        .then(devices => {
            devices.forEach(device => {
                if (device.kind === "videoinput") {

                    if (cameraId === device.label) {
                        console.log(`Câmera encontrada: ${device.label}, ID: ${device.deviceId}`);
                        startStreaming(clientId, device.deviceId);
                    } else {
                        console.log("Camera não encontrada")
                    }

                }
            });
        })
        .catch(error => console.error("Erro ao listar dispositivos:", error));
   
});

//Adiciona respostas do cliente ao transmissor
socket.on('answer', ({ from, answer }) => {
    console.log("answer " + from + " " + answer)

    peerConnections[from]?.setRemoteDescription(new RTCSessionDescription(answer));
});

//Adiciona candidatos ICE do cliente
socket.on('ice-candidate', ({ from, candidate }) => {
    console.log("ice-candidate " + from + " " + candidate)
    peerConnections[from]?.addIceCandidate(new RTCIceCandidate(candidate));
});




