using e_Agenda.ConsoleApp.Compartilhado;
using e_Agenda.ConsoleApp.MóduloContato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace e_Agenda.ConsoleApp.ModuloCompromisso
{
    public class TelaCadastroCompromisso : TelaBase, ITelaCadastravel
    {
        private readonly IRepositorio<Compromisso> _repositorioCompromisso;
        private readonly IRepositorio<Contato> _repositorioContato;
        private readonly TelaCadastroContato _telaCadastroContato;
        private readonly Notificador _notificador;

        public TelaCadastroCompromisso(IRepositorio<Compromisso> repositorioCompromisso, IRepositorio<Contato> repositorioContato,
            TelaCadastroContato telaCadastroContato, Notificador notificador)
            : base("Cadastro de Compromissos")
        {
            _repositorioCompromisso = repositorioCompromisso;
            _notificador = notificador;
            _repositorioContato = repositorioContato;
            _telaCadastroContato = telaCadastroContato;
        }

        public override string MostrarOpcoes()
        {
            MostrarTitulo(Titulo);

            Console.WriteLine("Digite 1 para Inserir");
            Console.WriteLine("Digite 2 para Editar");
            Console.WriteLine("Digite 3 para Excluir");
            Console.WriteLine("Digite 4 para Visualizar");
            Console.WriteLine("Digite 5 para Filtrar Compromissos");

            Console.WriteLine("Digite s para sair");

            string opcao = Console.ReadLine();

            return opcao;
        }

        public void Inserir()
        {
            MostrarTitulo("Inserindo novo Compromisso");

            Compromisso novoCompromisso = ObterCompromisso();

            _repositorioCompromisso.Inserir(novoCompromisso);

            _notificador.ApresentarMensagem("Compromisso cadastrado com sucesso!", TipoMensagemEnum.Sucesso);
        }

        public void Editar()
        {
            MostrarTitulo("Editando Compromisso");

            bool temCompromissosCadastrados = _repositorioCompromisso.ExistemRegistros();

            if (temCompromissosCadastrados == false)
            {
                _notificador.ApresentarMensagem("Nenhum Compromisso cadastrado para editar.", TipoMensagemEnum.Atencao);
                return;
            }
            else
            {
                var Compromissos = _repositorioCompromisso.SelecionarTodos();
                foreach (var Compromisso in Compromissos)
                {
                    Console.WriteLine(Compromisso.ToString());
                }
            }

            int idCompromisso = ObterId();

            Compromisso CompromissoAtualizado = ObterCompromisso();

            bool conseguiuEditar = _repositorioCompromisso.Editar(idCompromisso, CompromissoAtualizado);

            if (!conseguiuEditar)
                _notificador.ApresentarMensagem("Não foi possível editar.", TipoMensagemEnum.Erro);
            else
                _notificador.ApresentarMensagem("Compromisso editado com sucesso!", TipoMensagemEnum.Sucesso);
        }

        public void Excluir()
        {
            MostrarTitulo("Excluindo Compromisso");

            bool temCompromissosRegistrados = Visualizar("Pesquisando");

            if (temCompromissosRegistrados == false)
            {
                _notificador.ApresentarMensagem("Nenhum Compromisso cadastrado para excluir.", TipoMensagemEnum.Atencao);
                return;
            }

            int idCompromisso = ObterId();

            bool conseguiuExcluir = _repositorioCompromisso.Excluir(idCompromisso);

            if (!conseguiuExcluir)
                _notificador.ApresentarMensagem("Não foi possível excluir.", TipoMensagemEnum.Erro);
            else
                _notificador.ApresentarMensagem("Compromisso excluído com sucesso!", TipoMensagemEnum.Sucesso);
        }

        public bool Visualizar(string tipoVisualizacao)
        {
            if (tipoVisualizacao == "Tela")
                MostrarTitulo("Visualização de Compromissos");

            List<Compromisso> compromissos = _repositorioCompromisso.SelecionarTodos();

            if (compromissos.Count == 0)
            {
                _notificador.ApresentarMensagem("Nenhum Compromisso disponível.", TipoMensagemEnum.Atencao);
                return false;
            }

            var compromissosPassados = new List<Compromisso>();
            var compromissosFuturos = new List<Compromisso>();

            foreach (var compromisso in compromissos)
            {
                var hora = Convert.ToInt32(compromisso.HoraInicio.Split(":")[0]);
                var minuto = Convert.ToInt32(compromisso.HoraInicio.Split(":")[1]);

                var dataHoraCompromisso = new DateTime(compromisso.Data.Year, compromisso.Data.Month, compromisso.Data.Day, hora, minuto, 0);

                if (dataHoraCompromisso > DateTime.Now)
                    compromissosFuturos.Add(compromisso);
                else
                    compromissosPassados.Add(compromisso);
            }

            MostrarCompromissos(compromissosPassados, "passados");

            MostrarCompromissos(compromissosFuturos, "futuros");

            Console.ReadLine();

            return true;
        }

        private void MostrarCompromissos(List<Compromisso> compromissos, string tempoCompromissos)
        {
            Console.WriteLine();
            MostrarTitulo($"Compromissos {tempoCompromissos}: ", false);

            if (compromissos.Count == 0)
            {
                _notificador.ApresentarMensagem($"Não existem compromissos {tempoCompromissos}.", TipoMensagemEnum.Atencao, false);
            }
            else
            {
                foreach (var compromisso in compromissos)
                {
                    Console.WriteLine(compromisso.ToString());
                }
            }
        }

        public int ObterId()
        {
            int id;
            bool idEncontrado;

            do
            {
                Console.Write("Digite o ID do Compromisso que deseja editar: ");
                id = Convert.ToInt32(Console.ReadLine());

                idEncontrado = _repositorioCompromisso.ExisteRegistro(id);

                if (idEncontrado == false)
                    _notificador.ApresentarMensagem("ID do Compromisso não foi encontrado, digite novamente", TipoMensagemEnum.Atencao);

            } while (idEncontrado == false);

            return id;
        }

        public Compromisso ObterCompromisso()
        {
            Compromisso compromisso = new Compromisso();

            Console.Write("Digite o Assunto: ");
            compromisso.Assunto = Console.ReadLine();

            Console.Write("Digite o Local: ");
            compromisso.Local = Console.ReadLine();

            compromisso.Data = ObterData("do Compromisso");

            compromisso.HoraInicio = ObterHora("Horário de início: ");

            compromisso.HoraTermino = ObterHora("Horário de término: ");

            compromisso.AtualizarDataComHoraMinuto();

            compromisso.Contato = ObterContato();

            return compromisso;
        }

        private Contato ObterContato()
        {
            bool temContatosDisponiveis = _telaCadastroContato.Visualizar("Pesquisando");

            if (!temContatosDisponiveis)
            {
                _notificador.ApresentarMensagem("Não há nenhum contato disponível para cadastrar compromissos.", TipoMensagemEnum.Atencao);
                return null;
            }

            Console.Write("Digite o Id do contato para adicionar ao compromisso: ");
            int idContato = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine();

            Contato contatoSelecionado = _repositorioContato.SelecionarRegistro(idContato);

            return contatoSelecionado;
        }

        private string ObterHora(string mensagem)
        {
            Console.WriteLine($"Hora {mensagem}: ");
            string hora = Console.ReadLine();
            var horaEhValida = ValidarHora(hora);

            while (!horaEhValida)
            {
                Console.WriteLine("Horário inválido, por favor, insira um horário válido");
                hora = Console.ReadLine();
                horaEhValida = ValidarHora(hora);
            }

            return hora;
        }

        public bool ValidarHora(string hora)
        {
            Regex rg = new Regex(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$");

            if (rg.IsMatch(hora))
                return true;

            return false;
        }

        private DateTime ObterData(string mensagem)
        {
            Console.WriteLine($"Data {mensagem} (--/--/----):");
            string dataDigitada = Console.ReadLine();

            DateTime dataConvertida;
            while (!DateTime.TryParse(dataDigitada, out dataConvertida))
            {
                Console.WriteLine("Insira uma data válida:");
                dataDigitada = Console.ReadLine();
            }

            return dataConvertida;
        }

        public void FiltrarCompromissos()
        {
            MostrarTitulo("Filtrar Compromissos por Período: ");

            var dataInicio = ObterData("de Início da pesquisa:");
            var horaMinutoInicio = ObterHora("de início da pesquisa");

            var dataFinal = ObterData("final da pesquisa:");
            var horaMinutoFinal = ObterHora("final da pesquisa");

            var horaInicio = Convert.ToInt32(horaMinutoInicio.Split(":")[0]);
            var minutoInicio = Convert.ToInt32(horaMinutoInicio.Split(":")[1]);
            var dataHoraInicio = new DateTime(dataInicio.Year, dataInicio.Month, dataInicio.Day, horaInicio, minutoInicio, 0);

            var horaFinal = Convert.ToInt32(horaMinutoFinal.Split(":")[0]);
            var minutoFinal = Convert.ToInt32(horaMinutoFinal.Split(":")[1]);
            var dataHoraFinal = new DateTime(dataFinal.Year, dataFinal.Month, dataFinal.Day, horaFinal, minutoFinal, 0);

            List<Compromisso> compromissos = _repositorioCompromisso.SelecionarTodos();

            var compromissosFiltrados = compromissos.Where(x => x.Data >= dataHoraInicio && x.Data <= dataHoraFinal).ToList();

            if (compromissosFiltrados.Count == 0)
            {
                _notificador.ApresentarMensagem("Não existem dados para a pesquisa.", TipoMensagemEnum.Atencao);
            }
            else
            {
                foreach (var compromisso in compromissosFiltrados)
                {
                    Console.WriteLine(compromisso.ToString());
                    Console.WriteLine();
                }
            }            

            Console.ReadLine();
        }
    }
}
