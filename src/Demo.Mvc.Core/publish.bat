cd ClientApp
npm install
npm run build
cd..
dotnet publish ./Demo.Mvc.Core.csproj -c release-x64 -f netcoreapp2.2 -r win10-x64 -v detailed