CONFIGURATION = Release
BIN = bin/$(CONFIGURATION)
MDOC = $(BIN)/mdoc.exe
ENVIRONMENT = notwsl#use 'wsl' when running on wsl

all: build

build: $(MDOC)

$(MDOC):
	dotnet build -v:n apidoctools.sln /p:Configuration=$(CONFIGURATION)

prepare:
	git submodule update --init --recursive
	dotnet restore apidoctools.sln
	nuget install NUnit.Console -version 3.6.0 -NoCache -o packages

clean:
	dotnet build -v:n apidoctools.sln /t:clean /p:Configuration=$(CONFIGURATION)
	rm -rf bin/$(CONFIGURATION)

check: build check-monodoc check-mdoc

check-mdoc:
	$(MAKE) check -B -C mdoc

nuget:
	nuget pack mdoc/mdoc.nuspec -outputdirectory bin/Nuget

check-monodoc:
	$(MAKE) check -B -C monodoc

zip:
	rm -f $(BIN)/mdoc*.zip
	zip -j $(BIN)/mdoc.zip $(BIN)/*
	