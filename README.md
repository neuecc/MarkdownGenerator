# MarkdownGenerator

Generate markdown from C# binary & xml document for GitHub Wiki.

Sample: See [UniRx/wiki](https://github.com/neuecc/UniRx/wiki)

## Useage

Clone and open solution, build console application.

### Syntax

```
MarkdownGenerator.exe <dll file path> <output dir> [regex pattern]
```

### Parameter

Parameter|Description
---|---
`<dll file path>`|DLL file path to generate markdown file(s)
`<output dir>`|Output dir path of the generated markdown file(s)
`[regex pattern]`|RegEx pattern to select namespaces *(optional)*

### Example

Put .xml on same directory, use document comment for generate.

```cmd
> MarkdownGenerator.exe UniRx.dll docs
```
