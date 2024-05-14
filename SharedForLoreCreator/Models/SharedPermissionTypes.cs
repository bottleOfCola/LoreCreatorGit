namespace SharedForLoreCreator.Models;

public enum PermissionTypes
{
    Unknown = 0,
    AddMessage = 1,
    RemoveMessage = 2,
    AddElement = 3,
    RemoveElement = 4,
    AddConnection = 5,
    RemoveConnection = 6,
    AddConnectionType = 7,
    RemoveConnectionType = 8,
    AddTag = 9,
    RemoveTag = 10,
    RoleGiving = 11,
    RoleBacking = 12,
    AddRole = 13,
    RemoveRole = 14,
}

public class SharedPermissionType : BaseModel
{
    public PermissionTypes Id {  get; set; }
}