namespace MSSurfing.Data.Domain.Users
{
    public class User : BaseEntity
    {
        #region Properties
        public string Username { get; set; }
        public string Email { get; set; }
        public string Mobilephone { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        #endregion
    }
}
