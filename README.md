This software finds the diff between two gnucash files.

diff.Core contains the diff functionality.

diff.Cli is a console app that allows you to use find the diff between two gnucash files

diff.Tests contains tests for diff.Core

Dependencies: 
- [Gnucash](https://www.gnucash.org/download.phtml)
- [.net](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)


Tested with GnuCash 5.10 and .net 9.0 on Ubuntu 22.04

gnucash-cli must be in your PATH enviroment variable

Build:
```sh
dotnet build
```

Run:
```sh
cd diff.Cli/bin/Debug/net9.0
```
```sh
./diff.Cli --file_1, path/to/file_1, --file_2, path/to/file_2
```

Create self-contained executable (does not need the .net runtime to run):
```sh
dotnet publish diff.Cli/diff.Cli.csproj --sc
```

Test:
```sh
dotnet test
```