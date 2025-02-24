# PharmaWeb

PharmaWeb é um sistema de gestão de pedidos e estoque de medicamentos, desenvolvido com as tecnologias mais modernas e eficientes. Ele oferece um fluxo completo para o gerenciamento de pedidos de clientes, controle de estoque de medicamentos e matéria-prima, e facilita o processo de criação e manutenção de ordens.

## Funcionalidades principais:

- **Cadastro de Medicamentos**: Permite adicionar e atualizar informações sobre os medicamentos, incluindo a quantidade em estoque e o preço.
- **Cadastro de Clientes**: Gerencia as informações dos clientes, com fácil visualização e edição.
- **Gestão de Pedidos**: Os pedidos podem ser feitos com a seleção de medicamentos disponíveis, e o sistema calcula automaticamente o total de cada pedido.
- **Controle de Estoque**: Após a criação de um pedido, o sistema atualiza automaticamente o estoque dos medicamentos, verificando a quantidade disponível.
- **Composição de Medicamentos**: É possível cadastrar a composição de um medicamento, incluindo as matérias-primas utilizadas.
- **API RESTful**: O sistema é acessível através de uma API RESTful, permitindo integração com outros sistemas.

## Tecnologias Utilizadas:

- **Backend**: C# com ASP.NET Core
- **Banco de Dados**: SQL Server
- **Bibliotecas e Frameworks**: Entity Framework Core
- **API**: RESTful, com autenticação e comunicação com o banco de dados via HTTP

## Como Executar:

1. Clone este repositório em sua máquina.
2. Abra o projeto no Visual Studio.
3. Execute o banco de dados e configure a string de conexão.
4. Compile e inicie o projeto.
---

# **Sumário API**

1. [Endpoints](#endpoints)
    - [**/api/rawmaterial**](#apirawmaterial)
    - [**/api/medicine**](#apimedicine)
    - [**/api/order**](#apiorder)
    - [**/api/client**](#apiclient)
---

# **Documentação da API - PharmaWeb**   
---

### **1. Endpoints**

#### **/api/rawmaterial**

##### **GET** - **Obter todos as materias primas**
- **URL**: `/api/rawmaterial`
- **Método**: GET
- **Descrição**: Retorna todos as materias primas cadastrados no sistema.
- **Resposta**:
    - **Código 200**: Lista de materiais brutos.
    - **Código 500**: Erro ao tentar obter os materiais.

##### **GET {id}** - **Obter materia prima por ID**
- **URL**: `/api/rawmaterial/{id}`
- **Método**: GET
- **Descrição**: Retorna uma materia prima com base no ID fornecido.
- **Parâmetros**:
    - `id` (path): ID do material bruto.
- **Resposta**:
    - **Código 200**: Materia prima encontrado.
    - **Código 404**: Materia prima não encontrado.
    - **Código 500**: Erro ao buscar material bruto.

##### **POST** - **Criar materia prima**
- **URL**: `/api/rawmaterial`
- **Método**: POST
- **Descrição**: Cria um novo materia prima.
- **Corpo**:
    ```json
    {
    "name": "Sulfato de Magnésio",
    "description": "Utilizado em formulações para laxantes e antiácidos.",
    "supplier": "ChemCorp",
    "stockQuantity": 550,
    "expirationDate": "2026-06-30T00:00:00"
    }
    ```
- **Resposta**:
    - **Código 201**: materia prima criado com sucesso.
    - **Código 500**: Erro ao criar materia prima.

##### **PUT {id}** - **Atualizar materia prima**
- **URL**: `/api/rawmaterial/{id}`
- **Método**: PUT
- **Descrição**: Atualiza um materia prima existente.
- **Parâmetros**:
    - `id` (path): ID do materia prima a ser atualizado.
- **Corpo**:
    ```json
    {
    "rawMaterialId": 6,
    "name": "Sulfato de magnésio",
    "description": "Utilizado em formulações para laxantes e antiácidos",
    "supplier": "ChemCorpLtda",
    "stockQuantity": 700,
    "expirationDate": "2026-06-26T00:00:00"
    }
    ```
- **Resposta**:
    - **Código 204**: Atualização bem-sucedida.
    - **Código 400**: ID não encontrado.
    - **Código 500**: Erro ao atualizar materia prima.

##### **DELETE {id}** - **Deletar materia prima**
- **URL**: `/api/rawmaterial/{id}`
- **Método**: DELETE
- **Descrição**: Deleta um materia prima existente.
- **Parâmetros**:
    - `id` (path): ID do materia prima a ser deletado.
- **Resposta**:
    - **Código 204**: Deletado com sucesso.
    - **Código 404**: RawMaterial não encontrado.
    - **Código 500**: Erro ao deletar RawMaterial.

---

#### **/api/medicine**

##### **GET** - **Obter todos os medicamentos**
- **URL**: `/api/medicine`
- **Método**: GET
- **Descrição**: Retorna todos os medicamentos cadastrados no sistema.
- **Resposta**:
    - **Código 200**: Lista de medicamentos.
    - **Código 500**: Erro ao tentar obter medicamentos.

##### **GET {id}** - **Obter medicamento por ID**
- **URL**: `/api/medicine/{id}`
- **Método**: GET
- **Descrição**: Retorna um medicamento com base no ID fornecido.
- **Parâmetros**:
    - `id` (path): ID do medicamento.
- **Resposta**:
    - **Código 200**: Medicamento encontrado.
    - **Código 404**: Medicamento não encontrado.
    - **Código 500**: Erro ao buscar medicamento.

##### **POST** - **Criar medicamento**
- **URL**: `/api/medicine`
- **Método**: POST
- **Descrição**: Cria um novo medicamento.
- **Corpo**:
    ```json
    {
    "name": "Paracetamol",
    "description": "Analgésico",
    "price": 12.50,
    "stockQuantity": 100,
    "composition": [
        { "rawMaterialId": 8 },
        { "rawMaterialId": 6 }
      ]
    }
    ```
- **Resposta**:
    - **Código 201**: Medicamento criado com sucesso.
    - **Código 500**: Erro ao criar medicamento.

##### **PUT {id}** - **Atualizar medicamento**
- **URL**: `/api/medicine/{id}`
- **Método**: PUT
- **Descrição**: Atualiza um medicamento existente.
- **Parâmetros**:
    - `id` (path): ID do medicamento a ser atualizado.
- **Corpo**:
    ```json
    {
      "$id": "1",
      "medicineId": 2,
      "name": "Dorflex",
      "description": "Analgésico e relaxante muscular para dores nas costas, composto por Dipirona e outras substâncias.",
      "price": 18.80,
      "stockQuantity": 200
    }
    ```
- **Resposta**:
    - **Código 204**: Atualização bem-sucedida.
    - **Código 400**: ID inconsistente.
    - **Código 500**: Erro ao atualizar medicamento.

##### **DELETE {id}** - **Deletar medicamento**
- **URL**: `/api/medicine/{id}`
- **Método**: DELETE
- **Descrição**: Deleta um medicamento existente.
- **Parâmetros**:
    - `id` (path): ID do medicamento a ser deletado.
- **Resposta**:
    - **Código 204**: Deletado com sucesso.
    - **Código 404**: Medicamento não encontrado.
    - **Código 500**: Erro ao deletar medicamento.

---

#### **/api/order**

##### **GET** - **Obter todos os pedidos**
- **URL**: `/api/order`
- **Método**: GET
- **Descrição**: Retorna todos os pedidos cadastrados no sistema.
- **Resposta**:
    - **Código 200**: Lista de pedidos.
    - **Código 500**: Erro ao tentar obter pedidos.

##### **GET {id}** - **Obter pedido por ID**
- **URL**: `/api/order/{id}`
- **Método**: GET
- **Descrição**: Retorna um pedido com base no ID fornecido.
- **Parâmetros**:
    - `id` (path): ID do pedido.
- **Resposta**:
    - **Código 200**: Pedido encontrado.
    - **Código 404**: Pedido não encontrado.
    - **Código 500**: Erro ao buscar pedido.

##### **POST** - **Criar pedido**
- **URL**: `/api/order`
- **Método**: POST
- **Descrição**: Cria um novo pedido.
- **Corpo**:
    ```json
    {
      "clientId": 1,
      "ordersMedicines": [
        {
          "medicineId": 3,
          "quantity": 1
        },
        {
          "medicineId": 4,
          "quantity": 1
        }
      ]
    }  
    ```
- **Resposta**:
    - **Código 201**: Pedido criado com sucesso.
    - **Código 500**: Erro ao criar pedido.

##### **PUT {id}** - **Atualizar pedido**
- **URL**: `/api/order/{id}`
- **Método**: PUT
- **Descrição**: Atualiza um pedido existente.
- **Parâmetros**:
    - `id` (path): ID do pedido a ser atualizado.
- **Corpo**:
    ```json
    {
      "orderId": 3,
      "orderDate": "2025-02-21T00:00:00",
      "orderTotal": 0,
      "clientId": 2,
      "ordersMedicines": [
        {
          "orderId": 3,
          "medicineId": 3,
          "quantity": 2
        },
        {
          "orderId": 3,
          "medicineId": 4,
          "quantity": 2
        }
      ]
    }
    ```
- **Resposta**:
    - **Código 204**: Atualização bem-sucedida.
    - **Código 400**: ID inconsistente.
    - **Código 500**: Erro ao atualizar pedido.

##### **DELETE {id}** - **Deletar pedido**
- **URL**: `/api/order/{id}`
- **Método**: DELETE
- **Descrição**: Deleta um pedido existente.
- **Parâmetros**:
    - `id` (path): ID do pedido a ser deletado.
- **Resposta**:
    - **Código 204**: Deletado com sucesso.
    - **Código 404**: Pedido não encontrado.
    - **Código 500**: Erro ao deletar pedido.
---

##### **/api/client**

##### **GET** - **Obter todos os clientes**
- **URL**: `/api/client`
- **Método**: GET
- **Descrição**: Retorna todos os clientes cadastrados no sistema.
- **Resposta**:
    - **Código 200**: Lista de clientes.
    - **Código 500**: Erro ao tentar obter clientes.

##### **GET {id}** - **Obter cliente por ID**
- **URL**: `/api/client/{id}`
- **Método**: GET
- **Descrição**: Retorna um cliente com base no ID fornecido.
- **Parâmetros**:
    - `id` (path): ID do cliente.
- **Resposta**:
    - **Código 200**: Cliente encontrado.
    - **Código 404**: Cliente não encontrado.
    - **Código 500**: Erro ao buscar cliente.

##### **POST** - **Criar cliente**
- **URL**: `/api/client`
- **Método**: POST
- **Descrição**: Cria um novo cliente.
- **Corpo**:
    ```json
    {
        "name": "Joao Pedro Almeida",
        "cpf": "36587854502",
        "address": "Avenida Pereira Barreto, 55",
        "cellphone": "11968565252",
        "email": "joao.pedro@email.com"
    }
    ```
- **Resposta**:
    - **Código 201**: Cliente criado com sucesso.
    - **Código 500**: Erro ao criar cliente.

##### **PUT {id}** - **Atualizar cliente**
- **URL**: `/api/client/{id}`
- **Método**: PUT
- **Descrição**: Atualiza um cliente existente.
- **Parâmetros**:
    - `id` (path): ID do cliente a ser atualizado.
- **Corpo**:
    ```json
    {
        "clientId": 3,
        "name": "Carlos Santos Ramos",
        "cpf": "34567890126",
        "address": "Rua C, 300, Belo Horizonte",
        "cellphone": "31977770003",
        "email": "carlos.santos@email.com",
        "orders": {
          "$id": "2",
          "$values": []
        }
    }
    ```
- **Resposta**:
    - **Código 204**: Atualização bem-sucedida.
    - **Código 400**: ID inconsistente.
    - **Código 500**: Erro ao atualizar cliente.

##### **DELETE {id}** - **Deletar cliente**
- **URL**: `/api/client/{id}`
- **Método**: DELETE
- **Descrição**: Deleta um cliente existente.
- **Parâmetros**:
    - `id` (path): ID do cliente a ser deletado.
- **Resposta**:
    - **Código 204**: Deletado com sucesso.
    - **Código 404**: Cliente não encontrado.
    - **Código 500**: Erro ao deletar cliente.

---
