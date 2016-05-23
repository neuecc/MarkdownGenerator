MarkdownWikiGenerator
===
Generate markdown from C# binary & xml document for GitHub Wiki.

Sample: See [UniRx/wiki](https://github.com/neuecc/UniRx/wiki)

How to Use
---
Clone and open solution, build console application.

Command Line Argument
- `[0]` = dll src path
- `[1]` = output directory 

Put .xml on same directory, use document comment for generate.

for example

```
MarkdownWikiGenerator.exe UniRx.dll md
```