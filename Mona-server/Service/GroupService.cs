using Mona.Context;
using Mona.Exception;
using Mona.Model;
using Mona.Model.Dto;
using Mona.Service.Interface;

namespace Mona.Service;

public class GroupService(ApplicationContext context) : IGroupService
{
    public async Task<GroupModel> CreateGroup(GroupRequest request)
    {
        var group = context.Groups.Add(new GroupModel { Name = request.Name, Description = request.Description });
        var entityEntry =
            context.UserGroup.Add(new UserGroup { GroupId = group.Entity.Id, UserId = request.CreatorId! });
        await context.SaveChangesAsync();
        if (request.Members != null) await AddMembers(request.Members, group.Entity.Id);
        await context.Entry(group.Entity).Collection(m => m.Users).LoadAsync();
        return group.Entity;
    }

    public async Task<List<UserGroup>> AddMembers(IEnumerable<string> membersId, string groupId)
    {
        var relations = new List<UserGroup>();
        foreach (var id in membersId)
        {
            var exists = context.UserGroup
                .FirstOrDefault(m => string.Equals(m.GroupId, groupId) && string.Equals(m.UserId, id));
            if (exists != null) continue;
            var relation = new UserGroup { GroupId = groupId, UserId = id };
            relations.Add(relation);
            context.UserGroup.Add(relation);
        }

        await context.SaveChangesAsync();
        return relations;
    }

    public async Task<GroupModel> EditGroup(string groupId, GroupRequest request)
    {
        var group = context.Groups.FirstOrDefault(m => string.Equals(m.Id, groupId));
        if (group == null) throw new EntityNotFoundException();
        group.Name = request.Name;
        group.Description = request.Description;
        await context.SaveChangesAsync();
        return group;
    }
}