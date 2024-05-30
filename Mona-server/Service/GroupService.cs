using Microsoft.EntityFrameworkCore;
using Mona.Context;
using Mona.Model;
using Mona.Model.Dto;
using Mona.Service.Interface;

namespace Mona.Service;

public class GroupService(ApplicationContext context) : IGroupService
{
    public async Task<GroupModel> CreateGroup(GroupRequest request)
    {
        var group = context.Groups.Add(new GroupModel { Name = request.Name, Description = request.Description });
        context.UserGroup.Add(new UserGroup { GroupId = group.Entity.Id, UserId = request.CreatorId! });
        await context.SaveChangesAsync();
        if (request.Members != null) await AddMembers(group.Entity.Id, request.Members);
        await context.Entry(group.Entity).Collection(m => m.Users).LoadAsync();
        return group.Entity;
    }

    public async Task<List<GroupModel>> GetGroupList()
    {
        return await context.Groups.ToListAsync();
    }

    public async Task<List<GroupModel>> GetUserGroupList(string caller)
    {
        return await context.UserGroup.AsNoTracking()
            .Where(m => string.Equals(m.UserId, caller))
            .Include(m => m.GroupModel)
            .Include(m => m.GroupModel.Users)
            .Select(m => m.GroupModel)
            .ToListAsync();
    }

    public async Task<GroupModel> GetGroupInfo(string groupId)
    {
        return await context.Groups
            .Include(m => m.Users)
            .FirstAsync(m => string.Equals(m.Id, groupId));
    }

    public async Task<List<UserGroup>> AddMembers(string groupId, IEnumerable<string> membersId)
    {
        var group = GetGroup(groupId);
        var relations = new List<UserGroup>();
        foreach (var id in membersId)
        {
            try
            {
                var user = context.Users.First(m => string.Equals(m.Id, id));
                // Check if user is already in group
                var userGroup = context.UserGroup
                    .FirstOrDefault(m => string.Equals(m.GroupId, group.Id) && string.Equals(m.UserId, user.Id));
                if (userGroup != null) continue;
                var relation = new UserGroup { GroupId = group.Id, UserId = user.Id };
                var entityEntry = context.UserGroup.Add(relation);
                await AddNavigation(entityEntry.Entity);
                relations.Add(entityEntry.Entity);
            }
            catch
            {
                // ignored
            }
        }

        await context.SaveChangesAsync();
        return relations;
    }

    public async Task<List<UserGroup>> RemoveMembers(string groupId, IEnumerable<string> membersId)
    {
        var group = GetGroup(groupId);
        var relations = new List<UserGroup>();
        foreach (var id in membersId)
        {
            try
            {
                var user = context.Users.First(m => string.Equals(m.Id, id));
                var relation = context.UserGroup
                    .First(m => string.Equals(m.GroupId, group.Id) && string.Equals(m.UserId, user.Id));
                var entityEntry = context.UserGroup.Remove(relation);
                await AddNavigation(entityEntry.Entity);
                relations.Add(entityEntry.Entity);
            }
            catch
            {
                // ignored
            }
        }

        await context.SaveChangesAsync();
        return relations;
    }

    public async Task<UserGroup> LeaveGroup(string groupId, string caller)
    {
        var group = GetGroup(groupId);
        var relations = new UserGroup();
        try
        {
            var user = context.Users.First(m => string.Equals(m.Id, caller));
            var relation = context.UserGroup
                .First(m => string.Equals(m.GroupId, group.Id) && string.Equals(m.UserId, user.Id));
            var entityEntry = context.UserGroup.Remove(relation);
            await AddNavigation(entityEntry.Entity);
            relations = entityEntry.Entity;
        }
        catch
        {
            // ignored
        }

        await context.SaveChangesAsync();
        return relations;
    }

    public async Task<GroupModel> EditGroup(string groupId, GroupRequest request)
    {
        if (string.IsNullOrEmpty(request.Name))
            throw new ArgumentException("Group name cannot be null or empty");

        var group = GetGroup(groupId);
        group.Name = request.Name;
        group.Description = request.Description;
        await context.SaveChangesAsync();
        await AddNavigation(group);
        return group;
    }

    public async Task<List<UserGroup>> DeleteGroup(string groupId)
    {
        var group = GetGroup(groupId);
        context.Groups.Remove(group);
        var userGroups = await context.UserGroup
            .Where(m => string.Equals(m.GroupId, groupId))
            .Include(m => m.GroupModel)
            .Include(m => m.UserModel)
            .ToListAsync();
        context.UserGroup.RemoveRange(userGroups);
        await context.SaveChangesAsync();
        return userGroups;
    }

    private async Task AddNavigation(GroupModel entity)
    {
        await context.Entry(entity).Collection(m => m.Users).LoadAsync();
    }

    private async Task AddNavigation(UserGroup entity)
    {
        await context.Entry(entity).Reference(m => m.GroupModel).LoadAsync();
        await context.Entry(entity).Reference(m => m.UserModel).LoadAsync();
    }

    private GroupModel GetGroup(string groupId)
    {
        return context.Groups.First(m => string.Equals(m.Id, groupId));
    }
}