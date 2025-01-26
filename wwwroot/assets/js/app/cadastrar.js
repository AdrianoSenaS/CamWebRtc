const url = "/api/cam";
let ListarCameras = document.getElementById("ListarCameras");
const videoPlayer = document.getElementById("videoPlayer");
const addPlayerBtn = document.getElementById("btnCadastrar");

// Listar dispositivos de mídia
const listVideoDevices = async () => {
    let stream = await navigator.mediaDevices.getUserMedia({ video: true, audio: true });
    const devices = await navigator.mediaDevices.enumerateDevices();
    const videoDevices = devices.filter(device => device.kind === 'videoinput');

    // Limpar opções antigas
    ListarCameras.innerHTML = '<option>Selecione</option>';

    // Adicionar dispositivos ao select
    videoDevices.forEach((device) => {
        const option = document.createElement('option');
        option.value = device.deviceId;;
        option.text = device.label;
        ListarCameras.appendChild(option);
    });
};
//Adiciona o stream local
const playrLocal = (deviceId) => {
    const constraints = {
        video: {
            deviceId: { exact: deviceId }
        }
    };
    navigator.mediaDevices.getUserMedia(constraints).then((stream) => {
        window.stream = stream;
        videoPlayer.srcObject = stream; 
    });
};

//Envia os dados para api
async function createVideoPlayer(deviceId, deviceName) {
    const constraints = {
        video: {
            deviceId: { exact: deviceId }
        }
    };
    try {
        navigator.mediaDevices.getUserMedia(constraints).then((stream) => {
            const data = {
                "name": deviceName,
                "isActive": stream.active,
                "connectionID": deviceId
            };
            if (deviceName != null) {
                const token = Cookies()
                console.log(token)
                try{
                    Api(url, "POST", data, token);
                    alert(`Camera Cadastrada: ${deviceName}`);
                }catch(ex){
                    console.log(ex)
                }
            } else {
                alert("Selecione uma camera");
            }
        });
    } catch (error) {
        console.error('Erro ao acessar o dispositivo de vídeo:', error);
    }
}
//Evento Selecionando camera
ListarCameras.addEventListener("change", (e) => {
    const selectedDeviceId = ListarCameras.value;
    console.log(selectedDeviceId);
    playrLocal(selectedDeviceId);
});

// Evento de clique no botão para adicionar player
addPlayerBtn.addEventListener('click', () => {
    const selectedDeviceId = ListarCameras.value;
    const selectedDeviceName = ListarCameras.options[ListarCameras.selectedIndex].text;
    if (selectedDeviceId) {
        createVideoPlayer(selectedDeviceId, selectedDeviceName);
    } else {
        alert('Por favor, selecione uma câmera primeiro.');
    }
});

//Inicializar a lista de dispositivos ao carregar a página
listVideoDevices();
