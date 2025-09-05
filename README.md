# 📘 Azure Services Demo

This project showcases a full-stack Azure-integrated workflow built with Angular and .NET. It demonstrates how numeric input from a frontend is processed through a resilient, message-driven architecture using multiple Azure services.  


## 🌐 Live Demo

Explore the deployed Angular frontend and Azure-integrated workflow here:  
🔗 [Azure Integration Demo](https://delightful-desert-009c44900.2.azurestaticapps.net/azure-integration-demo)


## 🚀 Architecture Overview

An Angular frontend posts numeric input to a .NET Web API, which uses Polly for resilient HTTP calls to an Azure Function. That function publishes the number to an Azure Service Bus topic. A subscriber Azure Function then routes the number to either an API App or a Container Instance based on its odd/evenness — completing a robust, cloud-native message-driven pipeline.


## 🧩 Azure Services Used

- Azure Static Web App
- Azure API App
- Azure Function
- Azure Service Bus
- Azure Container Instance
- Docker Hub (Hosted [Image](https://hub.docker.com/r/gsoft85512/docker-number-minimal-api))


## 🛠️ Step-by-Step Flow

| Step | Azure Resource             | Description |
|------|----------------------------|-------------|
| 1    | Azure Static Web App       | Accepts numeric input and posts it to a backend .NET Web API hosted on Azure App Service. This is available in a separate Git repo. [Angular Component Code](https://github.com/buddhika85/Full-Stack-Demo-App/blob/main/Emp.Angular/src/app/components/azure-demo/azure-data-form/azure-data-form.ts) |
| 2    | Azure API App              | Receives the number, invokes an HTTP-triggered Azure Function, uses `HttpClientFactory`, and implements `Polly` with exponential backoff. This is available in a separate Git repo. [ASP.NET Web API Controller Code](https://github.com/buddhika85/Full-Stack-Demo-App/blob/main/Backend/Emp.Api/Controllers/AzureIntegrationController.cs) |
| 3    | Azure Function (Publisher) | Triggered by HTTP requests, accepts the number, and publishes it to an Azure Service Bus topic. [Azure Function Code](https://github.com/buddhika85/Full-Stack-Demo-Azure-Functions/blob/main/interview-azure-function-app/PushToAzureServiceBus.cs) |
| 4    | Azure Service Bus          | Hosts a topic named `OddEven`, which receives messages from the publisher function. |
| 5    | Azure Function (Subscriber)| Triggered by Service Bus topic messages, routes the number based on odd/evenness. If Even → posts to Azure API App, If Odd → posts to Azure Container Instance. [Azure Function Code](https://github.com/buddhika85/Full-Stack-Demo-Azure-Functions/blob/main/interview-azure-function-app/SubscribeServiceBusTopicAzureFunction.cs) |
| 6    | Azure API App              | Built as a .NET 8 Minimal API, stores even numbers in EF Core In-Memory DB, and exposes a `Get All` endpoint. [.NET Minimal API Project] (https://github.com/buddhika85/Full-Stack-Demo-Azure-Functions/tree/main/number-Minimal-API) |
| 7    | Azure Container Instance   | Hosts a public Dockerized .NET 8 Minimal API [Image](https://hub.docker.com/r/gsoft85512/docker-number-minimal-api), stores odd numbers in EF Core In-Memory DB, and exposes a `Get All` endpoint. [.NET Minimal API Project] (https://github.com/buddhika85/Full-Stack-Demo-Azure-Functions/tree/main/docker-number-Minimal-API) |
| 8    | Azure Static Web App       | Same frontend as Step 1, consumes APIs from both the API App and Container Instance to display even and odd numbers. This is available in a separate Git repo. [Angular Component Code](https://github.com/buddhika85/Full-Stack-Demo-App/blob/main/Emp.Angular/src/app/components/azure-demo/azure-data-form/azure-data-form.ts) |


## 🖼️ Workflow Diagram

![Workflow Diagram](https://raw.githubusercontent.com/buddhika85/Full-Stack-Demo-Azure-Functions/main/azure-demo-workflow-diagram.png)


## 📦 Source Code & CI/CD

- CI/CD Pipeline: Built with [GitHub Actions](https://github.com/buddhika85/Full-Stack-Demo-Azure-Functions/actions)
- Builds, tests, and deploys the .NET Web API to Azure App Service


## 📄 License

This project is open-source and available under the MIT License.


## 🎓 Acknowledgment
This example was inspired by the [.NET Developer Toolkit course  by Les Jackson](https://lesjackson.net/course/dotnet-developer-toolkit), particularly the Azure Developer Basics module. Many thanks to him for his generous guidance and educational resources.

