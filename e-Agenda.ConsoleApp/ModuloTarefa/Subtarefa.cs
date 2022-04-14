using e_Agenda.ConsoleApp.Compartilhado;
using System;

namespace e_Agenda.ConsoleApp.MóduloTarefa
{
    public class Subtarefa : EntidadeBase
    {
        public Subtarefa(int id, string descricao)
        {
            Descricao = descricao;
            Id = id;
        }

        public string Descricao { get; set; }

        public bool Concluida { get; set; }

        public override string ToString()
        {
            return "Id: " + Id + Environment.NewLine +
            "Descrição: " + Descricao + Environment.NewLine;
        }
    }
}
