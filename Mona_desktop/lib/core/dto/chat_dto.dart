import 'package:equatable/equatable.dart';
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

  factory ChatDto.fromJson(Map<String, dynamic> json) =>
      _$ChatDtoFromJson(json);

  ChatDto(
      {required this.chatId,
      required this.chatName,
      required this.message,
      required this.receiverId,
      required this.senderId,
      required this.senderName,
      required this.isForward,
      required this.messageTime});

  Map<String, dynamic> toJson() => _$ChatDtoToJson(this);

  @override
  List<Object> get props =>
      [chatId, chatName, message, messageTime, receiverId];
}
