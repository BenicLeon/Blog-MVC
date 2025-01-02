using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Blog.Models;

[Table("users")]
public partial class User
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }


    [Required]
    [StringLength(100, ErrorMessage = "Username must be between 4 and 100 characters.", MinimumLength = 4)]
    public string? Username { get; set; }


    [Required]
    [DataType(DataType.Password)]
    public string? Password{ get; set; }


    [Required]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string? Email { get; set; }
}
