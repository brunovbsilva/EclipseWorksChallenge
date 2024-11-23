# EclipseWorksChallenge

Este é um projeto proposto pela empresa EclipseWorks para avaliação de conhecimentos em desenvolvimento de software.

# Requisitos

Visual Studio 2022
Docker

# Como executar pelo Visual Studio

1. Abra o projeto em seu Visual Studio.
2. Selecione o item de inicialização "docker-compose" e execute o projeto.

# Executar via terminal

1. em seu CMD, navegue até o diretório do projeto.
2. execute o comando: `docker compose -p {container-name} up --build -d`

# Perguntas ao PO

Há algumas incongruencias em relação as regras de negócio e os modelos de entidades propostos:
- "Uma tarefa possui título, uma descrição, uma data de vencimento e um status", porém, há uma regra de prioridade que não foi contemplada no modelo de entidade.
- "Um usuário pode criar, visualizar e gerenciar vários projetos.", mas, há regras de deleção.
- "Os usuários podem adicionar comentários a uma tarefa para fornecer informações adicionais.", porém, não há na comentários entidade proposta.
- Há a necessidade de histórico e logs, porém, não há requisições feitas ao aplicativo para salvar logs no banco de dados.

# Pontos de Melhoria

O projeto, apesar de simples está bem estruturado, porém, há alguns pontos de melhoria:
- Separar mais as responsabilidades em mais controllers, assim como separar também os serviços.
- Utilizar um sistema de Cache para melhorar a performance.
- Melhorar o sistema de logs de ações que são salvas no banco de dados.
- Implementar um sistema de logs de erros para rastreabilidade em um middleware.
- Adicionar novas propriedades no usuário para ser mais fácil de identificar o mesmo.

notas: 
- O base controller possui um mock de usuário logado para simular a autenticação, que viria de um outro micro-serviço.
- Há como adicionar volumes no docker-compose para persistir os dados do banco de dados, porém, como é apenas um projeto para avaliação de conhecimentos, não foi adicionado.

