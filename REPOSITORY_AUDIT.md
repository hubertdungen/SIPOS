# SIPOS Repository Audit

This audit records evidence-based findings from the repository scan. It intentionally does **not** delete assets, templates, samples, icons, or generated-looking files because several may be useful for later design/runtime/testing work.

## Verification commands run

- `git status --short --branch`
- `git branch --show-current`
- `git remote -v`
- `find .. -name AGENTS.md -print`
- `git ls-files | sort`
- Python SHA-256 scan over `imgs`, `modelos_word`, `folhas_excel_test`, and `fonts`
- `rg -n '(<value>\.\.\\\\(imgs|fonts)|<data name=)' Properties/Resources.resx`
- `rg -n 'imgs[\\/]|modelos_word[\\/]|folhas_excel_test[\\/]|Agency_FB_BC|modelo_|\.xls|\.docx?|\.pdf|\.lnk' -S . -g '!bin' -g '!obj' -g '!*.resx' -g '!*.Designer.cs'`
- `rg -n "TODO|FIXME|SEM USO|APAGAR|unused|not used|não tem uso|por enquanto" -S . -g '!bin' -g '!obj'`
- `dotnet clean SIPOS.sln -p:EnableWindowsTargeting=true`
- `dotnet restore SIPOS.sln`
- `dotnet build SIPOS.sln -p:EnableWindowsTargeting=true --no-incremental`

## Build / troubleshooting status

A clean restore/build was tested with .NET SDK 6.0.428.

Result:

- Clean: succeeded.
- Restore: succeeded.
- Build: succeeded.
- Errors: 0.
- Warnings: 116.

The warnings are mostly existing nullable-reference, unused-field/event, unreachable-code, and WinForms high-DPI analyzer warnings. The build does not currently fail, but warning cleanup should be handled as a separate maintenance task because the warnings span multiple forms and shared mediator/state classes.

## Asset inventory summary

The asset scan covered these folders:

- `imgs`
- `modelos_word`
- `folhas_excel_test`
- `fonts`

Total files scanned: 93.

## Exact duplicate files found by SHA-256

Only two exact duplicate groups were found. These are byte-for-byte duplicates, not just similar filenames.

| Size | Files | Recommendation |
| --- | --- | --- |
| 371,200 bytes | `modelos_word/modelo_de_fim_de_semana.doc`, `modelos_word/modelo_de_semana.doc` | Do not delete yet. Confirm whether the duplicated content is intentional, because the filenames imply different weekday/weekend templates. |
| 380,928 bytes | `modelos_word/exemplares/2022-002-193.doc`, `modelos_word/exemplares/2022-002-192.doc` | Likely duplicate exemplar documents. Safe candidate for review/archive after manual Word comparison. |

## Assets that are referenced by the resource file

The following files are referenced by `Properties/Resources.resx` and should be considered active unless the corresponding generated resource and UI usage are removed intentionally:

- `imgs/bck_imgs/The_Rationalist_a_background_image_inspired_by_gears_workshops_v3.png`
- `imgs/icos/SIPOS_icon_4_lq.png`
- `imgs/bck_imgs/The_Rationalist_a_secret_communication_room_encryption_comms_ce_b7d0a0fb-60cd-4c35-ad01-1a383c6a481d.png`
- `imgs/bck_imgs/The_Rationalist_a_bright_faded_background_image_inspired_on_mil_13f1d255-e353-41a0-afec-ef4a9b34931e.png`
- `imgs/bck_imgs/The_Rationalist_a_faded_background_image_inspired_on_administra_5f95f8bb-7133-4227-834c-ea436c43770a.png`
- `imgs/bck_imgs/The_Rationalist_a_background_image_inspired_by_administrative_j_fad.png`
- `imgs/icos/SIPOS_icon_4.png`
- `imgs/icos/SIPOS_icon_4_lq_v2.png`
- `fonts/Agency_FB_BC.ttf`

## Assets not proven unused

Many image/icon/logo files are not directly referenced by filename in the code/resource scan. That is **not** enough evidence to delete them, because they may be design alternatives, source artwork, future UI assets, or manually selected resources.

Recommended treatment:

- Keep icons and logos unless a designer/developer confirms they are obsolete.
- Consider moving large unused design candidates into an `archive/design-assets/` folder in a separate PR if repository size becomes a problem.
- Consider Git LFS for large binary assets if the repository will continue to store PSDs, generated backgrounds, Office documents, and release artifacts.

## Generated-looking or environment-specific files to review

These files/folders may be generated, sample-only, or machine-specific. They should be reviewed before removal.

| Path | Why it needs review | Recommendation |
| --- | --- | --- |
| `modelos_word/exports/` | Contains exported `.doc`/`.pdf` outputs. Export code generates Word/PDF output names dynamically. | Treat as sample output or generated artifacts. Consider moving to samples or ignoring future exports after confirming runtime expectations. |
| `modelos_word/Microsoft Edge.lnk` | Windows shortcut files are commonly machine-specific. | Confirm whether the app needs this shortcut; otherwise archive/remove. |
| `modelos_word/Word.lnk` | Windows shortcut files are commonly machine-specific. | Confirm whether this is needed for launching Word on target machines; otherwise archive/remove. |
| `folhas_excel_test/old/` | Old test Excel files may be fixtures or backups. | Keep until current Excel parser tests are documented. |
| `folhas_excel_test/previsaoOAF_ORIGINAL.xls` | Looks like a baseline/original test workbook. | Keep as a fixture unless replaced by a formal test-data folder. |
| `imgs/logo/SIPOS_Logo.psd` | Large source design file. | Keep if design source is wanted; consider Git LFS. |

## Code findings / improvement candidates

### Strong candidate: remove or isolate unused Word page detector

`Word_Processor.cs` contains a method with an explicit comment saying it is unused for now and should be deleted after confirming it has no use:

- `detectLastPageNumber(string[] args)`

Recommendation: remove this method in a focused cleanup PR after a final Windows runtime check confirms it is not used by debugging/manual workflows.

### Strong candidate: validate configured Word template paths

`Forms/FormPropriedades.cs` contains a developer note warning that Word model paths need validation and should only accept Word `.docx` format.

Recommendation: implement validation for Word model/path inputs before saving settings, and decide whether `.doc` templates are still allowed because current repository templates include `.doc` files.

### Warning cleanup backlog

The no-incremental build reports 116 warnings. Suggested cleanup order:

1. Non-nullable fields/properties that are genuinely required.
2. Possible null dereferences around form/mediator state.
3. Unused fields/events and unreachable code.
4. WinForms high-DPI manifest warning.
5. Event-handler nullability signature mismatches.

## Safe next steps

1. Do not delete icons, logos, template docs, or Excel samples yet.
2. Create a focused cleanup branch for exact duplicate documents after manual confirmation.
3. Add a formal `samples/` or `testdata/` folder structure if Excel/Word files are retained as fixtures.
4. Add `.gitignore` rules only after confirming which output folders are generated at runtime.
5. Create a warning-reduction task in the planner/Asana, starting with high-confidence unused code and nullability fixes.
