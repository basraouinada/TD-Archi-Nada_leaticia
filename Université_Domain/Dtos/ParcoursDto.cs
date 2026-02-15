using Université_Domain.Entities;

namespace UniversiteDomain.Dtos;

public class ParcoursDto
{
    public long Id { get; set; }
    public string NomParcours { get; set; }
    public int AnneeFormation { get; set; }

    public ParcoursDto ToDto(Parcours parcours)
    {
        this.Id = parcours.Id;
        this.NomParcours = parcours.NomParcours;
        this.AnneeFormation = parcours.AnneeFormation;
        return this;
    }

    public List<ParcoursDto> ToDtos(List<Parcours> parcours)
    {
        return parcours.Select(parcour => new ParcoursDto
        {
            Id = parcour.Id,
            NomParcours = parcour.NomParcours,
            AnneeFormation = parcour.AnneeFormation

        }).ToList();
    }

    public Parcours ToEntity()
    {
        return new Parcours {Id = this.Id, NomParcours = this.NomParcours, AnneeFormation = this.AnneeFormation};
    }

    public String ToString()
    {
        return $"ID {Id} : {NomParcours} - {AnneeFormation}  ";
    }
}