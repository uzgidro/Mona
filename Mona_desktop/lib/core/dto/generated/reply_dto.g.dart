// GENERATED CODE - DO NOT MODIFY BY HAND

part of '../reply_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

ReplyDto _$ReplyDtoFromJson(Map<String, dynamic> json) => ReplyDto(
      id: json['id'] as String,
      replyTo: json['replyTo'] as String,
      repliedMessage: json['repliedMessage'] as String,
    );

Map<String, dynamic> _$ReplyDtoToJson(ReplyDto instance) => <String, dynamic>{
      'id': instance.id,
      'replyTo': instance.replyTo,
      'repliedMessage': instance.repliedMessage,
    };
