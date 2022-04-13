using e_Agenda.ConsoleApp.Compartilhado;
using e_Agenda.ConsoleApp.MóduloContato;
using e_Agenda.ConsoleApp.MóduloTarefa;
using System;

namespace e_Agenda.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Notificador notificador = new Notificador();
            TelaMenuPrincipal menuPrincipal = new TelaMenuPrincipal(notificador);

            while (true)
            {
                TelaBase telaSelecionada = menuPrincipal.ObterTela();

                if (telaSelecionada is null)
                    return;

                string opcaoSelecionada = telaSelecionada.MostrarOpcoes();

                if (telaSelecionada is TelaCadastroTarefa)
                    GerenciarCadastroTarefa(telaSelecionada, opcaoSelecionada);

                else if (telaSelecionada is TelaCadastroContato)
                    GerenciarCadastroContato(telaSelecionada, opcaoSelecionada);

            }
        }

        private static void GerenciarCadastroContato(TelaBase telaSelecionada, string opcaoSelecionada)
        {
            var telaCadastroContato = telaSelecionada as TelaCadastroContato;

            if (telaCadastroContato is null)
                return;

            if (opcaoSelecionada == "1")
                telaCadastroContato.Inserir();

            else if (opcaoSelecionada == "2")
                telaCadastroContato.Editar();

            else if (opcaoSelecionada == "3")
                telaCadastroContato.Excluir();

            else if (opcaoSelecionada == "4")
                telaCadastroContato.Visualizar("Tela");
        }

        public static void GerenciarCadastroTarefa(TelaBase telaSelecionada, string opcaoSelecionada)
        {
            var telaCadastroTarefa = telaSelecionada as TelaCadastroTarefa;

            if (telaCadastroTarefa is null)
                return;

            if (opcaoSelecionada == "1")
                telaCadastroTarefa.Inserir();

            else if (opcaoSelecionada == "2")
                telaCadastroTarefa.Editar();

            else if (opcaoSelecionada == "3")
                telaCadastroTarefa.Excluir();

            else if (opcaoSelecionada == "4")
                telaCadastroTarefa.Visualizar("Tela");

            else if (opcaoSelecionada == "5")
                telaCadastroTarefa.ConcluirSubtarefa();
        }
    }
}
