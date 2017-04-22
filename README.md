# `mdoc` and `monodoc`

This repository contains the source for Mono's [documentation toolchain](http://www.mono-project.com/docs/tools+libraries/tools/monodoc/generating-documentation/).

## Compiling

### CLI
If you've got `make` installed, you can run `make prepare all check`. The available targets are:

- `prepare`: initializes the submodules, and restores the nuget dependency of NUnit
- `all`: compiles everything
- `check`: runs unit tests for _monodoc_ and _mdoc_
- `check-mdoc`: runs only _mdoc_ tests
- `check-monodoc`: runs only _monodoc_ tests

You can also control some parameters from the command line:

If you want to compile in debug mode: `make all CONFIGURATION=Debug`  

### Visual Studio
Once you run `make prepare all` at least once (to restore submodules), you can open the solution in 
_Visual Studio_ to compile and debug.
