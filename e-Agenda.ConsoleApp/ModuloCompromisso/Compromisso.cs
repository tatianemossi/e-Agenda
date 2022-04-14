using e_Agenda.ConsoleApp.Compartilhado;
using e_Agenda.ConsoleApp.MóduloContato;
using System;
using System.Text.RegularExpressions;

namespace e_Agenda.ConsoleApp.ModuloCompromisso
{
    public class Compromisso : EntidadeBase
    {
        public string Assunto { get; set; }
        public string Local { get; set; }
        public DateTime Data { get; set; }
        public string HoraInicio { get; set; }
        public string HoraTermino { get; set; }
        public Contato Contato { get; set; }

        public override string ToString()
        {
            return "Id: " + Id + Environment.NewLine +
            "Local: " + Local + Environment.NewLine +
            "Data: " + Data + Environment.NewLine +
            "Hora de Início: " + HoraInicio + Environment.NewLine +
            "Hora de Término: " + HoraTermino + Environment.NewLine +
            "Contato: " + Contato?.Nome + "- Cargo: " + Contato?.Cargo;
        }

        public void AtualizarDataComHoraMinuto()
        {
            var hora = Convert.ToInt32(HoraInicio.Split(":")[0]);
            var minuto = Convert.ToInt32(HoraInicio.Split(":")[1]);
            Data = new DateTime(Data.Year, Data.Month, Data.Day, hora, minuto, 0);
        }
    }
}
