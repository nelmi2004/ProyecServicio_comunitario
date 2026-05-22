using System;
using System.Collections.Generic;

namespace ProyecServicio_comunitario.Models;

public partial class ApsEvento
{
    public int Id { get; set; }

    public Guid? EventoId { get; set; }

    public int? InvolucradoId { get; set; }

    public string DetalleAtencion { get; set; } = null!;

    public string? EstadoPacientePostAps { get; set; }

    public DateTime? FechaAtencion { get; set; }

    public virtual Evento? Evento { get; set; }

    public virtual InvolucradosEvento? Involucrado { get; set; }
}
