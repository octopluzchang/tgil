FROM mcr.microsoft.com/dotnet/aspnet:5.0
COPY publish/ App/
WORKDIR /App
ENTRYPOINT ["dotnet", "PAMO_TapPay.dll"]
ENV DOTNET_EnableDiagnostics=0