using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProyecServicio_comunitario.Models;

public partial class AngelDbContext : DbContext
{
    public AngelDbContext()
    {
    }

    public AngelDbContext(DbContextOptions<AngelDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ApsEvento> ApsEventos { get; set; }

    public virtual DbSet<Auditoria> Auditorias { get; set; }

    public virtual DbSet<Autopista> Autopistas { get; set; }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<CentrosSalud> CentrosSaluds { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Estatus> Estatuses { get; set; }

    public virtual DbSet<Evento> Eventos { get; set; }

    public virtual DbSet<GeneralResponseCode> GeneralResponseCodes { get; set; }

    public virtual DbSet<Grupo> Grupos { get; set; }

    public virtual DbSet<HerramientasEquipo> HerramientasEquipos { get; set; }

    public virtual DbSet<HerramientasEquipoEvento> HerramientasEquipoEventos { get; set; }

    public virtual DbSet<InvolucradosEvento> InvolucradosEventos { get; set; }

    public virtual DbSet<LocalizacionEvento> LocalizacionEventos { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Organismo> Organismos { get; set; }

    public virtual DbSet<OrganismosEvento> OrganismosEventos { get; set; }

    public virtual DbSet<Personal> Personals { get; set; }

    public virtual DbSet<PersonalEvento> PersonalEventos { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Servicio> Servicios { get; set; }

    public virtual DbSet<SpecificResponseCode> SpecificResponseCodes { get; set; }

    public virtual DbSet<TrasladosEvento> TrasladosEventos { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vehiculo> Vehiculos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=Qa;Username=postgres;Password=nelmiguel");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pgcrypto");

        modelBuilder.Entity<ApsEvento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("aps_caso_pkey");

            entity.ToTable("aps_evento");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('aps_caso_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.CasoId).HasColumnName("caso_id");
            entity.Property(e => e.DetalleAtencion).HasColumnName("detalle_atencion");
            entity.Property(e => e.EstadoPacientePostAps).HasColumnName("estado_paciente_post_aps");
            entity.Property(e => e.FechaAtencion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("fecha_atencion");
            entity.Property(e => e.InvolucradoId).HasColumnName("involucrado_id");

            entity.HasOne(d => d.Caso).WithMany(p => p.ApsEventos)
                .HasForeignKey(d => d.CasoId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("aps_caso_caso_id_fkey");

            entity.HasOne(d => d.Involucrado).WithMany(p => p.ApsEventos)
                .HasForeignKey(d => d.InvolucradoId)
                .HasConstraintName("aps_caso_involucrado_id_fkey");
        });

        modelBuilder.Entity<Auditoria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("auditorias_pkey");

            entity.ToTable("auditorias");

            entity.HasIndex(e => e.UsuarioId, "idx_auditoria_usuario");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Accion)
                .HasMaxLength(100)
                .HasColumnName("accion");
            entity.Property(e => e.DetalleAnterior)
                .HasColumnType("jsonb")
                .HasColumnName("detalle_anterior");
            entity.Property(e => e.DetalleNuevo)
                .HasColumnType("jsonb")
                .HasColumnName("detalle_nuevo");
            entity.Property(e => e.FechaAccion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("fecha_accion");
            entity.Property(e => e.RegistroId).HasColumnName("registro_id");
            entity.Property(e => e.TablaAfectada)
                .HasMaxLength(50)
                .HasColumnName("tabla_afectada");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Auditoria)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("auditorias_usuario_id_fkey");
        });

        modelBuilder.Entity<Autopista>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("autopistas_pkey");

            entity.ToTable("autopistas");

            entity.HasIndex(e => e.Nombre, "autopistas_nombre_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("categorias_pkey");

            entity.ToTable("categorias");

            entity.HasIndex(e => e.Nombre, "categorias_nombre_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<CentrosSalud>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("centros_salud_pkey");

            entity.ToTable("centros_salud");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Direccion).HasColumnName("direccion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .HasColumnName("nombre");
            entity.Property(e => e.Tipo)
                .HasMaxLength(50)
                .HasColumnName("tipo");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("company_pkey");

            entity.ToTable("company");

            entity.HasIndex(e => e.Rif, "company_rif_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DireccionFiscal).HasColumnName("direccion_fiscal");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.LogoUrl).HasColumnName("logo_url");
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .HasColumnName("nombre");
            entity.Property(e => e.Rif)
                .HasMaxLength(20)
                .HasColumnName("rif");
            entity.Property(e => e.TelefonoMaster)
                .HasMaxLength(20)
                .HasColumnName("telefono_master");
            entity.Property(e => e.UrlWeb)
                .HasMaxLength(100)
                .HasColumnName("url_web");
        });

        modelBuilder.Entity<Estatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("estatus_pkey");

            entity.ToTable("estatus");

            entity.HasIndex(e => e.Nombre, "estatus_nombre_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Evento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("casos_pkey");

            entity.ToTable("eventos");

            entity.HasIndex(e => e.NumeroCaso, "casos_numero_caso_key").IsUnique();

            entity.HasIndex(e => e.FechaSuceso, "idx_casos_fecha");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CategoriaId).HasColumnName("categoria_id");
            entity.Property(e => e.EstatusId).HasColumnName("estatus_id");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaSuceso).HasColumnName("fecha_suceso");
            entity.Property(e => e.HoraSuceso).HasColumnName("hora_suceso");
            entity.Property(e => e.NumeroCaso)
                .HasMaxLength(20)
                .HasColumnName("numero_caso");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Eventos)
                .HasForeignKey(d => d.CategoriaId)
                .HasConstraintName("casos_categoria_id_fkey");

            entity.HasOne(d => d.Estatus).WithMany(p => p.Eventos)
                .HasForeignKey(d => d.EstatusId)
                .HasConstraintName("casos_estatus_id_fkey");

            entity.HasMany(d => d.Vehiculos).WithMany(p => p.Casos)
                .UsingEntity<Dictionary<string, object>>(
                    "VehiculosEvento",
                    r => r.HasOne<Vehiculo>().WithMany()
                        .HasForeignKey("VehiculoId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("vehiculos_caso_vehiculo_id_fkey"),
                    l => l.HasOne<Evento>().WithMany()
                        .HasForeignKey("CasoId")
                        .HasConstraintName("vehiculos_caso_caso_id_fkey"),
                    j =>
                    {
                        j.HasKey("CasoId", "VehiculoId").HasName("vehiculos_caso_pkey");
                        j.ToTable("vehiculos_evento");
                        j.IndexerProperty<Guid>("CasoId").HasColumnName("caso_id");
                        j.IndexerProperty<int>("VehiculoId").HasColumnName("vehiculo_id");
                    });
        });

        modelBuilder.Entity<GeneralResponseCode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("general_response_codes_pkey");

            entity.ToTable("general_response_codes");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
        });

        modelBuilder.Entity<Grupo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("grupos_pkey");

            entity.ToTable("grupos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<HerramientasEquipo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("herramientas_equipos_pkey");

            entity.ToTable("herramientas_equipos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CantidadTotal)
                .HasDefaultValue(1)
                .HasColumnName("cantidad_total");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<HerramientasEquipoEvento>(entity =>
        {
            entity.HasKey(e => new { e.CasoId, e.HerramientaId }).HasName("herramientas_equipo_caso_pkey");

            entity.ToTable("herramientas_equipo_evento");

            entity.Property(e => e.CasoId).HasColumnName("caso_id");
            entity.Property(e => e.HerramientaId).HasColumnName("herramienta_id");
            entity.Property(e => e.CantidadUsada)
                .HasDefaultValue(1)
                .HasColumnName("cantidad_usada");

            entity.HasOne(d => d.Caso).WithMany(p => p.HerramientasEquipoEventos)
                .HasForeignKey(d => d.CasoId)
                .HasConstraintName("herramientas_equipo_caso_caso_id_fkey");

            entity.HasOne(d => d.Herramienta).WithMany(p => p.HerramientasEquipoEventos)
                .HasForeignKey(d => d.HerramientaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("herramientas_equipo_caso_herramienta_id_fkey");
        });

        modelBuilder.Entity<InvolucradosEvento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("involucrados_caso_pkey");

            entity.ToTable("involucrados_evento");

            entity.HasIndex(e => e.CasoId, "idx_involucrados_caso_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('involucrados_caso_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.CasoId).HasColumnName("caso_id");
            entity.Property(e => e.Cedula)
                .HasMaxLength(20)
                .HasColumnName("cedula");
            entity.Property(e => e.Edad).HasColumnName("edad");
            entity.Property(e => e.EstadoPaciente).HasColumnName("estado_paciente");
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(150)
                .HasColumnName("nombre_completo");
            entity.Property(e => e.Sexo)
                .HasMaxLength(1)
                .HasColumnName("sexo");

            entity.HasOne(d => d.Caso).WithMany(p => p.InvolucradosEventos)
                .HasForeignKey(d => d.CasoId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("involucrados_caso_caso_id_fkey");
        });

        modelBuilder.Entity<LocalizacionEvento>(entity =>
        {
            entity.HasKey(e => e.CasoId).HasName("localizacion_caso_pkey");

            entity.ToTable("localizacion_evento");

            entity.Property(e => e.CasoId)
                .ValueGeneratedNever()
                .HasColumnName("caso_id");
            entity.Property(e => e.AutopistaId).HasColumnName("autopista_id");
            entity.Property(e => e.DireccionExacta).HasColumnName("direccion_exacta");
            entity.Property(e => e.Latitud)
                .HasPrecision(9, 6)
                .HasColumnName("latitud");
            entity.Property(e => e.Longitud)
                .HasPrecision(9, 6)
                .HasColumnName("longitud");
            entity.Property(e => e.PuntoReferencia).HasColumnName("punto_referencia");

            entity.HasOne(d => d.Autopista).WithMany(p => p.LocalizacionEventos)
                .HasForeignKey(d => d.AutopistaId)
                .HasConstraintName("localizacion_caso_autopista_id_fkey");

            entity.HasOne(d => d.Caso).WithOne(p => p.LocalizacionEvento)
                .HasForeignKey<LocalizacionEvento>(d => d.CasoId)
                .HasConstraintName("localizacion_caso_caso_id_fkey");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("menus_pkey");

            entity.ToTable("menus");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Icono)
                .HasMaxLength(50)
                .HasColumnName("icono");
            entity.Property(e => e.IdPadre).HasColumnName("id_padre");
            entity.Property(e => e.Orden).HasColumnName("orden");
            entity.Property(e => e.Titulo)
                .HasMaxLength(50)
                .HasColumnName("titulo");
            entity.Property(e => e.Url)
                .HasMaxLength(100)
                .HasColumnName("url");

            entity.HasOne(d => d.IdPadreNavigation).WithMany(p => p.InverseIdPadreNavigation)
                .HasForeignKey(d => d.IdPadre)
                .HasConstraintName("menus_id_padre_fkey");

            entity.HasMany(d => d.Roles).WithMany(p => p.Menus)
                .UsingEntity<Dictionary<string, object>>(
                    "MenusRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("menus_roles_role_id_fkey"),
                    l => l.HasOne<Menu>().WithMany()
                        .HasForeignKey("MenuId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("menus_roles_menu_id_fkey"),
                    j =>
                    {
                        j.HasKey("MenuId", "RoleId").HasName("menus_roles_pkey");
                        j.ToTable("menus_roles");
                        j.IndexerProperty<int>("MenuId").HasColumnName("menu_id");
                        j.IndexerProperty<int>("RoleId").HasColumnName("role_id");
                    });
        });

        modelBuilder.Entity<Organismo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("organismos_pkey");

            entity.ToTable("organismos");

            entity.HasIndex(e => e.Nombre, "organismos_nombre_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<OrganismosEvento>(entity =>
        {
            entity.HasKey(e => new { e.CasoId, e.OrganismoId }).HasName("organismos_caso_pkey");

            entity.ToTable("organismos_evento");

            entity.Property(e => e.CasoId).HasColumnName("caso_id");
            entity.Property(e => e.OrganismoId).HasColumnName("organismo_id");
            entity.Property(e => e.Observaciones).HasColumnName("observaciones");

            entity.HasOne(d => d.Caso).WithMany(p => p.OrganismosEventos)
                .HasForeignKey(d => d.CasoId)
                .HasConstraintName("organismos_caso_caso_id_fkey");

            entity.HasOne(d => d.Organismo).WithMany(p => p.OrganismosEventos)
                .HasForeignKey(d => d.OrganismoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("organismos_caso_organismo_id_fkey");
        });

        modelBuilder.Entity<Personal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("personal_pkey");

            entity.ToTable("personal");

            entity.HasIndex(e => e.Cedula, "personal_cedula_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Apellidos)
                .HasMaxLength(100)
                .HasColumnName("apellidos");
            entity.Property(e => e.Cargo)
                .HasMaxLength(50)
                .HasColumnName("cargo");
            entity.Property(e => e.Cedula)
                .HasMaxLength(20)
                .HasColumnName("cedula");
            entity.Property(e => e.Nombres)
                .HasMaxLength(100)
                .HasColumnName("nombres");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");

            entity.HasMany(d => d.Grupos).WithMany(p => p.Personals)
                .UsingEntity<Dictionary<string, object>>(
                    "PersonalGrupo",
                    r => r.HasOne<Grupo>().WithMany()
                        .HasForeignKey("GrupoId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("personal_grupo_grupo_id_fkey"),
                    l => l.HasOne<Personal>().WithMany()
                        .HasForeignKey("PersonalId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("personal_grupo_personal_id_fkey"),
                    j =>
                    {
                        j.HasKey("PersonalId", "GrupoId").HasName("personal_grupo_pkey");
                        j.ToTable("personal_grupo");
                        j.IndexerProperty<int>("PersonalId").HasColumnName("personal_id");
                        j.IndexerProperty<int>("GrupoId").HasColumnName("grupo_id");
                    });
        });

        modelBuilder.Entity<PersonalEvento>(entity =>
        {
            entity.HasKey(e => new { e.CasoId, e.PersonalId }).HasName("personal_caso_pkey");

            entity.ToTable("personal_evento");

            entity.Property(e => e.CasoId).HasColumnName("caso_id");
            entity.Property(e => e.PersonalId).HasColumnName("personal_id");
            entity.Property(e => e.RolEnSitio)
                .HasMaxLength(50)
                .HasColumnName("rol_en_sitio");

            entity.HasOne(d => d.Caso).WithMany(p => p.PersonalEventos)
                .HasForeignKey(d => d.CasoId)
                .HasConstraintName("personal_caso_caso_id_fkey");

            entity.HasOne(d => d.Personal).WithMany(p => p.PersonalEventos)
                .HasForeignKey(d => d.PersonalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("personal_caso_personal_id_fkey");
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("profiles_pkey");

            entity.ToTable("profiles");

            entity.HasIndex(e => e.Nombre, "profiles_nombre_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Create)
                .HasDefaultValue(false)
                .HasColumnName("create");
            entity.Property(e => e.Delete)
                .HasDefaultValue(false)
                .HasColumnName("delete");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .HasColumnName("nombre");
            entity.Property(e => e.Read)
                .HasDefaultValue(false)
                .HasColumnName("read");
            entity.Property(e => e.Update)
                .HasDefaultValue(false)
                .HasColumnName("update");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.HasIndex(e => e.Nombre, "roles_nombre_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("reportes_pkey");

            entity.ToTable("servicios");

            entity.HasIndex(e => e.CasoId, "idx_reportes_caso_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CasoId).HasColumnName("caso_id");
            entity.Property(e => e.EstatusProcesamiento)
                .HasMaxLength(20)
                .HasDefaultValueSql("'pendiente'::character varying")
                .HasColumnName("estatus_procesamiento");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.NumeroReporte)
                .HasMaxLength(20)
                .HasColumnName("numero_reporte");
            entity.Property(e => e.TextoNatural).HasColumnName("texto_natural");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Caso).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.CasoId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("reportes_caso_id_fkey");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("reportes_usuario_id_fkey");
        });

        modelBuilder.Entity<SpecificResponseCode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("specific_response_codes_pkey");

            entity.ToTable("specific_response_codes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.GeneralResponseId).HasColumnName("general_response_id");
            entity.Property(e => e.MensajeEspecifico).HasColumnName("mensaje_especifico");

            entity.HasOne(d => d.GeneralResponse).WithMany(p => p.SpecificResponseCodes)
                .HasForeignKey(d => d.GeneralResponseId)
                .HasConstraintName("specific_response_codes_general_response_id_fkey");
        });

        modelBuilder.Entity<TrasladosEvento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("traslados_caso_pkey");

            entity.ToTable("traslados_evento");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('traslados_caso_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.CentroSaludId).HasColumnName("centro_salud_id");
            entity.Property(e => e.InvolucradoId).HasColumnName("involucrado_id");
            entity.Property(e => e.Observaciones).HasColumnName("observaciones");
            entity.Property(e => e.VehiculoId).HasColumnName("vehiculo_id");

            entity.HasOne(d => d.CentroSalud).WithMany(p => p.TrasladosEventos)
                .HasForeignKey(d => d.CentroSaludId)
                .HasConstraintName("traslados_caso_centro_salud_id_fkey");

            entity.HasOne(d => d.Involucrado).WithMany(p => p.TrasladosEventos)
                .HasForeignKey(d => d.InvolucradoId)
                .HasConstraintName("traslados_caso_involucrado_id_fkey");

            entity.HasOne(d => d.Vehiculo).WithMany(p => p.TrasladosEventos)
                .HasForeignKey(d => d.VehiculoId)
                .HasConstraintName("traslados_caso_vehiculo_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.HasIndex(e => e.Username, "users_username_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.EstaActivo)
                .HasDefaultValue(true)
                .HasColumnName("esta_activo");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            entity.Property(e => e.ProfileId).HasColumnName("profile_id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");

            entity.HasOne(d => d.Profile).WithMany(p => p.Users)
                .HasForeignKey(d => d.ProfileId)
                .HasConstraintName("users_profile_id_fkey");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("users_role_id_fkey");
        });

        modelBuilder.Entity<Vehiculo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("vehiculos_pkey");

            entity.ToTable("vehiculos");

            entity.HasIndex(e => e.Placa, "vehiculos_placa_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EstatusVehiculo)
                .HasMaxLength(20)
                .HasDefaultValueSql("'operativo'::character varying")
                .HasColumnName("estatus_vehiculo");
            entity.Property(e => e.Modelo)
                .HasMaxLength(50)
                .HasColumnName("modelo");
            entity.Property(e => e.Placa)
                .HasMaxLength(20)
                .HasColumnName("placa");
            entity.Property(e => e.Tipo)
                .HasMaxLength(30)
                .HasColumnName("tipo");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
