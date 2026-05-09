using System;
using System.Collections.Generic;

namespace ProyecServicio_comunitario.Models;

public partial class GeneralResponseCode
{
    public int Id { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<SpecificResponseCode> SpecificResponseCodes { get; set; } = new List<SpecificResponseCode>();
}
