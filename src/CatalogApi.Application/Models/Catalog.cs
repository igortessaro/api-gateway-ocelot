namespace CatalogApi.Application.Models
{
    public sealed class Catalog
    {
        public Catalog() { }

        public Catalog(int id, string name)
            : this()
        {
            this.Id = id;
            this.Name = name;
        }

        public int Id { get; set; }

        public string? Name { get; set; }
    }
}
