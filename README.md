# CamWebRtc

AS-WebRTC-Camera-Manager é um servidor de gerenciamento de câmeras em tempo real utilizando o protocolo WebRTC. O sistema foi projetado para fornecer uma solução escalável, segura e de fácil utilização para visualizar e gerenciar câmeras IP diretamente do navegador. Além disso, o servidor conta com um sistema de autenticação e gerenciamento de usuários para garantir que o acesso aos feeds de vídeo seja controlado de forma eficaz.

## Funcionalidades

- **Transmissão em tempo real via WebRTC:** Visualize feeds de vídeo das câmeras em tempo real com baixa latência.
- **Gerenciamento de câmeras:** Adicione, configure e remova câmeras com facilidade diretamente no painel de administração.
- **Autenticação de Usuários com JWT:** Controle o acesso aos feeds de câmeras por meio de autenticação segura.
- **Controle de permissões:** Defina as permissões de cada usuário para acessar as câmeras específicas.
- **Painel Web Intuitivo:** Interface de usuário simples e fácil de navegar para monitoramento e configuração de câmeras.
- **Escalabilidade:** Suporte para adicionar múltiplas câmeras e usuários com facilidade.

## Tecnologias Utilizadas

- **WebRTC:** Protocolo de comunicação em tempo real para transmissão de vídeo.
- **ASP.NET Core (v8.0):** Framework para construir a API e o backend do servidor.
- **JWT (JSON Web Tokens):** Autenticação segura para acesso aos feeds de vídeo.
- **SQLite/PostgreSQL:** Banco de dados utilizado para armazenar dados de câmeras, usuários e permissões.
- **SignalR:** Comunicação em tempo real entre o servidor e os clientes para garantir a baixa latência.
- **Docker (opcional):** Suporte para execução do servidor em containers para ambientes de produção escaláveis.

## Requisitos

- **.NET SDK 8:** Necessário para compilar e rodar o projeto.
- **Banco de Dados:** SQLite.
- **Navegador:** Qualquer navegador moderno (Google Chrome, Firefox, etc.).

