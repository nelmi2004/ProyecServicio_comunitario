using System;
using System.Collections.Generic;

namespace ProyecServicio_comunitario.Models;

public partial class InvolucradosEvento
{
    public int Id { get; set; }

    public Guid? CasoId { get; set; }

    public string? Cedula { get; set; }

    public string? NombreCompleto { get; set; }

    public char? Sexo { get; set; }

    public int? Edad { get; set; }

    public string? EstadoPaciente { get; set; }

    public virtual ICollection<ApsEvento> ApsEventos { get; set; } = new List<ApsEvento>();

    public virtual Evento? Caso { get; set; }

    public virtual ICollection<TrasladosEvento> TrasladosEventos { get; set; } = new List<TrasladosEvento>();
}
