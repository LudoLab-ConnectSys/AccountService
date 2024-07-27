namespace AccountService.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    [Key] [Column("id_usuario")] public int Id { get; set; }

    [Column("cedula_usuario")] public string Cedula { get; set; }

    [Column("HashContrasena")] public string Password { get; set; }

    [Column("nombre_usuario")] public string NombreUsuario { get; set; }

    [Column("apellidos_usuario")] public string ApellidoUsuario { get; set; }

    [Column("correo_usuario")] public string Email { get; set; }
}