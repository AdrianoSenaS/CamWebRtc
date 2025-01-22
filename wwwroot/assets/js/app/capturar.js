const url = "https://node-media-server-api-web-webrtc.fpumgv.easypanel.host/api/cam";
const socket = io('https://node-media-server-servernode.fpumgv.easypanel.host');
const ListarCameras = document.getElementById("ListarCameras")
const videoPlayer = document.getElementById("videolocal")
const addPlayerBtn = document.getElementById("btnCapturar")
let peerConnection;

//Configurar servidor TURN, para funcionar onde o firewall do roteador ou provedores bloqueiam conexões diretas
const configuration = {
    iceServers: [
        { urls: "stun:stun.l.google.com:19302" }
    ]
};
//buscando cameras na api e adicionando no html
fetch(url).then(json => {
    json.json()
        .then(cameras => {
            ListarCameras.innerHTML = '<option>Selecione uma camera</option>';
            cameras.map(camera => {
                const option = document.createElement('option')
                option.value = camera.name;
                option.text = camera.name
                ListarCameras.appendChild(option)
            })
        })
})


//Ao receber uma oferta do transmissor
socket.on('offer', async ({ from, offer }) => {
     peerConnection = new RTCPeerConnection(configuration);
     //Ao receber o stream de video
    peerConnection.ontrack = (event) => {
        const track = event.track;
        ///verifica se o stream é um video
        if (track.kind === 'video') {
            const mediaStream = event.streams[0];
            console.log(mediaStream)
            videoPlayer.srcObject = mediaStream;
            videoPlayer.play().catch((error) => {
                console.error('Erro ao tentar reproduzir o vídeo:', error);
            });
        }
    };
    //Ao receber ice-candidate
    peerConnection.onicecandidate = (event) => {
        if (event.candidate) {
            socket.emit('ice-candidate', { to: from, candidate: event.candidate });
        }
    };
    //Adicionando uma oferta para o transmissor
    await peerConnection.setRemoteDescription(new RTCSessionDescription(offer));
    const answer = await peerConnection.createAnswer();
    await peerConnection.setLocalDescription(answer);
    //Enviando oferta para o transmissor
    socket.emit('answer', { to: from, answer: peerConnection.localDescription });
    console.log(from)
});

//Adicionar candidatos ICE recebidos
socket.on('ice-candidate', ({ candidate }) => {
    if (peerConnection && candidate) {
        peerConnection.addIceCandidate(new RTCIceCandidate(candidate));
    }
});

//Ao clicar no botão capturar
addPlayerBtn.addEventListener("click", () => {
    if (ListarCameras.value !== "Selecione uma camera") {
        const cameraId = ListarCameras.value
        socket.emit('request-camera', { cameraId });
        console.log(`Solicitação enviada para câmera: ${cameraId}`);
    } else {
        alert("Selecione uma camera para caputrar uma camera")
    }
})
