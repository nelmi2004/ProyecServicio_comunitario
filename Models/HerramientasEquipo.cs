using System;
using System.Collections.Generic;

namespace ProyecServicio_comunitario.Models;

public partial class HerramientasEquipo
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int? CantidadTotal { get; set; }

    public virtual ICollection<HerramientasEquipoEvento> HerramientasEquipoEventos { get; set; } = new List<HerramientasEquipoEvento>();
}
