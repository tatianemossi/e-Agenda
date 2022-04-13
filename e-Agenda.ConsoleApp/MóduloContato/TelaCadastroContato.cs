using e_Agenda.ConsoleApp.Compartilhado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace e_Agenda.ConsoleApp.MóduloContato
{
    public class TelaCadastroContato : TelaBase, ITelaCadastravel
    {
        private readonly IRepositorio<Contato> _repositorioContato;
        private readonly Notificador _notificador;

        public TelaCadastroContato(IRepositorio<Contato> repositorioContato, Notificador notificador)
            : base("Cadastro de Contatos")
        {
            _repositorioContato = repositorioContato;
            _notificador = notificador;
        }
        

        public void Inserir()
        {
            MostrarTitulo("Inserindo novo Contato");

            Contato novoContato = ObterContato();

            _repositorioContato.Inserir(novoContato);

            _notificador.ApresentarMensagem("Contato cadastrado com sucesso!", TipoMensagemEnum.Sucesso);
        }

        public void Editar()
        {
            MostrarTitulo("Editando Contato");

            bool temContatosCadastrados = _repositorioContato.ExistemRegistros();

            if (temContatosCadastrados == false)
            {
                _notificador.ApresentarMensagem("Nenhum Contato cadastrado para editar.", TipoMensagemEnum.Atencao);
                return;
            }
            else
            {
                var Contatos = _repositorioContato.SelecionarTodos();
                foreach (var Contato in Contatos)
                {
                    Console.WriteLine(Contato.ToString());
                }
            }

            int idContato = ObterId();

            Contato ContatoAtualizado = ObterContato();

            bool conseguiuEditar = _repositorioContato.Editar(idContato, ContatoAtualizado);

            if (!conseguiuEditar)
                _notificador.ApresentarMensagem("Não foi possível editar.", TipoMensagemEnum.Erro);
            else
                _notificador.ApresentarMensagem("Contato editado com sucesso!", TipoMensagemEnum.Sucesso);
        }

        public void Excluir()
        {
            MostrarTitulo("Excluindo Contato");

            bool temContatosRegistrados = Visualizar("Pesquisando");

            if (temContatosRegistrados == false)
            {
                _notificador.ApresentarMensagem("Nenhum Contato cadastrada para excluir.", TipoMensagemEnum.Atencao);
                return;
            }

            int numeroContato = ObterId();

            bool conseguiuExcluir = _repositorioContato.Excluir(numeroContato);

            if (!conseguiuExcluir)
                _notificador.ApresentarMensagem("Não foi possível excluir.", TipoMensagemEnum.Erro);
            else
                _notificador.ApresentarMensagem("Contato excluído com sucesso!", TipoMensagemEnum.Sucesso);
        }

        public bool Visualizar(string tipoVisualizacao)
        {
            if (tipoVisualizacao == "Tela")
                MostrarTitulo("Visualização de Contatos");

            List<Contato> contatos = _repositorioContato.SelecionarTodos();

            if (contatos.Count == 0)
            {
                _notificador.ApresentarMensagem("Nenhum Contato disponível.", TipoMensagemEnum.Atencao);
                return false;
            }

            contatos = contatos.OrderBy(x => x.Cargo).ToList();

            var cargoAgrupador = contatos.First().Cargo;

            MostrarTitulo("Contatos: ");
            Console.WriteLine($"Cargo: {cargoAgrupador}");
            foreach (var contato in contatos)
            {
                if (contato.Cargo != cargoAgrupador)
                {
                    cargoAgrupador = contato.Cargo;
                    Console.WriteLine($"Cargo: {cargoAgrupador}");
                }

                Console.WriteLine(contato.ToString());
                Console.WriteLine();
                Console.WriteLine();
            }

            Console.ReadLine();

            return true;
        }


        public int ObterId()
        {
            int id;
            bool idEncontrado;

            do
            {
                Console.Write("Digite o ID do Contato que deseja editar: ");
                id = Convert.ToInt32(Console.ReadLine());

                idEncontrado = _repositorioContato.ExisteRegistro(id);

                if (idEncontrado == false)
                    _notificador.ApresentarMensagem("ID do Contato não foi encontrado, digite novamente", TipoMensagemEnum.Atencao);

            } while (idEncontrado == false);

            return id;
        }

        public Contato ObterContato()
        {
            Contato contato = new Contato();

            Console.Write("Digite o Nome: ");
            contato.Nome = Console.ReadLine();

            Console.Write("Digite o Email: ");
            var email = Console.ReadLine();
            var emailEhValido = contato.ValidarEmail(email);

            while (!emailEhValido)
            {
                Console.WriteLine("Email inválido, por favor, insira um Email válido");
                email = Console.ReadLine();
                emailEhValido = contato.ValidarEmail(email);
            }

            contato.Email = email;

            Console.Write("Digite o Telefone (99 999999999): ");
            var telefone = Console.ReadLine();
            var telefoneEhValido = contato.ValidarTelefone(telefone);

            while (!telefoneEhValido)
            {
                Console.WriteLine("Telefone inválido, por favor, insira um Telefone válido (99 999999999):");
                telefone = Console.ReadLine();
                telefoneEhValido = contato.ValidarTelefone(telefone);
            }

            contato.Telefone = telefone;

            Console.Write("Digite a Empresa: ");
            contato.Empresa = Console.ReadLine();

            Console.Write("Digite o Cargo: ");
            contato.Cargo = Console.ReadLine();

            return contato;
        }        
    }
}
