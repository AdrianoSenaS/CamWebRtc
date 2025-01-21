
const addPlayerBtn = document.getElementById("btnTransmissao")
const videoPlayer = document.getElementById("PlayerWeb")
const selecionarcameras = document.getElementById("selecionarcameras")
const url = "/api/Cam";
const socket = io('http://localhost:5000');
const camerasApi = fetch(url);
let peerConnections = {};
let ListarCameras;

camerasApi.then(json => {
    json.json()
        .then(cameras => {
            selecionarcameras.innerHTML = '<option>Selecione</option>';
            cameras.map(camera => {
                const option = document.createElement('option')
                option.value = camera.connectionID;
                option.text = camera.name
                selecionarcameras.appendChild(option)
            })
        })
})


const playrLocal = async (deviceId)=>{
    const constraints = {
        video: {
            deviceId: { exact: deviceId }
        }
    };
    const stream = await navigator.mediaDevices.getUserMedia(constraints);
    window.stream = stream;
    videoPlayer.srcObject = stream; 
}
    

//Inicia a transmissão
async function startStreaming(clientId, deviceId) {
    console.log(deviceId)
    const constraints = {
        video: {
            deviceId: { exact: deviceId }
        }
    };

    try {
        const stream = await navigator.mediaDevices.getUserMedia(constraints);
        window.stream = stream;
        videoPlayer.srcObject = stream; 
         // Adicionar tracks ao peerConnection
         const peerConnection = new RTCPeerConnection({
            iceServers: [{ urls: 'stun:stun.l.google.com:19302' }],
        });
        
         stream.getTracks().forEach((track) => {
            peerConnection.addTrack(track, stream);
        });

        // Gerenciar candidatos ICE
        peerConnection.onicecandidate = (event) => {
            if (event.candidate) {
                socket.emit('ice-candidate', { to: clientId, candidate: event.candidate });
            }
        };

        // Criar e enviar oferta
        const offer = await peerConnection.createOffer();
        await peerConnection.setLocalDescription(offer);

        // Verificar se o cliente ainda está conectado antes de emitir a oferta
        if (socket.connected) {
            socket.emit('offer', { to: clientId, offer });
        } else {
            console.error(`Cliente ${clientId} não está mais conectado.`);
        }

        // Armazene a conexão
        peerConnections[clientId] = peerConnection;
    
    } catch (error) {
        console.error('Erro ao acessar o dispositivo de vídeo:', error);
    }
}

// Ao receber uma nova solicitação de câmera
socket.on('new-client', async ({ clientId, cameraId }) => {
    console.log("novo cliente "+clientId)
     startStreaming(clientId, cameraId);  
});

// Adiciona respostas do cliente ao transmissor
socket.on('answer', ({ from, answer }) => {
    console.log("answer "+from +" "+answer)
   
    peerConnections[from]?.setRemoteDescription(new RTCSessionDescription(answer));
});

// Adiciona candidatos ICE do cliente
socket.on('ice-candidate', ({ from, candidate }) => {
    console.log("ice-candidate "+from +" "+candidate)
    peerConnections[from]?.addIceCandidate(new RTCIceCandidate(candidate));
});

playrLocal()



