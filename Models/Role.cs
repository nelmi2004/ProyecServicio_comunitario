using System;
using System.Collections.Generic;

namespace ProyecServicio_comunitario.Models;

public partial class Role
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public virtual ICollection<Menu> Menus { get; set; } = new List<Menu>();
}
