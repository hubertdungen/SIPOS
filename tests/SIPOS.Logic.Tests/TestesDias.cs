namespace SIPOS.Logic.Tests
{
    internal static class TestesDias
    {
        private static DateTime D(int ano, int mes, int dia) => new DateTime(ano, mes, dia);

        [Teste]
        private static void DiaUtil_CobreSoODiaSeguinte()
        {
            Assert.Sequencia(new[] { D(2022, 10, 13) }, PlaneadorDeDias.DiasDeEscala(D(2022, 10, 12)));
        }

        // Exemplar 2022-002-186: O.S. de sexta 30SET2022 → 01, 02 e 03OUT
        [Teste]
        private static void Sexta_CobreFimDeSemanaESegunda()
        {
            Assert.Sequencia(new[] { D(2022, 10, 1), D(2022, 10, 2), D(2022, 10, 3) }, PlaneadorDeDias.DiasDeEscala(D(2022, 9, 30)));
        }

        // Exemplar 2022-002-188: O.S. de terça 04OUT2022 → 05OUT (feriado) e 06OUT
        [Teste]
        private static void VesperaDeFeriado_CobreFeriadoEDiaUtilSeguinte()
        {
            Assert.Sequencia(new[] { D(2022, 10, 5), D(2022, 10, 6) }, PlaneadorDeDias.DiasDeEscala(D(2022, 10, 4)));
        }

        // Quinta-feira antes da Páscoa de 2026: Sexta-feira Santa, sábado, Páscoa e segunda
        [Teste]
        private static void Pascoa_CobreSextaSantaAteSegunda()
        {
            Assert.Sequencia(new[] { D(2026, 4, 3), D(2026, 4, 4), D(2026, 4, 5), D(2026, 4, 6) }, PlaneadorDeDias.DiasDeEscala(D(2026, 4, 2)));
        }

        // Modo início/fim: o 1.º dia é o da O.S.; sexta→segunda cobre sábado a segunda
        [Teste]
        private static void Intervalo_ComecaNoDiaSeguinteAoDaOS()
        {
            List<DateTime> dias = PlaneadorDeDias.DiasParaExportacao(D(2022, 9, 30), true, D(2022, 9, 30), D(2022, 10, 3));
            Assert.Sequencia(new[] { D(2022, 10, 1), D(2022, 10, 2), D(2022, 10, 3) }, dias);
        }

        // Modo ligado com um só dia selecionado, ou desligado: regra automática
        [Teste]
        private static void IntervaloDeUmDiaOuDesligado_UsaARegraAutomatica()
        {
            List<DateTime> esperado = PlaneadorDeDias.DiasDeEscala(D(2022, 10, 4));
            Assert.Sequencia(esperado, PlaneadorDeDias.DiasParaExportacao(D(2022, 10, 4), true, D(2022, 10, 4), D(2022, 10, 4)));
            Assert.Sequencia(esperado, PlaneadorDeDias.DiasParaExportacao(D(2022, 10, 4), false, D(2022, 9, 1), D(2022, 9, 30)));
        }

        [Teste]
        private static void DescreverDias_ComNomeDoFeriado()
        {
            string texto = PlaneadorDeDias.DescreverDias(new[] { D(2022, 10, 5), D(2022, 10, 6) });

            Assert.Verdade(texto.Contains("05/10/2022 (Implantação da República)"), texto);
            Assert.Verdade(texto.Contains(", ") && texto.Contains("06/10/2022") && !texto.EndsWith(")"), texto);
        }
    }
}
