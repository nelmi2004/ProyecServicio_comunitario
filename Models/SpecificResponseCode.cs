using System;
using System.Collections.Generic;

namespace ProyecServicio_comunitario.Models;

public partial class SpecificResponseCode
{
    public int Id { get; set; }

    public int? GeneralResponseId { get; set; }

    public string MensajeEspecifico { get; set; } = null!;

    public virtual GeneralResponseCode? GeneralResponse { get; set; }
}
