using System;

namespace e_Agenda.ConsoleApp.Compartilhado
{
    public abstract class TelaBase
    {
        protected string Titulo { get; set; }

        public TelaBase(string titulo)
        {
            Titulo = titulo;
        }


        public virtual string MostrarOpcoes()
        {
            MostrarTitulo(Titulo);

            Console.WriteLine("Digite 1 para Inserir");
            Console.WriteLine("Digite 2 para Editar");
            Console.WriteLine("Digite 3 para Excluir");
            Console.WriteLine("Digite 4 para Visualizar");

            Console.WriteLine("Digite s para sair");

            string opcao = Console.ReadLine();

            return opcao;
        }

        public virtual void MostrarTitulo(string titulo, bool limparConsole = true)
        {
            if (limparConsole)
                Console.Clear();

            Console.ForegroundColor = ConsoleColor.Magenta;

            Console.WriteLine(titulo);
             
            Console.ResetColor();

            Console.WriteLine();
        }
    }
}
