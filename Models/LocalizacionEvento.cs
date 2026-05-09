using System;
using System.Collections.Generic;

namespace ProyecServicio_comunitario.Models;

public partial class LocalizacionEvento
{
    public Guid CasoId { get; set; }

    public int? AutopistaId { get; set; }

    public string? DireccionExacta { get; set; }

    public string? PuntoReferencia { get; set; }

    public decimal? Latitud { get; set; }

    public decimal? Longitud { get; set; }

    public virtual Autopista? Autopista { get; set; }

    public virtual Evento Caso { get; set; } = null!;
}
