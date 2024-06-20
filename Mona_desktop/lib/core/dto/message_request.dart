import 'package:json_annotation/json_annotation.dart';

part 'message_request.g.dart';

@JsonSerializable()
class MessageRequest {
  final String? text;
  final String receiverId;
  final String? chatId;
  final String? replyId;
  final String? forwardId;
  final DateTime createdAt;

  MessageRequest(
      {required this.text,
      required this.receiverId,
      required this.chatId,
      required this.replyId,
      required this.forwardId,
      required this.createdAt});

  factory MessageRequest.fromJson(Map<String, dynamic> json) =>
      _$MessageRequestFromJson(json);

  Map<String, dynamic> toJson() => _$MessageRequestToJson(this);
}
