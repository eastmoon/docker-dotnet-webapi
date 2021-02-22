FROM mcr.microsoft.com/dotnet/sdk:3.1

# Install dotnet tools
RUN \
  dotnet tool install -g swashbuckle.aspnetcore.cli --version 5.6.3

# Setting Container
EXPOSE 5000/tcp
EXPOSE 5001/tcp
WORKDIR /repo
