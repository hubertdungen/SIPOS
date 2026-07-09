namespace SIPOS
{
    /// <summary>
    /// Motor de deteção de feriados nacionais portugueses.
    ///
    /// Cobre os feriados fixos e os feriados móveis dependentes da Páscoa
    /// (calculada pelo algoritmo de computus gregoriano de Meeus/Jones/Butcher).
    /// A classe é deliberadamente independente de Windows Forms e de Office
    /// interop para poder ser testada isoladamente e reutilizada em qualquer
    /// contexto (motor de escalas, exportação Word, futura UI de calendário).
    ///
    /// Início do epic Asana "Detectar: Feriados [v B-1.3.0]".
    /// </summary>
    public static class Feriados
    {
        /// <summary>Tipo de feriado, para permitir à UI distinguir obrigatórios de facultativos.</summary>
        public enum TipoFeriado
        {
            /// <summary>Feriado nacional obrigatório.</summary>
            NacionalObrigatorio,
            /// <summary>Tolerância de ponto / feriado facultativo habitual (ex.: Carnaval).</summary>
            Facultativo
        }

        /// <summary>Descreve um feriado concreto num dado ano.</summary>
        public readonly struct Feriado
        {
            public Feriado(DateTime data, string nome, TipoFeriado tipo)
            {
                Data = data;
                Nome = nome;
                Tipo = tipo;
            }

            public DateTime Data { get; }
            public string Nome { get; }
            public TipoFeriado Tipo { get; }
        }

        /// <summary>
        /// Domingo de Páscoa do ano indicado, no calendário gregoriano.
        /// Algoritmo "Anonymous Gregorian" (Meeus/Jones/Butcher).
        /// </summary>
        public static DateTime DomingoDePascoa(int ano)
        {
            int a = ano % 19;
            int b = ano / 100;
            int c = ano % 100;
            int d = b / 4;
            int e = b % 4;
            int f = (b + 8) / 25;
            int g = (b - f + 1) / 3;
            int h = (19 * a + b - d - g + 15) % 30;
            int i = c / 4;
            int k = c % 4;
            int l = (32 + 2 * e + 2 * i - h - k) % 7;
            int m = (a + 11 * h + 22 * l) / 451;
            int mes = (h + l - 7 * m + 114) / 31;
            int dia = ((h + l - 7 * m + 114) % 31) + 1;
            return new DateTime(ano, mes, dia);
        }

        /// <summary>
        /// Devolve todos os feriados (obrigatórios e, opcionalmente, facultativos)
        /// do ano indicado, ordenados por data.
        /// </summary>
        public static IReadOnlyList<Feriado> DoAno(int ano, bool incluirFacultativos = false)
        {
            DateTime pascoa = DomingoDePascoa(ano);

            var lista = new List<Feriado>
            {
                // Feriados fixos nacionais
                new Feriado(new DateTime(ano, 1, 1),   "Ano Novo",                          TipoFeriado.NacionalObrigatorio),
                new Feriado(new DateTime(ano, 4, 25),  "Dia da Liberdade",                  TipoFeriado.NacionalObrigatorio),
                new Feriado(new DateTime(ano, 5, 1),   "Dia do Trabalhador",                TipoFeriado.NacionalObrigatorio),
                new Feriado(new DateTime(ano, 6, 10),  "Dia de Portugal",                   TipoFeriado.NacionalObrigatorio),
                new Feriado(new DateTime(ano, 8, 15),  "Assunção de Nossa Senhora",         TipoFeriado.NacionalObrigatorio),
                new Feriado(new DateTime(ano, 10, 5),  "Implantação da República",          TipoFeriado.NacionalObrigatorio),
                new Feriado(new DateTime(ano, 11, 1),  "Todos os Santos",                   TipoFeriado.NacionalObrigatorio),
                new Feriado(new DateTime(ano, 12, 1),  "Restauração da Independência",      TipoFeriado.NacionalObrigatorio),
                new Feriado(new DateTime(ano, 12, 8),  "Imaculada Conceição",               TipoFeriado.NacionalObrigatorio),
                new Feriado(new DateTime(ano, 12, 25), "Natal",                             TipoFeriado.NacionalObrigatorio),

                // Feriados móveis dependentes da Páscoa
                new Feriado(pascoa.AddDays(-2), "Sexta-feira Santa", TipoFeriado.NacionalObrigatorio),
                new Feriado(pascoa,             "Páscoa",            TipoFeriado.NacionalObrigatorio),
                new Feriado(pascoa.AddDays(60), "Corpo de Deus",     TipoFeriado.NacionalObrigatorio),
            };

            if (incluirFacultativos)
            {
                // Carnaval não é feriado obrigatório nacional, mas é tolerância de ponto habitual.
                lista.Add(new Feriado(pascoa.AddDays(-47), "Carnaval", TipoFeriado.Facultativo));
            }

            lista.Sort((x, y) => x.Data.CompareTo(y.Data));
            return lista;
        }

        /// <summary>
        /// Indica se a data é feriado nacional. Por omissão considera apenas
        /// feriados obrigatórios; passar <paramref name="incluirFacultativos"/>
        /// a true também considera o Carnaval.
        /// </summary>
        public static bool IsFeriado(DateTime data, bool incluirFacultativos = false)
        {
            return NomeFeriado(data, incluirFacultativos) != null;
        }

        /// <summary>
        /// Devolve o nome do feriado correspondente à data, ou null se não for feriado.
        /// </summary>
        public static string? NomeFeriado(DateTime data, bool incluirFacultativos = false)
        {
            DateTime dia = data.Date;
            foreach (Feriado f in DoAno(dia.Year, incluirFacultativos))
            {
                if (f.Data == dia)
                {
                    return f.Nome;
                }
            }
            return null;
        }

        /// <summary>
        /// Indica se a data é dia de descanso para efeitos de escala, isto é,
        /// fim-de-semana (sábado/domingo) ou feriado.
        /// </summary>
        public static bool IsDiaDeDescanso(DateTime data, bool incluirFacultativos = false)
        {
            DayOfWeek dow = data.DayOfWeek;
            if (dow == DayOfWeek.Saturday || dow == DayOfWeek.Sunday)
            {
                return true;
            }
            return IsFeriado(data, incluirFacultativos);
        }
    }
}
