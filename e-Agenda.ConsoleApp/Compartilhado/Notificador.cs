using System;

namespace e_Agenda.ConsoleApp.Compartilhado
{
    public class Notificador
    {
        public void ApresentarMensagem(string mensagem, TipoMensagemEnum tipoMensagem, bool esperarAcaoUsuario = true)
        {
            switch (tipoMensagem)
            {
                case TipoMensagemEnum.Sucesso:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;

                case TipoMensagemEnum.Atencao:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;

                case TipoMensagemEnum.Erro:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;

                default:
                    break;
            }

            Console.WriteLine();
            Console.WriteLine(mensagem);
            Console.ResetColor();

            if (esperarAcaoUsuario)
            {
                Console.ReadLine();
            }            
        }
    }
}
