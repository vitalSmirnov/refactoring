namespace CloneIntime.Models.DTO
{
    public class AuditoryDTO
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }

        public string CreateName(string Number, string Name)
        {
            return $"{Number}({Name.Trim()})";
        }
    }
}
