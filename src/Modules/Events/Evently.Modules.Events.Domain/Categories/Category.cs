using Evently.Common.Domain;

namespace Evently.Modules.Events.Domain.Categories;

public sealed class Category : Entity
{
    // For EF Core to Materialise
    private Category() { }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public bool IsArchived { get; private set; }

    public static Category Create(string name)
    {
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = name,
            IsArchived = false
        };

        category.Raise(new CategoryCreatedDomainEvent(category.Id));

        return category;
    }

    public void Achive()
    {
        IsArchived = true;

        Raise(new CategoryArchivedDomainEvent(Id));
    }

    public void ChangeName(string name)
    {
        if (Name == name)
        {
            return;
        }

        Name = name;

        Raise(new CategoryNameChangedDomainEvent(Id, name));
    }
}
