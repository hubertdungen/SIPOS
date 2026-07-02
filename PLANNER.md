# SIPOS Planning Board

This document mirrors the live Asana planning state for SIPOS and keeps a repository-side planning copy for implementation work. Asana remains the source of truth; this file records the GitHub-side snapshot and the repo changes tied to each planning item.

## Asana snapshot verified on 2026-07-02

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
- [ ] `Modelar: Prevent empty or similar names from moving [v B-1.2.5]`
- [ ] `Modelar: Template ComboBox Logic [v B-1.2.6]`
- [ ] `Modelar: Form Layout Update & Menu Logic [v B-1.2.7]`

## B-1.1.0 Interpretação e FormDados epic details

The live Asana task shows this epic as complete with 4 / 4 subtasks complete:

- [x] `Interpretar: Actualizar range da folha Excel e interpretação de coordenadas dinâmicas de dados [v B-1.1.0]`
- [x] `Interpretar: Fixed an issue on "Range Finding" the escala dates (Formulas or Values) [v B-1.1.1]`
- [x] `FormDados: Fixed an issue where the preview text wasn't clearing the old text [v B-1.1.2]`
- [x] `FormDados: Fixed an issue where the progress bar percentage was jumping above 100%, thus throwing a fatal error [v B-1.1.3]`

## B-1.3.0 Detectar: Feriados details

- [ ] `Detectar: Detection Engine updates (starts detecting holidays) [v B-1.3.1]`
- [ ] `Calendário: Adicionar checkBox para ativar Start e End Date. [v B-1.3.2]`

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
- [x] Test `dotnet publish` with `PublishSingleFile=true`.
- [x] Test self-contained publish with `SelfContained=true`.
- [ ] Confirm Office automation works from the published folder on Windows.

### Phase 3 - Portable artifact

- [x] Produce local `SIPOS-Beta-1.2.2-win-x64-portable.zip`.
- [x] Include `SIPOS.exe`, debug symbols, and `README-PORTABLE.md` in the portable output.
- [x] Generate a SHA-256 checksum for the zip artifact.
- [ ] Smoke-test launch from an extracted folder path with spaces.
- [ ] Smoke-test Excel import and Word export on Windows.

### Phase 4 - Release / upload

- [ ] Decide the authoritative upload location: GitHub Releases, Asana attachment, shared drive, or another channel.
- [ ] Upload the portable artifact.
- [ ] Link the artifact in Asana and release notes.
- [ ] Mark the release task complete only after Windows runtime smoke testing passes.

## Repository branch repair notes

GitHub repair completed on 2026-07-02 and reflected in Asana task `Repo: Reparar branch default do GitHub sem perder a ultima versao`:

- The repository default branch name is still `SIPOS_v0-8-3`, but the branch content now points to the latest known SIPOS implementation from `SIPOS_v0-9-4`.
- Old default branch commit preserved at `backup/SIPOS_v0-8-3-before-2026-07-02`.
- Old default commit before repair: `4533fdd336c7348fa5d84a9f7ec0116e653ec3f4`.
- Latest branch commit used for repair: `9eb971f97353f00d4760df39ca4e05dcdc9d29f7`.
- GitHub reported no common ancestor between the old default and latest branch, so a normal merge was not safe. The repair used a backup branch plus a forced default-ref move to avoid losing the latest working code.

## Repository verification notes

- `dotnet restore` succeeds locally with .NET SDK 10.0.103 and Windows desktop targeting available.
- `dotnet build` succeeds locally for `net6.0-windows`.
- `dotnet publish /p:PublishProfile=win-x64-portable` succeeds locally.
- `scripts/Publish-Portable.ps1 -Version Beta-1.2.2` succeeds locally and creates `artifacts/SIPOS-Beta-1.2.2-win-x64-portable.zip` plus a `.sha256` checksum.
- Local artifact checksum: `455ABE811E8E4D4668B76AB3213C0360974F62D6340463B9D207AE090DBD467C`.
- Build still emits existing warnings, including `NETSDK1138` because `net6.0-windows` is out of support. That should be planned separately from this portable compatibility start.
- SIPOS still needs Windows runtime validation because it is a Windows Forms app and uses Microsoft Office interop.
- The app should not be marked uploaded/live based only on repository build success. Upload/live status must be confirmed in the chosen external distribution channel.
