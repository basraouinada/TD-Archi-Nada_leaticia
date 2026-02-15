using System.Globalization;
using CsvHelper;
using Université_Domain.DataAdapters;
using Université_Domain.DataAdapters.DataAdaptersFactory;
using Université_Domain.Entities;
using Université_Domain.Exceptions.NoteExceptions;






namespace UniversiteDomain.UseCases
{
    public class UploadCsvForUeNotesUseCase(IRepositoryFactory repositoryFactory,ValidationUseCase validationUseCase)
    {
        

        public async Task ExecuteAsync(Stream csvStream, long ueId)
        {
            using (var reader = new StreamReader(csvStream))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<dynamic>().ToList();

                // Validation des données
                var validationErrors = await validationUseCase.ValidateAsync(records, ueId);
                if (validationErrors.Any())
                {
                    throw new CsvProcessingException(string.Join("; ", validationErrors));
                }
                

                // Enregistrement des notes
                foreach (var record in records)
                {
                    long etudiantId = long.Parse(record.NumEtud);
                    double noteValue = double.Parse(record.Note);

                    var note = new Note
                    {
                        EtudiantId = etudiantId,
                        UeId = ueId,
                        Valeur = (float)noteValue
                    };

                    await repositoryFactory.NoteRepository().SaveOrUpdateAsync(note);
                }
            }
        }
    }
}
