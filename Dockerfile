FROM microsoft/aspnetcore:2.0
COPY . app/
# Setup NodeJs
RUN apt-get update && \
    apt-get install -y wget && \
    apt-get install -y gnupg2 && \
    wget -qO- https://deb.nodesource.com/setup_6.x | bash - && \
    apt-get install -y build-essential nodejs
# End setup
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "AplicacaoCondominial.dll"]
