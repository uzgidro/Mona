import 'package:json_annotation/json_annotation.dart';

import 'file_dto.dart';
import 'forward_dto.dart';
import 'reply_dto.dart';

part 'generated/message_dto.g.dart';

@JsonSerializable()
class MessageDto {
  final String id;
  final String senderId;
  final String senderName;
  final String chatId;
  final String? message;
  final List<FileDto>? files;
  final ForwardDto? forward;
  final ReplyDto? reply;
  final bool isPinned;
  final bool isEdited;
  final DateTime createdAt;

  MessageDto(
      {required this.id,
      required this.senderId,
      required this.senderName,
      required this.chatId,
      required this.message,
      required this.files,
      required this.forward,
      required this.reply,
      required this.isPinned,
      required this.isEdited,
      required this.createdAt});

  factory MessageDto.fromJson(Map<String, dynamic> json) =>
      _$MessageDtoFromJson(json);

  Map<String, dynamic> toJson() => _$MessageDtoToJson(this);
}
