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
            // DirectReceiverId = request.ReceiverId != null && request.ReceiverId.StartsWith("g-")
            //     ? null
            //     : request.ReceiverId,
            // GroupReceiverId =
            //     request.ReceiverId != null &&
            //     (request.ReceiverId.StartsWith("g-") || request.ReceiverId.IsNullOrEmpty())
            //         ? request.ReceiverId
            //         : null,
            ChatId = request.ChatId,
            CreatedAt = request.CreatedAt,
            ModifiedAt = request.CreatedAt,
            ReplyId = request.ReplyId,
            ForwardId = request.ForwardId,
            IsPinned = false,
            IsSent = false
        };
    }

    public static MessageDto ToDto(this MessageModel model)
    {
        var sender = model.Sender.FirstName + " " + model.Sender.LastName;
        var senderId = model.SenderId;
        ForwardDto? forward = null;
        var text = string.Empty;
        var files = new List<FileDto>();
        ReplyDto? replied = null;
        if (model.RepliedMessage != null)
        {
            string id;
            string replyTo;
            string replyText;
            var message = model.RepliedMessage;
            if (message.ForwardedMessage != null)
            {
                var forwarded = message.ForwardedMessage;
                replyTo = forwarded.Sender.FirstName + " " + forwarded.Sender.FirstName;
                id = forwarded.Id;
                if (!forwarded.Files.IsNullOrEmpty())
                {
                    replyText = forwarded.Files.Count + " files";
                }
                else
                {
                    replyText = forwarded.Text!;
                }
            }
            else
            {
                replyTo = message.Sender.FirstName + " " + message.Sender.FirstName;
                id = message.Id;
                if (!message.Files.IsNullOrEmpty())
                {
                    replyText = message.Files.Count + " files";
                }
                else
                {
                    replyText = message.Text!;
                }
            }

            replied = new ReplyDto { RepliedMessage = replyText, ReplyTo = replyTo, Id = id };
        }
        else if (model.ForwardedMessage != null)
        {
            var forwarded = model.ForwardedMessage;
            var forwardName = model.ForwardedMessage.Sender.FirstName + " " + model.ForwardedMessage.Sender.LastName;
            var forwardId = model.ForwardedMessage.SenderId;
            if (!forwarded.Files.IsNullOrEmpty())
            {
                files = forwarded.Files.Select(
                    m => new FileDto { Id = m.Id, Name = m.Name, Size = m.Size, Path = string.Empty }).ToList();
            }
            else
            {
                text = forwarded.Text;
            }

            forward = new ForwardDto { CreatorId = forwardId, CreatorName = forwardName };
        }
        else
        {
            if (!model.Files.IsNullOrEmpty())
            {
                files = model.Files.Select(
                    m => new FileDto { Id = m.Id, Name = m.Name, Size = m.Size, Path = string.Empty }).ToList();
            }
            else
            {
                text = model.Text;
            }
        }

        return new MessageDto
        {
            Id = model.Id,
            SenderId = senderId,
            SenderName = sender,
            ChatId = model.ChatId,
            Message = text,
            Files = files,
            Reply = replied,
            Forward = forward,
            IsEdited = model.IsEdited,
            IsPinned = model.IsPinned,
            CreatedAt = model.CreatedAt,
        };
    }
}