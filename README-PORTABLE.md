# SIPOS Portable Windows Build

This build profile is the first portable distribution path for SIPOS on Windows x64.

## Target

- Windows x64.
- Self-contained .NET desktop runtime.
- Single-file `SIPOS.exe` inside a publish folder that can be zipped.
- Microsoft Office desktop apps remain required for Excel and Word interop.

## Easiest Build

Double-click this file from the repository folder:

```text
Build-Portable.bat
```

The batch file:

- asks for a version label, defaulting to `Beta-1.3.0`;
- runs the portable publish profile;
- creates the portable zip in `artifacts`;
- creates a `.sha256` checksum beside the zip;
- pauses at the end so success or error messages stay visible.

The build computer must have the .NET SDK installed. The generated portable app is self-contained, so the target computer does not need a separate .NET runtime installed.

## Advanced Publish Command

From the repository root:

```powershell
powershell -ExecutionPolicy Bypass -File .\scripts\Publish-Portable.ps1 -Version Beta-1.3.0
```

The underlying publish output is:

```text
bin\Release\net10.0-windows\win-x64\publish\portable\
```

The final distributable zip is written to:

```text
artifacts\SIPOS-Beta-1.3.0-win-x64-portable.zip
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
