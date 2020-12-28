namespace MTCG.Components.DataManagement.Schemas
{
    public class UserSchema
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }

        public UserSchema(string username, string password, string role)
        {
            Username = username;
            Password = password;
            Role = UserRoleStringToEnum(role);
        }

        private static Role UserRoleStringToEnum(string role)
        {
            return role.ToLower() == "admin" ? Role.Admin : Role.User;
        }
    }

    public enum Role
    {
        User,
        Admin
    }
}