namespace Université_Domain.Exceptions.ParcoursExceptions;
[Serializable]
public class DuplicateAnneFormationException :Exception
{
    public DuplicateAnneFormationException() : base() { }
    public DuplicateAnneFormationException(string message) : base(message) { }
    public DuplicateAnneFormationException(string message, Exception inner) : base(message, inner) { }
}