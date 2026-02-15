namespace Université_Domain.Exceptions.NoteExceptions

{
    public class CsvProcessingException : Exception
    {
        public CsvProcessingException() : base("Erreur lors du traitement du fichier CSV.") { }

        public CsvProcessingException(string message) : base(message) { }

        public CsvProcessingException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}