using System;
using System.Collections.Generic;

namespace ProyecServicio_comunitario.Models;

public partial class Personal
{
    public int Id { get; set; }

    public string Cedula { get; set; } = null!;

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Cargo { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<PersonalEvento> PersonalEventos { get; set; } = new List<PersonalEvento>();

    public virtual ICollection<Grupo> Grupos { get; set; } = new List<Grupo>();
}
