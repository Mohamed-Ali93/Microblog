using Microblog.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Microblog.Permissions;

public class MicroblogPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(MicroblogPermissions.GroupName);

        var booksPermission = myGroup.AddPermission(MicroblogPermissions.Books.Default, L("Permission:Books"));
        booksPermission.AddChild(MicroblogPermissions.Books.Create, L("Permission:Books.Create"));
        booksPermission.AddChild(MicroblogPermissions.Books.Edit, L("Permission:Books.Edit"));
        booksPermission.AddChild(MicroblogPermissions.Books.Delete, L("Permission:Books.Delete"));
        //Define your own permissions here. Example:
        //myGroup.AddPermission(MicroblogPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<MicroblogResource>(name);
    }
}
