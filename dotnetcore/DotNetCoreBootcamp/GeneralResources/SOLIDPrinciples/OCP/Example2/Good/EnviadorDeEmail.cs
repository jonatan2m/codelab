using System;
using System.Collections.Generic;
using System.Text;

namespace SOLIDPrinciples.OCP.Example2.Good
{
    public static class Util
    {
        internal static void RemoverFormatacao(string mensagem)
        {
            throw new NotImplementedException();
        }

        internal static void InserirHtml(string mensagem)
        {
            throw new NotImplementedException();
        }

        internal static void CriptografarMensagem(string mensagem)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class Email
    {
        public abstract void Enviar(string assunto, string mensagem);
    }

    public class TextoEmail : Email
    {
        public override void Enviar(string assunto, string mensagem)
        {
            //Remove Formatacao e envia mensagem;
        }
    }

    public class HtmlEmail : Email
    {
        public override void Enviar(string assunto, string mensagem)
        {
            //Insere Html e envia mensagem;
        }
    }

    public class CriptografadoEmail : Email
    {
        public override void Enviar(string assunto, string mensagem)
        {
            //Criptografa Mensagem e envia mensagem;
        }
    }

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
