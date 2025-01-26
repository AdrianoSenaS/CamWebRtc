const url = "/api/cam";
const urlIce = "/api/IceServes";
const signalRHubUrl = '/signaling'; // Ajuste para o endpoint do SignalR
const ListarCameras = document.getElementById("ListarCameras");
const videoPlayer = document.getElementById("videolocal");
const addPlayerBtn = document.getElementById("btnCapturar");
const token = Cookies();
let username;
let credential;
let urlStun = [];
let urlTurn = [];


GetApi(urlIce, "GET", token).then(response => {
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



// Conectar ao SignalR Hub
const connection = new signalR.HubConnectionBuilder()
    .withUrl(signalRHubUrl, { accessTokenFactory: () => token }) // Adiciona o token de autenticação
    .withAutomaticReconnect() // Reconecta automaticamente se cair
    .build();

// Iniciar a conexão com o SignalR
connection.start()
    .then(() => console.log("Conectado ao SignalR Hub"))
    .catch(err => console.error("Erro ao conectar ao SignalR:", err));

// Buscar câmeras na API e adicionar no HTML
GetApi(url, "GET", token).then(cameras => {
    ListarCameras.innerHTML = '<option>Selecione uma câmera</option>';
    cameras.map(camera => {
        const option = document.createElement('option');
        option.value = camera.name;
        option.text = camera.name;
        ListarCameras.appendChild(option);
    });
});

const StartStream = async () => {

    // Ao receber uma oferta do transmissor
    connection.on('ReceiveOffer', async ({ from, offer }) => {
        console.log(offer)
        const response = await fetch("https://webrtcadriano.metered.live/api/v1/turn/credentials?apiKey=02bb2417b9bfdf3ac91c6dedca16b17e17e5");
        const iceServers = await response.json();
        const peerConnection =  new RTCPeerConnection({ iceServers: iceServers });

        // Ao receber o stream de vídeo
        peerConnection.ontrack = (event) => {
            const track = event.track;
            if (track.kind === 'video') {
                const mediaStream = event.streams[0];
                console.log(mediaStream);
                videoPlayer.srcObject = mediaStream;
                videoPlayer.play().catch((error) => {
                    console.error('Erro ao tentar reproduzir o vídeo:', error);
                });
            }
        };

        // Ao receber um candidato ICE
        peerConnection.onicecandidate = (event) => {

            if (event.candidate) {
                console.log("CandidateEvene " + event.candidate)
                connection.invoke('SendIceCandidate', from, event.candidate);
                console.log("SendIceCandidateClientId :" + from)
                console.log("SendIceCandidate :" + JSON.stringify(event.candidate))
            }
        };

        // Adicionar a oferta recebida
        await peerConnection.setRemoteDescription(new RTCSessionDescription(offer));
        const answer = await peerConnection.createAnswer();
        await peerConnection.setLocalDescription(answer);
        // Enviar a resposta para o transmissor
        connection.invoke('SendAnswer', from, answer);
        console.log("SendAnswer" + JSON.stringify(answer));
    });

    // Adicionar candidatos ICE recebidos
    connection.on('ReceiveIceCandidate', ({ candidate }) => {
        if (peerConnection && candidate) {
            console.lo("ReceiveIceCandidate " + JSON.stringify(candidate))
            peerConnection.addIceCandidate(new RTCIceCandidate(candidate));
        }
    });

}

StartStream()
// Ao clicar no botão "Capturar"
addPlayerBtn.addEventListener("click", () => {
    if (ListarCameras.value !== "Selecione uma câmera") {
        const cameraId = ListarCameras.value;
        connection.invoke('RequestCamera', cameraId)
            .then(() => console.log(`Solicitação enviada para câmera: ${cameraId}`))
            .catch(err => console.error('Erro ao solicitar câmera:', err));
    } else {
        alert("Selecione uma câmera para capturar");
    }
});
