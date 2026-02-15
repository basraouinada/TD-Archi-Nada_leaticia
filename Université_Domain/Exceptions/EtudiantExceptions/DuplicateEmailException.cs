namespace Université_Domain.Exceptions.EtudiantExceptions;

[Serializable]
public class DuplicateEmailEtudException : Exception
{
    public DuplicateEmailEtudException() : base() { }
    public DuplicateEmailEtudException(string message) : base(message) { }
    public DuplicateEmailEtudException(string message, Exception inner) : base(message, inner) { }
}