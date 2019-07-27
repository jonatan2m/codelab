using System;
using System.Collections.Generic;
using System.Text;

namespace SOLIDPrinciples.OCP.Example2.Worst
{
    public enum TipoEmail
    {
        Texto,
        Html,
        Criptografado
    }

    public class EnviadorDeEmail
    {
        public void Enviar(string mensagem, string assunto, TipoEmail tipo)
        {
            if (tipo == TipoEmail.Texto)
            {
                mensagem = this.RemoverFormatacao(mensagem);
            }
            else if (tipo == TipoEmail.Html)
            {
                mensagem = this.InserirHtml(mensagem);

            }
            else if (tipo == TipoEmail.Criptografado)
            {
                mensagem = this.CriptografarMensagem(mensagem);

            }

            // some code

            this.EnviarMensagem(mensagem, assunto);
        }

        private void EnviarMensagem(string mensagem, string assunto)
        {
            throw new NotImplementedException();
        }

        private string CriptografarMensagem(string mensagem)
        {
            throw new NotImplementedException();
        }

        private string InserirHtml(string mensagem)
        {
            throw new NotImplementedException();
        }

        private string RemoverFormatacao(string mensagem)
        {
            throw new NotImplementedException();
        }
    }
}
