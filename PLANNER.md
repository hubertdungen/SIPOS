# SIPOS Planning Board

This document mirrors the visible Asana planning screenshots for SIPOS and keeps a repository-side planning copy for development work that cannot directly write to Asana from this environment.

## Asana snapshot read from screenshots

| Field | Value |
| --- | --- |
| Project/task | SIPOS - Sistema Inteligente de Processamento de Ordens de Serviço |
| Assignee | Hubert |
| Priority | High |
| Tags | Força Aérea, C# |
| Boards/projects | Development, Força Aérea, Central Board |
| Status in Força Aérea board | In progress |
| Subtasks visible | 16 / 25 complete |
| Description visible in Asana | Automatização de Ordens de Serviço na UAL e no espectro da Força Aérea |

## Release status interpreted from Asana

The Asana screenshots show the main SIPOS task as an active project with 16 of 25 subtasks complete. The most recent completed development items shown are under the `Modelar: Drag&Drop Improvements and Modulation Mechanics [v B-1.2.0]` epic:

- `Modelar: Implement "Programar" and "Ficheiros" menus [v B-1.2.1]` — complete.
- `Modelar: Fix issue related to menu styling [v B-1.2.2]` — complete.

Therefore, repo/docs should describe Beta 1.2.2 as the latest completed code milestone seen in Asana, while planning should continue with the pending B-1.2.x items below.

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

### Current / upcoming items visible

- [ ] `Modelar: Drag&Drop Improvements and Modulation Mechanics [v B-1.2.0]`
- [ ] `Detectar: Feriados [v B-1.3.0]`
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

The Asana screenshots show this epic as complete with 4 / 4 subtasks complete:

- [x] `Interpretar: Actualizar range da folha Excel e interpretação de coordenadas dinâmicas de dados [v B-1.1.0]`
- [x] `Interpretar: Fixed an issue on "Range Finding" the escala dates (Formulas or Values) [v B-1.1.1]`
- [x] `FormDados: Fixed an issue where the preview text wasn't clearing the old text [v B-1.1.2]`
- [x] `FormDados: Fixed an issue where the progress bar percentage was jumping above 100%, thus throwing a fatal error [v B-1.1.3]`

## B-1.3.0 Detectar: Feriados details

- [ ] `Detectar: Detection Engine updates (starts detecting holidays) [v B-1.3.1]`
- [ ] `Calendário: Adicionar checkBox para ativar Start e End Date. [v B-1.3.2]`

## Portable app / portable .exe plan

Goal: add a planning path for making SIPOS available as a Windows `.exe` that can be distributed as a portable folder/zip where feasible. This should be added to Asana as a new planning item, because it is not visible in the screenshots.

### Suggested Asana task

`Release: Criar versão portátil / .exe para Windows [v B-1.4.0 or release task]`

### Phase 1 — Feasibility

- [ ] Confirm supported Windows versions and architecture target (`win-x64` first; decide later on `win-x86` or `win-arm64`).
- [ ] Confirm whether Microsoft Office is an accepted prerequisite for Word/Excel interop.
- [ ] Decide framework-dependent vs self-contained publish.
- [ ] Inventory required runtime assets: fonts, templates, manifest, default folders, and any documents used by Word/Excel automation.

### Phase 2 — Publish profile / command

- [ ] Add or document a Windows publish command for `net6.0-windows` and `win-x64`.
- [ ] Test `dotnet publish` with `-p:PublishSingleFile=true`.
- [ ] Test self-contained publish with `-p:SelfContained=true` if artifact size is acceptable.
- [ ] Confirm Office automation works from the published folder on Windows.

### Phase 3 — Portable artifact

- [ ] Produce `SIPOS-Beta-<version>-win-x64-portable.zip`.
- [ ] Include the `.exe`, required assets, and `README-PORTABLE.txt`.
- [ ] Generate a checksum for the zip artifact.
- [ ] Smoke-test launch from an extracted folder path with spaces.
- [ ] Smoke-test Excel import and Word export on Windows.

### Phase 4 — Release / upload

- [ ] Decide the authoritative upload location: GitHub Releases, Asana attachment, shared drive, or another channel.
- [ ] Upload the portable artifact.
- [ ] Link the artifact in Asana and release notes.
- [ ] Mark the release task complete only after Windows runtime smoke testing passes.

## Repository verification notes

- The repository can restore and build in this environment with .NET SDK 6.0 and Windows targeting enabled.
- Non-Windows builds are compile checks only; SIPOS still needs Windows runtime validation because it is a Windows Forms app and uses Microsoft Office interop.
- The app should not be marked uploaded/live based only on repository build success. Upload/live status must be confirmed in the chosen external distribution channel.
