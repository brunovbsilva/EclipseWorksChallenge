# EclipseWorksChallenge

Este é um projeto proposto pela **EclipseWorks** como parte do processo de avaliação de conhecimentos em desenvolvimento de software. O objetivo é implementar um sistema básico para gerenciamento de projetos e tarefas, contemplando boas práticas de desenvolvimento e organização de código.

---

## 🚀 **Como executar o projeto**

### Usando o Visual Studio
1. Abra o projeto no **Visual Studio**.
2. Selecione o item de inicialização **`docker-compose`**.
3. Execute o projeto pressionando **F5** ou clicando em **Executar**.

### Via terminal
1. Abra o **CMD** ou o terminal de sua preferência.
2. Navegue até o diretório raiz do projeto.
3. Execute o comando:
   ```bash
   docker compose -p {container-name} up --build -d

---

### 📝 **Postman Collection**

[Baixar Collection](https://drive.google.com/file/d/1OngGICsW2S_8doC4KLDnLMSr4UHUyYLK/view?usp=sharing)

---

## ❓ **Dúvidas para o Product Owner (PO)**

Durante o desenvolvimento, identificamos algumas inconsistências entre as regras de negócio e os modelos de entidades propostos. São elas:

1. **Regra de Prioridade**: 
   - As tarefas têm **título**, **descrição**, **data de vencimento** e **status**, mas há menção a um sistema de **prioridade** que não está presente no modelo atual.
   
2. **Gerenciamento de Projetos**: 
   - A especificação diz que "um usuário pode criar, visualizar e gerenciar vários projetos". No entanto, há menção a regras de **deleção de projetos**.

3. **Comentários em Tarefas**: 
   - É mencionado que "os usuários podem adicionar comentários às tarefas para fornecer informações adicionais", mas o modelo de entidade não contempla um campo ou relação para comentários.

4. **Histórico e Logs**:
   - Existe a necessidade de histórico de alterações e logs de ações realizadas, mas o modelo proposto não inclui requisições para salvar logs no banco de dados.

---

## 🔧 **Pontos de Melhoria**

Embora o projeto esteja bem estruturado, há oportunidades para aprimorar sua organização e desempenho:

### Organização e Arquitetura
- **Separação de Responsabilidades**:
  - Dividir as responsabilidades em mais controllers e serviços para facilitar a escalabilidade e manutenção do código.

- **Novas Propriedades para Usuários**:
  - Adicionar atributos como **nome completo**, **e-mail** e **foto de perfil** para facilitar a identificação e personalização.

### Performance e Logs
- **Sistema de Cache**:
  - Implementar um sistema de cache para reduzir a carga no banco de dados e melhorar a performance.

- **Logs de Ações e Erros**:
  - Ampliar o sistema de logs para salvar todas as ações relevantes no banco de dados.
  - Adicionar um middleware dedicado para registrar logs de erros e garantir rastreabilidade.

### Persistência de Dados
- Adicionar volumes no **`docker-compose`** para garantir a persistência dos dados do banco de dados.

### Autenticação
- O mock de autenticação presente no **BaseController** simula um usuário logado. Em um ambiente real, a autenticação seria realizada via **JWT** ou outro sistema centralizado.

---

### Observação Final

Este projeto tem como objetivo avaliar conhecimentos técnicos. Assim, algumas funcionalidades, como autenticação completa e configuração de persistência de dados no Docker, foram simplificadas para agilizar o desenvolvimento e foco na estruturação geral.
