# 🎮 FIAP Cloud Games · Tech Challenge Fase 1 – Grupo 132

[![Build & Test](https://github.com/rafaelismair/tc-nett-fase-1/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/rafaelismair/tc-nett-fase-1/actions)
[![Docker Image](https://img.shields.io/badge/DockerHub-rismair/fiap--apptechchallenge-blue?logo=docker)](https://hub.docker.com/r/rismair/fiap-apptechchallenge)
[![Deploy Railway](https://img.shields.io/website?down_color=red&down_message=down&label=API%20prod&up_color=brightgreen&up_message=up&url=https%3A%2F%2Ffiap-apptechchallenge-production.up.railway.app%2Fswagger)](https://fiap-apptechchallenge-production.up.railway.app/swagger)

MVP da plataforma **FIAP Cloud Games**, desenvolvido na Pós-graduação **Arquitetura de Soluções .NET – FIAP**.  
Na Fase 1 construímos uma **API REST** em .NET 8 para gerenciamento de usuários e seus jogos adquiridos, seguindo Clean Architecture + DDD.

---

## 👥 Integrantes

| Nome | RM |
|------|----|
| Gustavo Girotto Queiroz | 361174 |
| Leonardo G. B. Figueiredo | 363933 |
| Mylena Mel Silva de Farias | 364788 |
| Rafael Ismair Ferreira | 364211 |

---

## ✔️ Funcionalidades Fase 1

| Módulo | Detalhes |
|--------|----------|
| **Usuários** | • Registro com e-mail verificado  <br>• Senha forte (8+ chars, números, letras, especiais) |
| **Auth / JWT** | • Login • Refresh (future)  <br>• Perfis **User** e **Admin** |
| **Jogos** | • CRUD de jogos  <br>• Compra de jogo  <br>• Biblioteca do usuário |
| **Observabilidade** | • Métricas Prometheus (`/metrics`)  <br>• Dashboard Grafana pronto |

---

## 🏗️ Arquitetura

Presentation │ API (.NET 8) · Controllers · Middlewares
Application │ Services · DTOs
Domain │ Entidades · VOs · Regras de negócio
Infrastructure │ EF Core · Repositórios · Migrations

* **Clean Architecture** separa camadas de domínio/infra/API.  
* **DDD** aplica agregados e invariantes.  
* **Event Storming**: [Miro board](https://miro.com/app/board/uXjVI9-Ai5Q=/?share_link_id=145367113042).

---

## 🛠️ Stack

| Camada | Tecnologia |
|--------|------------|
| API    | .NET 8, ASP.NET Core, Swashbuckle |
| Persistência | PostgreSQL + EF Core |
| Auth   | JWT Bearer |
| Testes | xUnit + Moq |
| CI/CD  | GitHub Actions → Docker Hub |
| Deploy | **Railway** (Docker Image + PostgreSQL addon) |
| Observability | **Prometheus** + **Grafana** (template Railway) |

---

## 🚀 Clonando & rodando local

> Requer: **.NET 8 SDK**, Docker Desktop.

```bash
git clone https://github.com/rafaelismair/tc-nett-fase-1.git
cd tc-nett-fase-1

# subir ambiente completo (API + PG + Prometheus + Grafana)
docker compose up -d --build
# Swagger → http://localhost:4080/swagger
# Prometheus → http://localhost:9090
# Grafana → http://localhost:3000  (admin/admin)

