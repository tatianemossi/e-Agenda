namespace e_Agenda.ConsoleApp.Compartilhado
{
    public interface ITelaCadastravel
    {
        void Inserir();
        void Editar();
        void Excluir();
        bool Visualizar(string tipoVisualizacao);
    }
}
