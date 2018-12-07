namespace RoleShuffle.Base.Formatter
{
    public interface IEmailBodyFormatter<in TData> where TData : class
    {
        string Format(TData overview);
    }
}