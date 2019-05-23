namespace SOLIDPrinciples.ValidationClass.Example03
{
    public class NomeRequerido : IRegraDeValidacao<Cliente>
    {
        IRegraDeValidacao<Cliente> proximaRegra;
        public NomeRequerido(IRegraDeValidacao<Cliente> proximaRegra)
        {
            this.proximaRegra = proximaRegra;
        }
        public NomeRequerido() { }

        public bool ehValido(Cliente cliente)
        {
            //regra de validacao de nome.
            //Se tiver outras regras, chama o atributo passado no construtor
            return true;
        }
    }
}
