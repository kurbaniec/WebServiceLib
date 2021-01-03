namespace MTCG.Components.DataManagement.Schemas
{
    /// <summary>
    /// Data class that represent an user.
    /// </summary>
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

        /// <summary>
        /// Parses a string role information to the concrete enum.
        /// </summary>
        /// <param name="role"></param>
        /// <returns>
        /// Role enum
        /// </returns>
        private static Role UserRoleStringToEnum(string role)
        {
            return role.ToLower() == "admin" ? Role.Admin : Role.User;
        }
    }

    /// <summary>
    /// Enum that define user roles.
    /// </summary>
    public enum Role
    {
        User,
        Admin
    }
}