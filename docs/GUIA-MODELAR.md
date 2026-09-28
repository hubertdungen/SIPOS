# Guia do Sistema Modelar (v B-1.5.2)

Este guia explica como funciona o sistema de programação de exportações do SIPOS
— o "Modelar" — e como o testar no Windows.

## 1. O conceito

O Modelar deixa o utilizador **programar as operações que o SIPOS executa ao
exportar** uma Ordem de Serviço. Em vez de um fluxo fixo, a exportação passa a
ser descrita por um **programa**: uma lista ordenada de **linhas de ação**, que
pode incluir **loops** (ex.: "por cada dia selecionado") com ações aninhadas.

Exemplo — o fluxo clássico descrito como programa:

```text
Loop: por cada dia selecionado
 ├── Ler escalas do dia          (consulta as folhas Excel desse dia)
 ├── Inserir documento           (insere o modelo_escalas.doc no ponto 101)
 └── Substituir variáveis        (preenche as <tags> SÓ no bloco desse dia)
```

Com 3 dias selecionados, o SIPOS lê o Excel do dia 1 e preenche a tabela do
dia 1; depois repete para o dia 2 e para o dia 3. Cada tabela fica com os
valores **do seu próprio dia** — é isto que o fluxo clássico não conseguia
fazer (o `wdReplaceAll` punha o mesmo valor em todas as cópias).

## 2. Os tipos de ação disponíveis

| Tipo (`Tipo` no JSON) | O que faz |
| --- | --- |
| `LoopDias` | Repete as ações em `Filhos` uma vez por cada dia selecionado. |
| `LerEscalasDoDia` | Corre a triagem das folhas Excel para o dia atual do loop e carrega os escalados. |
| `InserirDocumento` | Insere o ficheiro Word indicado em `Ficheiro` no documento final (ver capítulo 3). |
| `SubstituirVariaveis` | Substitui as `<tags>` (ODU/CCS/SD/PD, OAF às quartas, `<dataEscalados>` e os dados da O.S.) **apenas no que foi inserido desde a última substituição**, com os valores do dia atual. |
| `QuebraDePagina` | Insere uma quebra de página no ponto de inserção. |

Cada linha de ação tem ainda: `Nome` (rótulo, não pode ser vazio nem repetido),
`Ativa` (true/false — linhas desativadas são saltadas, como o botão ✓ da UI) e,
nos tipos com documento, `Ficheiro` (caminho do .doc).

Novos tipos de ação (outros loops, condições, mais documentos) são acrescentados
ao enum `TipoDeAcao` sem partir programas já gravados.

## 3. Onde ficam os blocos de cada dia

Numa O.S. real de vários dias, os blocos "Para o dia …" ficam **seguidos dentro
do ponto 101. PESSOAL DE SERVIÇO**, antes do "102. AUSÊNCIAS E IMPEDIMENTOS". É
assim nos exemplares em `modelos_word/exemplares/`:

- O.S. de sexta 30SET2022 (`2022-002-186`): blocos de 01, 02 e 03OUT;
- O.S. de terça 04OUT2022 (`2022-002-188`): bloco de 05OUT (feriado) com a
  "ASSISTÊNCIA AOS FUNERAIS" logo a seguir, e depois o bloco de 06OUT.

O motor reproduz esta estrutura:

1. Os modelos base (`modelo_de_semana.doc`, `modelo_de_quarta.doc`) já trazem o
   bloco "Para o dia `<dataEscalados>`" por preencher. **Esse bloco serve o
   primeiro dia** — no modelo de quarta, com a secção dos funerais incluída.
2. Os blocos dos dias seguintes são inseridos **a seguir, antes do "102."**,
   pela ordem dos dias. Só os fragmentos que contêm `<dataEscalados>` (como o
   `modelo_escalas.doc`) contam como bloco de escala.
3. Com **um só dia**, o documento final é igual ao do fluxo clássico.

Para modelos de outras unidades, sem o título "102.": se o modelo base tiver o
texto `<fimEscalas>` num parágrafo próprio, os blocos dos dias seguintes entram
nesse sítio (o parágrafo do marcador é retirado). Sem título nem marcador, vão
para o fim do documento.

A inserção usa `Range.InsertFile` — não mexe na área de transferência do
Windows.

## 4. O ficheiro de programa

O programa vive num JSON chamado **`modelar_programa.json` ao lado do
`SIPOS.exe`** (regra portable, tal como o `settings.txt`). Exemplo completo —
o programa clássico:

```json
{
  "Nome": "Exportação clássica de O.S.",
  "Versao": 1,
  "Acoes": [
    {
      "Tipo": "LoopDias",
      "Nome": "Por cada dia selecionado",
      "Ativa": true,
      "Filhos": [
        { "Tipo": "LerEscalasDoDia",     "Nome": "Ler escalas do dia",        "Ativa": true },
        { "Tipo": "InserirDocumento",    "Nome": "Inserir tabela de escalas", "Ativa": true,
          "Ficheiro": "C:\\SIPOS\\modelos_word\\modelo_escalas.doc" },
        { "Tipo": "SubstituirVariaveis", "Nome": "Preencher variáveis do dia", "Ativa": true }
      ]
    }
  ]
}
```

Há uma cópia deste exemplo em `docs/modelar_programa.exemplo.json` — basta
copiá-la para junto do `SIPOS.exe`, renomear para `modelar_programa.json` e
ajustar o caminho do `Ficheiro`. Normalmente não é preciso: o separador
Modelar cria-o (capítulo 6).

## 5. Como o SIPOS decide que fluxo usar

Ao clicar **Exportar Word**:

1. **Sem** `modelar_programa.json` ao lado do exe → corre o **fluxo clássico**,
   exatamente como sempre, sem perguntas.
2. **Com** o ficheiro → o SIPOS mostra os dias que o programa vai gerar (com o
   nome dos feriados) e pergunta:
   - **Sim** — exportar com o programa Modelar;
   - **Não** — exportação clássica (um dia, como sempre);
   - **Cancelar** — não exportar.

   Antes de abrir o Word, o programa é validado (nomes vazios/duplicados, linhas
   "Inserir documento" sem ficheiro ou com ficheiro inexistente, loops vazios,
   programa sem ações ativas → mensagem e não exporta).
3. Se o ficheiro existir mas estiver **corrompido**, o SIPOS avisa e pergunta se
   quer continuar com a exportação clássica.

Os **dias** que alimentam o `LoopDias` vêm do separador Dados:

- checkbox **"Ativar data de início e fim" desligada** → regra automática com
  feriados: a partir do dia seguinte à O.S., todos os dias de descanso
  consecutivos (fins-de-semana **e feriados nacionais**) até ao primeiro dia
  útil. Ex.: O.S. de sexta → sáb+dom+seg; O.S. de véspera de feriado → feriado +
  dia útil seguinte.
- checkbox **ligada** → selecione **desde o dia da O.S. até ao último dia a
  cobrir**. O primeiro dia do intervalo é o dia da O.S. (é o que dá a data da
  O.S. e escolhe o modelo), por isso os dias gerados começam no dia seguinte:
  sexta→segunda gera sábado, domingo e segunda. Os "Dias de interrupção"
  mostram os dias entre os dois (sexta→segunda = 2, como a regra do sábado).

O documento base continua a ser o modelo de semana/quarta configurado nas
Propriedades; os cabeçalhos (`<numOS>`, `<dataOS>`, `<dataOS_abv>`) e a
numeração de páginas são tratados uma vez, como no fluxo clássico.

## 6. Editar o programa no separador Modelar

O separador **Modelar** edita o programa diretamente — não é preciso escrever
o JSON à mão para o caso comum:

- Cada linha tem um **ComboBox com o tipo de ação** (Inserir documento, Ler
  escalas do dia, Substituir variáveis, Quebra de página), o nome, o caminho do
  ficheiro (📄 abre um diálogo só para documentos Word; cancelar não mexe no
  caminho), o **✓/✗** de ativação (linhas desligadas ficam esbatidas e são
  saltadas) e as setas **▲/▼** para ordenar (também dá para arrastar pelo ⋯).
- O **seletor de modelos** (topo da lista) adiciona com ➕ um programa completo
  ("Exportação clássica") ou uma ação avulsa do tipo escolhido — a primeira
  linha vazia é reutilizada e os nomes são gerados únicos. As dicas por cima da
  lista mudam consoante o menu Programar/Ficheiros ativo.
- **💾 Guardar Programa** valida as linhas e grava-as em `modelar_programa.json`
  como filhos de um "Loop: por cada dia selecionado". Linhas completamente
  vazias (sem nome nem ficheiro) são ignoradas.
- **⭯ Recarregar** repõe a lista a partir do ficheiro gravado (fica uma linha
  por ação). Ao abrir o separador, um programa já gravado é carregado sozinho.

Programas mais avançados (vários loops, ações fora do loop) continuam a poder
ser escritos à mão no JSON; se um destes estiver carregado, o 💾 avisa antes de
o substituir pela lista.

## 7. Como testar no Windows (passo a passo)

1. Extrair o zip portátil para uma pasta nova com permissões de escrita.
2. Configurar as Propriedades como habitualmente (Excel das escalas, modelos,
   pasta de exportação, pasta do inspetor) e gravar.
3. **Fluxo clássico primeiro**: sem nenhum `modelar_programa.json`, escolher um
   dia no Dados, "Atualizar", e Exportar Word. Deve sair como sempre — e agora
   com os militares **em adaptação** (estado ADPT) preenchidos, que antes
   ficavam em branco.
4. No separador Modelar: seletor → "Programa: Exportação clássica" → ➕. Na
   linha "Inserir tabela de escalas", 📄 → escolher o `modelo_escalas.doc`.
   💾 Guardar Programa.
5. No Dados, escolher uma **sexta-feira** e Exportar Word → na pergunta devem
   aparecer sábado, domingo e segunda → **Sim**. Resultado esperado: três blocos
   "Para o dia …" seguidos no ponto 101, cada um com os nomes do seu dia, e o
   "102." logo a seguir ao terceiro.
6. Repetir com uma **terça-feira** antes de um feriado à quarta (ex.:
   04/10 com 05/10 feriado): quarta com os funerais e depois quinta.
7. Verificações úteis: responder **Não** faz a exportação clássica; um nome
   duplicado impede o 💾; um ficheiro inexistente impede a exportação com
   mensagem; o Gestor de Tarefas não deve ficar com WINWORD.EXE órfãos.

## 8. Testes automáticos

A lógica que não depende do Windows (programa, JSON, feriados, dias e a
colocação dos blocos, sobre um documento simulado com a estrutura dos modelos
reais) tem testes em `tests/SIPOS.Logic.Tests`. Para correr, na pasta do
repositório:

```text
dotnet run --project tests/SIPOS.Logic.Tests
```

## 9. Estado e próximos passos

| Peça | Estado |
| --- | --- |
| B-1.5.1 estrutura do programa + JSON | ✅ implementado e testado |
| B-1.5.3 lista de dias (feriados/intervalo) | ✅ implementado e testado |
| B-1.5.2 motor Word | ✅ implementado; colocação dos blocos testada; interop Word **por validar no Windows** |
| B-1.2.3 / B-1.2.6 / B-1.2.7 UI do Modelar | ✅ implementado, **por validar no Windows** |
| B-1.5.4 validação com exemplares reais | ⏳ pendente (capítulo 7) |
| Ação condicional "só às quartas" (funerais noutros dias do loop) | 💡 ideia para uma próxima versão |
