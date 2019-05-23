namespace SOLIDPrinciples.ValidationClass.Example03
{
    public class TelefoneRequerido : IRegraDeValidacao<Cliente>
    {
        IRegraDeValidacao<Cliente> proximaRegra;
        public TelefoneRequerido(IRegraDeValidacao<Cliente> proximaRegra)
        {
            this.proximaRegra = proximaRegra;
        }
        public TelefoneRequerido() { }

        public bool ehValido(Cliente cliente)
        {
            //regra de validacao de telefone.
            //Se tiver outras regras, chama o atributo passado no construtor
            return true;
        }
    }
}
