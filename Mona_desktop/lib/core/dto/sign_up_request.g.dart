// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'sign_up_request.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

SignUpRequest _$SignUpRequestFromJson(Map<String, dynamic> json) =>
    SignUpRequest(
      username: json['username'] as String,
      firstName: json['firstName'] as String,
      lastName: json['lastName'] as String,
      password: json['password'] as String,
    );

Map<String, dynamic> _$SignUpRequestToJson(SignUpRequest instance) =>
    <String, dynamic>{
      'username': instance.username,
      'firstName': instance.firstName,
      'lastName': instance.lastName,
      'password': instance.password,
    };
