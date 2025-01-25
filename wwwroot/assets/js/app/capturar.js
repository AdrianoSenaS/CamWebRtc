const url = "/api/cam";
const urlIce = "/api/IceServes";
const signalRHubUrl = '/signaling'; // Ajuste para o endpoint do SignalR
const ListarCameras = document.getElementById("ListarCameras");
const videoPlayer = document.getElementById("videolocal");
const addPlayerBtn = document.getElementById("btnCapturar");
let peerConnection;
const token = Cookies();
let username;
let credential;
let urlStun = [];
let urlTurn = [];
// Configurar servidor TURN para funcionar com firewalls
const IceServesConfiguration = async () => {
    const response = await GetApi(urlIce, "GET", token);
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

// Ao receber uma oferta do transmissor
connection.on('ReceiveOffer', async ({ from, offer }) => {
    const offerParse = JSON.parse(offer)
    console.log(offerParse)
    const configuration = IceServesConfiguration();
    peerConnection = new RTCPeerConnection(configuration);

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
            const cadidateJson = JSON.stringify(event.candidate)
            connection.invoke('SendIceCandidate', from, cadidateJson);
            console.log("SendIceCandidateClientId :" + from)
            console.log("SendIceCandidate :" + cadidateJson)
        }
    };

    // Adicionar a oferta recebida
    await peerConnection.setRemoteDescription(new RTCSessionDescription(offerParse));
    const answer = await peerConnection.createAnswer();
    await peerConnection.setLocalDescription(answer);
    const answerJson = JSON.stringify(peerConnection.localDescription)
    // Enviar a resposta para o transmissor
    connection.invoke('SendAnswer', from, answerJson);
    console.log("SendAnswer" + answerJson);
});

// Adicionar candidatos ICE recebidos
connection.on('ReceiveIceCandidate', ({ candidate }) => {
    const cadidatejson = JSON.parse(candidate)
    if (peerConnection && cadidatejson) {
        peerConnection.addIceCandidate(new RTCIceCandidate(candidate));
    }
});

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

// Função para buscar dados da API
const ApiGet = async (url, method, token) => {
    const options = {
        method: method,
        headers: {
            "Authorization": `Bearer ${token}`,
            "Content-Type": "application/json"
        },
    };
    try {
        const response = await fetch(url, options);
        return await response.json();
    } catch (ex) {
        console.error('Erro ao buscar dados:', ex);
    }
};
