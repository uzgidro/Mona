// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'chat_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

ChatDto _$ChatDtoFromJson(Map<String, dynamic> json) => ChatDto(
      json['chatId'] as String,
      json['chatName'] as String,
      json['message'] as String,
      json['receiverId'] as String,
      json['senderId'] as String,
      json['senderName'] as String,
      json['isForward'] as bool,
      DateTime.parse(json['messageTime'] as String),
      json['formattedTime'] as String?,
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
      'formattedTime': instance.formattedTime,
    };
