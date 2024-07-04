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
        if (request.CreatedAt.Year == 1) request.CreatedAt = DateTime.UtcNow;
        return new MessageModel
        {
            Text = request.ForwardId.IsNullOrEmpty() ? request.Text : null,
            SenderId = senderId,
            ChatId = request.ChatId,
            ReceiverId = request.ReceiverId,
            CreatedAt = request.CreatedAt.ToUniversalTime(),
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
        var text = model.Text;
        List<FileDto> files;
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
                replyTo = message.Sender.FirstName + " " + message.Sender.FirstName + " -> " +
                          forwarded.Sender.FirstName + " " + forwarded.Sender.FirstName;
                id = forwarded.Id;
                if (!forwarded.Text.IsNullOrEmpty())
                {
                    replyText = forwarded.Text!;
                }
                else
                {
                    replyText = forwarded.Files.Count + " files";
                }
            }
            else
            {
                replyTo = message.Sender.FirstName + " " + message.Sender.FirstName;
                id = message.Id;
                if (!message.Text.IsNullOrEmpty())
                {
                    replyText = message.Text!;
                }
                else
                {
                    replyText = message.Files.Count + " files";
                }
            }

            replied = new ReplyDto { RepliedMessage = replyText, ReplyTo = replyTo, Id = id };
            files = model.Files.Select(
                m => new FileDto { Id = m.Id, Name = m.Name, Size = m.Size, Path = string.Empty }).ToList();
            text = model.Text;
        }
        else if (model.ForwardedMessage != null)
        {
            var forwarded = model.ForwardedMessage;
            var forwardName = forwarded.Sender.FirstName + " " + forwarded.Sender.LastName;
            var forwardId = forwarded.SenderId;

            files = forwarded.Files.Select(
                m => new FileDto { Id = m.Id, Name = m.Name, Size = m.Size, Path = string.Empty }).ToList();
            text = forwarded.Text;

            forward = new ForwardDto { CreatorId = forwardId, CreatorName = forwardName };
        }
        else
        {
            files = model.Files.Select(
                m => new FileDto { Id = m.Id, Name = m.Name, Size = m.Size, Path = string.Empty }).ToList();
            text = model.Text;
        }

        return new MessageDto
        {
            Id = model.Id,
            SenderId = senderId,
            SenderName = sender,
            ChatId = model.ChatId,
            ReceiverId = model.ReceiverId,
            Receiver = model.Receiver,
            Message = text,
            Files = files,
            Reply = replied,
            Forward = forward,
            IsEdited = model.IsEdited,
            IsPinned = model.IsPinned,
            CreatedAt = DateTime.SpecifyKind(model.CreatedAt, DateTimeKind.Utc),
        };
    }

    public static ChatDto ToChatDto(this MessageDto messageDto)
    {
        return new ChatDto
        {
            ReceiverId = messageDto.ReceiverId,
            SenderId = messageDto.SenderId,
            SenderName = messageDto.SenderName,
            Message = messageDto.Message.IsNullOrEmpty() ? messageDto.Files.Count().ToString() : messageDto.Message,
            ChatName = messageDto.Receiver,
            ChatId = messageDto.ChatId,
            IsForward = messageDto.Forward != null,
            MessageTime = DateTime.SpecifyKind(messageDto.CreatedAt, DateTimeKind.Utc),
        };
    }
}