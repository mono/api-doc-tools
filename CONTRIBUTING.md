# How to contribute

Your feedback and contributions are greatly appreciated, and improve the quality of mdoc, monodoc, and related tools. Please read the following contribution guidelines to ensure a quick and timely review and acceptance of your patches.

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

You can do so either by using an interactive rebase of your branch if you need multiple commits, or simply doing a soft reset `git reset origin/master —soft` … which will stage all of your changes and let you create a new single commit that contains all changes.

### Tests

Your commit should introduce no regressions. You can make sure to run all unit tests locally by using the [instructions here](https://github.com/mono/api-doc-tools#cli).

### Pull Request

Create a fork against the [api-doc-tools](https://github.com/mono/api-doc-tools/pulls) repository. As mentioned previously, all tests should pass, and the code will be reviewed, discussed, and merged from there. Please be ready to address any issues brought up during code review.

## Legal

This software is licensed under [The MIT License](LICENSE.md).