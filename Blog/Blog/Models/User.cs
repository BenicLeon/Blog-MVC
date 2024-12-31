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

    
    public string Username { get; set; } = null!;

    
    public string Password { get; set; } = null!;

   
    public string Email { get; set; } = null!;
}
