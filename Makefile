CONFIGURATION = Release
ENVIRONMENT = notwsl#use 'wsl' when running on wsl

all: build

clean:
	dotnet clean apidoctools.sln -c $(CONFIGURATION)

build:
	dotnet build apidoctools.sln -c $(CONFIGURATION)

nuget:
	dotnet pack src/mdoc/mdoc.csproj -o bin/Nuget

check: build test check-monodoc check-mdoc

test:
	dotnet test apidoctools.sln -c $(CONFIGURATION)

check-mdoc:
	cd tests/mdoc; $(MAKE) check -B

check-monodoc:
	dotnet test -c $(CONFIGURATION)
