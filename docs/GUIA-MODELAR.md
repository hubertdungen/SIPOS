# Guia do Sistema Modelar (v B-1.5.x)

Este guia explica como funciona o sistema de programação de exportações do SIPOS
— o "Modelar" — introduzido nas versões B-1.5.x, e como o testar no Windows.

## 1. O conceito

O Modelar deixa o utilizador **programar as operações que o SIPOS executa ao
exportar** uma Ordem de Serviço. Em vez de um fluxo fixo, a exportação passa a
ser descrita por um **programa**: uma lista ordenada de **linhas de ação**, que
pode incluir **loops** (ex.: "por cada dia selecionado") com ações aninhadas.

Exemplo do fluxo clássico descrito como programa:

```text
Loop: por cada dia selecionado
 ├── Ler escalas do dia          (consulta as folhas Excel desse dia)
 ├── Inserir documento           (copia modelo_escalas.doc e cola no doc final)
 └── Substituir variáveis        (preenche as <tags> SÓ no bloco acabado de colar)
```

Com 3 dias selecionados, o SIPOS expande isto em 9 operações: lê o Excel do
dia 1, cola uma tabela, preenche-a com os nomes do dia 1; depois repete para o
dia 2 e para o dia 3. Cada tabela colada fica com os valores **do seu próprio
dia** — é isto que o fluxo clássico não conseguia fazer (o `wdReplaceAll`
punha o mesmo valor em todas as cópias).

## 2. Os tipos de ação disponíveis

| Tipo (`Tipo` no JSON) | O que faz |
| --- | --- |
| `LoopDias` | Repete as ações em `Filhos` uma vez por cada dia selecionado. |
| `LerEscalasDoDia` | Corre a triagem das folhas Excel para o dia atual do loop e carrega os escalados. |
| `InserirDocumento` | Abre o ficheiro Word indicado em `Ficheiro`, copia o conteúdo todo e cola-o no fim do documento final. |
| `SubstituirVariaveis` | Substitui as `<tags>` (ODU/CCS/SD/PD/OAF + `<dataEscalados>`) **apenas dentro do último bloco colado**, com os valores do dia atual. |
| `QuebraDePagina` | Insere uma quebra de página no fim do documento final. |

Cada linha de ação tem ainda: `Nome` (rótulo, não pode ser vazio nem repetido),
`Ativa` (true/false — linhas desativadas são saltadas, como o botão ✓ da UI) e,
nos tipos com documento, `Ficheiro` (caminho do .doc).

Novos tipos de ação (outros loops, condições, mais documentos) são acrescentados
ao enum `TipoDeAcao` sem partir programas já gravados.

## 3. O ficheiro de programa

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
ajustar o caminho do `Ficheiro`.

## 4. Como o SIPOS decide que fluxo usar

Ao clicar **Exportar Word**:

1. **Sem** `modelar_programa.json` ao lado do exe → corre o **fluxo clássico**,
   exatamente como sempre. Zero mudança de comportamento.
2. **Com** o ficheiro → o programa é validado (nomes vazios/duplicados, docs sem
   ficheiro, loops vazios → mensagem de erro e aborta) e o **motor Modelar**
   executa o plano.

Os **dias selecionados** que alimentam o `LoopDias` vêm de:

- checkbox **"Ativar data de início e fim" ligada** → todos os dias do intervalo
  escolhido no calendário;
- checkbox desligada → regra automática com feriados: a partir do dia seguinte
  à O.S., todos os dias de descanso consecutivos (fins-de-semana **e feriados
  nacionais**) até ao primeiro dia útil. Ex.: O.S. de sexta → sáb+dom+seg;
  O.S. de véspera de feriado → feriado + dia útil seguinte.

O documento base continua a ser o modelo de semana/quarta configurado nas
Propriedades; os cabeçalhos (`<numOS>`, `<dataOS>`, `<dataOS_abv>`) e a
numeração de páginas são tratados uma vez, como no fluxo clássico.

## 5. Editar o programa na própria UI (B-1.2.6)

O separador **Modelar** edita o programa diretamente — não é preciso escrever
o JSON à mão para o caso comum:

- Cada linha da lista tem um **ComboBox com o tipo de ação** (Inserir
  documento, Ler escalas do dia, Substituir variáveis, Quebra de página),
  além do nome, do caminho do ficheiro (📄), do **✓/✗** de ativação (linhas
  desligadas ficam esbatidas e são saltadas pelo motor) e das setas ▲/▼
  para ordenar.
- **💾 Guardar Programa** (menu superior) valida as linhas e grava-as em
  `modelar_programa.json` como filhos de um "Loop: por cada dia selecionado"
  — o caso do capítulo 1. **⭯ Recarregar** volta a preencher as linhas a
  partir do ficheiro gravado.
- Ao abrir o Modelar, se já existir um `modelar_programa.json` ao lado do
  exe, as linhas são preenchidas automaticamente a partir dele.

Programas mais avançados (vários loops, ações fora do loop) continuam a poder
ser escritos à mão no JSON — a UI cobre o caso comum de um loop de dias.

## 6. Como testar no Windows (passo a passo)

1. Extrair o zip portátil para uma pasta com permissões de escrita.
2. Configurar as Propriedades como habitualmente (Excel das escalas, modelos,
   pasta de exportação) e gravar.
3. Criar o programa: ou no separador Modelar (preencher as linhas e
   💾 Guardar Programa), ou copiando `docs/modelar_programa.exemplo.json`
   para junto do `SIPOS.exe`, renomeado para `modelar_programa.json` e com o
   caminho do `modelo_escalas.doc` corrigido.
4. No separador Dados, escolher um dia — para multi-dia, ligar a checkbox
   "Ativar data de início e fim" e arrastar um intervalo de 2–3 dias no
   calendário.
5. Exportar Word. O resultado deve ter uma tabela de escalas por dia, cada uma
   com os nomes do respetivo dia.
6. Verificações úteis: sem o JSON o export volta ao clássico; um JSON com nome
   duplicado deve mostrar a validação e abortar; o Gestor de Tarefas não deve
   ficar com WINWORD.EXE órfãos depois de um erro.

## 7. Estado e próximos passos

| Peça | Estado |
| --- | --- |
| B-1.5.1 estrutura do programa + JSON | ✅ implementado e testado (17 testes) |
| B-1.5.3 lista de dias (feriados/intervalo) | ✅ implementado e testado |
| B-1.5.2 motor Word (este branch) | ⚠️ implementado, **por validar no Windows** |
| B-1.2.6 UI: ComboBox de tipo + guardar/carregar programa (este branch) | ⚠️ implementado, **por validar no Windows** |
| B-1.2.3 design custom do ComboBox (este branch) | ⚠️ implementado com o `CustomComboBox` do projeto, **por validar no Windows** |
| B-1.5.4 validação com exemplares reais | ⏳ pendente (depende dos testes acima) |

O formato JSON é estável e validado ao carregar (ficheiro corrompido → o
SIPOS avisa e usa o fluxo clássico em vez de crashar).
