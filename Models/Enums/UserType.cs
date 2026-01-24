using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public enum UserType : byte
{
    [Display(Name = "SuperAdmin", 
            Description = "Super administrator with full system access.")]
    SuperAdmin = 1,

    [Display(Name = "Institution Admin",
            Description = "Institution administrator with institution-specific access.")]
    InstitutionAdmin = 2

}