# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

COPY diff.Api/. ./diff.Api/
COPY diff.Core/. ./diff.Core/
COPY NetCash/. ./NetCash/
WORKDIR /source/diff.Api
RUN dotnet restore
RUN dotnet publish -c Debug -o /app

# final stage/image
FROM ubuntu:24.04

#gnucash
RUN apt update && apt install -y build-essential git cmake libglib2.0-dev libgtk-3-dev guile-3.0-dev libxml2 gettext libxslt1-dev libicu-dev swig3.0 libwebkit2gtk-4.1-dev libdbi-dev zlib1g-dev libgwengui-gtk3-dev libaqbanking-dev libofx-dev xsltproc libgtest-dev libboost-all-dev libsecret-1-dev libdbd-mysql
WORKDIR /setup
RUN git clone https://github.com/Gnucash/gnucash.git 
WORKDIR /setup/gnucash
RUN git reset --hard a716cca
WORKDIR /setup/gnucash-build
RUN cmake ../gnucash
RUN make
RUN make install


#aspnetcore
RUN apt update && apt install -y software-properties-common
RUN add-apt-repository ppa:dotnet/backports
RUN apt update && apt install -y aspnetcore-runtime-9.0 


WORKDIR /app
COPY --from=build /app ./

ENTRYPOINT ["dotnet", "diff.Api.dll"]