# 📦 Glossário de Pacotes NuGet - TaskMaster API

Este documento consolida todas as bibliotecas e pacotes externos utilizados no ecossistema TaskMaster, detalhando suas versões, onde são aplicados e sua finalidade técnica.

| Pacote NuGet | Versão | Projeto(s) | Descrição / Finalidade |
| :--- | :--- | :--- | :--- |
| **Microsoft.AspNetCore.OpenApi** | 10.0.2 | `Api` | Fornece suporte nativo ao OpenAPI para geração de especificações de API no .NET 10. |
| **Scalar.AspNetCore** | 2.12.27 | `Api` | Interface interativa moderna (sucessora do Swagger UI) para documentação e teste de endpoints. |
| **Dapper** | 2.1.66 | `Infrastructure` | Micro-ORM extremamente rápido focado em performance e execução de SQL puro. |
| **Microsoft.Data.SqlClient** | 6.1.4 | `Infrastructure` | Driver oficial de conectividade para Microsoft SQL Server. |
| **Microsoft.EntityFrameworkCore** | 10.0.2 | `Infrastructure` | ORM principal utilizado para mapeamento objeto-relacional e produtividade no acesso a dados. |
| **Microsoft.EntityFrameworkCore.SqlServer** | 10.0.2 | `Infrastructure` | Provedor do Entity Framework Core específico para Microsoft SQL Server. |
| **Microsoft.EntityFrameworkCore.InMemory** | 10.0.2 | `Infrastructure` | Banco de dados em memória usado para desenvolvimento rápido e testes integrados. |
| **Microsoft.Extensions.Configuration.Abs** | 10.0.2 | `Infrastructure` | Abstrações para o sistema de configuração do .NET (usado para ler `appsettings.json`). |
| **xunit** | 2.9.3 | `UnitTests` | Framework principal para criação e execução de testes automatizados. |
| **Microsoft.NET.Test.Sdk** | 17.14.1 | `UnitTests` | SDK necessário para que o ambiente .NET reconheça e execute o projeto de testes. |
| **coverlet.collector** | 6.0.4 | `UnitTests` | Coletor usado para medir a cobertura de código durante a execução dos testes. |
| **xunit.runner.visualstudio** | 3.1.4 | `UnitTests` | Integração que permite rodar testes do xUnit diretamente pelo Test Explorer do Visual Studio/VS Code. |

---

### Por que documentar pacotes?
Manter um inventário claro de dependências é uma prática **Sênior** que auxilia em:
1.  **Onboarding:** Novos desenvolvedores entendem rapidamente o stack tecnológico.
2.  **Segurança:** Facilita a auditoria de versões e identificação de vulnerabilidades conhecidas.
3.  **Manutenção:** Planejamento de upgrades de versão de forma centralizada.
