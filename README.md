# SIPOS

**SIPOS** is the **Intelligent Service Order Processing System** (*Sistema Inteligente de Processamento de Ordens de Serviço*), a Windows Forms application built to help automate the creation of daily Service Orders (O.S.) for military units.

> Portuguese translation: [`README_PT.md`](README_PT.md)

## Project status

| Item | Status |
| --- | --- |
| Current visible beta milestone | **Beta 1.2.2** |
| Application type | Windows Forms desktop app |
| Target framework | `net6.0-windows` |
| Main language | C# |
| Office integration | Microsoft Word and Excel interop |
| Planning document | [`PLANNER.md`](PLANNER.md) |
| Repository audit | [`REPOSITORY_AUDIT.md`](REPOSITORY_AUDIT.md) |

Beta 1.2.2 is the latest completed milestone currently reflected in the repository planning notes. The project remains under active development, with upcoming work focused on Modelar improvements, holiday detection, message interpretation, configurable scheduling, broader unit adaptability, and future portable Windows `.exe` packaging.

## Purpose

Many daily Service Orders are still prepared manually by unit staff. This process can require collecting information from multiple documents, interpreting service schedules, validating data, and publishing final content after command review.

SIPOS aims to reduce that repetitive workload by helping users:

- interpret Excel-based service schedule data;
- prepare and fill Word-based Service Order documents;
- export Service Order output to Word/PDF workflows;
- manage configuration paths for templates, exports, and inspection/output folders;
- evolve toward a more adaptable tool for multiple units.

## Main capabilities

### Excel schedule processing

SIPOS reads Excel service schedule files and extracts relevant information such as dates, named personnel, states, effective service members, reserves, and adapted service rows.

### Word document generation

The application uses Microsoft Word interop to open template documents, replace placeholders, update generated content, and prepare Service Order documents.

### Export workflow

SIPOS supports Word/PDF-oriented export flows and configurable folders for generated Service Order documents.

### Modelar / template workflow

The project roadmap includes ongoing work around sortable document/template lists, custom ComboBox design, ordering controls, and form layout/menu logic.

### Future message interpretation

The Asana-derived roadmap includes planned work for interpreting message patterns, extracting message details/body, formatting content for destination sections, and detecting the Service Order topic to insert.

## Repository structure

| Path | Purpose |
| --- | --- |
| `SIPOS.sln` | Visual Studio solution file |
| `SIPOS.csproj` | Main Windows Forms project file |
| `Program.cs` | Application entry point |
| `Menu.cs`, `Menu.Designer.cs` | Main application shell/menu UI |
| `Forms/` | WinForms screens such as export, data, help, modelar, and properties |
| `Controls/` | Custom UI controls |
| `EscalasEngine.cs` | Excel schedule interpretation logic |
| `Word_Processor.cs` | Word document processing/export logic |
| `Mediator.cs` | Shared application state and coordination helpers |
| `Properties/Resources.resx` | Embedded/referenced image/font resources |
| `folhas_excel_test/` | Excel sample/test data |
| `modelos_word/` | Word templates, exemplars, and export samples |
| `imgs/` | Image, icon, and design assets |
| `fonts/` | Font assets used by the app |
| `PLANNER.md` | Asana-aligned roadmap and portable app plan |
| `REPOSITORY_AUDIT.md` | Evidence-based repository audit and cleanup notes |

## Requirements

### For development

- .NET SDK 6.x or compatible SDK capable of building `net6.0-windows` projects.
- Windows is recommended for full development and runtime validation.
- On non-Windows hosts, restore/build can be used as a compile check with Windows targeting enabled.

### For runtime

- Windows desktop environment.
- Microsoft Office / Word / Excel installed, because SIPOS currently uses Office interop automation.
- Access to the configured Word templates, Excel files, export folders, and inspection/output paths.

## Build instructions

### Windows

```bash
dotnet restore SIPOS.sln
dotnet build SIPOS.sln
```

### Linux/macOS compile check

SIPOS targets Windows Forms, so non-Windows builds are compile checks only and do not replace Windows runtime testing.

```bash
dotnet restore SIPOS.sln
dotnet build SIPOS.sln -p:EnableWindowsTargeting=true
```

The project file also enables Windows targeting automatically on non-Windows hosts.

## Testing and verification

There is not yet a formal automated unit-test suite in this repository. Current verification is focused on:

- solution restore;
- Windows-targeted build checks;
- manual validation of Excel interpretation;
- manual validation of Word generation/export workflows;
- repository audit checks documented in [`REPOSITORY_AUDIT.md`](REPOSITORY_AUDIT.md).

Recommended verification before release:

1. Run restore/build.
2. Launch the app on Windows.
3. Validate Excel schedule import with representative files.
4. Validate Word template generation.
5. Validate Word/PDF export locations.
6. Confirm Office automation works on the target machine.

## Portable `.exe` plan

A portable Windows distribution is planned but not complete. The current goal is to produce a versioned Windows portable artifact such as:

```text
SIPOS-Beta-<version>-win-x64-portable.zip
```

The portable package should include the executable, required assets/templates, a short portable usage guide, and checksum information. Details are tracked in [`PLANNER.md`](PLANNER.md).

## Roadmap overview

Planned work includes:

- Modelar improvements for template/document ordering and custom UI logic;
- holiday detection support;
- message interpretation and automatic topic detection;
- better user-configurable scheduling/properties;
- improved adaptability for other units;
- warning cleanup and code maintenance;
- portable Windows packaging.

See [`PLANNER.md`](PLANNER.md) for the full roadmap snapshot and Asana-aligned planning notes.

## Repository maintenance notes

The repository contains source code, Word templates, Excel samples, image/icon assets, and design files. Some files may be examples, fixtures, generated outputs, or design sources. Do not remove assets only because they are not referenced by filename; see [`REPOSITORY_AUDIT.md`](REPOSITORY_AUDIT.md) for evidence-based cleanup recommendations.

## License

See [`LICENSE.md`](LICENSE.md).
