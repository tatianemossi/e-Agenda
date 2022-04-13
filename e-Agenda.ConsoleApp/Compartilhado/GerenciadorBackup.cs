using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace e_Agenda.ConsoleApp.Compartilhado
{
    public static class GerenciadorBackup<T>
    {
        const string CaminhoArquivo = @"C:\Backup - e_Agenda";

        public static List<T> CarregarDados()
        {
            if (!Directory.Exists(CaminhoArquivo))
                Directory.CreateDirectory(CaminhoArquivo);

            var caminhoCompletoArquivo = @$"{CaminhoArquivo}\Backup_{typeof(T).Name}.txt";

            if (!File.Exists(caminhoCompletoArquivo))
            {
                var arquivo =  File.Create(caminhoCompletoArquivo);
                arquivo.Close();
            }                

            string jsonDados = File.ReadAllText(caminhoCompletoArquivo);
            if (jsonDados.Length == 0)
                return new List<T>();
            else
                return JsonSerializer.Deserialize<List<T>>(jsonDados);
        }

        public static void SalvarDados(List<T> listaEntidade)
        {
            var jsonDados = JsonSerializer.Serialize(listaEntidade);

            var caminhoCompletoArquivo = @$"{CaminhoArquivo}\Backup_{typeof(T).Name}.txt";
            File.WriteAllText(caminhoCompletoArquivo, jsonDados);
        }
    }
}
