# DocStat
DocStat is a tool for reporting on and munging ECMA XML files. Notionally, it picks up where `mdoc` leaves off, performing reporting and rudimentary query-based manipulation of ECMA source files. It is currently a little "raw," as I've been running it from the `Debug` folder, calling `mono ./DocStat.exe ...` and pointing it at my sources with command-line options. (I've prioritized the functionality that I need over the publishing/deployment story, for now.)

## Subcommands
The DocStat subcommands are `internalize`, `remaining`, `obsolete`, and `comparefix`.
- `internalize` looks for `EditorBrowsableState.Never` attribute values and replaces the summary text of the API element with default or custom "Internal only" text. 
-  `remaining` looks for the exact string `To be added.` and outputs a csv file useful for tracking remaining work in a documentation repo.
-  `obsolete` looks for `System.Obsolete` attribute values and replaces the summary text of the API element with default or custom obsolescence text.
- `comparefix` operates on two parallel documentation repositories, making DOM-wise comparisons of every type summary, type remarks section, member summary, member parameter, member type parameter, member remarks section, and member returns section in the `fix` directory against its equivalent in the `using` directory, and updates the former if the latter both exists and differs. It currently depends on matching file structures to locate the types.

## Standard Options
Every subcommand can take the following options
### `d|dir|directory=`
A top level directory that will be recursively searched for documentation, subject to omission by `e|exceptlist=`.
### `e|exceptlist=`
A path that points to a file that contains a list of file paths, one path per line, to ignore during processing. Use this for types that you don't want touched. Order is irrelevant, as these files *will* be removed from the list of files to process.
### `p|processlist=`
A path that points to a file that contains a list of file paths, one path per line, to process. Order is irrelevant. Files in this list that are also specified in the `e|exceptlist=` file will be removed, no matter how hard you press the keys when typing in this option.
### `n|namematches=`
A string that is passed to a `System.Text.RegularExpressions.Regex` instance to use to include files for processing. Order is irrelevant. No matter how well-crafted this regex pattern, it cannot include a file that has been omitted with `e|exceptlist=`.

## Notes on command-specific options
`m|message=` supplies the message to use for commands that replace summary text on the basis of an attribute, namely `obsolete` and `internalize`. Where the `s|sigil=` option is available, it defaults to `To be added.`, which is the default `mdoc` we-promise-the-content-is-coming stanza. `o|output|ofile=` is used by `remaining` to specify the file to which to write the csv data that it generates.

Finally, a perusal of the source code in `apistat.cs` yields a list of subcommands that will be familiar to those who have read this far. The items in this list, in turn, will point the motivated reader to the respective command classes, where further perusal will yield the allowable (implemented or aspirational) options for each command.
