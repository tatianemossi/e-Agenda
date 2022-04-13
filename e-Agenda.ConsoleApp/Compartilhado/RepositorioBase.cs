using System;
using System.Collections.Generic;
using System.Linq;

namespace e_Agenda.ConsoleApp.Compartilhado
{
    public class RepositorioBase<T> : IRepositorio<T> where T : EntidadeBase
    {
        protected readonly List<T> registros;

        public RepositorioBase()
        {
            registros = GerenciadorBackup<T>.CarregarDados();
        }

        public virtual void Inserir(T entidade)
        {
            entidade.Id = BuscarUltimoIdDisponivel();

            registros.Add(entidade);

            SalvarDados();
        }

        bool IRepositorio<T>.Editar(int idSelecionado, T novaEntidade)
        {
            foreach (T entidade in registros)
            {
                if (idSelecionado == entidade.Id)
                {
                    novaEntidade.Id = entidade.Id;

                    int posicaoParaEditar = registros.IndexOf(entidade);
                    registros[posicaoParaEditar] = novaEntidade;

                    return true;
                }
            }

            return false;
        }

        public bool Excluir(int idSelecionado)
        {
            foreach (T entidade in registros)
            {
                if (idSelecionado == entidade.Id)
                {
                    registros.Remove(entidade);
                    return true;
                }
            }
            return false;
        }

        public T SelecionarRegistro(int idSelecionado)
        {
            return registros.Find(x => x.Id == idSelecionado);
        }

        public List<T> SelecionarTodos()
        {
            return registros;
        }

        public bool ExisteRegistro(int idSelecionado)
        {
            foreach (T entidade in registros)
                if (idSelecionado == entidade.Id)
                    return true;

            return false;
        }

        private int BuscarUltimoIdDisponivel()
        {
            var id = registros.OrderBy(x => x.Id).Select(x => x.Id).LastOrDefault();

            return id + 1;
        }

        public void SalvarDados()
        {
            GerenciadorBackup<T>.SalvarDados(registros);
        }

        public bool ExistemRegistros()
        {
            return registros.Any();
        }
    }
}
