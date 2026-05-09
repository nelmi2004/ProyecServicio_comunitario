using System;
using System.Collections.Generic;

namespace ProyecServicio_comunitario.Models;

public partial class Organismo
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<OrganismosEvento> OrganismosEventos { get; set; } = new List<OrganismosEvento>();
}
