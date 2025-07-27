# 🎮 FIAP Cloud Games - Tech Challenge Fase 1

![Build Status](https://github.com/rafaelismair/tc-nett-fase-1/actions/workflows/dotnet-desktop.yml/badge.svg)

Este projeto é o MVP da plataforma **FIAP Cloud Games**, desenvolvido como parte do Tech Challenge da Pós Graduação em Arquitetura de Soluções .NET da FIAP.  

O objetivo é desenvolver uma plataforma de venda de jogos digitais e gestão de servidores para partidas online. Nessa primeira fase, o foco é criar uma API Rest em .NET para gerenciar usuários e seus jogos adquiridos, seguindo as boas práticas de desenvolvimento.

---
## Grupo 132
    - Gustavo Girotto Queiroz - RM361174
    - João Pedro Duarte Silva - RM364273
    - Leonardo Goncalves Barboza de Figueiredo - RM363933
    - Mylena Mel Silva de Farias - RM364788
    - Rafael Ismair Ferreira - RM364211
---

## ✔️ Funcionalidades Implementadas na Fase 1

- **Cadastro de Usuários**  
    - Identificação através de nome, e-mail e senha
    - Validação de e-mail e senha segura (mínimo de 8 caracteres com números, letras e caracteres especiais)

- **Autenticação e Autorização**
    - Autenticação via token JWT
    - Níveis de acesso:
        - **Usuário**
        - **Administrador**

- **Gerenciamento de jogos**
    - Cadastro de jogos
    - Aquisição de jogos 
    - Visualização da biblioteca de jogos adquiridos
    - Listagem de todos os jogos
---

## 🏗️ Arquitetura e Design

- O projeto foi desenvolvido utilizando arquitetura **monolítica**, conforme solicitado para o MVP. Adotamos, também, os princípios da **Arquitetura Limpa** e **Domain-Driven Design (DDD)** para organizar o código e as regras de negócio.

- A estrutura está dividida em:
    - API: Agrupa os controllers da aplicação, as expondo para os usuários, e agrupa os middlewares para tratamento de erros e de logs
    - Application: Orquestra as operações através das services
    - Domain: Armazena as entidades de negócio e as mantêm independentes do resto da aplicação
    - Infrastructure: Gerencia a persistência de dados 
    - Tests: Contém os testes da aplicação 

### DDD - Event Storming
[Event Storming do fluxo de criação de jogos e de usuários](https://miro.com/app/board/uXjVI9-Ai5Q=/?share_link_id=145367113042)

---

## 🛠 Tecnologias Utilizadas

- .NET **8**
- Entity Framework Core
- PostgreSQL
- JWT Authentication
- Swagger (Swashbuckle)
- xUnit + Moq
- Clean Architecture + DDD

---

## 📦 Instalação

### ✅ Requisitos

- .NET **8 SDK** 
- PostgreSQL (https://railway.com/)
- Visual Studio 2022+ ou VS Code

### 🚀 Clonando o projeto

```bash
git clone https://github.com/rafaelismair/tc-nett-fase-1.git
cd tc-nett-fase-1
dotnet restore
```
