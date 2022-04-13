using e_Agenda.ConsoleApp.Compartilhado;
using System;
using System.Collections.Generic;
using System.Linq;

namespace e_Agenda.ConsoleApp.MóduloTarefa
{
    public class Tarefa : EntidadeBase
    {
        public string Titulo { get; set; }
        public DateTime DataCriacao { get; }
        public DateTime? DataConclusao { get; set; }
        public decimal PercentualConcluido { get; set; }
        public PrioridadeEnum Prioridade { get; set; }
        public bool Concluida { get; set; }
        public List<Subtarefa> Subtarefas { get; set; }

        public Tarefa()
        {
            Subtarefas = new List<Subtarefa>();
            DataCriacao = DateTime.Now;
            Concluida = false;
            PercentualConcluido = 0;
        }

        public override string ToString()
        {
            return "Id: " + Id + Environment.NewLine +
            "Título: " + Titulo + Environment.NewLine +
            "Prioridade: " + Prioridade + Environment.NewLine +
            "Criação: " + DataCriacao + Environment.NewLine +
            "Percentual Concluído: " + PercentualConcluido + "%" + Environment.NewLine +
            "Data da Conclusão: " + DataConclusao;
        }

        public int ObterIdSubtarefaDisponivel()
        {
            var ultimoId = Subtarefas.OrderBy(x => x.Id).Select(x => x.Id).LastOrDefault();

            return ++ultimoId;
        }

        public string ListaSubtarefasToString()
        {
            if (Subtarefas == null || Subtarefas.Count == 0)
            {
                return "Nenhuma subtarefa cadastrada.";
            }

            var textoSubtarefas = "Subtarefas: ";

            if (Subtarefas != null)
            {
                foreach (var subtarefa in Subtarefas)
                {
                    var status = subtarefa.Concluida ? "Concluída" : "Pendente";
                    textoSubtarefas = $"{textoSubtarefas}{Environment.NewLine} Id: {subtarefa.Id} - {subtarefa.Descricao} - {status}";
                }
            }

            return textoSubtarefas;
        }

        public void ConcluirSubtarefa(int idSubtarefa)
        {
            var subtarefa = Subtarefas.FirstOrDefault(x => x.Id == idSubtarefa);

            subtarefa.Concluida = true;
        }

        public void AtualizarPorcentagem()
        {
            var quantidadeSubtarefasConcluidas = Subtarefas.Count(x => x.Concluida);
            PercentualConcluido = (quantidadeSubtarefasConcluidas * 100) / Subtarefas.Count;

            if (quantidadeSubtarefasConcluidas == Subtarefas.Count)
            { 
                Concluida = true;
                DataConclusao = DateTime.Now;
            }
        }
    }
}
