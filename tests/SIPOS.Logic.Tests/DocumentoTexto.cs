using System.Text;

namespace SIPOS.Logic.Tests
{
    /// <summary>
    /// Documento simulado para o ExecutorModelar: o texto principal de um
    /// documento Word, com '\r' a separar parágrafos e a marca de parágrafo
    /// final no fim (como Document.Content.Text). As operações reproduzem o
    /// que o adaptador Word faz: inserir um ficheiro inteiro numa posição,
    /// inserir uma quebra de página ('\f') e substituir tags numa zona.
    /// </summary>
    internal sealed class DocumentoTexto : IDocumentoModelar
    {
        private readonly StringBuilder texto;
        private readonly Dictionary<string, string> ficheiros;

        public DocumentoTexto(string textoInicial, Dictionary<string, string> ficheiros)
        {
            texto = new StringBuilder(textoInicial);
            this.ficheiros = ficheiros;
        }

        public string Texto => texto.ToString();

        /// <summary>Quantas vezes cada ficheiro foi aberto para ver se tem bloco de escala.</summary>
        public Dictionary<string, int> VerificacoesDeBloco { get; } = new Dictionary<string, int>();

        public int FimDoTexto => texto.Length - 1;

        public int InicioDoBlocoPorPreencher()
        {
            int i = Texto.IndexOf("<dataEscalados>", StringComparison.Ordinal);
            return i < 0 ? -1 : InicioDoParagrafo(i);
        }

        public int PontoDeInsercao(int aPartirDe)
        {
            string t = Texto;
            int marcador = t.IndexOf("<fimEscalas>", aPartirDe, StringComparison.Ordinal);
            if (marcador >= 0)
            {
                int inicio = InicioDoParagrafo(marcador);
                int fim = t.IndexOf('\r', marcador) + 1;
                texto.Remove(inicio, fim - inicio);
                return inicio;
            }

            int procura = aPartirDe;
            int titulo;
            while ((titulo = t.IndexOf("102.", procura, StringComparison.Ordinal)) >= 0)
            {
                int inicio = InicioDoParagrafo(titulo);
                if (string.IsNullOrWhiteSpace(t.Substring(inicio, titulo - inicio)))
                {
                    return inicio;
                }
                procura = titulo + 4;
            }
            return FimDoTexto;
        }

        public bool FicheiroTemBlocoDeEscala(string caminho)
        {
            VerificacoesDeBloco[caminho] = VerificacoesDeBloco.GetValueOrDefault(caminho) + 1;
            return ficheiros[caminho].Contains("<dataEscalados>");
        }

        public int InserirFicheiro(int posicao, string caminho)
        {
            int antes = texto.Length;
            int p = posicao;
            if (p > 0 && texto[p - 1] != '\r')
            {
                texto.Insert(p, "\r");
                p++;
            }
            texto.Insert(p, ficheiros[caminho]);
            return texto.Length - antes;
        }

        public int InserirQuebraDePagina(int posicao)
        {
            texto.Insert(posicao, "\f");
            return 1;
        }

        public int SubstituirVariaveis(int inicio, int fim, DateTime? dia)
        {
            DateTime d = dia ?? new DateTime(2000, 1, 1);
            string dd = d.ToString("dd/MM");

            string zona = texto.ToString(inicio, fim - inicio);
            string nova = zona
                .Replace("<dataEscalados>", dd)
                .Replace("<ODUefectivo>", $"ODU[{dd}]")
                .Replace("<ODUptpd>", "")
                .Replace("<ODUadapt>", "")
                .Replace("<ODUstatus>", "")
                .Replace("<ODUreserva>", $"RES-ODU[{dd}]")
                .Replace("<numOS>", "123");

            // OAF só às quartas, como no Word_Processor
            if (d.DayOfWeek == DayOfWeek.Wednesday)
            {
                nova = nova.Replace("<OAFefectivo>", $"OAF[{dd}]")
                           .Replace("<OAFptpd>", "")
                           .Replace("<OAFadapt>", "")
                           .Replace("<OAFstatus>", "")
                           .Replace("<OAFreserva>", $"RES-OAF[{dd}]");
            }

            texto.Remove(inicio, fim - inicio);
            texto.Insert(inicio, nova);
            return nova.Length - zona.Length;
        }

        private int InicioDoParagrafo(int posicao)
        {
            int anterior = posicao == 0 ? -1 : Texto.LastIndexOf('\r', posicao - 1);
            return anterior + 1;
        }
    }
}
