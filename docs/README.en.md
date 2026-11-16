# SIPOS

[Português](../README_PT.md)

**SIPOS** stands for **Sistema Inteligente de Processamento de Ordens de Serviço** (Intelligent Service Order Processing System). It is a Windows Forms application that helps automate the preparation of the daily service orders (*Ordens de Serviço*, O.S.) in military units.

## Project status

| Item | Status |
| --- | --- |
| Current visible beta milestone | **Beta 1.3.0** |
| Application type | Windows Forms desktop application |
| Target framework | `net10.0-windows` |
| Main language | C# |
| Office integration | Microsoft Word and Excel interop |
| Planning document | [`PLANNER.md`](../PLANNER.md) |

Beta 1.3.0 is the most recent working milestone reflected in the repository's planning notes. Development is active, with future work on the Modelar module, public-holiday detection, message interpretation, user scheduling/configuration, adaptation to other units, and a portable Windows `.exe` version.

## Purpose

Many service orders are still prepared by hand by soldiers of the unit. The process can involve gathering information from several documents, interpreting duty rosters, validating data and publishing the final text after the commander's review.

SIPOS aims to cut that repetitive work by helping the user to:

- interpret roster data from Excel files;
- prepare and fill in Word service-order documents;
- export results to Word/PDF workflows;
- manage paths for templates, exports and inspection/output folders;
- grow into a tool that adapts to several units.

## Main features

### Excel roster processing

SIPOS reads Excel duty-roster files and extracts the relevant information: dates, named personnel, statuses, headcounts, reserves and adapted rows.

### Word document generation

The application uses Microsoft Word interop to open templates, replace placeholders, refresh generated content and prepare service-order documents.

### Export workflow

SIPOS supports Word/PDF-oriented export workflows and lets you configure the folders for generated documents.

### Modelar / templates workflow

The roadmap includes continued work on sortable lists of documents/templates, a custom ComboBox, sorting controls and menu/layout logic.

### Future message interpretation

The Asana-derived roadmap includes planned work to interpret message patterns, extract message details/body, format content according to its destination and detect the O.S. topic where the information belongs.

## Repository layout

| Path | Purpose |
| --- | --- |
| `SIPOS.sln` | Visual Studio solution file |
| `SIPOS.csproj` | Main Windows Forms project |
| `Program.cs` | Application entry point |
| `Menu.cs`, `Menu.Designer.cs` | Main application UI/menu |
| `Forms/` | WinForms screens such as export, data, help, Modelar and properties |
| `Controls/` | Custom UI controls |
| `EscalasEngine.cs` | Excel roster interpretation logic |
| `Word_Processor.cs` | Word processing/export logic |
| `Mediator.cs` | Shared state and coordination helpers |
| `Properties/Resources.resx` | Image/font resources referenced by the application |
| `folhas_excel_test/` | Sample/test Excel data |
| `modelos_word/` | Word templates, examples and export samples |
| `imgs/` | Images, icons and design assets |
| `fonts/` | Fonts used by the application |
| `PLANNER.md` | Asana-aligned roadmap and portable-app plan |

## Requirements

### For development

- .NET SDK 10.x (or a compatible SDK) able to build `net10.0-windows` projects.
- Windows is recommended for full development and runtime validation.
- On non-Windows systems, restore/build can be used as a compile check with Windows targeting enabled.

### To run

- A Windows desktop environment.
- Microsoft Office / Word / Excel installed, because SIPOS currently relies on Office interop automation.
- Access to the Word templates, Excel files, export folders and configured inspection/output paths.

## Build instructions

### Windows

```bash
dotnet restore SIPOS.sln
dotnet build SIPOS.sln
```

### Compile check on Linux/macOS

SIPOS targets Windows Forms, so builds on non-Windows systems are compile checks only and do not replace real testing on Windows.

```bash
dotnet restore SIPOS.sln
dotnet build SIPOS.sln -p:EnableWindowsTargeting=true
```

The project file also enables Windows targeting automatically on non-Windows hosts.

## Tests and verification

There is no formal automated unit-test suite in this repository yet. Verification currently relies on:

- solution restore;
- build with Windows targeting;
- manual validation of Excel interpretation;
- manual validation of Word generation/export;
- manual review of the changes before each release.

Recommended checks before a release:

1. Run restore/build.
2. Open the application on Windows.
3. Validate Excel roster import with representative files.
4. Validate Word document generation.
5. Validate the Word/PDF export locations.
6. Confirm that Office automation works on the target machine.

## Portable `.exe` plan

A portable Windows distribution is planned but not finished. The goal is a versioned Windows artefact, for example:

```text
SIPOS-Beta-<version>-win-x64-portable.zip
```

The portable package should include the executable, the resources/templates it needs, a short portable-usage guide and checksum information. Details are in [`PLANNER.md`](../PLANNER.md). The first portable base is described in [`README-PORTABLE.md`](../README-PORTABLE.md).

## Roadmap overview

Planned work includes:

- Modelar improvements for sorting documents/templates and custom UI logic;
- public-holiday detection;
- message interpretation and automatic topic detection;
- better user scheduling/configuration;
- greater adaptability to other units;
- warning cleanup and code maintenance;
- portable Windows packaging.

See [`PLANNER.md`](../PLANNER.md) for the full roadmap and the Asana-aligned planning notes.

## Repository maintenance notes

The repository holds source code, Word templates, Excel samples, images/icons and design files. Some files may be examples, fixtures, generated outputs or design sources. Do not remove assets just because no file references them by name.

## Licence

See [`LICENSE.md`](../LICENSE.md).

<p align="center"><a href="https://dlforge.dungenlabs.com"><picture><source media="(prefers-color-scheme: dark)" srcset="branding/dlforge-logo-negative.svg"><source media="(prefers-color-scheme: light)" srcset="branding/dlforge-logo.svg"><img src="branding/dlforge-logo.svg" alt="DL Forge" width="120"></picture></a></p>
