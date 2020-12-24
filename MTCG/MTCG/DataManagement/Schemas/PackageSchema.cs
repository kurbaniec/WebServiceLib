namespace MTCG.DataManagement.Schemas
{
    public class PackageSchema
    {
        public string Id { get; set; }

        public PackageSchema(string id)
        {
            Id = id;
        }
    }
}