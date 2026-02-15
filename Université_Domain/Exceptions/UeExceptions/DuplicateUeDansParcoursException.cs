namespace Université_Domain.Exceptions.UeExceptions;

public class DuplicateUeDansParcoursException : Exception
{
    public DuplicateUeDansParcoursException() : base() { }
    public DuplicateUeDansParcoursException(  string message) : base( message) { }
    public DuplicateUeDansParcoursException(string message, Exception inner) : base(message, inner) { }
}