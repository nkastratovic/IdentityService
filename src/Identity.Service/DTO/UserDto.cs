namespace Identity.Service.DTO
{
    public class UserDto
    {
        public Guid ID { get; set; }
        public string Email { get; set; }
        public string PersonName { get; set; }
        public string PhoneNumber { get; set; }
    }
}