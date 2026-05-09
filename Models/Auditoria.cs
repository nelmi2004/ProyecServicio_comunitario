using System;
using System.Collections.Generic;

namespace ProyecServicio_comunitario.Models;

public partial class Auditoria
{
    public int Id { get; set; }

    public Guid? UsuarioId { get; set; }

    public string Accion { get; set; } = null!;

    public string? TablaAfectada { get; set; }

    public Guid? RegistroId { get; set; }

    public string? DetalleAnterior { get; set; }

    public string? DetalleNuevo { get; set; }

    public DateTime? FechaAccion { get; set; }

    public virtual User? Usuario { get; set; }
}
