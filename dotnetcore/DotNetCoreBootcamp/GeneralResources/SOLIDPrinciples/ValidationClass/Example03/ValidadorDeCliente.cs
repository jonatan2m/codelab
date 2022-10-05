namespace SOLIDPrinciples.ValidationClass.Example03
{
    public class ValidadorDeCliente
    {
        public void novoCliente(Cliente cliente)
        {
            IRegraDeValidacao<Cliente> validador =
                new NomeRequerido(
                    new TelefoneRequerido(
                        new CPFRequerido(
                            )));
            if (validador.ehValido(cliente))
            {
                //continua processamento;
            }
            else
            {
                //exibe erro para o usuario
            }
        }
    }
}
