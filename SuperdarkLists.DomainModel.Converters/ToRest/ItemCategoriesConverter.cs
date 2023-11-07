using SuperdarkLists.DomainModel.Rest.Model;

namespace SuperdarkLists.DomainModel.Converters.ToRest;

public static class ItemCategoriesConverter
{
    public static ItemCategory ToRestModel(this Database.Model.ItemCategory databaseModel)
    {
        return new ItemCategory()
        {
            Id = databaseModel.CategoryId,
            Name = databaseModel.Name,
        };
    }
}