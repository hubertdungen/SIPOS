using System.Reflection;

namespace SIPOS.Logic.Tests
{
    /// <summary>Executor mínimo: corre todos os métodos [Teste] e sai com código 1 se algum falhar.</summary>
    internal static class Program
    {
        private static int Main()
        {
            var testes = typeof(Program).Assembly.GetTypes()
                .SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                .Where(m => m.GetCustomAttribute<TesteAttribute>() != null)
                .OrderBy(m => m.DeclaringType!.Name).ThenBy(m => m.Name)
                .ToList();

            int falhas = 0;
            foreach (MethodInfo teste in testes)
            {
                string nome = $"{teste.DeclaringType!.Name}.{teste.Name}";
                try
                {
                    teste.Invoke(null, null);
                    Console.WriteLine($"  ok    {nome}");
                }
                catch (TargetInvocationException ex)
                {
                    falhas++;
                    Console.WriteLine($"  FALHA {nome}\n        {ex.InnerException?.Message}");
                }
            }

            Console.WriteLine($"\n{testes.Count - falhas}/{testes.Count} testes passaram.");
            return falhas == 0 ? 0 : 1;
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class TesteAttribute : Attribute { }

    internal static class Assert
    {
        public static void Igual<T>(T esperado, T obtido, string contexto = "")
        {
            if (!EqualityComparer<T>.Default.Equals(esperado, obtido))
            {
                throw new Exception($"{contexto} esperado <{Mostrar(esperado)}> mas foi <{Mostrar(obtido)}>");
            }
        }

        public static void Sequencia<T>(IEnumerable<T> esperado, IEnumerable<T> obtido, string contexto = "")
        {
            if (!esperado.SequenceEqual(obtido))
            {
                throw new Exception($"{contexto} esperado [{string.Join(", ", esperado)}] mas foi [{string.Join(", ", obtido)}]");
            }
        }

        public static void Verdade(bool condicao, string mensagem)
        {
            if (!condicao) { throw new Exception(mensagem); }
        }

        private static string Mostrar<T>(T valor)
        {
            return (valor?.ToString() ?? "null").Replace("\r", "⏎").Replace("\f", "⤓").Replace("\t", "→");
        }
    }
}
