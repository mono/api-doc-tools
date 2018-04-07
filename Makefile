MSBUILD = msbuild
CONFIGURATION = Release
BIN = bin/$(CONFIGURATION)
MDOC = $(BIN)/mdoc.exe

all: build

build: $(MDOC)

$(MDOC):
	$(MSBUILD) apidoctools.sln /p:Configuration=$(CONFIGURATION);

prepare:
	git submodule update --init --recursive
	nuget restore apidoctools.sln
	nuget install NUnit.Console -version 3.6.0 -NoCache -o packages

clean:
	#$(MSBUILD) apidoctools.sln /t:clean
	rm -rf bin/$(CONFIGURATION)

check: build check-monodoc check-mdoc

check-mdoc:
	cd mdoc; $(MAKE) check -B

nuget:
	nuget pack mdoc/mdoc.nuspec -outputdirectory bin/Nuget

check-monodoc:
	cd monodoc; $(MAKE) check -B
