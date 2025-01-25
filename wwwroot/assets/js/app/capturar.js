const url = "/api/cam";
const socket = io('https://node-media-server-servernode.fpumgv.easypanel.host');
const ListarCameras = document.getElementById("ListarCameras")
const videoPlayer = document.getElementById("videolocal")
const addPlayerBtn = document.getElementById("btnCapturar")
let peerConnection;
const token = Cookies()

//Configurar servidor TURN, para funcionar onde o firewall do roteador ou provedores bloqueiam conexões diretas
const configuration = {
    iceServers: [{ urls: ["stun:sp-turn1.xirsys.com"] }, { username: "TP-V6xiLlVi-Hmh76H6U2qmrCO9hL9FPpexh6s7xjzLqBj_ssdSVFWTsMWiIxhe9AAAAAGeRNxZhZHJpYW5vc2VuYQ==", credential: "a78c2292-d8ed-11ef-8f2a-0242ac120004", urls: ["turn:sp-turn1.xirsys.com:80?transport=udp", "turn:sp-turn1.xirsys.com:3478?transport=udp", "turn:sp-turn1.xirsys.com:80?transport=tcp", "turn:sp-turn1.xirsys.com:3478?transport=tcp", "turns:sp-turn1.xirsys.com:443?transport=tcp", "turns:sp-turn1.xirsys.com:5349?transport=tcp"] }]
};
//buscando cameras na api e adicionando no html
ApiGet(url, "GET",  token).then(cameras => {
    ListarCameras.innerHTML = '<option>Selecione uma camera</option>';
    cameras.map(camera => {
        const option = document.createElement('option')
        option.value = camera.name;
        option.text = camera.name
        ListarCameras.appendChild(option)
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


const APIGET = async (url, method, token)=>{
    const options = {
        method: method,
        headers: {
            "Authorization": `Bearer ${token}`,
            "Content-Type": "application/json" 
        },
    };
   try{
    const response = (await fetch(url, options)).statusText
    console.log(response)
   }catch(ex){
    return ex;
   }
}
APIGET(url, "GET", token)