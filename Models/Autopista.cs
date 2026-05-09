using System;
using System.Collections.Generic;

namespace ProyecServicio_comunitario.Models;

public partial class Autopista
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<LocalizacionEvento> LocalizacionEventos { get; set; } = new List<LocalizacionEvento>();
}
