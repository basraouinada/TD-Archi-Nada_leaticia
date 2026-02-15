using Université_Domain.DataAdapters;
using Université_Domain.DataAdapters.DataAdaptersFactory;

namespace UniversiteDomain.UseCases;

public class ValidationUseCase(IRepositoryFactory repositoryFactory)
    {
        

        public async Task<List<string>> ValidateAsync(IEnumerable<dynamic> records, long ueId)
        {
            var errors = new List<string>();

            // Vérifier si l'UE existe
            var ue = await repositoryFactory.UeRepository().GetByIdAsync(ueId);
            if (ue == null)
            {
                errors.Add($"L'UE avec l'ID {ueId} n'existe pas.");
                return errors;
            }

            foreach (var record in records)
            {
                // Validation du numéro d'étudiant
                if (string.IsNullOrWhiteSpace(record.NumEtud))
                {
                    errors.Add("Numéro d'étudiant manquant.");
                    continue;
                }

                // Vérifier si l'étudiant existe dans la base
                var etudiant = await repositoryFactory.EtudiantRepository().GetEtudiantsByUeIdAsync(Convert.ToInt64(record.NumEtud));
                if (etudiant == null)
                {
                    errors.Add($"L'étudiant avec le numéro {record.NumEtud} n'existe pas.");
                    continue;
                }

                // Validation de la note
                if (!double.TryParse(record.Note, out double noteValue))
                {
                    errors.Add($"Note invalide pour l'étudiant {record.NumEtud}. La note doit être un nombre.");
                    continue;
                }

                if (noteValue < 0 || noteValue > 20)
                {
                    errors.Add($"Note hors plage (0-20) pour l'étudiant {record.NumEtud}.");
                }
            }

            return errors;
        }
}