using System;
using System.Collections.Generic;

namespace ProyecServicio_comunitario.Models;

public partial class Evento
{
    public Guid Id { get; set; }

    public string NumeroCaso { get; set; } = null!;

    public DateOnly FechaSuceso { get; set; }

    public TimeOnly HoraSuceso { get; set; }

    public int? CategoriaId { get; set; }

    public int? EstatusId { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual ICollection<ApsEvento> ApsEventos { get; set; } = new List<ApsEvento>();

    public virtual Categoria? Categoria { get; set; }

    public virtual Estatus? Estatus { get; set; }

    public virtual ICollection<HerramientasEquipoEvento> HerramientasEquipoEventos { get; set; } = new List<HerramientasEquipoEvento>();

    public virtual ICollection<InvolucradosEvento> InvolucradosEventos { get; set; } = new List<InvolucradosEvento>();

    public virtual LocalizacionEvento? LocalizacionEvento { get; set; }

    public virtual ICollection<OrganismosEvento> OrganismosEventos { get; set; } = new List<OrganismosEvento>();

    public virtual ICollection<PersonalEvento> PersonalEventos { get; set; } = new List<PersonalEvento>();

    public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();

    public virtual ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
}
