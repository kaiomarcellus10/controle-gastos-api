# 💰 Controle de Gastos API

API REST desenvolvida em **.NET 8** com **Entity Framework Core** e **SQLite** para gerenciamento de receitas e despesas por pessoa.

---

## 🚀 Tecnologias Utilizadas

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- Swagger (OpenAPI)

---

## 📦 Estrutura do Projeto

### Entidades

#### Pessoa
- Id (Guid)
- Nome
- Idade
- Relacionamento 1:N com Transações

#### Categoria
- Id (Guid)
- Descricao
- Finalidade (Despesa ou Receita)

#### Transacao
- Id (Guid)
- Descricao
- Valor
- Tipo (Despesa ou Receita)
- CategoriaId (FK)
- PessoaId (FK)

## 📌 Regras de Negócio Implementadas

- Pessoa menor de 18 anos não pode registrar Receita
- Transação do tipo Despesa só pode usar categoria de finalidade Despesa
- Transação do tipo Receita só pode usar categoria de finalidade Receita

Validações retornam **HTTP 400 (BadRequest)** com mensagem personalizada.

## 🔄 Endpoints Disponíveis

### Pessoas
- GET /api/Pessoas
- POST /api/Pessoas

### Categorias
- GET /api/Categorias
- POST /api/Categorias

### Transações
- GET /api/Transacoes
- GET /api/Transacoes/{id}
- POST /api/Transacoes
- DELETE /api/Transacoes/{id}

## ⚙️ Como Executar o Projeto

Execute os comandos abaixo na raiz do projeto:

```
dotnet restore
dotnet ef database update
dotnet run
```

A API estará disponível em:

```
https://localhost:xxxx/swagger
```