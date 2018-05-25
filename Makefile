MSBUILD = msbuild
CONFIGURATION = Release
BIN = bin/$(CONFIGURATION)
MDOC = $(BIN)/mdoc.exe

all: build

build: $(MDOC)

$(MDOC):
	$(MSBUILD) apidoctools.sln /r /p:Configuration=$(CONFIGURATION);

prepare:
	git submodule update --init --recursive
	nuget install NUnit.Console -version 3.8.0 -NoCache -o packages

clean:
	#$(MSBUILD) apidoctools.sln /t:clean
	rm -rf bin/$(CONFIGURATION)

check: build check-monodoc check-mdoc

check-mdoc:
	cd mdoc; $(MAKE) check -B

nuget:
	nuget pack mdoc/mdoc.nuspec -outputdirectory bin/Nuget
	$(MSBUILD) monodoc/monodoc.csproj /p:Configuration=$(CONFIGURATION) /t:Pack

check-monodoc:
	cd monodoc; $(MAKE) check -B
