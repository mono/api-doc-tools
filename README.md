# `mdoc`

This repository contains the source for the .NET API Reference toolchain ([used
 by docs.microsoft.com](https://docs.microsoft.com/en-us/teamblog/announcing-unified-dotnet-experience-on-docs#built-with-open-source-in-mind)) 

## Contribution Guide

You can read the [contribution guide](CONTRIBUTING.md) for information about coding standards, and (perhaps most importantly), information about writing [unit and integration tests](CONTRIBUTING.md#tests).

## Compiling

### Dependencies

This repository uses submodules, so firstly, you have several options:

- You can include all submodules when you initially clone: `git clone --recursive https://github.com/mono/api-doc-tools.git`
- If you already have it local, you can use `git submodule update --init --recursive`
- If you have all CLI dependencies (see below), you can `make prepare`

### Visual Studio

On windows, you can build and compile [`apidoctools.sln`](apidoctools.sln). And you can run unit tests if you have NUnit installed. If you use [Visual Studio for Mac](https://www.visualstudio.com/vs/visual-studio-mac/), you can use its built-in support for NUnit tests to also run tests.

### CLI
If you've got `make` and `mono` installed, you can run `make prepare all check` in a _bash_ prompt to do a release build and run the full test suite (which for mdoc includes more than just the nunit tests). The available targets are:

- `prepare`: initializes the submodules, and restores the nuget dependency of NUnit
- `all`: compiles everything
- `check`: runs unit tests for _monodoc_ and _mdoc_
- `check-mdoc`: runs only _mdoc_ tests
- `check-monodoc`: runs only _monodoc_ tests

You can also control some parameters from the command line:

If you want to compile in debug mode: `make all CONFIGURATION=Debug`  

If you are using WSL, use: `make all ENVIRONMENT=wsl`

Feel free to open a pull request early in order to make it easy to ask questions during development, and for the maintainers to offer feedback.

## Troubleshooting

### Forcing Changes

When you make certain kinds of changes, it can confuse _Make_'s dependency ... if you're running the test suite and it doesn't seem to be reflecting your changes, add the `-B` parameter to the `make` invocation, and it will force a rebuild of all targets.

### WSL

Make sure you are at least on 16.04 (Xenial). You can check which version is currently installed by running the command `lsb_release -a`.

If you are running an older version (for example, if you originally installed WSL before the creator's update) and need to upgrade, you can run the following commands:

- `sudo do-release-upgrade` ... this updates the version of Ubuntu. _Please note_, this will likely disable any external package sources, such as the one added when installing Mono.
- Once you're done with that, then you just need to update `apt-get`
    - Add back Mono's package repository source, as described in [the download instructions](https://www.mono-project.com/download/stable/#download-lin).
    - `sudo apt-get update`
    - `sudo apt-get upgrade`
    - `sudo apt-get dist-upgrade`

With that run ... you should be able to run the build locally using `make all ENVIRONMENT=wsl`. 

### Linux

The following script will prepare, clone, and run a full build with tests of `mdoc` on an unbuntu docker container. 

```
apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF  
echo "deb http://download.mono-project.com/repo/ubuntu trusty main" | tee /etc/apt/sources.list.d/mono-official.list

apt-get update
apt-get install tzdata nuget -y --allow-unauthenticated
apt-get install git make mono-devel mono-vbnc ca-certificates-mono wget fsharp -y

wget -q 'http://mxr.mozilla.org/seamonkey/source/security/nss/lib/ckfw/builtins/certdata.txt?raw=1' -O "/tmp/certdata.txt" 
mozroots --import --ask-remove --file /tmp/certdata.txt


git clone https://github.com/mono/api-doc-tools
cd api-doc-tools
make prepare all check
```

Please review [mono's installation guide](http://www.mono-project.com/download/#download-lin) if you are using a different flavor of linux.
