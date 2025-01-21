const express = require('express');
const http = require('http');
const { Server } = require('socket.io');
const cors = require('cors');

const app = express();
const server = http.createServer(app);
const io = new Server(server, {
    cors: {
        origin: "*", // Permite conexões de qualquer origem.
        methods: ["GET", "POST"],
    },
});

const clients = {}; // Armazena os clientes e câmeras associadas.

app.use(cors());
app.use(express.static('public'));

// Quando um cliente se conecta
io.on('connection', (socket) => {
    console.log(`Cliente conectado: ${socket.id}`);

    // Quando um receptor solicita uma câmera
    socket.on('request-camera', (data) => {
        const { cameraId } = data;
        clients[socket.id] = cameraId; // Associa o cliente à câmera.
        console.log(`Cliente ${socket.id} solicitou câmera ${cameraId}`);
        
        // Notifica o transmissor para começar a enviar o stream
        socket.broadcast.emit('new-client', { clientId: socket.id, cameraId });
    });

    // Envia oferta do transmissor para o receptor
    socket.on('offer', (data) => {
        const { to, offer } = data;
        io.to(to).emit('offer', { from: socket.id, offer });
    });

    // Envia resposta do receptor para o transmissor
    socket.on('answer', (data) => {
        const { to, answer } = data;
        io.to(to).emit('answer', { from: socket.id, answer });
    });

    // Reencaminha candidatos ICE
    socket.on('ice-candidate', (data) => {
        const { to, candidate } = data;
        io.to(to).emit('ice-candidate', { from: socket.id, candidate });
    });

    // Remove cliente desconectado
    socket.on('disconnect', () => {
        console.log(`Cliente desconectado: ${socket.id}`);
        delete clients[socket.id];
        socket.broadcast.emit('client-disconnected', { clientId: socket.id });
    });
});

// Inicia o servidor
server.listen(5000, () => console.log('Servidor rodando na porta 5000'));
