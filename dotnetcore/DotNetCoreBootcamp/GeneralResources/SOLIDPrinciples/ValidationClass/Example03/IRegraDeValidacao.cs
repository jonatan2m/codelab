namespace SOLIDPrinciples.ValidationClass.Example03
{
    public interface IRegraDeValidacao<TIn>
    {
        bool ehValido(TIn cliente);
    }
}
