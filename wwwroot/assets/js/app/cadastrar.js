const ListarCameras = document.getElementById("ListarCameras");
const videoPlayer = document.getElementById("videoPlayer");
const addPlayerBtn = document.getElementById("btnCadastrar");
let camerName;
const url = "/api/Cam";

// Listar dispositivos de mídia
const listVideoDevices = async () => {
    let stream = await navigator.mediaDevices.getUserMedia({ video: true, audio: true });
    const devices = await navigator.mediaDevices.enumerateDevices();
    const videoDevices = devices.filter(device => device.kind === 'videoinput');

    // Limpar opções antigas
    ListarCameras.innerHTML = '<option>Selecione</option>';

    // Adicionar dispositivos ao select
    videoDevices.forEach((device, index) => {
        const option = document.createElement('option');
        option.value = device.deviceId;
        option.text = device.label || `Camera ${index + 1}`;
        ListarCameras.appendChild(option);
    });
};

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

// Criar player de vídeo para o dispositivo selecionado
async function createVideoPlayer(deviceId, deviceName) {
    const constraints = {
        video: {
            deviceId: { exact: deviceId }
        }
    };
    try {
        navigator.mediaDevices.getUserMedia(constraints).then((stream) => {
            window.stream = stream;
            videoPlayer.srcObject = stream; 
            const data = {
                "name": deviceName,
                "isActive": stream.active,
                "connectionID": deviceId
            };
            if (deviceName != null) {
                api(url, "POST", data);
            } else {
                alert("Selecione uma camera");
            }
        });
    } catch (error) {
        console.error('Erro ao acessar o dispositivo de vídeo:', error);
    }
}

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

const api = (url, method, data) => {
    // Opções da requisição
    const options = {
        method: method, // Método HTTP
        headers: {
            "Content-Type": "application/json" // Tipo de dado enviado
        },
        body: JSON.stringify(data) // Converte os dados para JSON
    };

    // Fazendo a requisição
    fetch(url, options)
        .then(response => {
            console.log(response);
            if (!response.ok) {
                throw new Error(`Erro: ${response.status}`);
            }
            alert("Camera Cadastrada");
            return response.json(); // Converte a resposta para JSON
        })
        .then(result => {
            console.log("Resposta do servidor:", result);
        })
        .catch(error => {
            console.error("Erro ao fazer a requisição:", error);
        });
};

// Inicializar a lista de dispositivos ao carregar a página
listVideoDevices();
