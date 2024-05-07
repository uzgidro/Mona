using Mona.Model;
using Mona.Model.Dto;

namespace Mona.Service.Interface;

public interface IGroupService
{
    public Task<GroupModel> CreateGroup(GroupRequest request);
    public Task<List<GroupModel>> GetGroupList();
    public Task<List<GroupModel>> GetUserGroupList(string caller);
    public Task<GroupModel> GetGroupInfo(string groupId);
    public Task<List<UserGroup>> AddMembers(string groupId, IEnumerable<string> membersId);
    public Task<List<UserGroup>> RemoveMembers(string groupId, IEnumerable<string> membersId);
    public Task<GroupModel> EditGroup(string groupId, GroupRequest request);
    public Task DeleteGroup(string groupId);
}