using System;
using System.Collections.Generic;

namespace ProyecServicio_comunitario.Models;

public partial class PersonalEvento
{
    public Guid CasoId { get; set; }

    public int PersonalId { get; set; }

    public string? RolEnSitio { get; set; }

    public virtual Evento Caso { get; set; } = null!;

    public virtual Personal Personal { get; set; } = null!;
}
