#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["Directory.Packages.props", "."]
COPY ["NuGet.config", "."]
COPY ["backend/", "backend/"]
COPY ["frontend/", "frontend/"]
COPY ["shared/", "shared/"]
COPY ["SharedWebComponents", "SharedWebComponents/"]
RUN dotnet restore "backend/MinimalApi.csproj"

WORKDIR "/src/backend"
FROM build AS publish
RUN dotnet publish "MinimalApi.csproj" -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MinimalApi.dll"]

ENV APPLICATIONINSIGHTS_CONNECTION_STRING="InstrumentationKey=3cb6b9dd-9621-4fa2-9a31-a282cf9b0c9b;IngestionEndpoint=https://eastus-8.in.applicationinsights.azure.com/;LiveEndpoint=https://eastus.livediagnostics.monitor.azure.com/;ApplicationId=bb267ed9-ae57-4428-b4a1-bba494c9fbde"
ENV APPLICATIONINSIGHTS_NAME="appi-yxbuwmswtbugu"
ENV AZD_PIPELINE_PROVIDER="github"
ENV AZD_PREPDOCS_RAN="true"
ENV AZURE_COMPUTERVISION_RESOURCE_GROUP=""
ENV AZURE_COMPUTERVISION_SERVICE=""
ENV AZURE_COMPUTERVISION_SERVICE_ENDPOINT=""
ENV AZURE_CONTAINER_ENVIRONMENT_NAME="cae-yxbuwmswtbugu"
ENV AZURE_CONTAINER_REGISTRY_ENDPOINT="cryxbuwmswtbugu.azurecr.io"
ENV AZURE_CONTAINER_REGISTRY_NAME="cryxbuwmswtbugu"
ENV AZURE_CONTAINER_REGISTRY_RESOURCE_GROUP="cryxbuwmswtbugu"
ENV AZURE_ENV_NAME="GAO-DEV-ENV"
ENV AZURE_FORMRECOGNIZER_RESOURCE_GROUP="rg-azure-search-openai-demo-csharp"
ENV AZURE_FORMRECOGNIZER_SERVICE="cog-fr-yxbuwmswtbugu"
ENV AZURE_FORMRECOGNIZER_SERVICE_ENDPOINT="https://cog-fr-yxbuwmswtbugu.cognitiveservices.azure.com/"
ENV AZURE_KEY_VAULT_ENDPOINT="https://kv-yxbuwmswtbugu.vault.azure.net/"
ENV AZURE_KEY_VAULT_NAME="kv-yxbuwmswtbugu"
ENV AZURE_KEY_VAULT_RESOURCE_GROUP="rg-azure-search-openai-demo-csharp"
ENV AZURE_LOCATION="eastus"
ENV AZURE_OPENAI_CHATGPT_DEPLOYMENT="chat"
ENV AZURE_OPENAI_CHATGPT_MODEL_NAME="gpt-35-turbo"
ENV AZURE_OPENAI_CHATGPT_MODEL_VERSION="0613"
ENV AZURE_OPENAI_EMBEDDING_DEPLOYMENT="embedding"
ENV AZURE_OPENAI_ENDPOINT="https://cog-yxbuwmswtbugu.openai.azure.com/"
ENV AZURE_OPENAI_RESOURCE_GROUP="rg-azure-search-openai-demo-csharp"
ENV AZURE_OPENAI_RESOURCE_LOCATION="eastus"
ENV AZURE_OPENAI_SERVICE="cog-yxbuwmswtbugu"
ENV AZURE_PIPELINE_CLIENT_ID="1c2dabc3-a1d1-410f-bde3-7404e74b1eee"
ENV AZURE_RESOURCE_GROUP="rg-azure-search-openai-demo-csharp"
ENV AZURE_SEARCH_INDEX="gptkbindex"
ENV AZURE_SEARCH_SERVICE="gptkb-yxbuwmswtbugu"
ENV AZURE_SEARCH_SERVICE_ENDPOINT="https://gptkb-yxbuwmswtbugu.search.windows.net/"
ENV AZURE_SEARCH_SERVICE_RESOURCE_GROUP="rg-azure-search-openai-demo-csharp"
ENV AZURE_STORAGE_ACCOUNT="styxbuwmswtbugu"
ENV AZURE_STORAGE_BLOB_ENDPOINT="https://styxbuwmswtbugu.blob.core.windows.net/"
ENV AZURE_STORAGE_CONTAINER="content"
ENV AZURE_STORAGE_RESOURCE_GROUP="rg-azure-search-openai-demo-csharp"
ENV AZURE_SUBSCRIPTION_ID="bfb3a031-f26f-4783-b278-60173de9ccf4"
ENV AZURE_TENANT_ID="e362c86f-e64a-4e58-b26a-c8bf314b1093"
ENV OPENAI_EMBEDDING_DEPLOYMENT="text-embedding-ada-002"
ENV SERVICE_FUNCTION_IDENTITY_PRINCIPAL_ID="3bd0ae1a-3bdb-42c7-809a-48d9cc9a2e61"
ENV SERVICE_WEB_IDENTITY_NAME="id-web-yxbuwmswtbugu"
ENV SERVICE_WEB_IMAGE_NAME="cryxbuwmswtbugu.azurecr.io/azure-search-openai-demo-csharp/web-azure-search-openai-demo-csharp:azd-deploy-1724348205"
ENV SERVICE_WEB_NAME="ca-web-yxbuwmswtbugu"
ENV SERVICE_WEBROOT_PATH="/app"
ENV SERVICE_WEB_RESOURCE_EXISTS="false"
ENV USE_AOAI="true"
ENV USE_VISION="false"
ENV OpenAIApiKey="sk-og2vq2cEbh4tmDmWhrfz48N7suD1XouTYX-8uASwy3T3BlbkFJBzcHL0NBm9hRU2OPR-_2hpCy376bEkrDeu6BM7dlkA"