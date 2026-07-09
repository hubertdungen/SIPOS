# SIPOS Planning Board

This document mirrors the live Asana planning state for SIPOS and keeps a repository-side planning copy for implementation work. Asana remains the source of truth; this file records the GitHub-side snapshot and the repo changes tied to each planning item.

## Asana snapshot verified on 2026-07-03

| Field | Value |
| --- | --- |
| Project/task | SIPOS - Sistema Inteligente de Processamento de Ordens de Serviço |
| Assignee | Hubert |
| Priority | High |
| Tags | Força Aérea, C# |
| Boards/projects | Development, Força Aérea, Central Board |
| Status in Força Aérea board | In progress |
| Main task subtasks visible | 17 / 27 complete |
| Description visible in Asana | Automatização de Ordens de Serviço na UAL e no espectro da Força Aérea |

## Release status interpreted from Asana

The live Asana task shows the main SIPOS work as active with 17 of 27 subtasks complete. The most recent completed development items remain under the `Modelar: Drag&Drop Improvements and Modulation Mechanics [v B-1.2.0]` epic, plus the repository repair task completed on 2026-07-02:

- `Modelar: Implement "Programar" and "Ficheiros" menus [v B-1.2.1]` - complete.
- `Modelar: Fix issue related to menu styling [v B-1.2.2]` - complete.
- `Repo: Reparar branch default do GitHub sem perder a ultima versao` - complete.

Therefore, repo/docs should describe Beta 1.2.2 as the latest completed code milestone reflected by Asana, while planning continues with pending B-1.2.x work, B-1.3.x holiday detection, and the new B-1.4 portable Windows release path.

Update 2026-07-09: repository work has since advanced past that snapshot — the portable release path (B-1.4) was fixed and shipped as a portable artifact, and the B-1.3.1 holiday-detection engine was implemented and unit-verified. The in-app version string is now `v B-1.3.0`. See the dedicated sections below.

## Main SIPOS roadmap from Asana

### Completed items visible

- [x] `ES (Escalas de Serviço): Detectar e Interpretar células [v A-0.1.0]`
- [x] `ES: Interpretar datas inseridas [v A-0.2.0]`
- [x] `ES: Parsing de texto das células [v A-0.3.0]`
- [x] `ES: Interpretar datas inseridas [v A-0.4.0]`
- [x] `ES: Separar linhas e células e carregar variáveis [v A-0.5.0]`
- [x] `ES: Interpretar várias escalas [v A-0.6.0]`
- [x] `UI: Minor Updates [v A-0.6.5]`
- [x] `Bugs Fix [v A-0.6.11]`
- [x] `Word: Preparar documento inteligente [v A-0.7.0]`
- [x] `Export: Converter dados C# em Word [v A-0.8.0]`
- [x] `UI: Major Updates [v A-0.9.0]`
- [x] `UI & Design: Updates [v A-0.9.10]`
- [x] `Word Processor & More UI/UX Features [v A-0.10.0]`
- [x] `Modelar: Drag&Drop Sortable Document List [v A-0.11.0]`
- [x] `ES: Interpretação, Detecção e Inputs de datas diferentes [v B-1.0.0]`
- [x] `Interpretação e FormDados: Excel & Data improvements [v B-1.1.0]`
- [x] `Repo: Reparar branch default do GitHub sem perder a ultima versao`

### Current / upcoming items visible

- [ ] `Modelar: Drag&Drop Improvements and Modulation Mechanics [v B-1.2.0]`
- [ ] `Detectar: Feriados [v B-1.3.0]`
- [ ] `Release: Criar versão portátil / .exe para Windows [v B-1.4.0]`
- [ ] `Mensagens: Interpretar mensagens (Detetar Padrões) [v 2.0.0]`
- [ ] `Mensagens: Capturar os detalhes das mensagens`
- [ ] `Mensagens: Capturar o corpo da mensagem`
- [ ] `Mensagens: Formatar o texto conforme destino`
- [ ] `Mensagens: Detetar o tópico a inserir na O.S.`
- [ ] `Propriedades: Função para o utilizador programar o software`
- [ ] `SIPOS: Mais adaptável a outras unidades`

## B-1.2.0 Modelar epic details

### Completed

- [x] `Modelar: Implement "Programar" and "Ficheiros" menus [v B-1.2.1]`
- [x] `Modelar: Fix issue related to menu styling [v B-1.2.2]`

### Pending

- [ ] `Modelar: Template ComboBox Custom Design [v B-1.2.3]`
- [ ] `Modelar: Arrows to switch order [v B-1.2.4]`
- [x] `Modelar: Prevent empty or similar names from moving [v B-1.2.5]` — implemented 2026-07-09 (shipped in Beta-1.3.1; Windows UI validation pending).
- [ ] `Modelar: Template ComboBox Logic [v B-1.2.6]`
- [ ] `Modelar: Form Layout Update & Menu Logic [v B-1.2.7]`

B-1.2.5 implementation notes: `FormModelar.elli_MouseDown` now refuses to start a drag when the row's `txtNameWBox` is empty or matches another row's name (case- and whitespace-insensitive). The blocked row's name box flashes red with a tooltip explaining the reason, and the drag state never engages, so `MouseMove`/`MouseUp` ignore the gesture. Needs visual confirmation on Windows.

## B-1.1.0 Interpretação e FormDados epic details

The live Asana task shows this epic as complete with 4 / 4 subtasks complete:

- [x] `Interpretar: Actualizar range da folha Excel e interpretação de coordenadas dinâmicas de dados [v B-1.1.0]`
- [x] `Interpretar: Fixed an issue on "Range Finding" the escala dates (Formulas or Values) [v B-1.1.1]`
- [x] `FormDados: Fixed an issue where the preview text wasn't clearing the old text [v B-1.1.2]`
- [x] `FormDados: Fixed an issue where the progress bar percentage was jumping above 100%, thus throwing a fatal error [v B-1.1.3]`

## B-1.3.0 Detectar: Feriados details

- [x] `Detectar: Detection Engine updates (starts detecting holidays) [v B-1.3.1]` — **motor implementado** em `Feriados.cs` (2026-07-09).
- [ ] `Calendário: Adicionar checkBox para ativar Start e End Date. [v B-1.3.2]`

### B-1.3.1 detection engine — implementado 2026-07-09

Nova classe autónoma `Feriados.cs` (sem dependências de WinForms/Office, por isso testável isoladamente):

- `DomingoDePascoa(ano)` — computus gregoriano (Meeus/Jones/Butcher).
- `DoAno(ano, incluirFacultativos)` — lista ordenada dos feriados nacionais fixos (Ano Novo, Dia da Liberdade, Dia do Trabalhador, Dia de Portugal, Assunção, Implantação da República, Todos os Santos, Restauração da Independência, Imaculada Conceição, Natal) e móveis (Sexta-feira Santa, Páscoa, Corpo de Deus), com o Carnaval como facultativo opcional.
- `IsFeriado(data)`, `NomeFeriado(data)`, `IsDiaDeDescanso(data)` (fim-de-semana ou feriado).
- Atalhos expostos em `Mediator`: `isDiaFeriado`, `nomeDoFeriado`, `isDiaDeDescanso`, prontos para a integração de UI de B-1.3.2 e para o cálculo de dias de interrupção, sem alterar os fluxos de escala já validados.

Verificação: teste isolado em .NET 10 confirmou as datas de Páscoa de 2000/2023/2024/2025/2026/2027 contra valores conhecidos e a deteção correta de feriados fixos, móveis e dias úteis. Todos os testes passaram. A integração real no cálculo de `plusDayIntrup`/calendário continua pendente de validação em Windows.

### Bug conhecido registado

`Mediator.returnEscalaDate(plusDay)` ignorava o parâmetro `plusDay`: `diaDeEscala.AddDays(plusDay)` descartava o resultado (DateTime é imutável) e tinha uma linha inalcançável a seguir ao `return`. O código morto foi removido e o comportamento atual (devolver a data sem deslocamento) foi preservado, porque aplicar realmente o `plusDay` afeta a filtragem de escalados por data e precisa de validação com dados reais em Windows antes de ser mudado.

## B-1.4.0 Portable Windows release

Asana task: `Release: Criar versão portátil / .exe para Windows [v B-1.4.0]`  
Asana link: https://app.asana.com/1/193050978126127/project/193050978126132/task/1216249466525311

### Phase 1 - Feasibility

- [x] Confirm first supported architecture target: `win-x64`.
- [x] Confirm Microsoft Office remains a prerequisite for Word/Excel interop.
- [x] Start with a self-contained single-file publish so the .NET runtime does not need to be installed separately.
- [ ] Inventory and validate every runtime asset on a clean Windows machine with Office installed.

### Phase 2 - Publish profile / command

- [x] Add a Windows publish profile for `net6.0-windows` and `win-x64`.
- [x] Add `scripts/Publish-Portable.ps1` to create a portable zip and checksum.
- [x] Add `Build-Portable.bat` as the one-click Windows build entrypoint.
- [x] Test `dotnet publish` with `PublishSingleFile=true`.
- [x] Test self-contained publish with `SelfContained=true`.

### Phase 3 - Portable artifact

- [x] Produce local `SIPOS-Beta-1.2.2-win-x64-portable.zip`.
- [x] Include `SIPOS.exe`, debug symbols, and `README-PORTABLE.md` in the portable output.
- [x] Generate a SHA-256 checksum for the zip artifact.
- [x] Fix portable `settings.txt` location: it is now anchored to the executable folder (`AppContext.BaseDirectory`) instead of the process current directory, and the file picker restores the current directory after browsing. Before this fix, browsing for an Excel file could silently move where preferences were saved/loaded.
- [ ] Smoke-test launch from an extracted folder path with spaces.
- [ ] Smoke-test Excel import and Word export on Windows.

Rebuilt artifact on 2026-07-08 (with the settings.txt portability fix) via cross-compilation on Linux with .NET SDK 10.0.109:

- Artifact: `SIPOS-Beta-1.2.2-win-x64-portable.zip`
- SHA-256: `86EFA952D77D2BA40107E85562DEDA881177B26FCBC4620849923E473A0B033E`

Superseded later on 2026-07-08 by the Beta-1.2.2-r2 maintenance rebuild described below.

## Maintenance r2 (2026-07-08): code cleanup and .NET 10 upgrade

Maintenance pass requested after PR #4 merged, before resuming B-1.2.x feature work:

- Removed dead code flagged by compiler warnings: unused `applyModeloEscala` and `detectLastPageNumber` methods in `Word_Processor.cs` (both contained hardcoded `C:\` paths and were never called), plus never-read private fields in `Mediator.cs`, `EscalasEngine.cs`, `Word_Processor.cs`, and `Forms/FormModelar.cs`.
- Fixed a duplicated `StreamReader` construction in `Mediator.readMemoryFile` that leaked the first reader.
- Moved DPI awareness out of `app.manifest` into the project property `ApplicationHighDpiMode=SystemAware` (same effective mode as before), clearing warning WFAC010.
- Upgraded the target framework from `net6.0-windows` (out of support, warning NETSDK1138) to `net10.0-windows` (LTS, supported until Nov 2028). The portable build stays self-contained, so end users still do not need any .NET runtime installed.
- Added the `DesignerSerializationVisibility` attributes required by the .NET 10 WinForms analyzer (WFO1000) to `Controls/CustomComboBox.cs` custom properties.
- Enabled `EnableCompressionInSingleFile`, shrinking `SIPOS.exe` from ~165 MB to ~85 MB on disk.
- Build warnings dropped from ~144 to 122; the remainder are pre-existing nullable-reference warnings (CS86xx) left for a dedicated pass.
- The in-app version string stays `v B-1.2.2` (no behavior changes), so existing `settings.txt` files keep loading without a version-mismatch prompt. The artifact label is `Beta-1.2.2-r2`.
- `settings.txt` format is unchanged, including the historical duplicated `fPathOSWord` line, to avoid breaking existing files; consolidating that format needs a version bump and belongs to a future change.
- Windows smoke tests (launch, Excel import, Word export with Office installed) remain pending for this rebuild, same as Phase 3.

Artifact rebuilt from this maintenance state:

- Artifact: `SIPOS-Beta-1.2.2-r2-win-x64-portable.zip`
- SHA-256: `C0C6170FD699FF0CA1819EBA91E5CAD238687445531541EA3FB99792A29029AB`
- Superseded by the Beta-1.3.0 artifact below once the holiday-detection engine landed.

## Release Beta-1.3.0 (2026-07-09): holiday detection engine

This is the first release carrying real feature work on top of the r2 maintenance base:

- Added the `Feriados.cs` national-holiday detection engine (B-1.3.1, see above).
- In-app version string bumped to `v B-1.3.0`. Existing `settings.txt` files from `v B-1.2.2` now trigger the built-in version-mismatch prompt, which lets the user keep or recreate their preferences — expected behavior for a feature release.
- Salvaged `README_PT.md` (Portuguese README) from the obsolete PR #3 branch, updated for the .NET 10 target and the current milestone, before closing that PR.
- Windows smoke tests remain pending, same as before.

Artifact:

- Artifact: `SIPOS-Beta-1.3.0-win-x64-portable.zip`
- SHA-256: `CF04D65C533D15790C1F288F2966AB64D774743FCA4EDBC0280F5AD1671C4E48`
- Published in-repo under `dist/` on the `claude/sipos-portability-release-m6x9tq` branch (GitHub Releases upload still pending the Phase 4 channel decision).

## Release Beta-1.3.1 (2026-07-09): resilience hardening + B-1.2.5

Troubleshooting/resilience pass over the runtime-critical paths, plus one Modelar feature:

- `Word_Processor.CreateWordDocument`: missing Word template now aborts cleanly before starting Word (previously it showed the error but continued into a null-document `SaveAs2` crash and leaked a WINWORD.EXE process). The whole export is now wrapped in try/catch/finally so the document and Word app always close, and the success message only shows when the export actually succeeded.
- `Word_Processor.GetLastPageNumber`: wrapped in try/finally with guarded close/quit/release — a failed read no longer leaves an orphaned Word process.
- `EscalasEngine.checkRows` finally block: each COM cleanup step (release worksheet, close/release workbook, quit/release Excel) is individually guarded so one COM failure cannot skip the rest and leak EXCEL.EXE.
- `Mediator.readMemoryFile`: resilient parsing — missing or corrupted lines in `settings.txt` fall back to safe defaults instead of crashing startup with FormatException; reader wrapped in `using`. File format unchanged.
- `Mediator.saveMemory`: writer wrapped in `using` so a failed write cannot keep `settings.txt` locked.
- `Mediator.GetNextOSNumber`: unset/missing export folder now returns 1 instead of throwing.
- `Mediator.GetPreviousOSFileName`: non-numeric/empty `osNumber` now returns null instead of throwing.
- B-1.2.5 (see the Modelar epic section above): drag of rows with empty or duplicate document names is blocked with visual feedback.
- Build warnings: 121 → 104 (remaining are pre-existing nullability warnings).
- In-app version bumped to `v B-1.3.1`.

Artifact:

- Artifact: `SIPOS-Beta-1.3.1-win-x64-portable.zip`
- SHA-256: `134EBC0D96475F88049EE8F26D80B6FA04FEF40592F3651AA1C476AC7484805B`
- Published in-repo under `dist/` (replaces the Beta-1.3.0 zip; Phase 4 channel decision still pending).

## Branch cleanup (2026-07-09)

The GitHub default branch is now `main` (the earlier `SIPOS_v0-8-3` repair is complete). Branch inventory reconciled against `main`:

- Fully merged into `main` (0 commits ahead), safe to delete: `SIPOS_v0-8-3`, `SIPOS_v0-9-4`, `codex/start-portable-compatibility`, `codex/verify-.net-installation-and-build-sipos`.
- PR #3 (`codex/verify-.net-installation-and-build-sipos-0vhcy4`): obsolete — its single commit branched from a pre-portable state and would delete the portable build infrastructure; its Office interop casts are already in `main`. Its only genuinely new content (`README_PT.md`) was salvaged; the PR should be closed.
- `backup/SIPOS_v0-8-3-before-2026-07-02`: deliberate pre-repair history backup (old default commit `4533fdd`); **decision confirmed 2026-07-09: keep this backup branch permanently** as a safety net.

Note: the remote session git proxy only accepts pushes to the designated working branch, and no branch-deletion tool is exposed, so the actual deletion of the merged branches must be done by the maintainer (GitHub UI → Branches, or `git push origin --delete <branch>` from a normal clone). Enabling "Automatically delete head branches" in the repo settings will keep this tidy going forward.

### Phase 4 - Release / upload

- [ ] Decide the authoritative upload location: GitHub Releases, Asana attachment, shared drive, or another channel.
- [ ] Upload the portable artifact.
- [ ] Link the artifact in Asana and release notes.
- [ ] Mark the release task complete only after Windows runtime smoke testing passes.

## Repository branch model

Recommended branch model as of 2026-07-03:

- `main` is the active/recommended branch for the latest useful SIPOS code.
- `SIPOS_v0-8-3` remains as a legacy branch name and still appears as the GitHub default branch setting because the available connector can create/move branches but does not expose repository-settings updates.
- `main` and `SIPOS_v0-8-3` should be kept aligned until the GitHub default branch setting can be changed to `main`.
- `backup/SIPOS_v0-8-3-before-2026-07-02` preserves the old default branch state from before the repair.
- Future version snapshots should use release-style branch names such as `release/beta-1.2.2`; feature work should use names such as `feature/<name>` or `codex/<name>`.

## Repository branch repair notes

GitHub repair completed on 2026-07-02 and reflected in Asana task `Repo: Reparar branch default do GitHub sem perder a ultima versao`:

- The repository default branch name is still `SIPOS_v0-8-3`, but the branch content now points to the latest known SIPOS implementation from `SIPOS_v0-9-4`.
- Old default branch commit preserved at `backup/SIPOS_v0-8-3-before-2026-07-02`.
- Old default commit before repair: `4533fdd336c7348fa5d84a9f7ec0116e653ec3f4`.
- Latest branch commit used for repair: `9eb971f97353f00d4760df39ca4e05dcdc9d29f7`.
- PR #2 merged portable compatibility into `SIPOS_v0-8-3` at merge commit `c0ba9f0fcf9ce12880b2c63ceb7d9bc3a4d3d722`.
- GitHub reported no common ancestor between the old default and latest branch, so a normal merge was not safe. The repair used a backup branch plus a forced default-ref move to avoid losing the latest working code.

## Repository verification notes

- `dotnet restore` succeeds locally with .NET SDK 10.0.103 and Windows desktop targeting available.
- `dotnet build` succeeds locally for `net6.0-windows`.
- `dotnet publish /p:PublishProfile=win-x64-portable` succeeds locally.
- `scripts/Publish-Portable.ps1 -Version Beta-1.2.2` succeeds locally and creates `artifacts/SIPOS-Beta-1.2.2-win-x64-portable.zip` plus a `.sha256` checksum.
- `Build-Portable.bat` succeeds locally with the default version label and creates the same portable zip/checksum through the one-click path.
- Local artifact checksum: `455ABE811E8E4D4668B76AB3213C0360974F62D6340463B9D207AE090DBD467C`.
- Build still emits existing warnings, including `NETSDK1138` because `net6.0-windows` is out of support. That should be planned separately from this portable compatibility start.
- SIPOS still needs Windows runtime validation because it is a Windows Forms app and uses Microsoft Office interop.
- The app should not be marked uploaded/live based only on repository build success. Upload/live status must be confirmed in the chosen external distribution channel.
