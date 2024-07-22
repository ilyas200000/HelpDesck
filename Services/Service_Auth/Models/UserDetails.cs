namespace Service_Auth.Models
{
    public class UserDetails
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public int? entite_id { get; set; }
        public List<string> Roles { get; set; }
        public List<ClaimDto> Claims { get; set; }
        public int Matricule { get; set; }
    }

    public class ClaimDto
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }

}
