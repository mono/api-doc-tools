CONFIGURATION = Release
ENVIRONMENT = notwsl#use 'wsl' when running on wsl

all: build

build:
	dotnet build apidoctools.sln -c $(CONFIGURATION)

clean:
	dotnet clean apidoctools.sln -c $(CONFIGURATION)

check: build check-monodoc check-mdoc

check-mdoc:
	cd mdoc; $(MAKE) check -B

nuget:
	dotnet pack mdoc/mdoc.csproj -o bin/Nuget

check-monodoc:
	cd monodoc; $(MAKE) check -B
