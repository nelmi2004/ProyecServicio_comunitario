using System;
using System.Collections.Generic;

namespace ProyecServicio_comunitario.Models;

public partial class Estatus
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Evento> Eventos { get; set; } = new List<Evento>();
}
