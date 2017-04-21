MSBUILD = msbuild
CONFIGURATION = Release
BIN = bin/$(CONFIGURATION)
MDOC = $(BIN)/mdoc.exe

all: build

build: $(MDOC)

$(MDOC): $(BIN)/ICSharpCode.SharpZipLib.dll $(BIN)/Mono.Cecil.dll
	$(MSBUILD) apidoctools.sln /p:Configuration=$(CONFIGURATION)

prepare:
	-mkdir -p bin/$(CONFIGURATION)
	git submodule update --init --recursive
	nuget install NUnit.Console -version 3.6.0 -NoCache -o packages

$(BIN)/ICSharpCode.SharpZipLib.dll:
	$(MSBUILD) external/SharpZipLib/ICSharpCode.SharpZipLib.NET45/ICSharpCode.SharpZipLib.csproj /p:Configuration=Release
	cp external/SharpZipLib/bin/Release/ICSharpCode.SharpZipLib.* $(BIN)/

$(BIN)/Mono.Cecil.dll:
	$(MSBUILD) external/Mono.Cecil/Mono.Cecil.sln /p:Configuration=net_4_5_Release
	cp external/Mono.Cecil/bin/net_4_0_Release/Mono.Cecil*.dll $(BIN)/

clean:
	$(MSBUILD) apidoctools.sln /t:clean
	$(MSBUILD) external/SharpZipLib/ICSharpCode.SharpZipLib.NET45/ICSharpCode.SharpZipLib.csproj /t:clean
	$(MSBUILD) external/Mono.Cecil/Mono.Cecil.sln /t:clean
	rm -rf bin/$(CONFIGURATION)

check: build check-monodoc check-mdoc

check-mdoc:
	cd mdoc; $(MAKE) check

check-monodoc:
	cd monodoc; $(MAKE) check
