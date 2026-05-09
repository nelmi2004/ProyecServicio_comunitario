using System;
using System.Collections.Generic;

namespace ProyecServicio_comunitario.Models;

public partial class Company
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Rif { get; set; }

    public string? Email { get; set; }

    public string? TelefonoMaster { get; set; }

    public string? UrlWeb { get; set; }

    public string? DireccionFiscal { get; set; }

    public string? LogoUrl { get; set; }
}
