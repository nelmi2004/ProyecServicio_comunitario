using System;
using System.Collections.Generic;

namespace ProyecServicio_comunitario.Models;

public partial class CentrosSalud
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Tipo { get; set; }

    public string? Direccion { get; set; }

    public virtual ICollection<TrasladosEvento> TrasladosEventos { get; set; } = new List<TrasladosEvento>();
}
