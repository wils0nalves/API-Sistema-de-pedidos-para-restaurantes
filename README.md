# 🍕 Sistema de Pedidos para Pizzaria / Restaurante

Sistema completo de gestão de pedidos para restaurantes, desenvolvido com **ASP.NET Core Web API** e **SQL Server**, simulando um ambiente real com tablets nas mesas, cozinha e caixa.

---

## 🚀 Funcionalidades

* ✅ **Gestão de Mesas:** Controle de status (Livre/Ocupada) em tempo real.
* ✅ **Abertura de Pedidos:** Associação automática de mesas e usuários (Garçons).
* ✅ **Cardápio Dinâmico:** Consumo de produtos direto da API com preços e categorias.
* ✅ **Carrinho de Compras:** Adição de itens com cálculo de subtotal por pedido.
* ✅ **Persistência de Dados:** Integração robusta com SQL Server via Entity Framework Core.
* 🛠️ **Painel da Cozinha:** Visualização de pedidos pendentes para produção (Em progresso).

---

## 🧠 Arquitetura

O sistema segue o modelo de separação de responsabilidades:

* **Backend (C#):** API RESTful responsável pelas regras de negócio, rotas e segurança.
* **Database (SQL Server):** Modelagem relacional garantindo a integridade dos dados.
* **Frontend (JS/HTML):** Interface leve e responsiva para operação em diversos dispositivos.

---

## 🛠️ Tecnologias Utilizadas

* **Backend:** ASP.NET Core 8, Entity Framework Core.
* **Frontend:** JavaScript Moderno (Fetch API), HTML5, CSS3.
* **Banco de Dados:** Microsoft SQL Server.
* **Documentação:** Swagger UI para testes de endpoints.

---

## 🔌 Endpoints Principais

| Método | Endpoint | Descrição |
| :--- | :--- | :--- |
| **POST** | `/api/pedido/abrir` | Inicia um novo atendimento para uma mesa específica. |
| **POST** | `/api/pedido/adicionar-item` | Adiciona produtos e quantidades ao pedido aberto. |
| **GET** | `/api/mesa` | Lista todas as mesas e seus status atuais (Livre/Ocupada). |
| **GET** | `/api/produto` | Retorna a lista completa de produtos do cardápio. |

---

## 📦 Estrutura do Projeto

* `Controllers/` → Endpoints da API (Mesa, Pedido, Produto, Usuario).
* `Models/` → Entidades do sistema e DTOs para transferência de dados.
* `Data/` → Contexto do banco de dados e configurações de acesso.
* `Frontend/` → Interface do usuário (Cardápio, Login, Cozinha).

---

## 🧪 Como rodar o projeto

1. **Clone o repositório:**
   ```bash
   git clone [https://github.com/seu-usuario/seu-repo.git](https://github.com/seu-usuario/seu-repo.git)

   Banco de Dados:
2. Certifique-se de que o SQL Server está rodando e execute os scripts para as tabelas: Mesa, Usuario, Produto, Pedido, PedidoItem e Pagamento.

3. Configuração:
Ajuste a Connection String no arquivo appsettings.json.

4. Execução:
Abra o projeto no Visual Studio, pressione F5 para rodar a API e abra o arquivo cardapio.html no seu navegador.

👨‍💻 Autor
Wilson Yuri Alves
Mid-level .NET Developer
📧 yuri.tinocoalves13@hotmail.com

📄 Licença
Este projeto é de uso livre para fins de estudo e portfólio (mas se quiser contratar um modelo pro seu negócio basta me contatar!).
