using Mona.Model;
using Mona.Model.Dto;

namespace Mona.Utilities;

public static class Extensions
{
    public static MessageModel ToMessageModel(this MessageRequest request, string? senderId)
    {
        if (string.IsNullOrEmpty(senderId)) throw new ArgumentNullException(nameof(senderId));
        if (string.IsNullOrEmpty(request.Text) && string.IsNullOrEmpty(request.ForwardId))
            throw new ArgumentNullException(nameof(senderId), "Text and Forward id cannot be null at the same message");

        return new MessageModel
        {
            Text = string.IsNullOrEmpty(request.ForwardId) ? request.Text : null,
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