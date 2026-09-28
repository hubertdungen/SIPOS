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

## Modelar — real concept (clarified by Hubert on 2026-07-09)

The Modelar module is for the user to model/program the OPERATIONS SIPOS performs on export, not just to manage a list of documents. Each row in the drag&drop list (name + Word file + active toggle + order) is one step of an "export program": an action template based on copying model documents, inserting everything into one final document, and substituting the `<tag>` variables for each selected day.

Example flow the user described: select 3 days → the engine queries the Excel escala list once per day → for each day it inserts a copy of a fragment (e.g. `modelo_escalas.doc`, the formatted O.S. escala report table) into the final document → and in each pasted copy substitutes that specific day's variables.

Evidence in the repository that supports this design:

- `modelo_escalas.doc` is a single-block fragment (each `<tag>` appears exactly once) — built to be copy-pasted N times;
- the old `applyModeloEscala` method (removed as dead code in Beta-1.2.2-r2) copied an escalas doc and pasted it at the end of the document — the embryo of this engine;
- `CreateWordDocument` still has a `for (int i = 0; i < 1; i++)` placeholder loop for iterating days;
- `FormDados.btn_refresh` already runs the escala triage 3× (sáb/dom/seg) when the day is Saturday;
- the B-1.3.2 start/end checkbox now provides multi-day selection input.

Current limitation: `FindAndReplace` uses `wdReplaceAll`, so every copy of a tag in the document receives the SAME value. Multi-day substitution requires either scoping the replace to the freshly pasted range or per-day tag suffixes.

Next epic (mirrored in Asana) — `Modelar/Exportar: Motor de execução de templates multi-dia [v B-1.5.0]`:

- [x] B-1.5.1 Bind the Modelar rows to a persisted program structure the exporter can read — **implemented 2026-07-09** in `ModelarPrograma.cs`.
- [x] B-1.5.2 Engine: for each selected day, insert the fragment into the final document → substitute variables only within that day's block — **implemented 2026-07-09, reworked in the 2026-09-28 review (blocks placed in section 101, base template's table reused for day 1) and merged in Beta 1.5.2**; Word interop still needs the Windows smoke test (B-1.5.4).
- [x] B-1.5.3 Derive the day list from the start/end range (B-1.3.2) and the holiday engine (`Feriados.cs`) — **implemented 2026-07-09** as `PlaneadorDeDias`.
- [ ] B-1.5.4 Windows validation against the real exemplares in `modelos_word/`.

### B-1.5.2 engine (2026-07-09, reworked 2026-09-28)

- `ModelarMotorWord.Executar(programa, dias, modeloBase, destino)`: validates the program (plus: no active actions, missing fragment files, fragment equal to the base model — all before starting Word), opens the base O.S. model once, prepares headers/footers and page numbering exactly like the classic flow, then runs the plan through `ExecutorModelar`. Word always closes on error (Beta-1.3.1 hardening pattern) and the Mediator's global date state is restored afterwards.
- **Block placement (`ExecutorModelar`, pure logic in `ModelarPrograma.cs`)**: real multi-day O.S. (`modelos_word/exemplares/2022-002-186`, `-188`) keep the "Para o dia …" blocks consecutive inside "101. PESSOAL DE SERVIÇO", before "102. AUSÊNCIAS…", with the Wednesday funerals section right after Wednesday's block. The base models already carry one unfilled block (`<dataEscalados>`), so that block serves **day 1**; the first insertion of a fragment containing `<dataEscalados>` is swapped for it, and the following days are inserted at the insertion point (start of the "102." paragraph; an optional `<fimEscalas>` paragraph overrides it; otherwise end of document, in a new paragraph). Each `SubstituirVariaveis` fills everything inserted since the previous one. With one day the result equals the classic flow.
- `IDocumentoModelar` separates the placement algorithm from Word: `ModelarMotorWord.DocumentoWord` implements it with `Range.InsertFile` (no clipboard), `InsertBreak`, scoped `Range.Find`, and length deltas of `Content.End` to keep positions right; the test suite implements it over plain text.
- `SubstituirVariaveis` calls `Word_Processor.SubstituirVariaveisNoRange(range, dia)`: escala tags (`<dataEscalados>`, ODU/CCS/SD/PD, OAF on Wednesdays) plus the O.S. tags (`<numOS>`, `<dataOS>`, `<dataOS_abv>`) **only inside the given range** (`Range.Find` with `wdFindStop`). The adapter loads the day's vars before and clears them after.
- `LerEscalasDoDia` points the global date state at the operation's day, removes that day's entries already in the escalados list (e.g. from a Dados refresh) and runs the Excel triage.
- Export wiring (`FormExport.EscolherExportacaoModelar`): without `modelar_programa.json` the classic flow runs with no questions; with it, a Sim/Não/Cancelar dialog lists the days the program will generate (with holiday names) — saving a program never silently changes the daily export; an unreadable JSON is reported and the user may continue with the classic flow. Days come from `PlaneadorDeDias.DiasParaExportacao`.
- User guide: `docs/GUIA-MODELAR.md` (concept, action types, block placement, JSON, flow choice, UI, Windows test script, automated tests).

### Review of PR #10 (2026-09-28)

Findings fixed before the merge (none was reachable in the classic flow except C1):

- E1 — engine left the base model's own table unfilled (raw tags) and appended the other days' tables at the end of the document, after the signatures. Fixed by the placement rules above.
- E2 — global date state (`diaDeEscala`, `escalaDay`, `isItSabado`, `isItQuarta`) stayed on the last loop day after an export. Now restored.
- E3 — re-reading a day already loaded by the Dados refresh duplicated its entries (with C1 fixed this would print "ADPT" twice). The day's entries are removed before the triage.
- E4 — range mode included the O.S. day itself (`DiasDoIntervalo(inicio, fim)`), unlike the automatic rule that starts the next day; and `FormDados` is recreated on each visit with the checkbox off while `Mediator.rangeAtivo` stayed on. Range now means O.S. day → last covered day (days = start+1..end), the flag resets with the form, and "Dias de interrupção" shows end−start−1 (Friday→Monday = 2, like the Saturday rule).
- E5 — corrupted JSON silently fell back to the classic flow although the guide promised a warning; hand-written JSON with `null` lists crashed validation. Both handled.
- C1 (classic flow, since v A-0.10.8) — the triage stores adaptation as `"ADPT"` (commit 275479a, as real O.S. show it) but `listToVarsEscalados` still filtered `"Adaptação"`, so personnel in adaptation never reached the Word document. Both values are now accepted.
- U1 — `FormModelar_Load` was not wired since the v B-1.2.2 Designer regeneration (`Load += FormModelar_Load` lost in db8f2e9), so **nothing in it ever ran**: no ▲▼ arrows (B-1.2.4), type ComboBox, 💾/⭯ buttons, template selector or auto-load. Wiring restored in the Designer.
- U2 — the name box and ✓ button live inside the `pnlTextNameW` sub-panel, but the row lookups searched direct children only: saving was impossible (every row "without name"), loaded names never showed, ✓ was never read and the B-1.2.5 validation never blocked. Lookups are now recursive (not descending into CustomComboBox internals).
- U3 — the template row was augmented only at Load, after the first row had already been cloned; now done in the constructor so every row gets the arrows and ComboBox.
- U4 — 📄 reused `Mediator.openFile()`: cancelling filled the path with the last file picked elsewhere (e.g. an Excel). Dedicated Word-filtered dialog; path changes only on OK.
- U5 — ⭯ Recarregar left extra rows; fully empty rows blocked saving; saving silently flattened hand-written programs; ✗ dimmed only the name box; the PROGRAMAS/FICHEIROS tabs (80% + 21% of the bar) would cover the new 💾/⭯ buttons. All fixed.
- Row `MaximumSize` (B-1.2.7) was a no-op — cloned rows never copy it and are sized to the list width — and was removed.

Verification: 0 build errors and 0 new warnings versus `main`; new `tests/SIPOS.Logic.Tests` (27 tests: program/JSON, days and holidays incl. the real exemplares' dates, and block placement over a simulated document with the real templates' structure); mutation check — reverting to "append at end", "no base-table reuse" or "range includes the O.S. day" makes the relevant tests fail.

### B-1.2.6 program-editing UI (2026-07-10, same branch)

FormModelar now edits `modelar_programa.json` directly:

- Each document row gains a code-created **action-type ComboBox** (`Inserir documento`, `Ler escalas do dia`, `Substituir variáveis`, `Quebra de página`); cloned rows inherit items and selection via a new ComboBox branch in `CloneControls`.
- New top-menu buttons: **💾 Guardar Programa** builds the program from the visible rows (in list order, wrapped in a `LoopDias`), runs `ProgramaModelar.Validar()` and saves beside SIPOS.exe; **⭯ Recarregar** re-populates the rows from the saved file. On form load, an existing program auto-populates the rows (cloning extra rows as needed).
- The previously empty `SwitchRowActivationState` stub is implemented: the ✓/✗ button now toggles and displays the row's `Ativa` state, dimming disabled rows; the engine skips inactive actions.
- `btnOpenWFile` (📄) switched from anchored to right-docked so it cannot be overlapped by the growing docked button stack (arrows + ComboBox).
- Windows UI validation pending, same as the engine. `docs/GUIA-MODELAR.md` updated with the UI chapter.
- 2026-09-28: none of this was active at runtime until the review fixes U1–U5 (see "Review of PR #10").

### B-1.2.7 form layout & menu logic (2026-07-10, same branch)

The previously dead selector panel (`cmbBoxTemplateName` + `btnAddtoList` + `richtxtBox_ProgramaHints`, present in the Designer but never wired) now has its intended function:

- The template ComboBox lists "Programa: Exportação clássica (loop de dias)" plus one "Ação: X" entry per action type; ➕ adds the chosen program (3 preconfigured rows) or a single row of that type, reusing the first empty row and generating unique names so the B-1.2.5 validation never blocks fresh rows.
- The hint box text switches with the active top menu: Programar shows program-editing guidance, Ficheiros shows Word-file guidance. `btnProgramas_Click`/`btnFicheiros_Click` were previously style-only.
- Windows UI validation pending, same as the rest of the branch. With this, every B-1.2.x item of the Modelar epic is implemented.
- 2026-09-28: the row `MaximumSize` change originally listed here was a no-op and was removed; the tab widths now leave room for the 💾/⭯ buttons (review fix U5).

### B-1.2.3 custom ComboBox design (2026-07-10, same branch)

The action-type selector now uses the project's own `Controls/CustomComboBox` (custom border, drawn arrow icon, styled dropdown) instead of the native ComboBox, themed to the SIPOS dark palette (surface 40/30/40, list 35/26/45, border 79/49/79, DeepSkyBlue icon). `CloneControls` gained a dedicated branch that builds a fresh CustomComboBox per cloned row (copying the selected type) and skips child recursion, since the composite control constructs its internals in its constructor. Windows UI validation pending.

### B-1.5.1 + B-1.5.3 implementation (2026-07-09, `ModelarPrograma.cs`)

Pure-logic foundation, no WinForms/Office dependencies, so it is testable in isolation:

- `TipoDeAcao` — extensible action-type enum: `InserirDocumento` (copy/paste a Word doc into the output), `LerEscalasDoDia` (query the Excel escala for the loop's current day), `SubstituirVariaveis` (fill the `<tag>` variables in the last pasted block), `LoopDias` (repeat nested child actions once per selected day), `QuebraDePagina`.
- `AcaoModelar` — one action line: type + name + optional Word file + active toggle + nested children (for loops). Mirrors the Modelar row concept (name box, file box, ✓ toggle, order) and generalizes it to an action program.
- `ProgramaModelar` — ordered action list with: JSON persistence (`modelar_programa.json` beside SIPOS.exe, portable rule; corrupted/missing file loads as null instead of crashing), validation coherent with B-1.2.5 (no empty/duplicate action names, document actions need a file, loops cannot be empty), `CriarProgramaClassico()` reproducing today's flow (loop over days → read escalas → insert `modelo_escalas.doc` fragment → substitute variables), and `ExpandirPlano(dias)` which resolves loops into a flat, ordered list of `OperacaoPlaneada` (action + concrete day + file) — the exact input the B-1.5.2 Word engine will execute step by step.
- `PlaneadorDeDias` — `DiasDeEscala(diaDaOS)`: starting the day after the O.S., include consecutive rest days (weekends and holidays via `Feriados.IsDiaDeDescanso`) and stop at the first working day. Reproduces the classic behavior (normal Wednesday O.S. → Thursday only; Friday O.S. → Sat+Sun+Mon) and generalizes it to holidays (O.S. on a holiday's eve covers the holiday plus the next working day). `DiasDoIntervalo(inicio, fim)` supports the B-1.3.2 start/end mode. Hard cap of 14 days guards against runaway expansion.

Verification: isolated .NET 10 test suite, 17 checks, all passing — day derivation against real 2026 dates (normal day, Friday, eve of Dia de Portugal, Christmas Friday + weekend = 4 days), plan expansion order across 3 days (9 operations, correct day per operation, inactive actions skipped, out-of-loop actions carry no day), validation catching all 4 defect classes, and JSON round-trip preserving nested loops plus corrupted/missing-file safety.

UI note: the pending `Template ComboBox` tasks (B-1.2.3/B-1.2.6) now have a clear target — the ComboBox selects each row's `TipoDeAcao`/template, mapping the row list onto `ProgramaModelar`.

## B-1.2.0 Modelar epic details

### Completed

- [x] `Modelar: Implement "Programar" and "Ficheiros" menus [v B-1.2.1]`
- [x] `Modelar: Fix issue related to menu styling [v B-1.2.2]`

### Pending

- [x] `Modelar: Template ComboBox Custom Design [v B-1.2.3]` — implemented 2026-07-10, active since Beta 1.5.2 (Windows UI validation pending).
- [x] `Modelar: Arrows to switch order [v B-1.2.4]` — implemented 2026-07-09, **active only since Beta 1.5.2** (Windows UI validation pending).
- [x] `Modelar: Prevent empty or similar names from moving [v B-1.2.5]` — implemented 2026-07-09, **effective only since Beta 1.5.2** (Windows UI validation pending).
- [x] `Modelar: Template ComboBox Logic [v B-1.2.6]` — implemented 2026-07-10, active since Beta 1.5.2 (Windows UI validation pending).
- [x] `Modelar: Form Layout Update & Menu Logic [v B-1.2.7]` — implemented 2026-07-10, active since Beta 1.5.2 (Windows UI validation pending).

B-1.2.5 implementation notes: `FormModelar.elli_MouseDown` now refuses to start a drag when the row's `txtNameWBox` is empty or matches another row's name (case- and whitespace-insensitive). The blocked row's name box flashes red with a tooltip explaining the reason, and the drag state never engages, so `MouseMove`/`MouseUp` ignore the gesture. Needs visual confirmation on Windows. (Until Beta 1.5.2 the name lookup searched only the row's direct children and never found the box, so nothing was blocked — review fix U2.)

B-1.2.4 implementation notes: each document row now carries ▲/▼ buttons (created in code in `AddOrderArrowButtons`, right-docked next to the existing ➕/➖ buttons, and wired by name in `CloneControls` so cloned rows get working arrows too). Clicking moves the row one position up/down via `SetChildIndex` + `RefreshListLayout`, honoring the B-1.2.5 name validation (blocked rows flash instead of moving). Needs visual confirmation on Windows. (Until Beta 1.5.2 the arrows were added in `FormModelar_Load`, which was not wired, so they never appeared — review fixes U1/U3.)

## B-1.1.0 Interpretação e FormDados epic details

The live Asana task shows this epic as complete with 4 / 4 subtasks complete:

- [x] `Interpretar: Actualizar range da folha Excel e interpretação de coordenadas dinâmicas de dados [v B-1.1.0]`
- [x] `Interpretar: Fixed an issue on "Range Finding" the escala dates (Formulas or Values) [v B-1.1.1]`
- [x] `FormDados: Fixed an issue where the preview text wasn't clearing the old text [v B-1.1.2]`
- [x] `FormDados: Fixed an issue where the progress bar percentage was jumping above 100%, thus throwing a fatal error [v B-1.1.3]`

## B-1.3.0 Detectar: Feriados details

- [x] `Detectar: Detection Engine updates (starts detecting holidays) [v B-1.3.1]` — **motor implementado** em `Feriados.cs` (2026-07-09).
- [x] `Calendário: Adicionar checkBox para ativar Start e End Date. [v B-1.3.2]` — implemented 2026-07-09 (shipped in Beta-1.3.2; Windows UI validation pending).

B-1.3.2 implementation notes: `FormDados` gains a code-created checkbox "Ativar data de início e fim" beside the "Dias de interrupção" spinner. Unchecked (default), the calendar keeps its classic single-day selection (`MaxSelectionCount = 1`) and nothing changes. Checked, the calendar accepts a start→end range (up to 62 days) and the interruption-days spinner (and `Mediator.plusDayIntrup`) is auto-filled with the number of days between start and end; unchecking collapses the selection back to a single day. Needs visual confirmation on Windows.

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

## Release Beta-1.3.2 (2026-07-09): B-1.2.4 order arrows + B-1.3.2 range checkbox

Two roadmap features in one cycle:

- B-1.2.4 (Modelar): ▲/▼ buttons on each document row to move it one position up or down, honoring the B-1.2.5 name validation. See the Modelar epic section for details.
- B-1.3.2 (Calendário): "Ativar data de início e fim" checkbox on FormDados enabling start→end range selection, auto-filling the interruption days from the range. See the B-1.3.0 epic section for details.
- In-app version bumped to `v B-1.3.2`.
- Both features need Windows UI validation (built and verified compile-clean on Linux only).

Artifact:

- Artifact: `SIPOS-Beta-1.3.2-win-x64-portable.zip`
- SHA-256: `1C0936A9F37B9327C212A09DB859156BE78341939D47C15DC33236EC7F3D4471`
- Published in-repo under `dist/` (replaces the Beta-1.3.1 zip; Phase 4 channel decision still pending).

## Release Beta-1.5.1 (2026-07-09): Modelar program foundation

- New `ModelarPrograma.cs` implementing B-1.5.1 (action-program structure + JSON persistence + validation + plan expansion) and B-1.5.3 (holiday-aware day derivation). See the Modelar concept section for details.
- No behavior change in the app yet — the classes are the foundation the B-1.5.2 Word engine and the B-1.2.3/B-1.2.6 ComboBox UI will consume.
- In-app version bumped to `v B-1.5.1`.

Artifact:

- Artifact: `SIPOS-Beta-1.5.1-win-x64-portable.zip`
- SHA-256: `B88CCB750AC890B32415F0ED7C534F84BFBE8AFB63139B23B84B22C890516046`
- Published in-repo under `dist/` (replaces the Beta-1.3.2 zip; Phase 4 channel decision still pending).

## Release Beta-1.5.2 (2026-09-28): Modelar multi-day engine + Modelar UI, after review

Merged from PR #10 after the review described in the Modelar concept section ("Review of PR #10"):

- Modelar multi-day Word engine (B-1.5.2): one "Para o dia …" block per day, consecutive in section 101 like real O.S.; the base model's own table serves day 1. Opt-in per export: when `modelar_programa.json` exists, a Sim/Não/Cancelar dialog lists the days to generate; otherwise the classic flow runs unchanged.
- Modelar UI (B-1.2.3/B-1.2.6/B-1.2.7, plus B-1.2.4/B-1.2.5 now actually active): action-type ComboBox per row, 💾 Guardar / ⭯ Recarregar, template selector, per-menu hints.
- Classic flow fix: personnel in adaptation (ADPT) now reach the Word document (broken since v A-0.10.8).
- Range mode (B-1.3.2) now means O.S. day → last covered day; "Dias de interrupção" consistent with the Saturday rule.
- New `tests/SIPOS.Logic.Tests` (27 tests; `dotnet run --project tests/SIPOS.Logic.Tests`), excluded from the app build.
- In-app version `v B-1.5.2`. Still pending: Windows smoke test of the Word interop and the Modelar UI (B-1.5.4, script in `docs/GUIA-MODELAR.md` chapter 7).

Artifact:

- Artifact: `SIPOS-Beta-1.5.2-win-x64-portable.zip` (self-contained win-x64 single file, .NET runtime 10.0.12)
- SHA-256: `330C0668D7A70ED1A30F1CCDE3D9DE3DBF41AAC7965491EC27B9EB212E9FE59B`
- Published in-repo under `dist/` (replaces the Beta-1.5.1 zip; earlier builds stay reachable in the git history).

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
