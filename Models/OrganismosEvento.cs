using System;
using System.Collections.Generic;

namespace ProyecServicio_comunitario.Models;

public partial class OrganismosEvento
{
    public Guid CasoId { get; set; }

    public int OrganismoId { get; set; }

    public string? Observaciones { get; set; }

    public virtual Evento Caso { get; set; } = null!;

    public virtual Organismo Organismo { get; set; } = null!;
}
