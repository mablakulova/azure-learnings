# AzureBlobManager
[![Azure](https://img.shields.io/badge/Azure-0089D6?logo=microsoft-azure&logoColor=white)](https://azure.microsoft.com)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com)

A sample .NET solution demonstrating Azure cloud services integration with modern architecture.

## 📁 Solution Structure

```text
AzureBlobManager.sln
├── src/
│   ├── AzureBlobManager.Domain      # Entities, value objects, interfaces
│   ├── AzureBlobManager.Application # Commands, queries, MediatR handlers
│   ├── AzureBlobManager.Infrastructure
│   │      # EF Core DbContext, ServiceBus & Blob Storage adapters
│   └── AzureBlobManager.WebApi      # ASP.NET Core Web API project
```

## 🛠️ Azure Services Covered
- **Azure Database for PostgreSQL** - Relational database service
- **Azure Blob Storage** - File storage implementation
- **Azure Service Bus** - Message queue for file processing
- **Azure Key Vault** - Secrets management
- **Azure Application Insights** - Application monitoring
- **Azure App Services** - Deployment target

## 🎯 Learning Objectives
- Cloud-native application development
- Azure PaaS service integration
- Secure secret management
- Distributed system monitoring