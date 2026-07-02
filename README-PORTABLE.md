# SIPOS Portable Windows Build

This build profile is the first portable distribution path for SIPOS on Windows x64.

## Target

- Windows x64.
- Self-contained .NET desktop runtime.
- Single-file `SIPOS.exe` inside a publish folder that can be zipped.
- Microsoft Office desktop apps remain required for Excel and Word interop.

## Publish

From the repository root:

```powershell
dotnet publish SIPOS.csproj /p:PublishProfile=win-x64-portable
```

The publish output is:

```text
bin\Release\net6.0-windows\win-x64\publish\portable\
```

## Portable Folder Rules

- Extract the folder into a user-writable location.
- Do not run the app directly from the zip.
- Avoid protected folders such as `C:\Program Files` unless the app is installed with the correct permissions.
- Keep the generated `settings.txt` beside `SIPOS.exe`; this is intentional for portable use.

## Smoke Test Checklist

- Launch `SIPOS.exe` from an extracted folder path that contains spaces.
- Save preferences and confirm `settings.txt` is created beside the executable.
- Close and reopen SIPOS, then confirm preferences load.
- Import an Excel escala file.
- Export a Word O.S. document.
- Confirm no Office permission prompts or COM errors block the flow.
