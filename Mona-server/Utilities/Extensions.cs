using Mona.Model;
using Mona.Model.Dto;

namespace Mona.Utilities;

public static class Extensions
{
    public static MessageModel ToMessageModel(this MessageRequest request, string senderId)
    {
        return new MessageModel
        {
            Text = request.Text,
            SenderId = senderId,
            DirectReceiverId = request.ReceiverId.StartsWith("g-") ? null : request.ReceiverId,
            GroupReceiverId = request.ReceiverId.StartsWith("g-") ? request.ReceiverId : null,
            CreatedAt = request.CreatedAt,
            ModifiedAt = request.CreatedAt,
            ReplyId = request.ReplyId,
            ForwardId = request.ForwardId,
            IsSent = false
        };
    }
}