using System;
using System.Collections.Generic;

namespace ProyecServicio_comunitario.Models;

public partial class HerramientasEquipoEvento
{
    public Guid EventoId { get; set; }

    public int HerramientaId { get; set; }

    public int? CantidadUsada { get; set; }

    public virtual Evento Evento { get; set; } = null!;

    public virtual HerramientasEquipo Herramienta { get; set; } = null!;
}
