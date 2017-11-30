CONFIGURATION = Release
PROGRAM = ../bin/$(CONFIGURATION)/mdoc.exe
MONO = mono
CSCOMPILE = mcs
TEST_CSCFLAGS = -target:library

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
	Test/CLILibraryTypes.dtd				  \
	Test/DocTest-v1.cs                                        \
	Test/DocTest-v2.patch                                     \
	Test/msxdoc-expected.importslashdoc.xml                   \
	Test/TestEcmaDocs.xml                                     \
	Test/validate.check.monodocer                             \
	Test/validate.check.monodocer.importslashdoc              \
	Test/validate.check.monodocer.since

EXTRA_DISTFILES = \
	$(MDOC_RESOURCES) \
	$(MDOC_TEST_FILES)

MULTI-CLASSIC = Test/DocTest-DropNS-classic.dll Test/DocTest-DropNS-classic-multitest.dll
MULTI-UNIFIED = Test/DocTest-DropNS-unified.dll Test/DocTest-DropNS-unified-multitest.dll

DIFF = diff -rup
DIFF_QUIET = diff --brief
ifeq ($(PLATFORM), win32)
DIFF = diff -rupZ
DIFF_QUIET = diff --brief -Z
endif

cleanup:
	-rm -Rf Test/en.actual Test/html.actual

nunit: 
	mono ../packages/NUnit.ConsoleRunner.3.6.0/tools/nunit3-console.exe mdoc.Test/bin/$(CONFIGURATION)/mdoc.Test.dll

Test/DocTest-addNonGeneric.dll:
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ Test/DocTest-addNonGeneric.cs

Test/DocTest-addNonGeneric-v2.dll:
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ Test/DocTest-addNonGeneric.cs /define:V2

Test/DocTest-DropNS-classic-secondary.dll:
	@echo $(value @)
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ Test/DocTest-DropNS-classic-secondary.cs -doc:Test/DocTest-DropNS-classic-secondary.xml

Test/DocTest-DropNS-classic.dll:
	@echo $(value @)
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ Test/DocTest-DropNS-classic.cs -doc:Test/DocTest-DropNS-classic.xml

Test/DocTest-DropNS-unified.dll:
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ Test/DocTest-DropNS-unified.cs

Test/DocTest-DropNS-unified-multitest.dll:
	rm -f $@
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ Test/DocTest-DropNS-unified.cs /define:MULTITEST

Test/DocTest-DropNS-classic-multitest.dll:
	rm -f $@
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ Test/DocTest-DropNS-classic.cs /define:MULTITEST

Test/DocTest-DropNS-unified-deletetest.dll:
	rm -f Test/DocTest-DropNS-unified-deletetest.dll
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ Test/DocTest-DropNS-unified.cs /define:DELETETEST

Test/DocTest-DropNS-unified-deletetest-V2.dll:
	rm -f Test/DocTest-DropNS-unified-deletetest.dll
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:Test/DocTest-DropNS-unified-deletetest.dll Test/DocTest-DropNS-unified.cs /define:DELETETEST,V2

Test/DocTest-DropNS-classic-deletetest.dll:
	rm -f Test/DocTest-DropNS-classic-deletetest.dll
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ Test/DocTest-DropNS-classic.cs /define:DELETETEST

Test/DocTest-DropNS-classic-deletetest-V2.dll:
	rm -f Test/DocTest-DropNS-classic-deletetest.dll
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:Test/DocTest-DropNS-classic-deletetest.dll Test/DocTest-DropNS-classic.cs /define:DELETETEST,V2

Test/DocTest.dll: 
	$(CSCOMPILE) $(TEST_CSCFLAGS) -debug -optimize -unsafe -target:library -out:$@ Test/DocTest.cs -r:System.Core.dll -r:Microsoft.CSharp.dll

Test/DocTest-InternalInterface.dll: 
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ Test/DocTest-InternalInterface.cs

Test/DocTest-framework-inheritance-one.dll:
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ Test/DocTest-framework-inheritance.cs /define:FXONE

Test/DocTest-framework-inheritance-two.dll:
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ Test/DocTest-framework-inheritance.cs /define:FXTWO

Test/DocTest.dll-v1: 
	-rm -f Test/DocTest.cs
	cp Test/DocTest-v1.cs Test/DocTest.cs
	-rm -f Test/DocTest.dll
	$(MAKE) Test/DocTest.dll

Test/DocTest.dll-v2: 
	-rm -f Test/DocTest.cs
	cp Test/DocTest-v1.cs Test/DocTest.cs
	cd Test && patch -p0 < DocTest-v2.patch
	-rm -f Test/DocTest.dll
	$(MAKE) Test/DocTest.dll

Test/DocTest-enumerations.dll: 
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ Test/DocTest-enumerations.cs

Test/FrameworkTestData: Test/DocTest-addNonGeneric.dll Test/DocTest-DropNS-classic.dll Test/DocTest-DropNS-classic-secondary.dll
	rm -rf Test/FrameworkTestData
	mkdir Test/FrameworkTestData
	mkdir Test/FrameworkTestData/One
	mkdir Test/FrameworkTestData/Two
	cp Test/DocTest-addNonGeneric.dll Test/FrameworkTestData/One/
	cp Test/DocTest-DropNS-classic.dll Test/FrameworkTestData/One/
	cp Test/DocTest-addNonGeneric.dll Test/FrameworkTestData/Two/
	cp Test/DocTest-DropNS-classic-secondary.dll Test/FrameworkTestData/Two/
	$(MONO) $(PROGRAM) fx-bootstrap Test/FrameworkTestData

Test/FrameworkTestData-fx-inheritance: Test/DocTest-framework-inheritance-one.dll Test/DocTest-framework-inheritance-two.dll
	rm -rf Test/FrameworkTestData-fx-inheritance
	mkdir Test/FrameworkTestData-fx-inheritance
	mkdir Test/FrameworkTestData-fx-inheritance/One
	mkdir Test/FrameworkTestData-fx-inheritance/Two
	cp Test/DocTest-framework-inheritance-one.dll Test/FrameworkTestData-fx-inheritance/One/
	cp Test/DocTest-framework-inheritance-two.dll Test/FrameworkTestData-fx-inheritance/Two/
	$(MONO) $(PROGRAM) fx-bootstrap Test/FrameworkTestData-fx-inheritance

check-monodocer-frameworks: Test/FrameworkTestData
	-rm -Rf Test/en.actual
	$(MONO) $(PROGRAM) update -o Test/en.actual -frameworks Test/FrameworkTestData
	$(DIFF) Test/en.expected-frameworks Test/en.actual

check-monodocer-frameworks-inheritance: Test/FrameworkTestData-fx-inheritance
	-rm -Rf Test/en.actual
	$(MONO) $(PROGRAM) update -o Test/en.actual -frameworks Test/FrameworkTestData-fx-inheritance
	$(DIFF) Test/en.expected-frameworks-inheritance Test/en.actual
	
check-monodocer-frameworks-with-nuget: 
	-rm -Rf Test/en.actual
	-rm -rf Test/test-nuget-information/input_data
	mkdir Test/test-nuget-information/input_data
	mkdir Test/test-nuget-information/input_data/One
	mkdir Test/test-nuget-information/input_data/Two
	cp Test/DocTest-addNonGeneric.dll Test/test-nuget-information/input_data/One/
	cp Test/DocTest-DropNS-classic.dll Test/test-nuget-information/input_data/One/
	cp Test/DocTest-addNonGeneric.dll Test/test-nuget-information/input_data/Two/
	cp Test/DocTest-DropNS-classic-secondary.dll Test/test-nuget-information/input_data/Two/
	cp Test/frameworks.xml Test/test-nuget-information/input_data/
	$(MONO) $(PROGRAM) update -o Test/en.actual -frameworks Test/test-nuget-information/input_data
	$(DIFF) Test/test-nuget-information/en.expected-frameworks-with-nuget-information Test/en.actual

check-monodocer-docid: Test/FrameworkTestData
	-rm -Rf Test/en.actual
	$(MONO) $(PROGRAM) update -use-docid -o Test/en.actual -frameworks Test/FrameworkTestData
	$(DIFF) Test/en.expected-docid Test/en.actual
	
check-monodocer-vbnet: Test/FrameworkTestData
	-rm -Rf Test/en.actual
	$(MONO) $(PROGRAM) update -lang vb.net -o Test/en.actual -frameworks Test/FrameworkTestData
	$(DIFF) Test/en.expected-vbnet Test/en.actual
	
check-monodocer-vbnet2: 
	-rm -Rf Test/en.actual
	$(MAKE) Test/DocTest.dll-v1
	$(MONO) $(PROGRAM) update -lang vb.net -o Test/en.actual Test/DocTest.dll
	$(DIFF) Test/en.expected-vbnet2 Test/en.actual

check-monodocer-addNonGeneric: 
	-rm -Rf Test/en.actual
	# first, make a docset with the generic method
	$(MAKE) Test/DocTest-addNonGeneric.dll
	$(MONO) $(PROGRAM) update --exceptions=all -o Test/en.actual Test/DocTest-addNonGeneric.dll

	# now add a non-generic version of the method and update several times
	$(MAKE) Test/DocTest-addNonGeneric-v2.dll
	$(MONO) $(PROGRAM) update --exceptions=all -o Test/en.actual Test/DocTest-addNonGeneric-v2.dll
	$(MONO) $(PROGRAM) update --exceptions=all -o Test/en.actual Test/DocTest-addNonGeneric-v2.dll
	$(MONO) $(PROGRAM) update --exceptions=all -o Test/en.actual Test/DocTest-addNonGeneric-v2.dll
	$(DIFF) Test/en.expected-addNonGeneric Test/en.actual

check-monodocer-membergroup: Test/DocTest-addNonGeneric-v2.dll Test/DocTest-addNonGeneric.dll
	-rm -Rf Test/en.actual
	cp -r Test/en.expected-membergroup Test/en.actual
	$(MONO) $(PROGRAM) update --debug --exceptions=all -o Test/en.actual Test/DocTest-addNonGeneric-v2.dll
	$(DIFF) Test/en.expected-membergroup Test/en.actual
	$(MONO) $(PROGRAM) validate -f ecma Test/en.actual

check-monodocer-dropns-classic: 
	# tests the simplest --dropns case, a single class where the root namespace was dropped.
	-rm -Rf Test/en.actual
	$(MAKE) Test/DocTest-DropNS-classic.dll
	$(MONO) $(PROGRAM) update --exceptions=all -o Test/en.actual Test/DocTest-DropNS-classic.dll --api-style=classic
	$(MAKE) update-monodocer-dropns-unified
	$(DIFF) Test/en.expected-dropns-classic-v1 Test/en.actual

check-monodocer-dropns-multi: 
	-rm -Rf Test/en.actual
	$(MAKE) Test/DocTest-DropNS-classic.dll
	$(MAKE) Test/DocTest-DropNS-unified.dll
	$(MAKE) Test/DocTest-DropNS-classic-multitest.dll
	$(MAKE) Test/DocTest-DropNS-unified-multitest.dll

	# mdoc update for both classic and unified
	$(MONO) $(PROGRAM) update --exceptions=all -o Test/en.actual $(MULTI-CLASSIC) --api-style=classic 
	$(MONO) $(PROGRAM) update --exceptions=all -o Test/en.actual $(MULTI-UNIFIED) --api-style=unified --dropns Test/DocTest-DropNS-unified.dll=MyFramework --dropns Test/DocTest-DropNS-unified-multitest.dll=MyFramework 
	
	# now run it again to verify idempotency
	$(MONO) $(PROGRAM) update --exceptions=all -o Test/en.actual $(MULTI-CLASSIC) --api-style=classic 
	$(MONO) $(PROGRAM) update --exceptions=all -o Test/en.actual $(MULTI-UNIFIED) --api-style=unified --dropns Test/DocTest-DropNS-unified.dll=MyFramework --dropns Test/DocTest-DropNS-unified-multitest.dll=MyFramework 
	
	$(DIFF) Test/en.expected-dropns-multi Test/en.actual


check-monodocer-dropns-multi-withexisting: 
	-rm -Rf Test/en.actual
	$(MAKE) Test/DocTest-DropNS-classic.dll
	$(MAKE) Test/DocTest-DropNS-unified.dll
	$(MAKE) Test/DocTest-DropNS-classic-multitest.dll
	$(MAKE) Test/DocTest-DropNS-unified-multitest.dll

	# mdoc update to show a pre-existing set of documents
	$(MONO) $(PROGRAM) update --exceptions=all -o Test/en.actual Test/DocTest-DropNS-classic.dll --api-style=classic
	$(MONO) $(PROGRAM) update --exceptions=all -o Test/en.actual Test/DocTest-DropNS-unified.dll --api-style=unified --dropns Test/DocTest-DropNS-unified.dll=MyFramework 
	
	# mdoc update for both classic and unified
	$(MONO) $(PROGRAM) update --exceptions=all -o Test/en.actual $(MULTI-CLASSIC) --api-style=classic 
	$(MONO) $(PROGRAM) update --exceptions=all -o Test/en.actual $(MULTI-UNIFIED) --api-style=unified --dropns Test/DocTest-DropNS-unified.dll=MyFramework --dropns Test/DocTest-DropNS-unified-multitest.dll=MyFramework 
	
	$(DIFF) Test/en.expected-dropns-multi-withexisting Test/en.actual

check-monodocer-dropns-delete: 
	-rm -Rf Test/en.actual
	-rm -Rf Test/actual_statistics.txt
	rm -Rf Test/DocTest-DropNS-classic-deletetest.dll
	rm -Rf Test/DocTest-DropNS-unified-deletetest.dll
	$(MAKE) Test/DocTest-DropNS-classic-deletetest.dll
	$(MONO) $(PROGRAM) update --delete --exceptions=all -o Test/en.actual Test/DocTest-DropNS-classic-deletetest.dll --api-style=classic
	$(MAKE) Test/DocTest-DropNS-unified-deletetest.dll
	$(MONO) $(PROGRAM) update --delete --exceptions=all -o Test/en.actual Test/DocTest-DropNS-unified-deletetest.dll --api-style=unified --dropns Test/DocTest-DropNS-unified-deletetest.dll=MyFramework
	$(MAKE) Test/DocTest-DropNS-classic-deletetest-V2.dll
	$(MONO) $(PROGRAM) update --delete --exceptions=all -o Test/en.actual Test/DocTest-DropNS-classic-deletetest.dll --api-style=classic
	$(MAKE) Test/DocTest-DropNS-unified-deletetest-V2.dll
	$(MONO) $(PROGRAM) update --delete --exceptions=all -o Test/en.actual Test/DocTest-DropNS-unified-deletetest.dll --api-style=unified --dropns Test/DocTest-DropNS-unified-deletetest.dll=MyFramework -statistics Test/actual_statistics.txt
	$(DIFF) Test/en.expected-dropns-delete Test/en.actual
	$(DIFF) Test/expected_remove_statistics.txt Test/actual_statistics.txt

check-monodocer-dropns-classic-withsecondary: 
	# tests case where a secondary assembly is included with a --dropns parameter
	-rm -Rf Test/en.actual
	$(MAKE) Test/DocTest-DropNS-classic.dll
	$(MAKE) Test/DocTest-DropNS-classic-secondary.dll
	$(MONO) $(PROGRAM) update --exceptions=all -o Test/en.actual Test/DocTest-DropNS-classic.dll Test/DocTest-DropNS-classic-secondary.dll --api-style=classic
	$(MAKE) update-monodocer-dropns-unified-withsecondary
	$(DIFF) Test/en.expected-dropns-classic-withsecondary Test/en.actual

update-monodocer-dropns-unified: 
	$(MAKE) Test/DocTest-DropNS-unified.dll
	$(MONO) $(PROGRAM) update --debug --exceptions=all -o Test/en.actual Test/DocTest-DropNS-unified.dll --api-style=unified --dropns Test/DocTest-DropNS-unified.dll=MyFramework

update-monodocer-dropns-unified-withsecondary: 
	$(MAKE) Test/DocTest-DropNS-unified.dll
	$(MONO) $(PROGRAM) update --exceptions=all -o Test/en.actual Test/DocTest-DropNS-unified.dll Test/DocTest-DropNS-classic-secondary.dll --api-style=unified --dropns Test/DocTest-DropNS-unified.dll=MyFramework

update-monodocer-dropns-classic-secondary: 
	$(MAKE) Test/DocTest-DropNS-classic-secondary.dll
	$(MONO) $(PROGRAM) update --exceptions=all -o Test/en.actual Test/DocTest-DropNS-classic-secondary.dll --api-style=classic

check-monodocer-internal-interface: 
	# Tests to make sure internal interfaces that are explicitly implemented are not documented
	-rm -Rf Test/en.actual
	$(MAKE) Test/DocTest-InternalInterface.dll
	$(MONO) $(PROGRAM) update --exceptions=all -o Test/en.actual Test/DocTest-InternalInterface.dll -lang VB.NET
	$(DIFF) Test/en.expected-internal-interface Test/en.actual

check-monodocer-enumerations: 
	-rm -Rf Test/en.actual
	$(MAKE) Test/DocTest-enumerations.dll
	$(MONO) $(PROGRAM) update --exceptions=all -o Test/en.actual Test/DocTest-enumerations.dll
	$(DIFF) Test/en.expected-enumerations Test/en.actual

check-monodocer-update: 
	find Test/en.expected -name \*.xml -exec rm "{}" \;
	$(MAKE) Test/DocTest.dll-v1
	$(MONO) $(PROGRAM) update --exceptions=all -o Test/en.expected Test/DocTest.dll

check-monodocer: 
	-rm -Rf Test/en.actual
	$(MAKE) Test/DocTest.dll-v1
	$(MONO) $(PROGRAM) update --debug --exceptions=all -o Test/en.actual Test/DocTest.dll
	$(DIFF) Test/en.expected Test/en.actual
	$(MONO) $(PROGRAM) update --debug --exceptions=all -o Test/en.actual Test/DocTest.dll 
	$(DIFF) Test/en.expected Test/en.actual

check-monodocer-since-update: 
	find Test/en.expected.since -name \*.xml -exec rm "{}" \;
	$(MAKE) Test/DocTest.dll-v1
	$(MONO) $(PROGRAM) update --exceptions=all -o Test/en.expected.since Test/DocTest.dll 
	$(MAKE) Test/DocTest.dll-v2
	$(MONO) $(PROGRAM) update --exceptions=all --since="Version 2.0" \
		-o Test/en.expected.since Test/DocTest.dll 

check-monodocer-since: 
	rm -Rf Test/en.actual
	$(MAKE) Test/DocTest.dll-v1
	$(MONO) $(PROGRAM) --debug update --exceptions=all -o Test/en.actual Test/DocTest.dll 
	$(MAKE) Test/DocTest.dll-v2
	$(MONO) $(PROGRAM) --debug update --exceptions=all --since="Version 2.0" \
		-o Test/en.actual Test/DocTest.dll 
	$(DIFF) Test/en.expected.since Test/en.actual

check-monodocer-delete-update: 
	find Test/en.expected.delete -type f -exec rm "{}" \;
	$(MAKE) Test/DocTest.dll-v1
	$(MONO) $(PROGRAM) update --exceptions=all -o Test/en.expected.delete Test/DocTest.dll
	$(MAKE) Test/DocTest.dll-v2
	$(MONO) $(PROGRAM) update --exceptions=all -o Test/en.expected.delete Test/DocTest.dll
	$(MAKE) Test/DocTest.dll-v1
	$(MONO) $(PROGRAM) update -fno-assembly-versions --delete --exceptions=all \
		-o Test/en.expected.delete Test/DocTest.dll

check-monodocer-delete: 
	rm -Rf Test/en.actual
	$(MAKE) Test/DocTest.dll-v1
	$(MONO) $(PROGRAM) --debug update --exceptions=all -o Test/en.actual Test/DocTest.dll
	$(MAKE) Test/DocTest.dll-v2
	$(MONO) $(PROGRAM) --debug update --exceptions=all -o Test/en.actual Test/DocTest.dll
	$(MAKE) Test/DocTest.dll-v1
	$(MONO) $(PROGRAM) --debug update -fno-assembly-versions --delete --exceptions=all -o Test/en.actual Test/DocTest.dll
	$(DIFF) Test/en.expected.delete Test/en.actual

check-monodocer-ignore-invalid-assemblies: Test/DocTest-addNonGeneric.dll Test/DocTest-addNonGeneric-v2.dll
	-rm -Rf Test/en.actual
	touch Test/notActuallyA.dll
	$(MONO) $(PROGRAM) --debug update -o Test/en.actual Test/DocTest-addNonGeneric.dll Test/notActuallyA.dll
	$(MONO) $(PROGRAM) --debug update -o Test/en.actual Test/DocTest-addNonGeneric-v2.dll Test/notActuallyA.dll
	$(DIFF) Test/en.expected-addNonGeneric Test/en.actual

check-monodocer-importslashdoc-update: 
	find Test/en.expected.importslashdoc -name \*.xml -exec rm "{}" \;
	$(MAKE) Test/DocTest.dll-v1 TEST_CSCFLAGS=-doc:Test/DocTest.xml
	$(MONO) $(PROGRAM) --debug update --exceptions=all -i Test/DocTest.xml \
		-o Test/en.expected.importslashdoc Test/DocTest.dll 

check-monodocer-importslashdoc: 
	rm -Rf Test/en.actual
	$(MAKE) Test/DocTest.dll-v1 TEST_CSCFLAGS=-doc:Test/DocTest.xml
	$(MONO) $(PROGRAM) --debug update --exceptions=all -i Test/DocTest.xml \
		-o Test/en.actual Test/DocTest.dll 
	$(DIFF) Test/en.expected.importslashdoc Test/en.actual

check-monodocer-importecmadoc-update: 
	find Test/en.expected.importecmadoc -name \*.xml -exec rm "{}" \;
	$(MAKE) Test/DocTest.dll-v1
	$(MONO) $(PROGRAM) --debug update --exceptions=all -i Test/TestEcmaDocs.xml \
		'--type=System.Action`1' --type=System.AsyncCallback \
		--type=System.Environment --type=System.Array \
		-o Test/en.expected.importecmadoc Test/DocTest.dll 

check-monodocer-importecmadoc: 
	rm -Rf Test/en.actual
	$(MAKE) Test/DocTest.dll-v1
	$(MONO) $(PROGRAM) --debug update --exceptions=all -i Test/TestEcmaDocs.xml \
		'--type=System.Action`1' --type=System.AsyncCallback \
		--type=System.Environment --type=System.Array \
		-o Test/en.actual Test/DocTest.dll 
	$(DIFF) Test/en.expected.importecmadoc Test/en.actual

.PHONY: check-monodocer-import-fx-work
check-monodocer-import-fx-work: Test/DocTest.dll-v1 Test/DocTest-DropNS-classic-secondary.dll Test/DocTest-DropNS-classic.dll
	rm -Rf Test/en.actual
	rm -Rf Test/fx-import
	rm -Rf Test/actual_statistics.txt
	mkdir Test/fx-import
	mkdir Test/fx-import/one
	mkdir Test/fx-import/two
	cp Test/DocTest.dll Test/fx-import/one
	cp Test/DocTest-DropNS-classic-secondary.dll Test/fx-import/two
	cp Test/DocTest-DropNS-classic.dll Test/fx-import/two
	cp Test/DocTest-DropNS-classic-secondary.xml Test/fx-import/TestEcmaDocs2.xml
	cp Test/DocTest-DropNS-classic.xml Test/fx-import/DocTest-DropNS-classic.xml
	cp Test/DocTest.xml Test/fx-import/TestEcmaDocs.xml
	cp Test/CLILibraryTypes.dtd Test/fx-import/
	cp Test/fx-import-configuration.xml Test/fx-import/frameworks.xml
	$(MONO) $(PROGRAM) update -o Test/en.actual -frameworks Test/fx-import -statistics Test/actual_statistics.txt
	$(DIFF) Test/expected_statistics.txt Test/actual_statistics.txt
	
.PHONY: check-monodocer-fx-statistics-remove	
check-monodocer-fx-statistics-remove:
	rm -Rf Test/en.actual
	rm -Rf Test/fx-import
	rm -Rf Test/actual_statistics.txt
	mkdir Test/fx-import
	mkdir Test/fx-import/one
	cp Test/fx-statistics-remove-configuration.xml Test/fx-import/frameworks.xml
	rm -Rf Test/DocTest-DropNS-unified-deletetest.dll
	$(MAKE) Test/DocTest-DropNS-unified-deletetest.dll
	cp Test/DocTest-DropNS-unified-deletetest.dll Test/fx-import/one/DocTest.dll
	$(MONO) $(PROGRAM) update -o Test/en.actual -frameworks Test/fx-import
	rm -Rf Test/DocTest-DropNS-unified.dll
	$(MAKE) Test/DocTest-DropNS-unified.dll
	rm -rf Test/fx-import/one/DocTest.dll
	cp Test/DocTest-DropNS-unified.dll Test/fx-import/one/DocTest.dll
	$(MONO) $(PROGRAM) update -o Test/en.actual -frameworks Test/fx-import -statistics Test/actual_statistics.txt
	$(DIFF) Test/expected_fx_remove_statistics.txt Test/actual_statistics.txt
	
check-monodocer-fsharp:
	-rm -Rf Test/en.actual
	msbuild ../mdoc/mdoc.Test/mdoc.Test.FSharp/mdoc.Test.FSharp.fsproj -property:Configuration=Release
	$(MONO) $(PROGRAM) update -lang fsharp -o Test/en.actual ../mdoc/mdoc.Test/mdoc.Test.FSharp/bin/Release/mdoc.Test.FSharp.dll
	$(DIFF) Test/en.expected-fsharp Test/en.actual


.PHONY: check-monodocer-import-fx
check-monodocer-import-fx: check-monodocer-import-fx-work
	$(DIFF) Test/en.expected-fx-import Test/en.actual

.PHONY: check-monodocer-import-fx-update
check-monodocer-import-fx-update: check-monodocer-import-fx-work
	rm -Rf Test/en.expected-fx-import
	mv Test/en.actual Test/en.expected-fx-import

check-monodocer-attached-entities:
	-rm -Rf Test/en.actual
	msbuild ../mdoc/Test/AttachedEventsAndProperties/AttachedEventsAndProperties.csproj -property:Configuration=Release
	$(MONO) $(PROGRAM) update -o Test/en.actual Test/AttachedEventsAndProperties/bin/Release/AttachedEventsAndProperties.dll -lang docid
	$(DIFF) Test/en.expected-attached-entities Test/en.actual
	
Test/TestClass.dll:
	$(CSCOMPILE) $(TEST_CSCFLAGS) -unsafe -debug -optimize -target:library -out:$@ mdoc.Test/SampleClasses/Test*.cs

.PHONY: check-monodocer-operators-work
check-monodocer-operators-work: Test/TestClass.dll
	rm -Rf Test/en.actual
	$(MONO) $(PROGRAM) update Test/TestClass.dll -o Test/en.actual
	cp mdoc.Test/SampleClasses/TestClass-OldOpSig.xml Test/en.actual/mdoc.Test.SampleClasses/TestClass.xml
	$(MONO) $(PROGRAM) update Test/TestClass.dll -o Test/en.actual

.PHONY: check-monodocer-operators
check-monodocer-operators: check-monodocer-operators-work 
	$(DIFF) Test/en.expected-operators Test/en.actual

.PHONY: check-monodocer-operators-update
check-monodocer-operators-update: check-monodocer-operators-work
	rm -Rf Test/en.expected-operators
	mv Test/en.actual Test/en.expected-operators

check-mdoc-export-html-update: 
	find Test/html.expected -name \*.html -exec rm "{}" \;
	$(MONO) $(PROGRAM) export-html -o Test/html.expected \
		Test/en.expected.importslashdoc 

check-mdoc-export-html-with-array-extension: 
	rm -Rf Test/html.actual
	$(MAKE) check-monodocer
	$(MONO) $(PROGRAM) export-html -o Test/html.actual \
		Test/en.actual
	$(DIFF) Test/html.expected-with-array-extension Test/html.actual

check-mdoc-export-html: check-monodocer 
	rm -Rf Test/html.actual
	$(MONO) $(PROGRAM) export-html -o Test/html.actual \
		Test/en.expected.importslashdoc
	$(DIFF) Test/html.expected Test/html.actual

check-mdoc-export-html-with-version: 
	rm -Rf Test/html.actual.v0 Test/html.actual.since-with-v0 .v0.txt .v2.txt
	$(MONO) $(PROGRAM) export-html -o Test/html.actual.v0 \
		Test/en.expected
	$(MONO) $(PROGRAM) export-html -o Test/html.actual.since-with-v0 \
		Test/en.expected.since -with-version 0.0.0.0
	(cd Test/html.actual.v0            && find . -type f) | sort > .v0.txt
	(cd Test/html.actual.since-with-v0 && find . -type f) | sort > .v2.txt
	$(DIFF) .v0.txt .v2.txt   # assert no types added

check-md-html-dir: 
	rm -Rf Test/html.actual
	$(MONO) $(PROGRAM) export-html -dest:Test/html.actual $(DIR) 
	$(DIFF) Test/html.expected Test/html.actual

check-mdoc-export-msxdoc-update:
	$(MONO) $(PROGRAM) export-msxdoc -o - Test/en.expected.importslashdoc \
		> Test/msxdoc-expected.importslashdoc.xml

check-mdoc-export-msxdoc:
	$(MONO) $(PROGRAM) export-msxdoc -o - Test/en.expected.importslashdoc \
		| $(DIFF_QUIET) - Test/msxdoc-expected.importslashdoc.xml

my_abs_top_srcdir = $(shell cd . && pwd)

check-mdoc-validate-update: 
	$(MONO) $(PROGRAM) validate -f ecma Test/en.expected 2>&1 | \
		sed 's#file://$(my_abs_top_srcdir)/##g' > \
		Test/validate.check.monodocer
	$(MONO) $(PROGRAM) validate -f ecma Test/en.expected.importslashdoc 2>&1 | \
		sed 's#file://$(my_abs_top_srcdir)/##g' > \
		Test/validate.check.monodocer.importslashdoc
	$(MONO) $(PROGRAM) validate -f ecma Test/en.expected.since 2>&1 | \
		sed 's#file://$(my_abs_top_srcdir)/##g' > \
		Test/validate.check.monodocer.since

check-mdoc-validate: 
	$(MONO) $(PROGRAM) validate -f ecma Test/en.expected 2>&1 | \
		sed 's#file://$(my_abs_top_srcdir)/##g' | \
		$(DIFF_QUIET) - Test/validate.check.monodocer
	$(MONO) $(PROGRAM) validate -f ecma Test/en.expected.importslashdoc 2>&1 | \
		sed 's#file://$(my_abs_top_srcdir)/##g' | \
		$(DIFF_QUIET) - Test/validate.check.monodocer.importslashdoc
	$(MONO) $(PROGRAM) validate -f ecma Test/en.expected.since 2>&1 | \
		sed 's#file://$(my_abs_top_srcdir)/##g' | \
		$(DIFF_QUIET) - Test/validate.check.monodocer.since
		
check-overwrite-attribute: 
	rm -Rf Test/en.actual
	$(CSCOMPILE) $(TEST_CSCFLAGS) -target:library Test/test-overwrite-attribute/SomeClass.cs -doc:Test/test-overwrite-attribute/SomeClass.xml
	$(MONO) $(PROGRAM) update Test/test-overwrite-attribute/SomeClass.dll -o Test/en.actual/ -import Test/test-overwrite-attribute/SomeClass.xml
	cp Test/test-overwrite-attribute/Input_SomeClass.xml Test/en.actual/SomeClass.xml
	$(MONO) $(PROGRAM) update Test/test-overwrite-attribute/SomeClass.dll -o Test/en.actual/ -import Test/test-overwrite-attribute/SomeClass.xml
	$(DIFF) Test/test-overwrite-attribute/Expected_SomeClass.xml Test/en.actual/SomeClass.xml
run-test-local: check-doc-tools

run-test-update : check-doc-tools-update

check-doc-tools: check-monodocer-since \
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
	check-monodocer-dropns-delete \
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
	check-monodocer-fx-statistics-remove \
	check-overwrite-attribute \
	check-monodocer-vbnet \
	check-monodocer-vbnet2 \
	check-monodocer-frameworks-with-nuget\
	check-monodocer-fsharp \
	check-monodocer-attached-entities \


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
