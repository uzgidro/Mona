// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'token_pair_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

TokenPairDto _$TokenPairDtoFromJson(Map<String, dynamic> json) => TokenPairDto(
      accessToken: json['accessToken'] as String,
      refreshToken: json['refreshToken'] as String,
    );

Map<String, dynamic> _$TokenPairDtoToJson(TokenPairDto instance) =>
    <String, dynamic>{
      'accessToken': instance.accessToken,
      'refreshToken': instance.refreshToken,
    };
