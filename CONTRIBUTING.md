# How to contribute

Your feedback and contributions are greatly appreciated, and improve the quality of mdoc, monodoc, and related tools. Please read the following contribution guidelines to ensure a quick and timely review and acceptance of your patches. As an overview, the following are the contribution guidelines, and any deviations from the norm should be discussed in a github issue.

- All code must be written in **C#**
- We are currently targeting **.NET Framework 4.5** ... we will be transitioning to .NET Standard 2 in the future, and supporting other targets such as dotnet core.
- All projects and solutions created must load and compile in **Visual Studio 2017** and **Visual Studio for Mac**.
- New dependencies (such as nuget references) must be _discussed and reviewed_. _mdoc_ is distributed via the Mono project, and as such we must be careful with what dependencies we distribute. 
- **NUnit** is used to write unit tests.
- Integration tests use **Make** on **Bash**. 
- This project uses **Git** hosted on **GitHub** as the central repository host.
- All pull requests should pass all unit tests successfully (which means being able to run the CLI build via `make`).

## Bug Reports and Feature Requests

We love bug reports and feature requests! It is through feedback from our wonderful customers that we know how to prioritize our time. Please file issues and requests on [github issues](https://github.com/mono/api-doc-tools/issues/new). 

For problems, please try to include a reproducible example … ideally in the form of an assembly (or compilable source code), and related script to reproduce the issue.

For feature requests, please include all relevant details including input, output, and ideal file formats.

## Contribution Workflow

When contributing bug fixes and enhancements, please follow this workflow and guidelines.

### Coding Standards

This project is currently in transition. Historically, it has used the [Mono coding guidelines](http://www.mono-project.com/community/contributing/coding-guidelines/); and indeed, most of the code is currently still using this. 

However, starting with the `MDocUpdater` class and related entities, we are changing to a new coding standard, based on the default formatting options available in _Visual Studio_ and _Visual Studio for Mac_. This is meant to simplify contributions, so that the IDE does not work against you without changes to the default format settings.

Additionally, many areas of the code contain multiple classes in the same file. While there’s nothing wrong in general with this approach, going forward we will maintain each class in a separate `.cs` file, and organize functionality into better namespaces.

If you are working in an area of the code that has transitioned to the new coding standard, please use that. If you are working with code that is still using the mono coding guidelines, and the change is small, just stick the existing format … otherwise, file a separate GitHub issue to request that the target code be reformatted. I would like to keep those formatting changes in a standalone commit (ie. no feature/functionality changes).

### Commits

This project tries to avoid chatty in-progress commits that are common during development. Once you’ve completed your development and are ready to move to the next stage of the contribution workflow, collapse your changes into as few feature-scoped commits as possible. Ideally if possible, every commit should pass all unit tests and be standalone.

You can do so either by using an interactive rebase of your branch if you need multiple commits, or simply doing a soft reset `git reset origin/main —soft` … which will stage all of your changes and let you create a new single commit that contains all changes.

The commit message should be verbose enough to explain what behavior is changing, bug is being fixed, or feature being added. It should also contain a reference to the github issue being resolved, as [described here](https://github.com/blog/1386-closing-issues-via-commit-messages).

### Tests

Your commit should introduce no regressions. While we do not require 100% code coverage, every newly-introduced feature should have a new unit or integration test written. You can make sure to run all unit tests locally by using the [instructions here](https://github.com/mono/api-doc-tools#cli). There are two kinds of tests:

#### Unit Tests

Written with _NUnit_, there are two unit test projects: *mdoc.Test*, and *Monodoc.Test*. _Visual Studio for Mac_ has built-in support for _NUnit_, and on Windows, you can install the [_NUnit 3 Test Adapter_](https://marketplace.visualstudio.com/items?itemName=NUnitDevelopers.NUnit3TestAdapter)

#### Integration Tests

_mdoc_ has integration tests implemented as [`make` targets](mdoc/Makefile). You can look at the existing tests to see how they are structured, but generally speaking:

- It usually involves a sample code file, such as [/mdoc/Test/DocTest-InternalInterface.cs](mdoc/Test/DocTest-InternalInterface.cs).
- A target (such as `Test/DocTest-InternalInterface.dll`) to build/comple the test assembly.
- A target to execute the scenario (such as `check-monodocer-import-fx`), which itself depends on the above target.
  - Usually, this setup involves deleting the `Test/en.actual` folder (which serves as the output)
  - the execution of an `mdoc` subcommand, using `$(MONO) $(PROGRAM) <the subcommand> <args>`.
  - The assertion, using `$(DIFF)`, against an existing expected form of the output (which is committed to git, such as `Test/en.expected-fx-import`).
  
 Depending on the change that you're making, it might cause the output to change (for example, if changing a formatter to reflect a new language feature); in those cases, when you make that change and run the integration tests, you will get a number of failures ... you will have to update the test xml files to reflect the new file changes.

### Pull Request

Create a fork against the [api-doc-tools](https://github.com/mono/api-doc-tools/pulls) repository. As mentioned previously, all tests should pass, and the code will be reviewed, discussed, and merged from there. *Please be ready to address any issues* brought up during code review.

## Legal

This software is licensed under [The MIT License](LICENSE.md).
