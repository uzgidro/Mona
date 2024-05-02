using Mona.Model;
using Mona.Model.Dto;

namespace Mona.Service.Interface;

public interface IGroupService
{
    public Task<GroupModel> CreateGroup(GroupRequest request);
    public Task<List<UserGroup>> AddMembers(string groupId, IEnumerable<string> membersId);
    public Task<GroupModel> EditGroup(string groupId, GroupRequest request);
}