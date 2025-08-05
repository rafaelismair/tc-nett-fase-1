
# 📊 Monitoramento com Prometheus + Grafana

Este projeto inclui uma stack de monitoramento utilizando **Prometheus** e **Grafana** para coletar métricas da aplicação ASP.NET Core.

---

## ✅ Pré-requisitos

- Docker e Docker Compose instalados
- .NET 8 SDK instalado
- Projeto buildando localmente

---

## 📦 Instalação de dependências

Execute na pasta da API para adicionar o pacote Prometheus:

```bash
dotnet add package prometheus-net.AspNetCore
```

---

## 🚀 Subindo os containers

Rode o seguinte comando na raiz do projeto:

```bash
docker-compose up -d
```

Isso iniciará:

| Serviço     | Porta        | URL                        |
|-------------|--------------|----------------------------|
| API         | 5000         | http://localhost:5000      |
| Prometheus  | 9090         | http://localhost:9090      |
| Grafana     | 3000         | http://localhost:3000      |

---

## 🔧 Configurando o Grafana

1. Acesse o Grafana: [http://localhost:3000](http://localhost:3000)
2. Login padrão: `admin / admin`
3. Vá em **Configuration > Data Sources > Add data source**
4. Escolha **Prometheus**
5. Em URL, coloque: `http://prometheus:9090`
6. Clique em **Save & Test**

---

## 📈 Criando Dashboard

Exemplos de métricas disponíveis (via `/metrics`):

- `http_requests_received_total`
- `http_request_duration_seconds`
- `process_cpu_seconds_total`
- `process_working_set_bytes`

Você pode importar dashboards prontos do Grafana Labs, como:
- [ASP.NET Core Prometheus Dashboard #13332](https://grafana.com/grafana/dashboards/13332)

---

## 🧪 Verificando o endpoint de métricas

Acesse o endpoint direto da API:

[http://localhost:5000/metrics](http://localhost:5000/metrics)

---

## 📂 Arquivos importantes

- `Program.cs` → Middleware Prometheus ativado
- `prometheus.yml` → Configura o Prometheus para coletar métricas da API
- `docker-compose.yml` → Define os serviços (API, Prometheus, Grafana)

---

## ✅ Pronto!

Você agora consegue visualizar métricas da sua API em tempo real 🎯
