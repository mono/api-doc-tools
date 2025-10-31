CONFIGURATION = Release
PROGRAM = ../bin/$(CONFIGURATION)/mdoc.exe
MONO = $(if $(filter Windows_NT,$(OS)),,mono)
CSCOMPILE = mcs
VBCOMPILE = vbnc
TEST_CSCFLAGS = -target:library
ENVIRONMENT = notwsl#use 'wsl' when running on wsl

MDOC_COMMON_FLAGS = \
	/resource:../../class/monodoc/Resources/mdoc-html-format.xsl,mdoc-html-format.xsl    \
	/resource:../../class/monodoc/Resources/mdoc-html-utils.xsl,mdoc-html-utils.xsl      \
	/resource:../../class/monodoc/Resources/mdoc-sections-css.xsl,mdoc-sections-css.xsl  \
	/resource:../../class/monodoc/Resources/mono-ecma-css.xsl,mono-ecma-css.xsl          \
	/resource:Resources/defaulttemplate.xsl,defaulttemplate.xsl                 \
	/resource:Resources/monodoc-ecma.xsd,monodoc-ecma.xsd                       \
	/resource:Resources/msitomsx.xsl,msitomsx.xsl                               \
	/resource:Resources/overview.xsl,overview.xsl                               \
	/resource:Resources/stylesheet.xsl,stylesheet.xsl                           \


MONODOC_RESOURCES = \
	../../class/monodoc/Resources/mdoc-html-utils.xsl    \
	../../class/monodoc/Resources/mdoc-sections-css.xsl  \
	../../class/monodoc/Resources/mono-ecma-css.xsl

MDOC_RESOURCES = \
	Resources/defaulttemplate.xsl     \
	Resources/monodoc-ecma.xsd        \
	Resources/msitomsx.xsl            \
	Resources/overview.xsl            \
	Resources/stylesheet.xsl

MDOC_TEST_FILES = \
	../tests/mdoc/Test/CLILibraryTypes.dtd				  \
	../tests/mdoc/Test/DocTest-v1.cs                                        \
	../tests/mdoc/Test/DocTest-v2.patch                                     \
	../tests/mdoc/Test/msxdoc-expected.importslashdoc.xml                   \
	../tests/mdoc/Test/TestEcmaDocs.xml                                     \
	../tests/mdoc/Test/validate.check.monodocer                             \
	../tests/mdoc/Test/validate.check.monodocer.importslashdoc              \
	../tests/mdoc/Test/validate.check.monodocer.since

EXTRA_DISTFILES = \
	$(MDOC_RESOURCES) \
	$(MDOC_TEST_FILES)

MULTI-CLASSIC = ../tests/mdoc/Test/DocTest-DropNS-classic.dll ../tests/mdoc/Test/DocTest-DropNS-classic-multitest.dll
MULTI-UNIFIED = ../tests/mdoc/Test/DocTest-DropNS-unified.dll ../tests/mdoc/Test/DocTest-DropNS-unified-multitest.dll

DIFF = diff -rup $(if $(filter Windows_NT,$(OS)),--strip-trailing-cr,)
DIFF_QUIET = diff --brief $(if $(filter Windows_NT,$(OS)),--strip-trailing-cr,)

ifeq ($(ENVIRONMENT),wsl)
DIFF = diff -rup -Z
DIFF_QUIET = diff --brief -Z
endif

cleanup:
	-rm -Rf ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/html.actual

nunit: 
	$(MONO) ../packages/NUnit.ConsoleRunner.3.6.0/tools/nunit3-console.exe ../tests/mdoc/mdoc.Test/bin/$(CONFIGURATION)/mdoc.Test.dll

ifeq ($(filter Windows_NT,$(OS)),)

../tests/mdoc/Test/DocTest-VB-Eii.dll:
	$(VBCOMPILE) -out:../tests/mdoc/Test/DocTest-VB-Eii.dll  ../tests/mdoc/Test/ClassEnumerator.vb

../tests/mdoc/mdoc.Test.FSharp/bin/Release/mdoc.Test.FSharp.dll:
	msbuild ../tests/mdoc/mdoc.Test.FSharp/mdoc.Test.FSharp.fsproj -property:Configuration=Release

../tests/mdoc/TestInterfaceImplementation/bin/Release/TestInterfaceImplementation.dll:
	msbuild ../tests/mdoc/TestInterfaceImplementation/TestInterfaceImplementation.csproj -property:Configuration=Release

../tests/mdoc/AttachedEventsAndProperties/bin/Release/AttachedEventsAndProperties.dll:
	msbuild ../tests/mdoc/AttachedEventsAndProperties/AttachedEventsAndProperties.csproj -property:Configuration=Release

else # Full .NET Framework has extra attributes and slightly different generated types

../tests/mdoc/Test/DocTest-VB-Eii.dll:
	cp ../external/Test/DocTest-VB-Eii.dll ../tests/mdoc/Test/DocTest-VB-Eii.dll

../tests/mdoc/mdoc.Test.FSharp/bin/Release/mdoc.Test.FSharp.dll:
	cp ../external/Test/mdoc.Test.FSharp.dll ../tests/mdoc/mdoc.Test.FSharp/bin/Release/mdoc.Test.FSharp.dll

../tests/mdoc/TestInterfaceImplementation/bin/Release/TestInterfaceImplementation.dll:
	mkdir -p ../tests/mdoc/TestInterfaceImplementation/bin/Release/
	cp ../external/Test/TestInterfaceImplementation.dll ../tests/mdoc/TestInterfaceImplementation/bin/Release/TestInterfaceImplementation.dll 

../tests/mdoc/AttachedEventsAndProperties/bin/Release/AttachedEventsAndProperties.dll:
	mkdir -p ../tests/mdoc/AttachedEventsAndProperties/bin/Release/
	cp ../external/Test/AttachedEventsAndProperties.dll ../tests/mdoc/AttachedEventsAndProperties/bin/Release/AttachedEventsAndProperties.dll

endif

../tests/mdoc/Test/DocTest-addNonGeneric.dll:
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ ../tests/mdoc/Test/DocTest-addNonGeneric.cs

../tests/mdoc/Test/DocTest-addNonGeneric-v2.dll:
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ ../tests/mdoc/Test/DocTest-addNonGeneric.cs /define:V2

../tests/mdoc/Test/DocTest-DropNS-classic-secondary.dll:
	@echo $(value @)
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ ../tests/mdoc/Test/DocTest-DropNS-classic-secondary.cs -doc:../tests/mdoc/Test/DocTest-DropNS-classic-secondary.xml

../tests/mdoc/Test/DocTest-DropNS-classic.dll:
	@echo $(value @)
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ ../tests/mdoc/Test/DocTest-DropNS-classic.cs -doc:../tests/mdoc/Test/DocTest-DropNS-classic.xml

../tests/mdoc/Test/DocTest-DropNS-unified.dll:
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ ../tests/mdoc/Test/DocTest-DropNS-unified.cs

../tests/mdoc/Test/DocTest-DropNS-unified-multitest.dll:
	rm -f $@
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ ../tests/mdoc/Test/DocTest-DropNS-unified.cs /define:MULTITEST

../tests/mdoc/Test/DocTest-DropNS-classic-multitest.dll:
	rm -f $@
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ ../tests/mdoc/Test/DocTest-DropNS-classic.cs /define:MULTITEST

../tests/mdoc/Test/DocTest-DropNS-unified-deletetest.dll:
	rm -f ../tests/mdoc/Test/DocTest-DropNS-unified-deletetest.dll
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ ../tests/mdoc/Test/DocTest-DropNS-unified.cs /define:DELETETEST

../tests/mdoc/Test/DocTest-DropNS-unified-deletetest-V2.dll:
	rm -f ../tests/mdoc/Test/DocTest-DropNS-unified-deletetest.dll
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:../tests/mdoc/Test/DocTest-DropNS-unified-deletetest.dll ../tests/mdoc/Test/DocTest-DropNS-unified.cs /define:DELETETEST,V2

../tests/mdoc/Test/DocTest-DropNS-classic-deletetest.dll:
	rm -f ../tests/mdoc/Test/DocTest-DropNS-classic-deletetest.dll
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ ../tests/mdoc/Test/DocTest-DropNS-classic.cs /define:DELETETEST

../tests/mdoc/Test/DocTest-DropNS-classic-deletetest-V2.dll:
	rm -f ../tests/mdoc/Test/DocTest-DropNS-classic-deletetest.dll
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:../tests/mdoc/Test/DocTest-DropNS-classic-deletetest.dll ../tests/mdoc/Test/DocTest-DropNS-classic.cs /define:DELETETEST,V2

../tests/mdoc/Test/DocTest.dll: 
	$(CSCOMPILE) $(TEST_CSCFLAGS) -debug -optimize -unsafe -target:library -out:$@ ../tests/mdoc/Test/DocTest.cs -r:System.Core.dll -r:Microsoft.CSharp.dll

../tests/mdoc/Test/DocTest-InternalInterface.dll: 
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ ../tests/mdoc/Test/DocTest-InternalInterface.cs

../tests/mdoc/Test/DocTest-framework-inheritance-one.dll:
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ ../tests/mdoc/Test/DocTest-framework-inheritance.cs /define:FXONE

../tests/mdoc/Test/DocTest-framework-inheritance-two.dll:
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ ../tests/mdoc/Test/DocTest-framework-inheritance.cs /define:FXTWO

../tests/mdoc/Test/DocTest-framework-inheritance-three.dll:
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ ../tests/mdoc/Test/DocTest-framework-inheritance.cs /define:FXTHREE

../tests/mdoc/Test/DocTest.dll-v1: 
	-rm -f ../tests/mdoc/Test/DocTest.cs
	cp ../tests/mdoc/Test/DocTest-v1.cs ../tests/mdoc/Test/DocTest.cs
	-rm -f ../tests/mdoc/Test/DocTest.dll
	$(MAKE) ../tests/mdoc/Test/DocTest.dll

../tests/mdoc/Test/DocTest.dll-v2: 
	-rm -f ../tests/mdoc/Test/DocTest.cs
	cp ../tests/mdoc/Test/DocTest-v1.cs ../tests/mdoc/Test/DocTest.cs
	cd ../tests/mdoc/Test && patch -p0 --binary < DocTest-v2.patch
	-rm -f ../tests/mdoc/Test/DocTest.dll
	$(MAKE) ../tests/mdoc/Test/DocTest.dll

../tests/mdoc/Test/DocTest-enumerations.dll: 
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ ../tests/mdoc/Test/DocTest-enumerations.cs
	
../tests/mdoc/Test/DocTest-embedded-type.dll: 
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ ../tests/mdoc/Test/DocTest-embedded-type.cs

../tests/mdoc/Test/DocTest-differentTypeDefinitions-First.dll:
	rm -f $@
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ ../tests/mdoc/Test/DocTest-differentTypeDefinitions.cs /define:FIRST

../tests/mdoc/Test/DocTest-differentTypeDefinitions-Second.dll:
	rm -f $@
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ ../tests/mdoc/Test/DocTest-differentTypeDefinitions.cs /define:SECOND

../tests/mdoc/Test/DocTest-differentTypeParameterNames-First.dll:
	rm -f $@
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ ../tests/mdoc/Test/DocTest-differentTypeParameterNames.cs /define:FIRST

../tests/mdoc/Test/DocTest-differentTypeParameterNames-Second.dll:
	rm -f $@
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ ../tests/mdoc/Test/DocTest-differentTypeParameterNames.cs /define:SECOND

../tests/mdoc/Test/DocTest-typeForwards-First.dll:
	rm -f $@
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ ../tests/mdoc/Test/DocTest-typeForwards.cs /define:FIRST

.PHONY: ../tests/mdoc/Test/DocTest-typeForwards-Second.dll
../tests/mdoc/Test/DocTest-typeForwards-Second.dll:
	rm -f $@
	rm -f ../tests/mdoc/Test/DocTest-typeForwards-Second-First.dll
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ ../tests/mdoc/Test/DocTest-typeForwards.cs /define:FIRST
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:../tests/mdoc/Test/DocTest-typeForwards-Second-First.dll /reference:$@ ../tests/mdoc/Test/DocTest-typeForwards.cs /define:SECOND

.PHONY: ../tests/mdoc/Test/DocTest-typeForwards-Third.dll
../tests/mdoc/Test/DocTest-typeForwards-Third.dll:
	rm -f $@
	rm -f ../tests/mdoc/Test/DocTest-typeForwards-Third-First.dll
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ ../tests/mdoc/Test/DocTest-typeForwards.cs /define:FIRST
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:../tests/mdoc/Test/DocTest-typeForwards-Third-First.dll /reference:$@ ../tests/mdoc/Test/DocTest-typeForwards.cs /define:THIRD

../tests/mdoc/Test/DocTest-TypeProjection.dll:
	rm -f DocTest-TypeProjection.dll
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ ../tests/mdoc/Test/DocTest-TypeProjection.cs

# build test dll to test forwardings nested type
../tests/mdoc/Test/DocTest-nestedType-typeForwards-First.dll:
	rm -f $@
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ ../tests/mdoc/Test/DocTest-nestedType-typeForwards.cs /define:FIRST

../tests/mdoc/Test/DocTest-LongFileName.dll:
	rm -f $@
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ ../tests/mdoc/Test/DocTest-LongFileName.cs

.PHONY: ../tests/mdoc/Test/DocTest-nestedType-typeForwards-Second.dll
../tests/mdoc/Test/DocTest-nestedType-typeForwards-Second.dll:
	rm -f $@
	rm -f ../tests/mdoc/Test/DocTest-nestedType-typeForwards-Second-First.dll
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ ../tests/mdoc/Test/DocTest-nestedType-typeForwards.cs /define:FIRST
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:../tests/mdoc/Test/DocTest-nestedType-typeForwards-Second-First.dll /reference:$@ ../tests/mdoc/Test/DocTest-nestedType-typeForwards.cs /define:SECOND

.PHONY: ../tests/mdoc/Test/DocTest-nestedType-typeForwards-Third.dll
../tests/mdoc/Test/DocTest-nestedType-typeForwards-Third.dll:
	rm -f $@
	rm -f ../tests/mdoc/Test/DocTest-nestedType-typeForwards-Third-First.dll
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ ../tests/mdoc/Test/DocTest-nestedType-typeForwards.cs /define:FIRST
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:../tests/mdoc/Test/DocTest-nestedType-typeForwards-Third-First.dll /reference:$@ ../tests/mdoc/Test/DocTest-nestedType-typeForwards.cs /define:THIRD

.PHONY: ../tests/mdoc/Test/FrameworkTestData
../tests/mdoc/Test/FrameworkTestData: ../tests/mdoc/Test/DocTest-addNonGeneric.dll ../tests/mdoc/Test/DocTest-DropNS-classic.dll ../tests/mdoc/Test/DocTest-DropNS-classic-secondary.dll
	rm -rf ../tests/mdoc/Test/FrameworkTestData
	mkdir ../tests/mdoc/Test/FrameworkTestData
	mkdir ../tests/mdoc/Test/FrameworkTestData/One
	mkdir ../tests/mdoc/Test/FrameworkTestData/Two
	cp ../tests/mdoc/Test/DocTest-addNonGeneric.dll ../tests/mdoc/Test/FrameworkTestData/One/
	cp ../tests/mdoc/Test/DocTest-DropNS-classic.dll ../tests/mdoc/Test/FrameworkTestData/One/
	cp ../tests/mdoc/Test/DocTest-addNonGeneric.dll ../tests/mdoc/Test/FrameworkTestData/Two/
	cp ../tests/mdoc/Test/DocTest-DropNS-classic-secondary.dll ../tests/mdoc/Test/FrameworkTestData/Two/
	$(MONO) $(PROGRAM) fx-bootstrap -fx ../tests/mdoc/Test/FrameworkTestData

../tests/mdoc/Test/FrameworkTestData-fx-inheritance: ../tests/mdoc/Test/DocTest-framework-inheritance-one.dll ../tests/mdoc/Test/DocTest-framework-inheritance-two.dll ../tests/mdoc/Test/DocTest-framework-inheritance-three.dll
	rm -rf ../tests/mdoc/Test/FrameworkTestData-fx-inheritance
	mkdir ../tests/mdoc/Test/FrameworkTestData-fx-inheritance
	mkdir ../tests/mdoc/Test/FrameworkTestData-fx-inheritance/One
	mkdir ../tests/mdoc/Test/FrameworkTestData-fx-inheritance/Two
	mkdir ../tests/mdoc/Test/FrameworkTestData-fx-inheritance/Three
	cp ../tests/mdoc/Test/DocTest-framework-inheritance-one.dll ../tests/mdoc/Test/FrameworkTestData-fx-inheritance/One/
	cp ../tests/mdoc/Test/DocTest-framework-inheritance-two.dll ../tests/mdoc/Test/FrameworkTestData-fx-inheritance/Two/
	cp ../tests/mdoc/Test/DocTest-framework-inheritance-three.dll ../tests/mdoc/Test/FrameworkTestData-fx-inheritance/Three/
	$(MONO) $(PROGRAM) fx-bootstrap -fx ../tests/mdoc/Test/FrameworkTestData-fx-inheritance

.PHONY: check-monodocer-differentTypeDefinitions
check-monodocer-differentTypeDefinitions : ../tests/mdoc/Test/DocTest-differentTypeDefinitions-First.dll ../tests/mdoc/Test/DocTest-differentTypeDefinitions-Second.dll
	-rm -Rf ../tests/mdoc/Test/en.actual

	-rm -Rf ../tests/mdoc/Test/FrameworkTestData-fx-differentTypeDefinitions
	mkdir ../tests/mdoc/Test/FrameworkTestData-fx-differentTypeDefinitions
	mkdir ../tests/mdoc/Test/FrameworkTestData-fx-differentTypeDefinitions/One
	cp ../tests/mdoc/Test/DocTest-differentTypeDefinitions-First.dll ../tests/mdoc/Test/FrameworkTestData-fx-differentTypeDefinitions/One
	cp ../tests/mdoc/Test/DocTest-differentTypeDefinitions-Second.dll ../tests/mdoc/Test/FrameworkTestData-fx-differentTypeDefinitions/One
	$(MONO) $(PROGRAM) fx-bootstrap -fx ../tests/mdoc/Test/FrameworkTestData-fx-differentTypeDefinitions

	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual -frameworks ../tests/mdoc/Test/FrameworkTestData-fx-differentTypeDefinitions
	$(DIFF) ../tests/mdoc/Test/en.expected.differentTypeDefinitions ../tests/mdoc/Test/en.actual

.PHONY: check-monodocer-differentTypeParameterNames
check-monodocer-differentTypeParameterNames : ../tests/mdoc/Test/DocTest-differentTypeParameterNames-First.dll ../tests/mdoc/Test/DocTest-differentTypeParameterNames-Second.dll
	-rm -Rf ../tests/mdoc/Test/en.actual

	-rm -Rf ../tests/mdoc/Test/FrameworkTestData-fx-differentTypeParameterNames
	mkdir ../tests/mdoc/Test/FrameworkTestData-fx-differentTypeParameterNames
	mkdir ../tests/mdoc/Test/FrameworkTestData-fx-differentTypeParameterNames/One
	mkdir ../tests/mdoc/Test/FrameworkTestData-fx-differentTypeParameterNames/Two
	cp ../tests/mdoc/Test/DocTest-differentTypeParameterNames-First.dll ../tests/mdoc/Test/FrameworkTestData-fx-differentTypeParameterNames/One
	cp ../tests/mdoc/Test/DocTest-differentTypeParameterNames-Second.dll ../tests/mdoc/Test/FrameworkTestData-fx-differentTypeParameterNames/Two
	$(MONO) $(PROGRAM) fx-bootstrap -fx ../tests/mdoc/Test/FrameworkTestData-fx-differentTypeParameterNames

	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual -frameworks ../tests/mdoc/Test/FrameworkTestData-fx-differentTypeParameterNames
	$(DIFF) ../tests/mdoc/Test/en.expected.differentTypeParameterNames ../tests/mdoc/Test/en.actual

.PHONY: check-monodocer-typeForwards
check-monodocer-typeForwards : ../tests/mdoc/Test/DocTest-typeForwards-First.dll ../tests/mdoc/Test/DocTest-typeForwards-Second.dll ../tests/mdoc/Test/DocTest-typeForwards-Third.dll
	-rm -Rf ../tests/mdoc/Test/en.actual

	# set up the fx test data
	# all frameworks One, Two, Three should have the type forwarding from DocTest-typeForwards-Second-First to DocTest-typeForwards-Second \
	# so in xml should find no "FrameworkAlternate" attribute  for this forwarding, the attribute  is removed as expected since it is full frameworks list
	# frameworks Two and Three should have the type forwarding from DocTest-typeForwards-Third-First to DocTest-typeForwards-third \
	# in xml we should see FrameworkAlternate="Three;Two"
	-rm -Rf ../tests/mdoc/Test/FrameworkTestData-fx-typeForwards
	mkdir ../tests/mdoc/Test/FrameworkTestData-fx-typeForwards
	mkdir ../tests/mdoc/Test/FrameworkTestData-fx-typeForwards/One
	mkdir ../tests/mdoc/Test/FrameworkTestData-fx-typeForwards/Two
	mkdir ../tests/mdoc/Test/FrameworkTestData-fx-typeForwards/Three
	mkdir ../tests/mdoc/Test/FrameworkTestData-fx-typeForwards/dependencies
	mkdir ../tests/mdoc/Test/FrameworkTestData-fx-typeForwards/dependencies/One
	mkdir ../tests/mdoc/Test/FrameworkTestData-fx-typeForwards/dependencies/Two
	mkdir ../tests/mdoc/Test/FrameworkTestData-fx-typeForwards/dependencies/Three
	cp ../tests/mdoc/Test/DocTest-typeForwards-First.dll ../tests/mdoc/Test/FrameworkTestData-fx-typeForwards/One
	cp ../tests/mdoc/Test/DocTest-typeForwards-Second-First.dll ../tests/mdoc/Test/FrameworkTestData-fx-typeForwards/One
	cp ../tests/mdoc/Test/DocTest-typeForwards-Second.dll ../tests/mdoc/Test/FrameworkTestData-fx-typeForwards/dependencies/One
	cp ../tests/mdoc/Test/DocTest-typeForwards-Second-First.dll ../tests/mdoc/Test/FrameworkTestData-fx-typeForwards/Two
	cp ../tests/mdoc/Test/DocTest-typeForwards-Third-First.dll ../tests/mdoc/Test/FrameworkTestData-fx-typeForwards/Two
	cp ../tests/mdoc/Test/DocTest-typeForwards-Second.dll ../tests/mdoc/Test/FrameworkTestData-fx-typeForwards/dependencies/Two
	cp ../tests/mdoc/Test/DocTest-typeForwards-Third.dll ../tests/mdoc/Test/FrameworkTestData-fx-typeForwards/dependencies/Two
	cp ../tests/mdoc/Test/DocTest-typeForwards-Second-First.dll ../tests/mdoc/Test/FrameworkTestData-fx-typeForwards/Three
	cp ../tests/mdoc/Test/DocTest-typeForwards-Third-First.dll ../tests/mdoc/Test/FrameworkTestData-fx-typeForwards/Three
	cp ../tests/mdoc/Test/DocTest-typeForwards-Second.dll ../tests/mdoc/Test/FrameworkTestData-fx-typeForwards/dependencies/Three
	cp ../tests/mdoc/Test/DocTest-typeForwards-Third.dll ../tests/mdoc/Test/FrameworkTestData-fx-typeForwards/dependencies/Three
	$(MONO) $(PROGRAM) fx-bootstrap -fx ../tests/mdoc/Test/FrameworkTestData-fx-typeForwards

	# now run mdoc update
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual -frameworks ../tests/mdoc/Test/FrameworkTestData-fx-typeForwards
	$(DIFF) ../tests/mdoc/Test/en.expected.typeForwards ../tests/mdoc/Test/en.actual

.PHONY: check-monodocer-nestedType-typeForwards
check-monodocer-nestedType-typeForwards : ../tests/mdoc/Test/DocTest-nestedType-typeForwards-First.dll ../tests/mdoc/Test/DocTest-nestedType-typeForwards-Second.dll ../tests/mdoc/Test/DocTest-nestedType-typeForwards-Third.dll
	-rm -Rf ../tests/mdoc/Test/en.actual

	# set up the fx test data
	-rm -Rf ../tests/mdoc/Test/FrameworkTestData-fx-nestedType-typeForwards
	mkdir ../tests/mdoc/Test/FrameworkTestData-fx-nestedType-typeForwards
	mkdir ../tests/mdoc/Test/FrameworkTestData-fx-nestedType-typeForwards/One
	mkdir ../tests/mdoc/Test/FrameworkTestData-fx-nestedType-typeForwards/Two
	mkdir ../tests/mdoc/Test/FrameworkTestData-fx-nestedType-typeForwards/Three
	mkdir ../tests/mdoc/Test/FrameworkTestData-fx-nestedType-typeForwards/dependencies
	mkdir ../tests/mdoc/Test/FrameworkTestData-fx-nestedType-typeForwards/dependencies/One
	mkdir ../tests/mdoc/Test/FrameworkTestData-fx-nestedType-typeForwards/dependencies/Two
	mkdir ../tests/mdoc/Test/FrameworkTestData-fx-nestedType-typeForwards/dependencies/Three
	cp ../tests/mdoc/Test/DocTest-nestedType-typeForwards-First.dll ../tests/mdoc/Test/FrameworkTestData-fx-nestedType-typeForwards/One
	cp ../tests/mdoc/Test/DocTest-nestedType-typeForwards-Second-First.dll ../tests/mdoc/Test/FrameworkTestData-fx-nestedType-typeForwards/One
	cp ../tests/mdoc/Test/DocTest-nestedType-typeForwards-Second.dll ../tests/mdoc/Test/FrameworkTestData-fx-nestedType-typeForwards/dependencies/One
	cp ../tests/mdoc/Test/DocTest-nestedType-typeForwards-Second-First.dll ../tests/mdoc/Test/FrameworkTestData-fx-nestedType-typeForwards/Two
	cp ../tests/mdoc/Test/DocTest-nestedType-typeForwards-Third-First.dll ../tests/mdoc/Test/FrameworkTestData-fx-nestedType-typeForwards/Two
	cp ../tests/mdoc/Test/DocTest-nestedType-typeForwards-Second.dll ../tests/mdoc/Test/FrameworkTestData-fx-nestedType-typeForwards/dependencies/Two
	cp ../tests/mdoc/Test/DocTest-nestedType-typeForwards-Third.dll ../tests/mdoc/Test/FrameworkTestData-fx-nestedType-typeForwards/dependencies/Two
	cp ../tests/mdoc/Test/DocTest-nestedType-typeForwards-Second-First.dll ../tests/mdoc/Test/FrameworkTestData-fx-nestedType-typeForwards/Three
	cp ../tests/mdoc/Test/DocTest-nestedType-typeForwards-Third-First.dll ../tests/mdoc/Test/FrameworkTestData-fx-nestedType-typeForwards/Three
	cp ../tests/mdoc/Test/DocTest-nestedType-typeForwards-Second.dll ../tests/mdoc/Test/FrameworkTestData-fx-nestedType-typeForwards/dependencies/Three
	cp ../tests/mdoc/Test/DocTest-nestedType-typeForwards-Third.dll ../tests/mdoc/Test/FrameworkTestData-fx-nestedType-typeForwards/dependencies/Three
	$(MONO) $(PROGRAM) fx-bootstrap -fx ../tests/mdoc/Test/FrameworkTestData-fx-nestedType-typeForwards

	# now run mdoc update
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual -frameworks ../tests/mdoc/Test/FrameworkTestData-fx-nestedType-typeForwards
	$(DIFF) ../tests/mdoc/Test/en.expected-nestedType.typeForwards ../tests/mdoc/Test/en.actual
	
check-monodocer-frameworks: ../tests/mdoc/Test/FrameworkTestData
	-rm -Rf ../tests/mdoc/Test/en.actual
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual -frameworks ../tests/mdoc/Test/FrameworkTestData
	$(DIFF) ../tests/mdoc/Test/en.expected-frameworks ../tests/mdoc/Test/en.actual

check-monodocer-frameworks-inheritance: ../tests/mdoc/Test/FrameworkTestData-fx-inheritance
	-rm -Rf ../tests/mdoc/Test/en.actual
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual -frameworks ../tests/mdoc/Test/FrameworkTestData-fx-inheritance
	$(DIFF) ../tests/mdoc/Test/en.expected-frameworks-inheritance ../tests/mdoc/Test/en.actual
	
check-monodocer-frameworks-with-nuget: 
	-rm -Rf ../tests/mdoc/Test/en.actual
	-rm -rf ../tests/mdoc/Test/test-nuget-information/input_data
	mkdir ../tests/mdoc/Test/test-nuget-information/input_data
	mkdir ../tests/mdoc/Test/test-nuget-information/input_data/One
	mkdir ../tests/mdoc/Test/test-nuget-information/input_data/Two
	cp ../tests/mdoc/Test/DocTest-addNonGeneric.dll ../tests/mdoc/Test/test-nuget-information/input_data/One/
	cp ../tests/mdoc/Test/DocTest-DropNS-classic.dll ../tests/mdoc/Test/test-nuget-information/input_data/One/
	cp ../tests/mdoc/Test/DocTest-addNonGeneric.dll ../tests/mdoc/Test/test-nuget-information/input_data/Two/
	cp ../tests/mdoc/Test/DocTest-DropNS-classic-secondary.dll ../tests/mdoc/Test/test-nuget-information/input_data/Two/
	cp ../tests/mdoc/Test/frameworks.xml ../tests/mdoc/Test/test-nuget-information/input_data/
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual -frameworks ../tests/mdoc/Test/test-nuget-information/input_data
	$(DIFF) ../tests/mdoc/Test/test-nuget-information/en.expected-frameworks-with-nuget-information ../tests/mdoc/Test/en.actual

check-monodocer-docid: ../tests/mdoc/Test/FrameworkTestData
	-rm -Rf ../tests/mdoc/Test/en.actual
	$(MONO) $(PROGRAM) update -use-docid -o ../tests/mdoc/Test/en.actual -frameworks ../tests/mdoc/Test/FrameworkTestData
	$(DIFF) ../tests/mdoc/Test/en.expected-docid ../tests/mdoc/Test/en.actual
	
check-monodocer-vbnet: ../tests/mdoc/Test/FrameworkTestData
	-rm -Rf ../tests/mdoc/Test/en.actual
	$(MONO) $(PROGRAM) update -lang vb.net -o ../tests/mdoc/Test/en.actual -frameworks ../tests/mdoc/Test/FrameworkTestData
	$(DIFF) ../tests/mdoc/Test/en.expected-vbnet ../tests/mdoc/Test/en.actual

check-monodocer-Eii: 
	-rm -Rf ../tests/mdoc/Test/en.actual
	$(MAKE) ../tests/mdoc/Test/DocTest-VB-Eii.dll
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest-VB-Eii.dll -lang vb.net -lang fsharp -lang javascript -lang c++/cli -lang c++/cx -lang c++/winrt
	$(DIFF) ../tests/mdoc/Test/en.expected-eii-implementation ../tests/mdoc/Test/en.actual	
	
check-monodocer-Eii-importecmadoc-oldNames: 
	-rm -Rf ../tests/mdoc/Test/en.actual
	$(MAKE) ../tests/mdoc/Test/DocTest-VB-Eii.dll
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest-VB-Eii.dll -lang vb.net -lang fsharp -lang javascript -lang c++/cli -lang c++/cx -lang c++/winrt -import ../tests/mdoc/Test/ClassEnumeratorECMA.xml
	$(DIFF) ../tests/mdoc/Test/en.expected-eii-implementation-ecmadoc ../tests/mdoc/Test/en.actual	
	
check-monodocer-Eii-importslashdoc: 
	-rm -Rf ../tests/mdoc/Test/en.actual
	$(MAKE) ../tests/mdoc/Test/DocTest-VB-Eii.dll	
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest-VB-Eii.dll -lang vb.net -lang fsharp -lang javascript -lang c++/cli -lang c++/cx -lang c++/winrt -import ../tests/mdoc/Test/ClassEnumeratorSlashDoc.xml
	$(DIFF) ../tests/mdoc/Test/en.expected-eii-implementation-slashdoc ../tests/mdoc/Test/en.actual	
	
check-monodocer-cppcli: ../tests/mdoc/Test/FrameworkTestData
	-rm -Rf ../tests/mdoc/Test/en.actual
	$(MONO) $(PROGRAM) update -lang c++/cli -o ../tests/mdoc/Test/en.actual -frameworks ../tests/mdoc/Test/FrameworkTestData
	$(DIFF) ../tests/mdoc/Test/en.expected-cppcli ../tests/mdoc/Test/en.actual

check-monodocer-cppwinrtUwp: 
	-rm -Rf ../tests/mdoc/Test/en.actual
	$(MONO) $(PROGRAM) update -lib ../external/Windows --lang c++/winrt -o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/UwpTestWinRtComponentCpp.winmd
	$(DIFF) ../tests/mdoc/Test/ex.expected-cppwinrtuwp ../tests/mdoc/Test/en.actual
	
check-monodocer-cppcx: ../tests/mdoc/Test/FrameworkTestData
	-rm -Rf ../tests/mdoc/Test/en.actual
	$(MONO) $(PROGRAM) update -lang c++/cx -o ../tests/mdoc/Test/en.actual -frameworks ../tests/mdoc/Test/FrameworkTestData
	$(DIFF) ../tests/mdoc/Test/en.expected-cppcx ../tests/mdoc/Test/en.actual

check-monodocer-cppcxDocTest: ../tests/mdoc/Test/DocTest.dll
	-rm -Rf ../tests/mdoc/Test/en.actual
	$(MONO) $(PROGRAM) update -lang c++/cx -o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest.dll
	$(DIFF) ../tests/mdoc/Test/en.expected-cppcx2 ../tests/mdoc/Test/en.actual

check-monodocer-cppwinrt: ../tests/mdoc/Test/FrameworkTestData
	-rm -Rf ../tests/mdoc/Test/en.actual
	$(MONO) $(PROGRAM) update -lang c++/winrt -o ../tests/mdoc/Test/en.actual -frameworks ../tests/mdoc/Test/FrameworkTestData
	$(DIFF) ../tests/mdoc/Test/en.expected-cppwinrt ../tests/mdoc/Test/en.actual

	
check-monodocer-cppwinrtDocTest: ../tests/mdoc/Test/DocTest.dll
	-rm -Rf ../tests/mdoc/Test/en.actual
	$(MONO) $(PROGRAM) update -lang c++/winrt -o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest.dll 
	$(DIFF) ../tests/mdoc/Test/en.expected-cppwinrt2 ../tests/mdoc/Test/en.actual


check-monodocer-vbnet2: 
	-rm -Rf ../tests/mdoc/Test/en.actual
	$(MAKE) ../tests/mdoc/Test/DocTest.dll-v1
	$(MONO) $(PROGRAM) update -lang vb.net -o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest.dll
	$(DIFF) ../tests/mdoc/Test/en.expected-vbnet2 ../tests/mdoc/Test/en.actual

check-monodocer-javascript: 
	-rm -Rf ../tests/mdoc/Test/en.actual
	$(MAKE) ../tests/mdoc/Test/DocTest.dll-v1
	$(MONO) $(PROGRAM) update -lang javascript -o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest.dll
	$(DIFF) ../tests/mdoc/Test/en.expected-javascript ../tests/mdoc/Test/en.actual
	
check-monodocer-embedded-type: 
	-rm -Rf ../tests/mdoc/Test/en.actual	
	$(MAKE) ../tests/mdoc/Test/DocTest-embedded-type.dll
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest-embedded-type.dll
	$(DIFF) ../tests/mdoc/Test/en.expected-embedded-type ../tests/mdoc/Test/en.actual

check-monodocer-addNonGeneric: 
	-rm -Rf ../tests/mdoc/Test/en.actual
	# first, make a docset with the generic method
	$(MAKE) ../tests/mdoc/Test/DocTest-addNonGeneric.dll
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest-addNonGeneric.dll

	# now add a non-generic version of the method and update several times
	$(MAKE) ../tests/mdoc/Test/DocTest-addNonGeneric-v2.dll
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest-addNonGeneric-v2.dll
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest-addNonGeneric-v2.dll
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest-addNonGeneric-v2.dll
	$(DIFF) ../tests/mdoc/Test/en.expected-addNonGeneric ../tests/mdoc/Test/en.actual

check-monodocer-membergroup: ../tests/mdoc/Test/DocTest-addNonGeneric-v2.dll ../tests/mdoc/Test/DocTest-addNonGeneric.dll
	-rm -Rf ../tests/mdoc/Test/en.actual
	cp -r ../tests/mdoc/Test/en.expected-membergroup ../tests/mdoc/Test/en.actual
	$(MONO) $(PROGRAM) update --debug -o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest-addNonGeneric-v2.dll
	$(DIFF) ../tests/mdoc/Test/en.expected-membergroup ../tests/mdoc/Test/en.actual
	$(MONO) $(PROGRAM) validate -f ecma ../tests/mdoc/Test/en.actual

check-monodocer-dropns-classic: 
	# tests the simplest --dropns case, a single class where the root namespace was dropped.
	-rm -Rf ../tests/mdoc/Test/en.actual
	$(MAKE) ../tests/mdoc/Test/DocTest-DropNS-classic.dll
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest-DropNS-classic.dll --api-style=classic
	$(MAKE) update-monodocer-dropns-unified
	$(DIFF) ../tests/mdoc/Test/en.expected-dropns-classic-v1 ../tests/mdoc/Test/en.actual

check-monodocer-dropns-multi: 
	-rm -Rf ../tests/mdoc/Test/en.actual
	$(MAKE) ../tests/mdoc/Test/DocTest-DropNS-classic.dll
	$(MAKE) ../tests/mdoc/Test/DocTest-DropNS-unified.dll
	$(MAKE) ../tests/mdoc/Test/DocTest-DropNS-classic-multitest.dll
	$(MAKE) ../tests/mdoc/Test/DocTest-DropNS-unified-multitest.dll

	# mdoc update for both classic and unified
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual $(MULTI-CLASSIC) --api-style=classic 
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual $(MULTI-UNIFIED) --api-style=unified --dropns ../tests/mdoc/Test/DocTest-DropNS-unified.dll=MyFramework --dropns ../tests/mdoc/Test/DocTest-DropNS-unified-multitest.dll=MyFramework 
	
	# now run it again to verify idempotency
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual $(MULTI-CLASSIC) --api-style=classic 
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual $(MULTI-UNIFIED) --api-style=unified --dropns ../tests/mdoc/Test/DocTest-DropNS-unified.dll=MyFramework --dropns ../tests/mdoc/Test/DocTest-DropNS-unified-multitest.dll=MyFramework 
	
	$(DIFF) ../tests/mdoc/Test/en.expected-dropns-multi ../tests/mdoc/Test/en.actual


check-monodocer-dropns-multi-withexisting: 
	-rm -Rf ../tests/mdoc/Test/en.actual
	$(MAKE) ../tests/mdoc/Test/DocTest-DropNS-classic.dll
	$(MAKE) ../tests/mdoc/Test/DocTest-DropNS-unified.dll
	$(MAKE) ../tests/mdoc/Test/DocTest-DropNS-classic-multitest.dll
	$(MAKE) ../tests/mdoc/Test/DocTest-DropNS-unified-multitest.dll

	# mdoc update to show a pre-existing set of documents
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest-DropNS-classic.dll --api-style=classic
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest-DropNS-unified.dll --api-style=unified --dropns ../tests/mdoc/Test/DocTest-DropNS-unified.dll=MyFramework 
	
	# mdoc update for both classic and unified
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual $(MULTI-CLASSIC) --api-style=classic 
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual $(MULTI-UNIFIED) --api-style=unified --dropns ../tests/mdoc/Test/DocTest-DropNS-unified.dll=MyFramework --dropns ../tests/mdoc/Test/DocTest-DropNS-unified-multitest.dll=MyFramework 
	
	$(DIFF) ../tests/mdoc/Test/en.expected-dropns-multi-withexisting ../tests/mdoc/Test/en.actual

check-monodocer-dropns-delete: 
	-rm -Rf ../tests/mdoc/Test/en.actual
	-rm -Rf ../tests/mdoc/Test/actual_statistics.txt
	rm -Rf ../tests/mdoc/Test/DocTest-DropNS-classic-deletetest.dll
	rm -Rf ../tests/mdoc/Test/DocTest-DropNS-unified-deletetest.dll
	$(MAKE) ../tests/mdoc/Test/DocTest-DropNS-classic-deletetest.dll
	$(MONO) $(PROGRAM) update --delete -o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest-DropNS-classic-deletetest.dll --api-style=classic
	$(MAKE) ../tests/mdoc/Test/DocTest-DropNS-unified-deletetest.dll
	$(MONO) $(PROGRAM) update --delete -o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest-DropNS-unified-deletetest.dll --api-style=unified --dropns ../tests/mdoc/Test/DocTest-DropNS-unified-deletetest.dll=MyFramework
	$(MAKE) ../tests/mdoc/Test/DocTest-DropNS-classic-deletetest-V2.dll
	$(MONO) $(PROGRAM) update --delete -o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest-DropNS-classic-deletetest.dll --api-style=classic
	$(MAKE) ../tests/mdoc/Test/DocTest-DropNS-unified-deletetest-V2.dll
	$(MONO) $(PROGRAM) update --delete -o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest-DropNS-unified-deletetest.dll --api-style=unified --dropns ../tests/mdoc/Test/DocTest-DropNS-unified-deletetest.dll=MyFramework -statistics ../tests/mdoc/Test/actual_statistics.txt
	$(DIFF) ../tests/mdoc/Test/en.expected-dropns-delete ../tests/mdoc/Test/en.actual
	$(DIFF) ../tests/mdoc/Test/expected_remove_statistics.txt ../tests/mdoc/Test/actual_statistics.txt

check-monodocer-dropns-classic-withsecondary: 
	# tests case where a secondary assembly is included with a --dropns parameter
	-rm -Rf ../tests/mdoc/Test/en.actual
	$(MAKE) ../tests/mdoc/Test/DocTest-DropNS-classic.dll
	$(MAKE) ../tests/mdoc/Test/DocTest-DropNS-classic-secondary.dll
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest-DropNS-classic.dll ../tests/mdoc/Test/DocTest-DropNS-classic-secondary.dll --api-style=classic
	$(MAKE) update-monodocer-dropns-unified-withsecondary
	$(DIFF) ../tests/mdoc/Test/en.expected-dropns-classic-withsecondary ../tests/mdoc/Test/en.actual

update-monodocer-dropns-unified: 
	$(MAKE) ../tests/mdoc/Test/DocTest-DropNS-unified.dll
	$(MONO) $(PROGRAM) update --debug -o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest-DropNS-unified.dll --api-style=unified --dropns ../tests/mdoc/Test/DocTest-DropNS-unified.dll=MyFramework

update-monodocer-dropns-unified-withsecondary: 
	$(MAKE) ../tests/mdoc/Test/DocTest-DropNS-unified.dll
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest-DropNS-unified.dll ../tests/mdoc/Test/DocTest-DropNS-classic-secondary.dll --api-style=unified --dropns ../tests/mdoc/Test/DocTest-DropNS-unified.dll=MyFramework

update-monodocer-dropns-classic-secondary: 
	$(MAKE) ../tests/mdoc/Test/DocTest-DropNS-classic-secondary.dll
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest-DropNS-classic-secondary.dll --api-style=classic

check-monodocer-internal-interface: 
	# Tests to make sure internal interfaces that are explicitly implemented are not documented
	-rm -Rf ../tests/mdoc/Test/en.actual
	$(MAKE) ../tests/mdoc/Test/DocTest-InternalInterface.dll
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest-InternalInterface.dll -lang VB.NET
	$(DIFF) ../tests/mdoc/Test/en.expected-internal-interface ../tests/mdoc/Test/en.actual

check-monodocer-enumerations: 
	-rm -Rf ../tests/mdoc/Test/en.actual
	$(MAKE) ../tests/mdoc/Test/DocTest-enumerations.dll
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest-enumerations.dll
	$(DIFF) ../tests/mdoc/Test/en.expected-enumerations ../tests/mdoc/Test/en.actual

check-monodocer-update: 
	find ../tests/mdoc/Test/en.expected -name \*.xml -exec rm "{}" \;
	$(MAKE) ../tests/mdoc/Test/DocTest.dll-v1
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.expected ../tests/mdoc/Test/DocTest.dll -lang docid -lang vb.net -lang fsharp -lang javascript -lang c++/cli -lang c++/cx -lang c++/winrt

check-monodocer: 
	-rm -Rf ../tests/mdoc/Test/en.actual
	$(MAKE) ../tests/mdoc/Test/DocTest.dll-v1
	$(MONO) $(PROGRAM) update --debug -o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest.dll -lang docid -lang vb.net -lang fsharp -lang javascript -lang c++/cli -lang c++/cx -lang c++/winrt
	$(DIFF) ../tests/mdoc/Test/en.expected ../tests/mdoc/Test/en.actual
	$(MONO) $(PROGRAM) update --debug -o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest.dll -lang docid -lang vb.net -lang fsharp -lang javascript -lang c++/cli -lang c++/cx -lang c++/winrt
	$(DIFF) ../tests/mdoc/Test/en.expected ../tests/mdoc/Test/en.actual

check-monodocer-since-update: 
	find ../tests/mdoc/Test/en.expected.since -name \*.xml -exec rm "{}" \;
	$(MAKE) ../tests/mdoc/Test/DocTest.dll-v1
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.expected.since ../tests/mdoc/Test/DocTest.dll 
	$(MAKE) ../tests/mdoc/Test/DocTest.dll-v2
	$(MONO) $(PROGRAM) update --since="Version 2.0" \
		-o ../tests/mdoc/Test/en.expected.since ../tests/mdoc/Test/DocTest.dll 

check-monodocer-since: 
	rm -Rf ../tests/mdoc/Test/en.actual
	$(MAKE) ../tests/mdoc/Test/DocTest.dll-v1
	$(MONO) $(PROGRAM) --debug update -o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest.dll 
	$(MAKE) ../tests/mdoc/Test/DocTest.dll-v2
	$(MONO) $(PROGRAM) --debug update --since="Version 2.0" \
		-o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest.dll 
	$(DIFF) ../tests/mdoc/Test/en.expected.since ../tests/mdoc/Test/en.actual

check-monodocer-delete-update: 
	find ../tests/mdoc/Test/en.expected.delete -type f -exec rm "{}" \;
	$(MAKE) ../tests/mdoc/Test/DocTest.dll-v1
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.expected.delete ../tests/mdoc/Test/DocTest.dll
	$(MAKE) ../tests/mdoc/Test/DocTest.dll-v2
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.expected.delete ../tests/mdoc/Test/DocTest.dll
	$(MAKE) ../tests/mdoc/Test/DocTest.dll-v1
	$(MONO) $(PROGRAM) update -fno-assembly-versions --delete \
		-o ../tests/mdoc/Test/en.expected.delete ../tests/mdoc/Test/DocTest.dll

check-monodocer-delete: 
	rm -Rf ../tests/mdoc/Test/en.actual
	$(MAKE) ../tests/mdoc/Test/DocTest.dll-v1
	$(MONO) $(PROGRAM) --debug update -o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest.dll
	$(MAKE) ../tests/mdoc/Test/DocTest.dll-v2
	$(MONO) $(PROGRAM) --debug update -o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest.dll
	$(MAKE) ../tests/mdoc/Test/DocTest.dll-v1
	$(MONO) $(PROGRAM) --debug update -fno-assembly-versions --delete -o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest.dll
	$(DIFF) ../tests/mdoc/Test/en.expected.delete ../tests/mdoc/Test/en.actual

check-monodocer-ignore-invalid-assemblies: ../tests/mdoc/Test/DocTest-addNonGeneric.dll ../tests/mdoc/Test/DocTest-addNonGeneric-v2.dll
	-rm -Rf ../tests/mdoc/Test/en.actual
	touch ../tests/mdoc/Test/notActuallyA.dll
	$(MONO) $(PROGRAM) --debug update -o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest-addNonGeneric.dll ../tests/mdoc/Test/notActuallyA.dll
	$(MONO) $(PROGRAM) --debug update -o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest-addNonGeneric-v2.dll ../tests/mdoc/Test/notActuallyA.dll
	$(DIFF) ../tests/mdoc/Test/en.expected-addNonGeneric ../tests/mdoc/Test/en.actual

check-monodocer-importslashdoc-update: 
	find ../tests/mdoc/Test/en.expected.importslashdoc -name \*.xml -exec rm "{}" \;
	$(MAKE) ../tests/mdoc/Test/DocTest.dll-v1 TEST_CSCFLAGS=-doc:../tests/mdoc/Test/DocTest.xml
	$(MONO) $(PROGRAM) --debug update -i ../tests/mdoc/Test/DocTest.xml \
		-o ../tests/mdoc/Test/en.expected.importslashdoc ../tests/mdoc/Test/DocTest.dll 

check-monodocer-importslashdoc: 
	rm -Rf ../tests/mdoc/Test/en.actual
	$(MAKE) ../tests/mdoc/Test/DocTest.dll-v1 TEST_CSCFLAGS=-doc:../tests/mdoc/Test/DocTest.xml
	$(MONO) $(PROGRAM) --debug update -i ../tests/mdoc/Test/DocTest.xml \
		-o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest.dll 
	$(DIFF) ../tests/mdoc/Test/en.expected.importslashdoc ../tests/mdoc/Test/en.actual
	
check-multiple-mdoc: 
	rm -Rf ../tests/mdoc/Test/en.actual
	$(CSCOMPILE) -debug -optimize -unsafe  -target:library ../tests/mdoc/Test/DocTest.cs -doc:../tests/mdoc/Test/DocTest.xml
	$(MONO) $(PROGRAM) --debug update --exceptions=asm -i ../tests/mdoc/Test/DocTest.xml \
		-o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest.dll 
	cp ../tests/mdoc/Test/test-multiple-mdoc/Widget.xml ../tests/mdoc/Test/en.actual/Mono.DocTest/Widget.xml	
	$(MONO) $(PROGRAM) --debug update --exceptions=asm -i ../tests/mdoc/Test/DocTest.xml \
		-o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest.dll 
	$(DIFF) ../tests/mdoc/Test/test-multiple-mdoc/en.expected.test.multiple.mdoc ../tests/mdoc/Test/en.actual

check-monodocer-importecmadoc-update: 
	find ../tests/mdoc/Test/en.expected.importecmadoc -name \*.xml -exec rm "{}" \;
	$(MAKE) ../tests/mdoc/Test/DocTest.dll-v1
	$(MONO) $(PROGRAM) --debug update -i ../tests/mdoc/Test/TestEcmaDocs.xml \
		'--type=System.Action`1' --type=System.AsyncCallback \
		--type=System.Environment --type=System.Array \
		-o ../tests/mdoc/Test/en.expected.importecmadoc ../tests/mdoc/Test/DocTest.dll 

check-monodocer-importecmadoc: 
	rm -Rf ../tests/mdoc/Test/en.actual
	$(MAKE) ../tests/mdoc/Test/DocTest.dll-v1
	$(MONO) $(PROGRAM) --debug update -i ../tests/mdoc/Test/TestEcmaDocs.xml \
		'--type=System.Action`1' --type=System.AsyncCallback \
		--type=System.Environment --type=System.Array \
		-o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/DocTest.dll 
	$(DIFF) ../tests/mdoc/Test/en.expected.importecmadoc ../tests/mdoc/Test/en.actual

.PHONY: check-monodocer-import-fx-work
check-monodocer-import-fx-work: ../tests/mdoc/Test/DocTest.dll-v1 ../tests/mdoc/Test/DocTest-DropNS-classic-secondary.dll ../tests/mdoc/Test/DocTest-DropNS-classic.dll
	rm -Rf ../tests/mdoc/Test/en.actual
	rm -Rf ../tests/mdoc/Test/fx-import
	rm -Rf ../tests/mdoc/Test/actual_statistics.txt
	mkdir ../tests/mdoc/Test/fx-import
	mkdir ../tests/mdoc/Test/fx-import/one
	mkdir ../tests/mdoc/Test/fx-import/two
	cp ../tests/mdoc/Test/DocTest.dll ../tests/mdoc/Test/fx-import/one
	cp ../tests/mdoc/Test/DocTest-DropNS-classic-secondary.dll ../tests/mdoc/Test/fx-import/two
	cp ../tests/mdoc/Test/DocTest-DropNS-classic.dll ../tests/mdoc/Test/fx-import/two
	cp ../tests/mdoc/Test/DocTest-DropNS-classic-secondary.xml ../tests/mdoc/Test/fx-import/TestEcmaDocs2.xml
	cp ../tests/mdoc/Test/DocTest-DropNS-classic.xml ../tests/mdoc/Test/fx-import/DocTest-DropNS-classic.xml
	cp ../tests/mdoc/Test/DocTest.xml ../tests/mdoc/Test/fx-import/TestEcmaDocs.xml
	cp ../tests/mdoc/Test/CLILibraryTypes.dtd ../tests/mdoc/Test/fx-import/
	cp ../tests/mdoc/Test/fx-import-configuration.xml ../tests/mdoc/Test/fx-import/frameworks.xml
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual -frameworks ../tests/mdoc/Test/fx-import -statistics ../tests/mdoc/Test/actual_statistics.txt
	$(DIFF) ../tests/mdoc/Test/expected_statistics.txt ../tests/mdoc/Test/actual_statistics.txt
	
.PHONY: check-monodocer-fx-statistics-remove	
check-monodocer-fx-statistics-remove:
	rm -Rf ../tests/mdoc/Test/en.actual
	rm -Rf ../tests/mdoc/Test/fx-import
	rm -Rf ../tests/mdoc/Test/actual_statistics.txt
	mkdir ../tests/mdoc/Test/fx-import
	mkdir ../tests/mdoc/Test/fx-import/one
	cp ../tests/mdoc/Test/fx-statistics-remove-configuration.xml ../tests/mdoc/Test/fx-import/frameworks.xml
	rm -Rf ../tests/mdoc/Test/DocTest-DropNS-unified-deletetest.dll
	$(MAKE) ../tests/mdoc/Test/DocTest-DropNS-unified-deletetest.dll
	cp ../tests/mdoc/Test/DocTest-DropNS-unified-deletetest.dll ../tests/mdoc/Test/fx-import/one/DocTest.dll
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual -frameworks ../tests/mdoc/Test/fx-import
	rm -Rf ../tests/mdoc/Test/DocTest-DropNS-unified.dll
	$(MAKE) ../tests/mdoc/Test/DocTest-DropNS-unified.dll
	rm -rf ../tests/mdoc/Test/fx-import/one/DocTest.dll
	cp ../tests/mdoc/Test/DocTest-DropNS-unified.dll ../tests/mdoc/Test/fx-import/one/DocTest.dll
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual -frameworks ../tests/mdoc/Test/fx-import -statistics ../tests/mdoc/Test/actual_statistics.txt
	$(DIFF) ../tests/mdoc/Test/expected_fx_remove_statistics.txt ../tests/mdoc/Test/actual_statistics.txt
	
check-monodocer-fsharp:
	-rm -Rf ../tests/mdoc/Test/en.actual
	$(MAKE) ../tests/mdoc/mdoc.Test.FSharp/bin/Release/mdoc.Test.FSharp.dll
	$(MONO) $(PROGRAM) update -lang fsharp -o ../tests/mdoc/Test/en.actual ../tests/mdoc/mdoc.Test.FSharp/bin/Release/mdoc.Test.FSharp.dll

ifeq ($(ENVIRONMENT), wsl)
	$(DIFF) ../tests/mdoc/Test/en.expected-fsharp-wsl ../tests/mdoc/Test/en.actual
else
	$(DIFF) ../tests/mdoc/Test/en.expected-fsharp ../tests/mdoc/Test/en.actual
endif


check-monodocer-members-implementation:
	-rm -Rf ../tests/mdoc/Test/en.actual
	$(MAKE) ../tests/mdoc/TestInterfaceImplementation/bin/Release/TestInterfaceImplementation.dll
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual ../tests/mdoc/TestInterfaceImplementation/bin/Release/TestInterfaceImplementation.dll
	$(DIFF) ../tests/mdoc/Test/en.expected.members-implementation ../tests/mdoc/Test/en.actual

# ------- Framework Alternate Test(s) -------
.PHONY: ../tests/mdoc/Test/FrameworkTestData-frameworkalternate
../tests/mdoc/Test/FrameworkTestData-frameworkalternate: ../tests/mdoc/Test/DocTest-frameworkalternate-one.dll ../tests/mdoc/Test/DocTest-frameworkalternate-two.dll
	rm -rf ../tests/mdoc/Test/FrameworkTestData-frameworkalternate
	mkdir ../tests/mdoc/Test/FrameworkTestData-frameworkalternate
	mkdir ../tests/mdoc/Test/FrameworkTestData-frameworkalternate/One
	mkdir ../tests/mdoc/Test/FrameworkTestData-frameworkalternate/Two
	mkdir ../tests/mdoc/Test/FrameworkTestData-frameworkalternate/Three
	cp ../tests/mdoc/Test/DocTest-frameworkalternate-one.dll ../tests/mdoc/Test/FrameworkTestData-frameworkalternate/One/
	cp ../tests/mdoc/Test/DocTest-frameworkalternate-two.dll ../tests/mdoc/Test/FrameworkTestData-frameworkalternate/Two/
	cp ../tests/mdoc/Test/DocTest-frameworkalternate-one.dll ../tests/mdoc/Test/FrameworkTestData-frameworkalternate/Three/
	$(MONO) $(PROGRAM) fx-bootstrap -fx ../tests/mdoc/Test/FrameworkTestData-frameworkalternate

.PHONY: ../tests/mdoc/Test/FrameworkTestData-frameworkalternate-fromold
../tests/mdoc/Test/FrameworkTestData-frameworkalternate-fromold: ../tests/mdoc/Test/DocTest-frameworkalternate-one.dll ../tests/mdoc/Test/DocTest-frameworkalternate-two.dll
	rm -rf ../tests/mdoc/Test/FrameworkTestData-frameworkalternate-fromold
	mkdir ../tests/mdoc/Test/FrameworkTestData-frameworkalternate-fromold
	mkdir ../tests/mdoc/Test/FrameworkTestData-frameworkalternate-fromold/One
	mkdir ../tests/mdoc/Test/FrameworkTestData-frameworkalternate-fromold/Two
	mkdir ../tests/mdoc/Test/FrameworkTestData-frameworkalternate-fromold/Three
	cp ../tests/mdoc/Test/DocTest-frameworkalternate-one.dll ../tests/mdoc/Test/FrameworkTestData-frameworkalternate-fromold/One/
	cp ../tests/mdoc/Test/DocTest-frameworkalternate-two.dll ../tests/mdoc/Test/FrameworkTestData-frameworkalternate-fromold/Two/
	cp ../tests/mdoc/Test/DocTest-frameworkalternate-one.dll ../tests/mdoc/Test/FrameworkTestData-frameworkalternate-fromold/Three/
	$(MONO) $(PROGRAM) fx-bootstrap -fx ../tests/mdoc/Test/FrameworkTestData-frameworkalternate-fromold

../tests/mdoc/Test/DocTest-frameworkalternate-one.dll: 
	$(CSCOMPILE) $(TEST_CSCFLAGS) -debug -target:library -out:$@ ../tests/mdoc/Test/DocTest-frameworkalternate.cs /define:FXONE

../tests/mdoc/Test/DocTest-frameworkalternate-two.dll: 
	$(CSCOMPILE) $(TEST_CSCFLAGS) -debug -target:library -out:$@ ../tests/mdoc/Test/DocTest-frameworkalternate.cs /define:FXTWO

.PHONY: check-monodocer-frameworkalternate
check-monodocer-frameworkalternate: ../tests/mdoc/Test/FrameworkTestData-frameworkalternate
	-rm -Rf ../tests/mdoc/Test/en.actual

	# Run Test
	echo "First run"
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual -frameworks ../tests/mdoc/Test/FrameworkTestData-frameworkalternate
	$(DIFF) ../tests/mdoc/Test/en.expected-frameworkalternate ../tests/mdoc/Test/en.actual
	# run test again to make sure subsequent runs maintain data
	echo "Second run"
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual -frameworks ../tests/mdoc/Test/FrameworkTestData-frameworkalternate
	$(DIFF) ../tests/mdoc/Test/en.expected-frameworkalternate ../tests/mdoc/Test/en.actual

	# Test Future FX Alignment ... 
	echo "compile new version of 'two' that looks like 'one'"
	
	rm ../tests/mdoc/Test/DocTest-frameworkalternate-two.dll
	$(CSCOMPILE) $(TEST_CSCFLAGS) -debug -target:library -out:../tests/mdoc/Test/DocTest-frameworkalternate-two.dll ../tests/mdoc/Test/DocTest-frameworkalternate.cs /define:FXONE
	yes | cp ../tests/mdoc/Test/DocTest-frameworkalternate-two.dll ../tests/mdoc/Test/FrameworkTestData-frameworkalternate/Two/DocTest-frameworkalternate-two.dll
	# mdoc update fxmode again
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual -frameworks ../tests/mdoc/Test/FrameworkTestData-frameworkalternate
	# diff against aligned but with FrameworkAlternate="one;two;three"
	$(DIFF) ../tests/mdoc/Test/en.expected-frameworkalternate-aligned ../tests/mdoc/Test/en.actual


.PHONY: check-monodocer-frameworkalternate-fromold
check-monodocer-frameworkalternate-fromold: ../tests/mdoc/Test/FrameworkTestData-frameworkalternate-fromold
	-rm -Rf ../tests/mdoc/Test/en.actual

	# synchronize all frameworks
	yes | cp ../tests/mdoc/Test/DocTest-frameworkalternate-one.dll ../tests/mdoc/Test/FrameworkTestData-frameworkalternate-fromold/Two/DocTest-frameworkalternate-two.dll
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual -frameworks ../tests/mdoc/Test/FrameworkTestData-frameworkalternate-fromold

	yes | cp ../tests/mdoc/Test/DocTest-frameworkalternate-two.dll ../tests/mdoc/Test/FrameworkTestData-frameworkalternate-fromold/Two/DocTest-frameworkalternate-two.dll
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual -frameworks ../tests/mdoc/Test/FrameworkTestData-frameworkalternate-fromold
	$(DIFF) ../tests/mdoc/Test/en.expected-frameworkalternate ../tests/mdoc/Test/en.actual
	

# ------- Framework Alternate Test(s) -------

.PHONY: check-monodocer-import-fx
check-monodocer-import-fx: check-monodocer-import-fx-work
	$(DIFF) ../tests/mdoc/Test/en.expected-fx-import ../tests/mdoc/Test/en.actual

.PHONY: check-monodocer-import-fx-update
check-monodocer-import-fx-update: check-monodocer-import-fx-work
	rm -Rf ../tests/mdoc/Test/en.expected-fx-import
	mv ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/en.expected-fx-import

check-monodocer-attached-entities:
	-rm -Rf ../tests/mdoc/Test/en.actual
	$(MAKE) ../tests/mdoc/AttachedEventsAndProperties/bin/Release/AttachedEventsAndProperties.dll
	$(MONO) $(PROGRAM) update --debug -o ../tests/mdoc/Test/en.actual ../tests/mdoc/AttachedEventsAndProperties/bin/Release/AttachedEventsAndProperties.dll -lang docid -lang vb.net -lang fsharp -lang javascript -lang c++/cli -lang c++/cx -lang c++/winrt
	$(DIFF) ../tests/mdoc/Test/en.expected-attached-entities ../tests/mdoc/Test/en.actual

	# now make sure it will delete a previously run/duplicated attachedproperty/property
	cp ../tests/mdoc/AttachedEventsAndProperties/AttachedPropertyExample.xml ../tests/mdoc/Test/en.actual/AttachedEventsAndProperties/
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual ../tests/mdoc/AttachedEventsAndProperties/bin/Release/AttachedEventsAndProperties.dll --delete -lang docid -lang vb.net -lang fsharp -lang javascript -lang c++/cli -lang c++/cx -lang c++/winrt
	$(DIFF) ../tests/mdoc/Test/en.expected-attached-entities ../tests/mdoc/Test/en.actual
	
../tests/mdoc/Test/TestClass.dll:
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ ../tests/mdoc/mdoc.Test/SampleClasses/Test*.cs

../tests/mdoc/Test/GuidClass.dll:
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ ../tests/mdoc/mdoc.Test/SampleClasses/GuidClass.cs

.PHONY: check-monodocer-operators-work
check-monodocer-operators-work: ../tests/mdoc/Test/TestClass.dll
	rm -Rf ../tests/mdoc/Test/en.actual
	$(MONO) $(PROGRAM) update ../tests/mdoc/Test/TestClass.dll -o ../tests/mdoc/Test/en.actual
	cp ../tests/mdoc/mdoc.Test/SampleClasses/TestClass-OldOpSig.xml ../tests/mdoc/Test/en.actual/mdoc.Test.SampleClasses/TestClass.xml
	$(MONO) $(PROGRAM) update ../tests/mdoc/Test/TestClass.dll -o ../tests/mdoc/Test/en.actual --delete

.PHONY: check-monodocer-operators
check-monodocer-operators: check-monodocer-operators-work 
	$(DIFF) ../tests/mdoc/Test/en.expected-operators ../tests/mdoc/Test/en.actual

.PHONY: check-monodocer-operators-update
check-monodocer-operators-update: check-monodocer-operators-work
	rm -Rf ../tests/mdoc/Test/en.expected-operators
	mv ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/en.expected-operators

check-monodocer-guid: ../tests/mdoc/Test/GuidClass.dll
	rm -Rf ../tests/mdoc/Test/en.actual
	$(MONO) $(PROGRAM) update -o ../tests/mdoc/Test/en.actual ../tests/mdoc/Test/GuidClass.dll -lang c++/cx -lang c++/winrt
	$(DIFF) ../tests/mdoc/Test/en.expected-guid ../tests/mdoc/Test/en.actual

check-mdoc-export-html-update: 
	find ../tests/mdoc/Test/html.expected -name \*.html -exec rm "{}" \;
	$(MONO) $(PROGRAM) export-html -o ../tests/mdoc/Test/html.expected \
		../tests/mdoc/Test/en.expected.importslashdoc 

check-mdoc-export-html-with-array-extension: 
	rm -Rf ../tests/mdoc/Test/html.actual
	$(MAKE) check-monodocer
	$(MONO) $(PROGRAM) export-html -o ../tests/mdoc/Test/html.actual \
		../tests/mdoc/Test/en.actual
	$(DIFF) ../tests/mdoc/Test/html.expected-with-array-extension ../tests/mdoc/Test/html.actual

check-mdoc-export-html: check-monodocer 
	rm -Rf ../tests/mdoc/Test/html.actual
	$(MONO) $(PROGRAM) export-html -o ../tests/mdoc/Test/html.actual \
		../tests/mdoc/Test/en.expected.importslashdoc
	$(DIFF) ../tests/mdoc/Test/html.expected ../tests/mdoc/Test/html.actual

check-mdoc-export-html-with-version: 
	rm -Rf ../tests/mdoc/Test/html.actual.v0 ../tests/mdoc/Test/html.actual.since-with-v0 .v0.txt .v2.txt
	$(MONO) $(PROGRAM) export-html -o ../tests/mdoc/Test/html.actual.v0 \
		../tests/mdoc/Test/en.expected
	$(MONO) $(PROGRAM) export-html -o ../tests/mdoc/Test/html.actual.since-with-v0 \
		../tests/mdoc/Test/en.expected.since -with-version 0.0.0.0
	(cd ../tests/mdoc/Test/html.actual.v0            && find . -type f) | sort > .v0.txt
	(cd ../tests/mdoc/Test/html.actual.since-with-v0 && find . -type f) | sort > .v2.txt
	$(DIFF) .v0.txt .v2.txt   # assert no types added

check-md-html-dir: 
	rm -Rf ../tests/mdoc/Test/html.actual
	$(MONO) $(PROGRAM) export-html -dest:Test/html.actual $(DIR) 
	$(DIFF) ../tests/mdoc/Test/html.expected ../tests/mdoc/Test/html.actual

check-mdoc-export-msxdoc-update:
	$(MONO) $(PROGRAM) export-msxdoc -o - ../tests/mdoc/Test/en.expected.importslashdoc \
		> ../tests/mdoc/Test/msxdoc-expected.importslashdoc.xml

check-mdoc-export-msxdoc:
	$(MONO) $(PROGRAM) export-msxdoc -o - ../tests/mdoc/Test/en.expected.importslashdoc \
		| $(DIFF) - ../tests/mdoc/Test/msxdoc-expected.importslashdoc.xml

my_abs_top_srcdir = $(shell cd . && pwd)

check-mdoc-validate-update: 
	$(MONO) $(PROGRAM) validate -f ecma ../tests/mdoc/Test/en.expected 2>&1 | \
		sed 's#file://$(my_abs_top_srcdir)/##g' > \
		../tests/mdoc/Test/validate.check.monodocer
	$(MONO) $(PROGRAM) validate -f ecma ../tests/mdoc/Test/en.expected.importslashdoc 2>&1 | \
		sed 's#file://$(my_abs_top_srcdir)/##g' > \
		../tests/mdoc/Test/validate.check.monodocer.importslashdoc
	$(MONO) $(PROGRAM) validate -f ecma ../tests/mdoc/Test/en.expected.since 2>&1 | \
		sed 's#file://$(my_abs_top_srcdir)/##g' > \
		../tests/mdoc/Test/validate.check.monodocer.since

check-mdoc-validate: 
	$(MONO) $(PROGRAM) validate -f ecma ../tests/mdoc/Test/en.expected 2>&1 | \
		sed 's#file://$(my_abs_top_srcdir)/##g' | \
		$(DIFF_QUIET) - ../tests/mdoc/Test/validate.check.monodocer
	$(MONO) $(PROGRAM) validate -f ecma ../tests/mdoc/Test/en.expected.importslashdoc 2>&1 | \
		sed 's#file://$(my_abs_top_srcdir)/##g' | \
		$(DIFF_QUIET) - ../tests/mdoc/Test/validate.check.monodocer.importslashdoc
	$(MONO) $(PROGRAM) validate -f ecma ../tests/mdoc/Test/en.expected.since 2>&1 | \
		sed 's#file://$(my_abs_top_srcdir)/##g' | \
		$(DIFF_QUIET) - ../tests/mdoc/Test/validate.check.monodocer.since
		
check-overwrite-attribute: 
	rm -Rf ../tests/mdoc/Test/en.actual
	$(CSCOMPILE) $(TEST_CSCFLAGS) -target:library ../tests/mdoc/Test/test-overwrite-attribute/SomeClass.cs -doc:../tests/mdoc/Test/test-overwrite-attribute/SomeClass.xml
	$(MONO) $(PROGRAM) update ../tests/mdoc/Test/test-overwrite-attribute/SomeClass.dll -o ../tests/mdoc/Test/en.actual/ -import ../tests/mdoc/Test/test-overwrite-attribute/SomeClass.xml
	cp ../tests/mdoc/Test/test-overwrite-attribute/Input_SomeClass.xml ../tests/mdoc/Test/en.actual/SomeClass.xml
	$(MONO) $(PROGRAM) update ../tests/mdoc/Test/test-overwrite-attribute/SomeClass.dll -o ../tests/mdoc/Test/en.actual/ -import ../tests/mdoc/Test/test-overwrite-attribute/SomeClass.xml
	$(DIFF) ../tests/mdoc/Test/test-overwrite-attribute/Expected_SomeClass.xml ../tests/mdoc/Test/en.actual/SomeClass.xml
	
check-ignored-namespace-generic: 
	rm -Rf ../tests/mdoc/Test/en.actual
	-rm -f ../tests/mdoc/Test/test-generic-ignored-namespace/ReadOnlySpan.dll
	$(CSCOMPILE) $(TEST_CSCFLAGS) -target:library ../tests/mdoc/Test/test-generic-ignored-namespace/ReadOnlySpan.cs
	$(MONO) $(PROGRAM) update ../tests/mdoc/Test/test-generic-ignored-namespace/ReadOnlySpan.dll -o ../tests/mdoc/Test/en.actual/

	$(DIFF) ../tests/mdoc/Test/test-generic-ignored-namespace/en.expected-generic-ignored-namespace ../tests/mdoc/Test/en.actual/

check-type-projection: ../tests/mdoc/Test/DocTest-TypeProjection.dll
	rm -Rf ../tests/mdoc/Test/en.actual
	rm -Rf ../tests/mdoc/Test/test-type-projection

	mkdir ../tests/mdoc/Test/test-type-projection
	mkdir ../tests/mdoc/Test/test-type-projection/DocTest-TypeProjection
	mkdir ../tests/mdoc/Test/en.actual

	cp ../tests/mdoc/Test/DocTest-TypeProjection.dll ../tests/mdoc/Test/test-type-projection/DocTest-TypeProjection
	cp ../tests/mdoc/Test/TestTypeMap.xml ../tests/mdoc/Test/test-type-projection/
	mv ../tests/mdoc/Test/test-type-projection/TestTypeMap.xml ../tests/mdoc/Test/test-type-projection/TypeMap.xml

	$(MONO) $(PROGRAM) fx-bootstrap -fx ../tests/mdoc/Test/test-type-projection
	$(MONO) $(PROGRAM) update -fx ../tests/mdoc/Test/test-type-projection/ -o ../tests/mdoc/Test/en.actual/
	$(DIFF) ../tests/mdoc/Test/en.expected-type-projection ../tests/mdoc/Test/en.actual/

check-long-file-name: ../tests/mdoc/Test/DocTest-LongFileName.dll
	rm -Rf ../tests/mdoc/Test/en.actual
	rm -Rf ../tests/mdoc/Test/test-long-file-name

	mkdir ../tests/mdoc/Test/test-long-file-name
	mkdir ../tests/mdoc/Test/test-long-file-name/DocTest-LongFileName
	mkdir ../tests/mdoc/Test/en.actual

	cp ../tests/mdoc/Test/DocTest-LongFileName.dll ../tests/mdoc/Test/test-long-file-name/DocTest-LongFileName

	$(MONO) $(PROGRAM) fx-bootstrap -fx ../tests/mdoc/Test/test-long-file-name
	$(MONO) $(PROGRAM) update -fx ../tests/mdoc/Test/test-long-file-name -o ../tests/mdoc/Test/en.actual/

run-test-local: check-doc-tools

run-test-update : check-doc-tools-update

check-doc-tools:  \
	check-monodocer-differentTypeDefinitions \
	check-monodocer-typeForwards \
	check-monodocer-nestedType-typeForwards \
	check-monodocer-Eii-importslashdoc \
	check-monodocer-Eii-importecmadoc-oldNames \
	check-monodocer-Eii \
	check-monodocer-since \
	check-monodocer-importecmadoc \
	check-monodocer-importslashdoc \
	check-monodocer-import-fx \
	check-monodocer \
	check-monodocer-delete \
	check-mdoc-export-html \
	check-mdoc-export-html-with-version \
	check-mdoc-export-html-with-array-extension \
	check-mdoc-export-msxdoc \
	check-mdoc-validate \
	check-monodocer-dropns-classic \
	check-monodocer-dropns-classic-withsecondary \
	check-monodocer-internal-interface \
	check-monodocer-addNonGeneric \
	check-monodocer-membergroup \
	check-monodocer-ignore-invalid-assemblies \
	check-monodocer-enumerations \
	check-monodocer-dropns-multi \
	check-monodocer-dropns-multi-withexisting \
	check-monodocer-frameworks \
	check-monodocer-frameworks-inheritance \
	check-monodocer-docid \
	check-monodocer-operators \
	check-monodocer-guid \
	check-monodocer-fx-statistics-remove \
	check-overwrite-attribute \
	check-monodocer-vbnet \
	check-monodocer-vbnet2 \
	check-monodocer-javascript \
	check-monodocer-frameworks-with-nuget\
	check-monodocer-fsharp \
	check-monodocer-attached-entities \
	check-monodocer-members-implementation \
	check-monodocer-cppcli \
	check-monodocer-cppcxDocTest\
	check-monodocer-cppcx\
	check-monodocer-cppwinrtDocTest\
	check-monodocer-cppwinrt\
	check-monodocer-cppwinrtUwp\
	check-monodocer-frameworkalternate-fromold \
	check-monodocer-frameworkalternate \
	check-monodocer-embedded-type \
	check-ignored-namespace-generic \
	check-multiple-mdoc \
	check-type-projection \
	check-long-file-name \
	check-monodocer-differentTypeParameterNames

#check-monodocer-dropns-delete 
check-doc-tools-update: check-monodocer-since-update \
	check-monodocer-importecmadoc-update \
	check-monodocer-importslashdoc-update \
	check-monodocer-update \
	check-monodocer-delete-update \
	check-mdoc-export-html-update \
	check-mdoc-export-msxdoc-update \
	check-mdoc-validate-update 

check: nunit check-doc-tools
	@echo "mdoc Tests Complete!"
