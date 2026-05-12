using System.ComponentModel.DataAnnotations;


namespace ProyecServicio_comunitario
{
    /// <summary>
    /// Representa a un vehiculo
    /// </summary>
    public class VehiculoBaseDto
    {
        ///<summary>
        /// Placa del vehiculo, codigo alfanumerico el cual es unico para cada vehiculo.
        ///</summary>
        /// <example>ABC123</example>
        [MaxLength(20, ErrorMessage = "El campo Placa no puede tener mas de 20 caracteres")]
        public string? placa { get; set; }

        ///<summary>
        /// Tipo de vehiculo, descripcion de la clase de vehiculo automotor.
        ///</summary>
        /// <example>Moto</example>
        [MaxLength(30, ErrorMessage = "El campo tipo no puede tener mas de 30 caracteres")]
        public string? tipo { get; set; }

        ///<summary>
        /// Modelo del vehiculo
        ///</summary>
        /// <example>Toyota Corrolla</example>
        [MaxLength(50, ErrorMessage = "El campo Modelo no puede tener mas de 50 caracteres")]
        public string? modelo { get; set; }

        ///<summary>
        ///Condicion del vehiculo, describe la condicion actual del vehiculo.
        ///</summary>
        ///<example>fuera de servicio</example>
        [MaxLength(20, ErrorMessage = "El campo Condicion no puede tener mas de 20 caracteres")]
        public string? condicion { get; set; }

        ///<summary>
        ///Estado del vehiculo, indica si el vehiculo esta disponible o no para su uso(true por defecto).
        /// </summary>
        /// <example>true</example>
        public bool? estado { get; set; } = true;
    }

    public class VehiculoDto : VehiculoBaseDto {
        ///<summary>
        /// Placa del vehiculo, codigo alfanumerico el cual es unico para cada vehiculo.
        ///</summary>
        /// <example>ABC123</example>
        [Required(ErrorMessage = "El campo Placa es requerido")]
        [MaxLength(20, ErrorMessage = "El campo Placa no puede tener mas de 20 caracteres")]
        public string placa { get; set; }

        ///<summary>
        /// Tipo de vehiculo, descripcion de la clase de vehiculo automotor.
        ///</summary>
        /// <example>Moto</example>
        [Required(ErrorMessage = "El campo tipo es requerido")]
        [MaxLength(30, ErrorMessage = "El campo tipo no puede tener mas de 30 caracteres")]
        public string tipo { get; set; }
    }

    public class vehicleResponseDto : VehiculoDto
    {
        /// <summary>
        ///   Id del vehiculo
        /// </summary>
        /// <example>1</example>
        public int id { get; set; }
    }
    //Valida que todos los campos sean incluidos en la peticion
    public class VehicleUpdateDto : VehiculoDto
    {
        ///<summary>
        /// Modelo del vehiculo
        ///</summary>
        /// <example>Toyota Corrolla</example>
        [Required(ErrorMessage = "El campo Modelo es requerido")]
        [MaxLength(50, ErrorMessage = "El campo Modelo no puede tener mas de 50 caracteres")]
        public string modelo { get; set; }

        ///<summary>
        ///Condicion del vehiculo, describe la condicion actual del vehiculo.
        ///</summary>
        ///<example>fuera de servicio</example>
        [Required(ErrorMessage = "El campo Condicion es requerido")]
        [MaxLength(20, ErrorMessage = "El campo Condicion no puede tener mas de 20 caracteres")]
        public string condicion { get; set; }

        ///<summary>
        ///Estado del vehiculo, indica si el vehiculo esta disponible o no para su uso(true por defecto).
        /// </summary>
        /// <example>true</example>
        [Required(ErrorMessage = "El campo Estado es requerido")]
        public bool estado { get; set; } = true;
    }

    //Valida el formato de los datos pero no son obligatorios
    public class VehiclePartialDto : VehiculoBaseDto
    {
        ///<summary>
        ///Estado del vehiculo, indica si el vehiculo esta disponible o no para su uso(true por defecto).
        /// </summary>
        /// <example>true</example>
        public bool? estado { get; set; }
    }
}
