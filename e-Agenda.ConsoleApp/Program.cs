using e_Agenda.ConsoleApp.Compartilhado;
using e_Agenda.ConsoleApp.MóduloTarefa;

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

                if (telaSelecionada is ITelaCadastravel)
                    GerenciarCadastroTarefa(telaSelecionada, opcaoSelecionada);
            }
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
