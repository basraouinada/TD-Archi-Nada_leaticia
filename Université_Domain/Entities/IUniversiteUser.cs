namespace Université_Domain.Entities;

public interface IUniversiteUser
{
    long ? EtudiantId { get; set; }
    Etudiant? Etudiant { get; set; }
}