using System;
using System.Collections.Generic;

namespace e_Agenda.ConsoleApp.Compartilhado
{
    public interface IRepositorio<T> where T : EntidadeBase
    {
        void Inserir(T entidade);
        bool Editar(int idSelecionado, T novaEntidade);
        bool Excluir(int idSelecionado);
        bool ExisteRegistro(int idSelecionado);
        T SelecionarRegistro(int idSelecionado);
        List<T> SelecionarTodos();
        void SalvarDados();
    }
}