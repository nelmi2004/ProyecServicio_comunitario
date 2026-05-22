using System;
using System.Collections.Generic;

namespace ProyecServicio_comunitario.Models;

public partial class Servicio
{
    public Guid Id { get; set; }

    public Guid? EventoId { get; set; }

    public string NumeroReporte { get; set; } = null!;

    public string TextoNatural { get; set; } = null!;

    public Guid? UsuarioId { get; set; }

    public string? EstatusProcesamiento { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual Evento? Evento { get; set; }

    public virtual User? Usuario { get; set; }
}
