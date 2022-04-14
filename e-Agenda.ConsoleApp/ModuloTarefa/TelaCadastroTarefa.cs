using e_Agenda.ConsoleApp.Compartilhado;
using e_Agenda.ConsoleApp.MóduloTarefa;
using System;
using System.Collections.Generic;
using System.Linq;

namespace e_Agenda.ConsoleApp.ModuloTarefa
{
    public class TelaCadastroTarefa : TelaBase, ITelaCadastravel
    {
        private readonly IRepositorio<Tarefa> _repositorioTarefa;
        private readonly Notificador _notificador;

        public TelaCadastroTarefa(IRepositorio<Tarefa> repositorioTarefa, Notificador notificador)
            : base("Cadastro de Tarefas")
        {
            _repositorioTarefa = repositorioTarefa;
            _notificador = notificador;
        }

        public override string MostrarOpcoes()
        {
            MostrarTitulo(Titulo);

            Console.WriteLine("Digite 1 para Inserir");
            Console.WriteLine("Digite 2 para Editar");
            Console.WriteLine("Digite 3 para Excluir");
            Console.WriteLine("Digite 4 para Visualizar");
            Console.WriteLine("Digite 5 para concluir Subtarefa");

            Console.WriteLine("Digite s para sair");

            string opcao = Console.ReadLine();

            return opcao;
        }

        public void Inserir()
        {
            MostrarTitulo("Inserindo nova Tarefa");

            Tarefa novaTarefa = ObterTarefa();

            _repositorioTarefa.Inserir(novaTarefa);

            _notificador.ApresentarMensagem("Tarefa cadastrada com sucesso!", TipoMensagemEnum.Sucesso);
        }

        public void Editar()
        {
            MostrarTitulo("Editando Tarefa");

            bool temTarefasCadastradas = _repositorioTarefa.ExistemRegistros();

            if (temTarefasCadastradas == false)
            {
                _notificador.ApresentarMensagem("Nenhuma tarefa cadastrada para editar.", TipoMensagemEnum.Atencao);
                return;
            }
            else
            {
                var tarefas = _repositorioTarefa.SelecionarTodos();
                foreach (var tarefa in tarefas)
                {
                    Console.WriteLine(tarefa.VisualizarResumo());
                }
            }

            int idTarefa = ObterId();

            Tarefa tarefaAtualizada = _repositorioTarefa.SelecionarRegistro(idTarefa);

            Console.Write("Digite o título: ");
            tarefaAtualizada.Titulo = Console.ReadLine();
            tarefaAtualizada.Prioridade = ObterPrioridade();

            bool conseguiuEditar = _repositorioTarefa.Editar(idTarefa, tarefaAtualizada);

            if (!conseguiuEditar)
                _notificador.ApresentarMensagem("Não foi possível editar.", TipoMensagemEnum.Erro);
            else
                _notificador.ApresentarMensagem("Tarefa editada com sucesso!", TipoMensagemEnum.Sucesso);
        }

        public void Excluir()
        {
            MostrarTitulo("Excluindo Tarefa");

            bool temTarefasRegistradas = Visualizar("Pesquisando");

            if (temTarefasRegistradas == false)
            {
                _notificador.ApresentarMensagem("Nenhuma tarefa cadastrada para excluir.", TipoMensagemEnum.Atencao);
                return;
            }

            int numeroTarefa = ObterId();

            bool conseguiuExcluir = _repositorioTarefa.Excluir(numeroTarefa);

            if (!conseguiuExcluir)
                _notificador.ApresentarMensagem("Não foi possível excluir.", TipoMensagemEnum.Erro);
            else
                _notificador.ApresentarMensagem("Tarefa excluída com sucesso!", TipoMensagemEnum.Sucesso);
        }

        public bool Visualizar(string tipoVisualizacao)
        {
            if (tipoVisualizacao == "Tela")
                MostrarTitulo("Visualização de Tarefas");

            List<Tarefa> tarefas = _repositorioTarefa.SelecionarTodos();

            if (tarefas.Count == 0)
            {
                _notificador.ApresentarMensagem("Nenhuma tarefa disponível.", TipoMensagemEnum.Atencao);
                return false;
            }

            MostrarTitulo("Tarefas Pendentes: ");
            foreach (var tarefa in tarefas.Where(x => x.Concluida == false).OrderBy(x => x.Prioridade))
            {
                Console.WriteLine(tarefa.ToString());
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write(tarefa.ListaSubtarefasToString());
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine();
            }

            MostrarTitulo("Tarefas Concluídas: ", false);
            foreach (var tarefa in tarefas.Where(x => x.Concluida).OrderBy(x => x.Prioridade))
            {
                Console.WriteLine(tarefa.ToString());
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(tarefa.ListaSubtarefasToString());
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine();
            }

            Console.ReadLine();

            return true;
        }

        public void ConcluirSubtarefa()
        {
            Visualizar("Tela");
            Console.WriteLine("Digite o Id da Tarefa: ");
            var idTarefaSelecionado = Convert.ToInt32(Console.ReadLine());
            var tarefa = _repositorioTarefa.SelecionarRegistro(idTarefaSelecionado);

            Console.WriteLine(tarefa.ListaSubtarefasToString());

            Console.WriteLine("Digite o Id da Subtarefa que deseja concluir: ");
            var idSubarefaSelecionado = Convert.ToInt32(Console.ReadLine());

            tarefa.ConcluirSubtarefa(idSubarefaSelecionado);

            tarefa.AtualizarPorcentagem();

            _repositorioTarefa.SalvarDados();

        }

        public int ObterId()
        {
            int numero;
            bool numeroEncontrado;

            do
            {
                Console.Write("Digite o ID da tarefa que deseja editar: ");
                numero = Convert.ToInt32(Console.ReadLine());

                numeroEncontrado = _repositorioTarefa.ExisteRegistro(numero);

                if (numeroEncontrado == false)
                    _notificador.ApresentarMensagem("ID da tarefa não foi encontrado, digite novamente", TipoMensagemEnum.Atencao);

            } while (numeroEncontrado == false);

            return numero;
        }

        public Tarefa ObterTarefa()
        {
            Tarefa tarefa = new Tarefa();

            Console.Write("Digite o título: ");
            tarefa.Titulo = Console.ReadLine();
            tarefa.Prioridade = ObterPrioridade();
            PreencherSubtarefas(tarefa);

            return tarefa;
        }

        private void PreencherSubtarefas(Tarefa tarefa)
        {
            Console.Write("Digite a descrição da Subtarefa:");
            string descricao = Console.ReadLine();

            Subtarefa subtarefa = new Subtarefa(tarefa.ObterIdSubtarefaDisponivel(), descricao);
            tarefa.Subtarefas.Add(subtarefa);

            _notificador.ApresentarMensagem("Subtarefa adicionada", TipoMensagemEnum.Sucesso);

            Console.WriteLine("Digite 1. Adicionar outra subtarefa - Digite 2. Para encerrar");
            string opcaoSelecionada = Console.ReadLine();

            while (opcaoSelecionada == "1")
            {
                Console.Write("Digite a descrição da Subtarefa:");
                descricao = Console.ReadLine();

                var subtarefaNova = new Subtarefa(tarefa.ObterIdSubtarefaDisponivel(), descricao);
                tarefa.Subtarefas.Add(subtarefaNova);

                _notificador.ApresentarMensagem("Subtarefa adicionada", TipoMensagemEnum.Sucesso);

                Console.WriteLine("Digite 1. Adicionar outra subtarefa - Digite 2. Para encerrar");
                opcaoSelecionada = Console.ReadLine();
            }

        }

        private PrioridadeEnum ObterPrioridade()
        {
            Console.Write("Digite a prioridade (1. Alta, 2. Normal, 3. Baixa):");
            string opcaoSelecionada = Console.ReadLine();

            while (opcaoSelecionada != "1" && opcaoSelecionada != "2" && opcaoSelecionada != "3")
            {
                _notificador.ApresentarMensagem("Opção inválida", TipoMensagemEnum.Atencao);

                Console.Write("Digite a prioridade (1. Alta, 2. Normal, 3. Baixa):");
                opcaoSelecionada = Console.ReadLine();
            }

            switch (opcaoSelecionada)
            {
                case "1":
                    return PrioridadeEnum.Alta;

                case "2":
                    return PrioridadeEnum.Normal;

                case "3":
                    return PrioridadeEnum.Baixa;

                default:
                    return PrioridadeEnum.Normal;
            }
        }
    }
}
