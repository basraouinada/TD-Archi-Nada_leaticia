using System.Globalization;
using CsvHelper;
using Université_Domain.DataAdapters;
using Université_Domain.DataAdapters.DataAdaptersFactory;
using Université_Domain.Exceptions.NoteExceptions;
namespace UniversiteDomain.UseCases
{
    public class GenerateCsvForUeNotesUseCase(IRepositoryFactory repositoryFactory)
    {
      

        

        public async Task<string> ExecuteAsync(long ueId)
        {
            var ue = await repositoryFactory.UeRepository().GetByIdAsync(ueId);
            if (ue == null) throw new CsvProcessingException("UE non trouvée.");

            var etudiants = await repositoryFactory.EtudiantRepository().GetEtudiantsByUeIdAsync(ueId);
            if (etudiants == null || !etudiants.Any()) throw new CsvProcessingException("Aucun étudiant trouvé pour cette UE.");

            using (var writer = new StringWriter())
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteField("NumEtud");
                csv.WriteField("Nom");
                csv.WriteField("Prenom");
                csv.WriteField("Note");
                csv.NextRecord();

                foreach (var etudiant in etudiants)
                {
                    csv.WriteField(etudiant.NumEtud);
                    csv.WriteField(etudiant.Nom);
                    csv.WriteField(etudiant.Prenom);
                    csv.WriteField("");  // Colonne vide pour la saisie des notes
                    csv.NextRecord();
                }

                return writer.ToString();
            }
        }
    }
}
