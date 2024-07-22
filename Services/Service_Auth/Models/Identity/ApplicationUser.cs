using Microsoft.AspNetCore.Identity;

namespace Service_Auth.Models.Identity
{
    public class ApplicationUser : IdentityUser<int>
    {
        
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }

        public int? entite_id { get; set; }
        public virtual EntiteSiege? EntiteSiege { get; set; }
        public int Matricule { get; set; }

    }

    


    public class ApplicationRole : IdentityRole<int>
    {
        public virtual ICollection<ApplicationUserRole>? UserRoles { get; set; }
        public virtual ICollection<ApplicationRoleClaim>? RoleClaims { get; set; }
        public virtual ICollection<Droit>? DroitRoles { get; set; }



    }

    public class ApplicationUserRole : IdentityUserRole<int>
    {
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }

    public class ApplicationRoleClaim : IdentityRoleClaim<int>
    {
        public virtual ApplicationRole Role { get; set; }
    }

    public class EntiteSiege
    {
        public int ENTT_SG_ID { get; set; }

        public string ENTT_SG_NOM { get; set; }

        public string ENTT_SG_CODE { get; set; }

        public string? ENTT_SG_MAIL { get; set; }

        public bool? ENTT_SG_REG_FLAG { get; set; }
    }

    public class DROIT_ROLE
    {
        public int DROIT_ROLE_ID { get; set; }

        public int DRT_ID { get; set; }
        public Droit Droit { get; set; }

        public int RoleId { get; set; }
        public ApplicationRole Role { get; set; }
    }

    public class Droit
    {
        public int DRT_ID { get; set; }
        public string DRT_LIB {  get; set; }
        
    }

}
