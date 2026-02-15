using UniversiteDomain.Dtos;

namespace Université_Domain.Entities;

public class EtudiantWithParcoursName
{
    public EtudiantDto EtudiantDto { get; set; }
    public long idParcours { get; set; }
}