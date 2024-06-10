import 'package:equatable/equatable.dart';
import 'package:json_annotation/json_annotation.dart';

part 'generated/chat_dto.g.dart';

@JsonSerializable()
class ChatDto extends Equatable {
  final String chatId;
  final String chatName;
  final String message;
  final String receiverId;
  final DateTime messageTime;

  factory ChatDto.fromJson(Map<String, dynamic> json) =>
      _$ChatDtoFromJson(json);

  ChatDto(
      {required this.chatId,
      required this.chatName,
      required this.receiverId,
      required this.message,
      required this.messageTime});

  Map<String, dynamic> toJson() => _$ChatDtoToJson(this);

  @override
  List<Object> get props => [chatId, chatName, message, messageTime, receiverId];
}
