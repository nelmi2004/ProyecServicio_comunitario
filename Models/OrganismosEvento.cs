using System;
using System.Collections.Generic;

namespace ProyecServicio_comunitario.Models;

public partial class OrganismosEvento
{
    public Guid EventoId { get; set; }

    public int OrganismoId { get; set; }

    public string? Observaciones { get; set; }

    public virtual Evento Evento { get; set; } = null!;

    public virtual Organismo Organismo { get; set; } = null!;
}
