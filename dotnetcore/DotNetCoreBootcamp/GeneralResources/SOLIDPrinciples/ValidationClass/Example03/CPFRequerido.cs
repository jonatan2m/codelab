namespace SOLIDPrinciples.ValidationClass.Example03
{
    public class CPFRequerido : IRegraDeValidacao<Cliente>
    {
        IRegraDeValidacao<Cliente> proximaRegra;
        public CPFRequerido(IRegraDeValidacao<Cliente> proximaRegra)
        {
            this.proximaRegra = proximaRegra;
        }
        public CPFRequerido() { }

        public bool ehValido(Cliente cliente)
        {
            //regra de validacao de CPF.
            //Se tiver outras regras, chama o atributo passado no construtor
            return true;
        }
    }
}
