CONFIGURATION = Release
ENVIRONMENT = notwsl#use 'wsl' when running on wsl

all: build

build:
	dotnet build apidoctools.sln -c $(CONFIGURATION)

clean:
	dotnet clean apidoctools.sln -c $(CONFIGURATION)

check: build check-monodoc check-mdoc

nuget:
	dotnet pack src/mdoc/mdoc.csproj -o bin/Nuget

check-mdoc:
	cd tests/mdoc; $(MAKE) check -B

check-monodoc:
	dotnet test -c $(CONFIGURATION)
