const url = "/api/Cam";
const camerasApi = fetch(url);
const ListarCameras = document.getElementById("ListarCameras")
const videoPlayer = document.getElementById("videolocal")
const addPlayerBtn = document.getElementById("btnCapturar")
const urlParams = new URLSearchParams(window.location.search);
const streamId = urlParams.get('streamId');
const socket = io('http://localhost:5000');


camerasApi.then(json => {
    json.json()
        .then(cameras => {
            ListarCameras.innerHTML = '<option>Selecione uma camera</option>';
            cameras.map(camera => {
                const option = document.createElement('option')
                option.value = camera.connectionID;
                option.text = camera.name
                ListarCameras.appendChild(option)
            })
        })
})

let peerConnection;

 // Ao receber uma oferta do transmissor
 socket.on('offer', async ({ from, offer }) => {
  peerConnection = new RTCPeerConnection({
      iceServers: [{ urls: 'stun:stun.l.google.com:19302' }],
  });

  peerConnection.ontrack = (event) => {
      const remoteStream = new MediaStream();
      remoteStream.addTrack(event.track);
      window.stream = remoteStream;
      videoPlayer.srcObject = remoteStream; 
  };

  peerConnection.onicecandidate = (event) => {
      if (event.candidate) {
          socket.emit('ice-candidate', { to: from, candidate: event.candidate });
      }
  };

  await peerConnection.setRemoteDescription(new RTCSessionDescription(offer));
  const answer = await peerConnection.createAnswer();
  await peerConnection.setLocalDescription(answer);

  socket.emit('answer', { to: from, answer: peerConnection.localDescription });
  console.log(from)
});

// Adicionar candidatos ICE recebidos
socket.on('ice-candidate', ({ candidate }) => {
  if (peerConnection && candidate) {
      peerConnection.addIceCandidate(new RTCIceCandidate(candidate));
  }
});


  addPlayerBtn.addEventListener("click",()=>{
    if(ListarCameras.value !== "Selecione uma camera"){
      const cameraId = ListarCameras.value
      socket.emit('request-camera', { cameraId });
      console.log(`Solicitação enviada para câmera: ${cameraId}`);
    }else{
        alert("Selecione uma camera para caputrar uma camera")
    }
  })
