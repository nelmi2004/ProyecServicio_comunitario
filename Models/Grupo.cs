using System;
using System.Collections.Generic;

namespace ProyecServicio_comunitario.Models;

public partial class Grupo
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Personal> Personals { get; set; } = new List<Personal>();
}
