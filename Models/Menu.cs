using System;
using System.Collections.Generic;

namespace ProyecServicio_comunitario.Models;

public partial class Menu
{
    public int Id { get; set; }

    public int? IdPadre { get; set; }

    public string Titulo { get; set; } = null!;

    public string? Icono { get; set; }

    public string? Url { get; set; }

    public int? Orden { get; set; }

    public virtual Menu? IdPadreNavigation { get; set; }

    public virtual ICollection<Menu> InverseIdPadreNavigation { get; set; } = new List<Menu>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
