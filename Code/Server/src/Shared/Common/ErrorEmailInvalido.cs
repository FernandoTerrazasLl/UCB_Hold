
public class ErrorEmailInvalido : DomainException
{
    public ErrorEmailInvalido() 
        : base($"El formato del email no es válido")
    {
    }
}
