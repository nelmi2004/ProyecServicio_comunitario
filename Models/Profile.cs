using System;
using System.Collections.Generic;

namespace ProyecServicio_comunitario.Models;

public partial class Profile
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public bool? Read { get; set; }

    public bool? Create { get; set; }

    public bool? Update { get; set; }

    public bool? Delete { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
