# SIPOS
## Sistema Inteligente de Processamento de Ordens de Serviço
O SIPOS é um software que visa automatizar o processo de criação das Ordens de Serviço (O.S.) diárias para cada unidade militar, a fim de otimizar recursos humanos e reduzir a necessidade de trabalho manual repetitivo. Atualmente, as O.S. são feitas de forma manual por 1 ou 2 militares de cada unidade, que precisam juntar a informação por meio de documentos correspondidos e publicá-los na O.S. após despacho e indicação do comandante da unidade.

O SIPOS irá futuramente utilizar Machine Learning para detectar e classificar documentos e, em seguida, inserir as informações em um formato reformulado, além de detectar informações específicas, como movimentos de militares e civis na unidade, transferências, escalas de serviço, louvores, punições, entre outros assuntos relacionados.


## Overview / Status atual do software (Beta 1.2.2)
O SIPOS v B-1.2.2 mantém o foco na interpretação de dados das folhas Excel e na flexibilidade do sistema, com validação recente de restauro e build para ambientes de desenvolvimento atualizados. Esta versão beta também prepara o projeto para empacotamento e distribuição como executável Windows portátil.

## Portable Windows build
A primeira base de compatibilidade portátil está documentada em `README-PORTABLE.md`. O caminho mais simples é fazer duplo clique em `Build-Portable.bat`, escolher ou aceitar a versão padrão, e usar o zip gerado em `artifacts`.

O perfil `Properties/PublishProfiles/win-x64-portable.pubxml` gera um build Windows x64, self-contained e single-file, e o helper `scripts/Publish-Portable.ps1` cria o zip portátil e o checksum SHA-256. O computador que gera o pacote precisa do .NET SDK; o executável gerado não precisa de runtime .NET separado.

O Microsoft Office continua a ser requisito para as funcionalidades que usam interoperabilidade com Word e Excel.

## Repository branch model
A branch `main` é a branch ativa/recomendada para o estado atual do projeto. A branch antiga `SIPOS_v0-8-3` ainda pode aparecer como default branch no GitHub até a configuração do repositório ser alterada, mas deve ser tratada apenas como nome legado da linha principal atual.

Branches de versão ou trabalho devem usar nomes próprios, como `release/beta-1.2.2`, `feature/<nome>`, ou `backup/<nome>`.

## Optics
Desenvolvido em C#, o SIPOS continua a evoluir com uma interface intuitiva e amigável, focando na facilidade de uso e eficiência para usuários de todos os níveis de experiência técnica.

## Planos
Futuras atualizações incluirão a integração de recursos de Machine Learning através de adaptações para Python e uso de bibliotecas como TensorFlow, visando ampliar as capacidades de análise e processamento de dados do sistema. O planeamento de release e a sincronização com Asana estão documentados em `PLANNER.md`.

