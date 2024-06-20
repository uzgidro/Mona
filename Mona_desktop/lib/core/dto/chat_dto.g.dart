// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'chat_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

ChatDto _$ChatDtoFromJson(Map<String, dynamic> json) => ChatDto(
      chatId: json['chatId'] as String,
      chatName: json['chatName'] as String,
      message: json['message'] as String,
      receiverId: json['receiverId'] as String,
      senderId: json['senderId'] as String,
      senderName: json['senderName'] as String,
      isForward: json['isForward'] as bool,
      messageTime: DateTime.parse(json['messageTime'] as String),
    );

Map<String, dynamic> _$ChatDtoToJson(ChatDto instance) => <String, dynamic>{
      'chatId': instance.chatId,
      'chatName': instance.chatName,
      'message': instance.message,
      'receiverId': instance.receiverId,
      'senderId': instance.senderId,
      'senderName': instance.senderName,
      'isForward': instance.isForward,
      'messageTime': instance.messageTime.toIso8601String(),
    };
