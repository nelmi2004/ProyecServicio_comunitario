using System;
using System.Collections.Generic;

namespace ProyecServicio_comunitario.Models;

public partial class Vehiculo
{
    public int Id { get; set; }

    public string Placa { get; set; } = null!;

    public string Tipo { get; set; } = null!;

    public string? Modelo { get; set; }

    public bool? EstatusVehiculo { get; set; }

    public string? Condicion { get; set; }

    public virtual ICollection<TrasladosEvento> TrasladosEventos { get; set; } = new List<TrasladosEvento>();

    public virtual ICollection<Evento> Eventos { get; set; } = new List<Evento>();
}
