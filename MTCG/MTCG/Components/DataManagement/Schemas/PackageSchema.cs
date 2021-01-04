namespace MTCG.Components.DataManagement.Schemas
{
    /// <summary>
    /// Data class that represent a package.
    /// </summary>
    public class PackageSchema
    {
        public int Id { get; set; }

        public PackageSchema(int id)
        {
            Id = id;
        }
    }
}