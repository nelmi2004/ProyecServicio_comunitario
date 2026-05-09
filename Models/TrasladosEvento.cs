using System;
using System.Collections.Generic;

namespace ProyecServicio_comunitario.Models;

public partial class TrasladosEvento
{
    public int Id { get; set; }

    public int? InvolucradoId { get; set; }

    public int? VehiculoId { get; set; }

    public int? CentroSaludId { get; set; }

    public string? Observaciones { get; set; }

    public virtual CentrosSalud? CentroSalud { get; set; }

    public virtual InvolucradosEvento? Involucrado { get; set; }

    public virtual Vehiculo? Vehiculo { get; set; }
}
