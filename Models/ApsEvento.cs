using System;
using System.Collections.Generic;

namespace ProyecServicio_comunitario.Models;

public partial class ApsEvento
{
    public int Id { get; set; }

    public Guid? CasoId { get; set; }

    public int? InvolucradoId { get; set; }

    public string DetalleAtencion { get; set; } = null!;

    public string? EstadoPacientePostAps { get; set; }

    public DateTime? FechaAtencion { get; set; }

    public virtual Evento? Caso { get; set; }

    public virtual InvolucradosEvento? Involucrado { get; set; }
}
