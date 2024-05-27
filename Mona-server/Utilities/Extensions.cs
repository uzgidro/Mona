using Microsoft.IdentityModel.Tokens;
using Mona.Model;
using Mona.Model.Dto;

namespace Mona.Utilities;

public static class Extensions
{
    public static MessageModel ToMessageModel(this MessageRequest request, string? senderId)
    {
        if (string.IsNullOrEmpty(senderId)) throw new ArgumentNullException(nameof(senderId));
        //if (string.IsNullOrEmpty(request.Text) && string.IsNullOrEmpty(request.ForwardId))
        //  throw new ArgumentNullException(nameof(senderId), "Text and Forward id cannot be null at the same message");
        return new MessageModel
        {
            Text = request.ForwardId.IsNullOrEmpty() ? request.Text : null,
            SenderId = senderId,
            DirectReceiverId = request.ReceiverId != null && request.ReceiverId.StartsWith("g-")
                ? null
                : request.ReceiverId,
            GroupReceiverId =
                request.ReceiverId != null &&
                (request.ReceiverId.StartsWith("g-") || request.ReceiverId.IsNullOrEmpty())
                    ? request.ReceiverId
                    : null,
            ChatId = request.ChatId,
            CreatedAt = request.CreatedAt,
            ModifiedAt = request.CreatedAt,
            ReplyId = request.ReplyId,
            ForwardId = request.ForwardId,
            IsPinned = false,
            IsSent = false
        };
    }
}