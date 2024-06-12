// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'file_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

FileDto _$FileDtoFromJson(Map<String, dynamic> json) => FileDto(
      id: json['id'] as String?,
      name: json['name'] as String,
      size: json['size'] as num,
      path: json['path'] as String,
    );

Map<String, dynamic> _$FileDtoToJson(FileDto instance) => <String, dynamic>{
      'id': instance.id,
      'name': instance.name,
      'size': instance.size,
      'path': instance.path,
    };
