using e_Agenda.ConsoleApp.MóduloContato;
using e_Agenda.ConsoleApp.MóduloTarefa;
using System;

namespace e_Agenda.ConsoleApp.Compartilhado
{
    public class TelaMenuPrincipal
    {
        private RepositorioTarefa _repositorioTarefa;
        private TelaCadastroTarefa _telaCadastroTarefa;

        private RepositorioContato _repositorioContato;
        private TelaCadastroContato _telaCadastroContato;


        public TelaMenuPrincipal(Notificador notificador)
        {
            _repositorioTarefa = new RepositorioTarefa();
            _telaCadastroTarefa = new TelaCadastroTarefa(_repositorioTarefa, notificador);

            _repositorioContato = new RepositorioContato();
            _telaCadastroContato = new TelaCadastroContato(_repositorioContato, notificador);
        }

        public string MostrarOpcoes()
        {
            MostrarTitulo("Aplicativo e_Agenda 1.0");

            Console.WriteLine("Digite 1 para Gerenciar Tarefas");

            Console.WriteLine("Digite 2 para Gerenciar Contatos");

            Console.WriteLine("Digite s para sair");

            string opcaoSelecionada = Console.ReadLine();

            return opcaoSelecionada;
        }

        public TelaBase ObterTela()
        {
            string opcao = MostrarOpcoes();

            TelaBase tela = null;

            if (opcao == "1")
                tela = _telaCadastroTarefa;

            else if (opcao == "2")
                tela = _telaCadastroContato;

            return tela;
        }
        public void MostrarTitulo(string titulo)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Magenta;

            Console.WriteLine(titulo);

            Console.ResetColor();

            Console.WriteLine();
        }
    }
}
