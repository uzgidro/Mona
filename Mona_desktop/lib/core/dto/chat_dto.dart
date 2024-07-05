import 'package:equatable/equatable.dart';
import 'package:intl/intl.dart';
import 'package:json_annotation/json_annotation.dart';

part 'chat_dto.g.dart';

@JsonSerializable()
class ChatDto extends Equatable {
  final String chatId;
  final String chatName;
  final String message;
  final String receiverId;
  final String senderId;
  final String senderName;
  final bool isForward;
  final DateTime messageTime;
  final String? formattedTime;

  factory ChatDto.fromJson(Map<String, dynamic> json) {
    var chatDtoFromJson = _$ChatDtoFromJson(json);
    var chatTime = chatDtoFromJson.messageTime.toLocal();
    var difference = DateTime.now().difference(chatTime);
    String? formattedTime;

    if (difference.inDays < 1) {
      formattedTime = DateFormat.Hm().format(chatTime);
    } else if (difference.inDays < 8) {
      formattedTime = DateFormat('EE').format(chatTime);
    } else {
      formattedTime = DateFormat('d.MM.yyyy').format(chatTime);
    }
    return chatDtoFromJson.copyWith(formattedTime: formattedTime);
  }

  ChatDto(this.chatId,
      this.chatName,
      this.message,
      this.receiverId,
      this.senderId,
      this.senderName,
      this.isForward,
      this.messageTime,
      this.formattedTime);

  Map<String, dynamic> toJson() => _$ChatDtoToJson(this);

  @override
  List<Object> get props =>
      [chatId, chatName, message, messageTime, receiverId];

  ChatDto copyWith({
    String? chatId,
    String? chatName,
    String? message,
    String? receiverId,
    String? senderId,
    String? senderName,
    bool? isForward,
    DateTime? messageTime,
    String? formattedTime,
  }) {
    return ChatDto(
      chatId ?? this.chatId,
      chatName ?? this.chatName,
      message ?? this.message,
      receiverId ?? this.receiverId,
      senderId ?? this.senderId,
      senderName ?? this.senderName,
      isForward ?? this.isForward,
      messageTime ?? this.messageTime,
      formattedTime ?? this.formattedTime,
    );
  }
}
