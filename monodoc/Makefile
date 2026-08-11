CONFIGURATION = Release
MONO = $(if $(filter Windows_NT,$(OS)),,mono)

check: 
	$(MONO) ../packages/NUnit.ConsoleRunner.3.6.0/tools/nunit3-console.exe Test/bin/$(CONFIGURATION)/Monodoc.Test.dll
	@echo "monodoc Tests Complete!"
