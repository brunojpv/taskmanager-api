# TaskManager API

Uma API de gerenciamento de tarefas e projetos desenvolvida em .NET 8, seguindo princípios de Clean Architecture, DDD e SOLID.

## 📋 Funcionalidades

- **Projetos**: criação, visualização e gerenciamento de projetos
- **Tarefas**: criação, atualização, visualização e remoção de tarefas
- **Comentários**: adição de comentários a tarefas
- **Relatórios**: geração de relatórios de desempenho da equipe
- **Histórico**: rastreamento de todas as alterações em tarefas

## 🔧 Tecnologias

- **.NET 8**
- **Entity Framework Core**
- **PostgreSQL**
- **Docker**
- **Swagger**
- **FluentValidation**
- **AutoMapper**
- **XUnit**

## 🚀 Arquitetura

O projeto segue uma arquitetura limpa (Clean Architecture) com as seguintes camadas:

- **Domain**: entidades, regras de negócio e interfaces de repositório
- **Application**: casos de uso da aplicação, DTOs e serviços
- **Infrastructure**: implementações concretas de repositórios e persistência
- **API**: endpoints RESTful e configuração da aplicação

## 📈 Regras de Negócio

1. Cada tarefa tem uma prioridade (baixa, média, alta) que não pode ser alterada após criação
2. Projetos não podem ser removidos se tiverem tarefas pendentes
3. Um histórico é registrado para cada alteração em tarefas
4. Cada projeto tem um limite máximo de 20 tarefas
5. Relatórios de desempenho só são acessíveis a usuários com função de gerente
6. Comentários em tarefas são registrados no histórico

## 🐳 Executando com Docker

Para executar o projeto usando Docker:

```bash
# Clone o repositório
git clone https://github.com/seu-usuario/taskmanager.git
cd taskmanager

# Inicie os containers
docker-compose up -d
```

A API estará disponível em `http://localhost:5000`, e a documentação Swagger em `http://localhost:5000`.

## 🧪 Testes

O projeto inclui testes de unidade com cobertura de mais de 80% para validar todas as regras de negócio.

Para executar os testes:

```bash
dotnet test
```

## 📝 Exemplos de Uso

O projeto inclui uma collection do Postman que pode ser usada para testar a API. Importe o arquivo `TaskManager API - Postman Collection.json` no Postman para começar.

## 🔍 Refinamento - Perguntas para o PO

Para refinar o sistema em futuras implementações, estas são as principais questões que levantaria com o Product Owner:

1. **Escalabilidade do sistema:**
   - Qual o número esperado de usuários simultâneos?
   - Qual o volume médio e máximo de tarefas que o sistema deve suportar?
   - Existe necessidade de armazenamento de longo prazo ou arquivamento de projetos/tarefas antigas?

2. **Funcionalidades adicionais:**
   - Existe a necessidade de implementar um sistema de notificações para prazos próximos do vencimento?
   - Seria útil integrar o sistema com outras ferramentas (Slack, MS Teams, email)?
   - Há interesse em funcionalidades de templates de projetos para padronização?
   - Seria valoroso implementar um sistema de etiquetas/tags para categorização de tarefas?

3. **Experiência do usuário:**
   - Quais são os principais pontos de atrito no fluxo atual de trabalho?
   - Seria útil implementar visualizações adicionais (Kanban, Gantt, calendário)?
   - A equipe beneficiaria de recursos de colaboração em tempo real?

4. **Segurança e conformidade:**
   - Existem requisitos específicos de segurança ou conformidade regulatória?
   - É necessário implementar registros de auditoria mais detalhados?
   - Qual o nível de granularidade necessário para permissões de usuários?

5. **Relatórios e analytics:**
   - Quais métricas adicionais seriam valiosas para a equipe gerencial?
   - Há necessidade de exportação de relatórios em formatos específicos?
   - Existem integrações com ferramentas de BI a considerar?

6. **Mobile e acessibilidade:**
   - Existe previsão para desenvolvimento de clientes mobile nativos?
   - Quais são os requisitos de acessibilidade a considerar?

## 🚀 Melhorias Futuras

Com base na experiência de desenvolvimento deste projeto, identifico as seguintes oportunidades de melhoria:

### Aprimoramentos Técnicos

1. **Arquitetura e padrões:**
   - Implementar CQRS (Command Query Responsibility Segregation) para melhorar a escalabilidade
   - Adotar o padrão Mediator para desacoplar componentes (usando MediatR)
   - Implementar Event Sourcing para manter histórico de todas as operações

2. **Infraestrutura Cloud:**
   - Migrar para uma arquitetura de microserviços (separando projetos, tarefas e relatórios)
   - Implementar containerização com Kubernetes para orquestração
   - Utilizar serviços gerenciados como Azure App Service ou AWS ECS
   - Implementar CI/CD usando GitHub Actions ou Azure DevOps

3. **Performance:**
   - Adicionar cache distribuído (Redis) para queries frequentes
   - Implementar leitura/escrita separadas com CQRS
   - Utilizar bancos de dados otimizados para diferentes tipos de dados (SQL para transações, MongoDB para histórico)
   - Implementar compressão de resposta HTTP

4. **Segurança:**
   - Implementar autenticação OAuth2/OIDC
   - Adicionar proteção contra ataques CSRF, XSS e injeção
   - Implementar rate limiting para prevenção de ataques de força bruta
   - Adicionar validação de JWT com rotação de chaves

5. **Monitoramento:**
   - Implementar logging centralizado com Elasticsearch + Kibana
   - Adicionar APM (Application Performance Monitoring)
   - Configurar alertas para anomalias de performance
   - Implementar health checks e métricas

### Melhorias funcionais

1. **Usabilidade:**
   - Implementar bulk operations para tarefas
   - Adicionar recursos de busca avançada e filtros
   - Criar templates de projetos reutilizáveis

2. **Colaboração:**
   - Implementar sistema de notificações em tempo real
   - Adicionar menções em comentários (@username)
   - Implementar "seguir" tarefas para receber atualizações

3. **Integração:**
   - Criar webhooks para integração com sistemas externos
   - Desenvolver API para importação/exportação em massa
   - Integrar com serviços de armazenamento para anexos

4. **Análise de dados:**
   - Implementar dashboards de produtividade
   - Adicionar previsão de conclusão baseada em histórico
   - Criar relatórios personalizáveis

## 📄 Licença

Este projeto está licenciado sob a [MIT License](LICENSE).

## 👤 Autor

Bruno Vieira - [GitHub](https://github.com/seu-usuario) - [Email](mailto:seu-email@exemplo.com)
