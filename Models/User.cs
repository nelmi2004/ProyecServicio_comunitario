using System;
using System.Collections.Generic;

namespace ProyecServicio_comunitario.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int? RoleId { get; set; }

    public int? ProfileId { get; set; }

    public bool? EstaActivo { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual ICollection<Auditoria> Auditoria { get; set; } = new List<Auditoria>();

    public virtual Profile? Profile { get; set; }

    public virtual Role? Role { get; set; }
    public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
}
