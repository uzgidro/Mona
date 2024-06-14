// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'message_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

MessageDto _$MessageDtoFromJson(Map<String, dynamic> json) => MessageDto(
      id: json['id'] as String,
      senderId: json['senderId'] as String,
      senderName: json['senderName'] as String,
      chatId: json['chatId'] as String,
      receiverId: json['receiverId'] as String,
      message: json['message'] as String?,
      files: (json['files'] as List<dynamic>)
          .map((e) => FileDto.fromJson(e as Map<String, dynamic>))
          .toList(),
      forward: json['forward'] == null
          ? null
          : ForwardDto.fromJson(json['forward'] as Map<String, dynamic>),
      reply: json['reply'] == null
          ? null
          : ReplyDto.fromJson(json['reply'] as Map<String, dynamic>),
      isPinned: json['isPinned'] as bool,
      isEdited: json['isEdited'] as bool,
      createdAt: DateTime.parse(json['createdAt'] as String),
    );

Map<String, dynamic> _$MessageDtoToJson(MessageDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'senderId': instance.senderId,
      'senderName': instance.senderName,
      'chatId': instance.chatId,
      'receiverId': instance.receiverId,
      'message': instance.message,
      'files': instance.files,
      'forward': instance.forward,
      'reply': instance.reply,
      'isPinned': instance.isPinned,
      'isEdited': instance.isEdited,
      'createdAt': instance.createdAt.toIso8601String(),
    };
