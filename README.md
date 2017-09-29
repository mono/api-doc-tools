# `mdoc` and `monodoc`

This repository contains the source for Mono's [documentation toolchain](http://www.mono-project.com/docs/tools+libraries/tools/monodoc/generating-documentation/).

## Compiling

### Dependencies

This repository uses submodules, so firstly, you have several options:

- You can include all submodules when you initially clone: `git clone --recursive https://github.com/mono/api-doc-tools.git`
- If you already have it local, you can use `git submodule update --init --recursive`
- If you have all CLI dependencies (see below), you can `make prepare`

### Visual Studio

On windows, you can build and compile [`apidoctools.sln`](apidoctools.sln). And you can run unit tests if you have NUnit installed. If you use [Visual Studio for Mac](https://www.visualstudio.com/vs/visual-studio-mac/), you can use its built-in support for NUnit tests to also run tests.

### CLI
If you've got `make` and `mono` installed, you can run `make prepare all check` to do a release build and run the full test suite (which for mdoc includes more than just the nunit tests). The available targets are:

- `prepare`: initializes the submodules, and restores the nuget dependency of NUnit
- `all`: compiles everything
- `check`: runs unit tests for _monodoc_ and _mdoc_
- `check-mdoc`: runs only _mdoc_ tests
- `check-monodoc`: runs only _monodoc_ tests

You can also control some parameters from the command line:

If you want to compile in debug mode: `make all CONFIGURATION=Debug`  

## Troubleshooting

### WSL

On the windows subsystem for linux, there is unfortunately a defect that causes difficulty making https calls, which in turn doesn't allow the `prepare` make target to complete (nuget fails). You will see an error that says:

```
Unable to load the service index for source https://api.nuget.org/v3/index.json.
  An error occurred while sending the request
  Error: ConnectFailure (Value does not fall within the expected range.)
  Value does not fall within the expected range.
```

[this mono issue](https://github.com/mono/mono/pull/5003) Seems to indicate that the issue is resolved in newer insider builds of WSL.

### Linux

The following script will prepare, clone, and run a full build with tests of `mdoc` on an unbuntu docker container. 

```
apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF  
echo "deb http://download.mono-project.com/repo/ubuntu trusty main" | tee /etc/apt/sources.list.d/mono-official.list

apt-get update
apt-get install git make mono-devel ca-certificates-mono wget nuget -s

wget -q 'http://mxr.mozilla.org/seamonkey/source/security/nss/lib/ckfw/builtins/certdata.txt?raw=1' -O "/tmp/certdata.txt" 
mozroots --import --ask-remove --file /tmp/certdata.txt


git clone https://github.com/mono/api-doc-tools
cd api-doc-tools
make prepare all check
```

Please review [mono's installation guide](http://www.mono-project.com/download/#download-lin) if you are using a different flavor of linux.
