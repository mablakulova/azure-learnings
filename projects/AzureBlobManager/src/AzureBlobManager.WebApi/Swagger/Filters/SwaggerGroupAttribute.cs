namespace AzureBlobManager.WebApi.Swagger.Filters;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class SwaggerGroupAttribute : Attribute
{
    public string GroupName { get; set; }

    public SwaggerGroupAttribute(string groupName)
    {
        GroupName = groupName;
    }
}