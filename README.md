# 🍕 Sistema de Pedidos para Pizzaria / Restaurante

Sistema completo de gestão de pedidos para restaurantes, desenvolvido com **ASP.NET Core Web API** e **SQL Server**, simulando um ambiente real com tablets nas mesas, cozinha e caixa.

---

## 🚀 Funcionalidades

* ✅ Abertura de pedido por mesa
* ✅ Associação de pedido ao usuário (e garçom)
* ✅ Adição de itens ao pedido
* ✅ Controle de status do pedido
* ✅ Integração com banco de dados relacional (SQL Server)
* ✅ Estrutura preparada para cálculo de total e fechamento de conta

---

## 🧠 Arquitetura

O sistema segue o modelo de separação em camadas:

* **API (Backend)** → Regras de negócio e controle dos pedidos
* **Banco de Dados** → Persistência das informações
* **Frontend (em desenvolvimento)** → Interface para mesas, cozinha e caixa

---

## 🛠️ Tecnologias utilizadas

* ASP.NET Core 8
* Entity Framework Core
* SQL Server
* Swagger (documentação e testes da API)

---

## 📦 Estrutura do Projeto

* `Controllers/` → Endpoints da API
* `Models/` → Entidades do sistema
* `Data/` → Contexto do banco de dados
* `Migrations/` → Versionamento do banco

---

## 🔌 Endpoints principais

### 📌 Abrir pedido

POST /api/pedido/abrir?mesaId=1&usuarioId=5

---

### 📌 Adicionar item (em desenvolvimento)

POST /api/pedido/adicionar-item

---

### 📌 Listar pedidos

GET /api/pedido

---

## 🧪 Como rodar o projeto

1. Clone o repositório:

```bash
git clone https://github.com/seu-usuario/seu-repo.git
```

2. Configure a connection string no `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=NomeDoSeuBanco;Trusted_Connection=True;"
}
```

3. Execute as migrations (ou crie o banco manualmente)

4. Rode o projeto:

```bash
dotnet run
```

5. Acesse o Swagger:

```
https://localhost:xxxx/swagger
```

---

## 🧾 Banco de Dados

Principais tabelas:

* **Mesa**
* **Usuario**
* **Produto**
* **Pedido**
* **PedidoItem**
* **Pagamento**

---

## 🎯 Objetivo do Projeto

* Boas práticas de desenvolvimento backend
* Modelagem de banco de dados
* Construção de APIs REST
* Simulação de sistema real de restaurante

---

## 👨‍💻 Autor

Wilson Yuri Alves
📧 [yuri.tinocoalves13@hotmail.com](mailto:yuri.tinocoalves13@hotmail.com)

---

## 📄 Licença

Este projeto é de uso livre para fins de estudo (Mas se quiser alugar como SAAS pra sua empresa, podemos conversar, basta me contatar!).
