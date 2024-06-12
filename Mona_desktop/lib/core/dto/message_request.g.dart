// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'message_request.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

MessageRequest _$MessageRequestFromJson(Map<String, dynamic> json) =>
    MessageRequest(
      text: json['text'] as String?,
      receiverId: json['receiverId'] as String,
      chatId: json['chatId'] as String?,
      replyId: json['replyId'] as String?,
      forwardId: json['forwardId'] as String?,
      createdAt: DateTime.parse(json['createdAt'] as String),
    );

Map<String, dynamic> _$MessageRequestToJson(MessageRequest instance) =>
    <String, dynamic>{
      'text': instance.text,
      'receiverId': instance.receiverId,
      'chatId': instance.chatId,
      'replyId': instance.replyId,
      'forwardId': instance.forwardId,
      'createdAt': instance.createdAt.toIso8601String(),
    };
