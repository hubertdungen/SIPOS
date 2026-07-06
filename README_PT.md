# SIPOS

**SIPOS** significa **Sistema Inteligente de Processamento de Ordens de Serviço**. É uma aplicação Windows Forms criada para ajudar a automatizar a preparação das Ordens de Serviço (O.S.) diárias em unidades militares.

> Versão em inglês: [`README.md`](README.md)

## Estado do projecto

| Item | Estado |
| --- | --- |
| Marco beta visível actual | **Beta 1.2.2** |
| Tipo de aplicação | Aplicação desktop Windows Forms |
| Framework alvo | `net6.0-windows` |
| Linguagem principal | C# |
| Integração Office | Interop com Microsoft Word e Excel |
| Documento de planeamento | [`PLANNER.md`](PLANNER.md) |
| Auditoria do repositório | [`REPOSITORY_AUDIT.md`](REPOSITORY_AUDIT.md) |

A Beta 1.2.2 é o último marco concluído actualmente reflectido nas notas de planeamento do repositório. O projecto continua em desenvolvimento activo, com trabalho futuro em melhorias do módulo Modelar, detecção de feriados, interpretação de mensagens, programação/configuração pelo utilizador, adaptação a outras unidades e futura criação de uma versão Windows portátil em `.exe`.

## Objectivo

Muitas Ordens de Serviço continuam a ser preparadas manualmente por militares da unidade. Esse processo pode exigir recolha de informação em vários documentos, interpretação de escalas, validação de dados e publicação final após revisão/despacho do comando.

O SIPOS pretende reduzir esse trabalho repetitivo, ajudando o utilizador a:

- interpretar dados de escalas em ficheiros Excel;
- preparar e preencher documentos Word de Ordem de Serviço;
- exportar resultados para fluxos Word/PDF;
- gerir caminhos de modelos, exportações e pastas de inspecção/saída;
- evoluir para uma ferramenta mais adaptável a várias unidades.

## Funcionalidades principais

### Processamento de escalas em Excel

O SIPOS lê ficheiros Excel de escalas de serviço e extrai informação relevante, incluindo datas, militares nomeados, estados, efectivos, reservas e linhas adaptadas.

### Geração de documentos Word

A aplicação usa interop com Microsoft Word para abrir modelos, substituir marcadores, actualizar conteúdo gerado e preparar documentos de Ordem de Serviço.

### Fluxo de exportação

O SIPOS suporta fluxos de exportação orientados a Word/PDF e permite configurar pastas para documentos gerados.

### Fluxo Modelar / modelos

O roadmap inclui trabalho contínuo em listas ordenáveis de documentos/modelos, desenho personalizado de ComboBox, controlos de ordenação e lógica de menus/layouts.

### Interpretação futura de mensagens

O roadmap derivado do Asana inclui trabalho planeado para interpretar padrões de mensagens, extrair detalhes/corpo da mensagem, formatar conteúdo conforme o destino e detectar o tópico da O.S. onde inserir a informação.

## Estrutura do repositório

| Caminho | Finalidade |
| --- | --- |
| `SIPOS.sln` | Ficheiro de solução Visual Studio |
| `SIPOS.csproj` | Projecto principal Windows Forms |
| `Program.cs` | Ponto de entrada da aplicação |
| `Menu.cs`, `Menu.Designer.cs` | Interface/menu principal da aplicação |
| `Forms/` | Ecrãs WinForms como exportação, dados, ajuda, modelar e propriedades |
| `Controls/` | Controlos personalizados de UI |
| `EscalasEngine.cs` | Lógica de interpretação de escalas em Excel |
| `Word_Processor.cs` | Lógica de processamento/exportação Word |
| `Mediator.cs` | Estado partilhado e auxiliares de coordenação da aplicação |
| `Properties/Resources.resx` | Recursos de imagens/fontes referenciados pela aplicação |
| `folhas_excel_test/` | Dados de exemplo/teste em Excel |
| `modelos_word/` | Modelos Word, exemplares e amostras de exportação |
| `imgs/` | Imagens, ícones e recursos de design |
| `fonts/` | Fontes usadas pela aplicação |
| `PLANNER.md` | Roadmap alinhado com Asana e plano de app portátil |
| `REPOSITORY_AUDIT.md` | Auditoria do repositório e notas de limpeza |

## Requisitos

### Para desenvolvimento

- .NET SDK 6.x ou SDK compatível capaz de compilar projectos `net6.0-windows`.
- Windows é recomendado para desenvolvimento completo e validação em runtime.
- Em sistemas não-Windows, o restore/build pode ser usado como verificação de compilação com Windows targeting activo.

### Para execução

- Ambiente desktop Windows.
- Microsoft Office / Word / Excel instalado, porque o SIPOS usa actualmente automação Office interop.
- Acesso aos modelos Word, ficheiros Excel, pastas de exportação e caminhos de inspecção/saída configurados.

## Instruções de build

### Windows

```bash
dotnet restore SIPOS.sln
dotnet build SIPOS.sln
```

### Verificação de compilação em Linux/macOS

O SIPOS tem como alvo Windows Forms, por isso builds em sistemas não-Windows são apenas verificações de compilação e não substituem testes reais em Windows.

```bash
dotnet restore SIPOS.sln
dotnet build SIPOS.sln -p:EnableWindowsTargeting=true
```

O ficheiro de projecto também activa Windows targeting automaticamente em hosts não-Windows.

## Testes e verificação

Ainda não existe uma suite formal de testes unitários automatizados neste repositório. A verificação actual foca-se em:

- restore da solução;
- build com Windows targeting;
- validação manual da interpretação de Excel;
- validação manual da geração/exportação Word;
- verificações de auditoria documentadas em [`REPOSITORY_AUDIT.md`](REPOSITORY_AUDIT.md).

Verificação recomendada antes de uma release:

1. Executar restore/build.
2. Abrir a aplicação em Windows.
3. Validar importação de escalas Excel com ficheiros representativos.
4. Validar geração de documentos Word.
5. Validar localizações de exportação Word/PDF.
6. Confirmar que a automação Office funciona na máquina alvo.

## Plano para `.exe` portátil

Está planeada uma distribuição Windows portátil, mas ainda não está concluída. O objectivo é produzir um artefacto Windows versionado, por exemplo:

```text
SIPOS-Beta-<versão>-win-x64-portable.zip
```

O pacote portátil deverá incluir o executável, recursos/modelos necessários, um pequeno guia de utilização portátil e informação de checksum. Os detalhes estão em [`PLANNER.md`](PLANNER.md).

## Visão geral do roadmap

O trabalho planeado inclui:

- melhorias no Modelar para ordenação de documentos/modelos e lógica de UI personalizada;
- suporte para detecção de feriados;
- interpretação de mensagens e detecção automática de tópicos;
- melhor programação/configuração pelo utilizador;
- maior adaptabilidade a outras unidades;
- limpeza de warnings e manutenção de código;
- empacotamento Windows portátil.

Consulte [`PLANNER.md`](PLANNER.md) para o roadmap completo e notas de planeamento alinhadas com Asana.

## Notas de manutenção do repositório

O repositório contém código-fonte, modelos Word, exemplos Excel, imagens/ícones e ficheiros de design. Alguns ficheiros podem ser exemplos, fixtures, outputs gerados ou fontes de design. Não remova recursos apenas por não estarem referenciados directamente pelo nome do ficheiro; consulte [`REPOSITORY_AUDIT.md`](REPOSITORY_AUDIT.md) para recomendações de limpeza baseadas em evidência.

## Licença

Consulte [`LICENSE.md`](LICENSE.md).
