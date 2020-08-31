FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base

ARG UID=9999
ARG USERNAME=appuser
ARG GROUP=appgroup

ENV USERNAME=${USERNAME}
ENV UID=${UID}
ENV GROUP=${GROUP}
ENV HOME=/home/${USERNAME}

WORKDIR /

RUN groupadd -r -g ${UID} ${GROUP} && \
    useradd -r -u ${UID} -g ${GROUP} -d ${HOME} -s /sbin/nologin -c "app user" ${USERNAME}
RUN mkdir -p ${HOME}/app && \
    chown -R ${USERNAME} ${HOME}
 


FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /

# reference cache
COPY ["src/LogBook/LogBook.csproj", "src/LogBook/"]
COPY ["src/LogBook.WebApi/LogBook.WebApi.csproj", "src/LogBook.WebApi/"]
RUN dotnet restore "src/LogBook/LogBook.csproj"
RUN dotnet restore "src/LogBook.WebApi/LogBook.WebApi.csproj"


COPY ["src", "src/"]

WORKDIR "/src/LogBook.WebApi"
RUN dotnet build "LogBook.WebApi.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "LogBook.WebApi.csproj" -c Release -o /app/publish


FROM base AS final
USER ${USERNAME}
WORKDIR ${HOME}/app

COPY --chown=${USERNAME}:${GROUP} --from=publish /app/publish ./

ENTRYPOINT ["dotnet", "LogBook.WebApi.dll"]

#CMD tail -f /dev/null 