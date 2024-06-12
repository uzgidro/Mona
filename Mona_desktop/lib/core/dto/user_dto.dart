import 'package:json_annotation/json_annotation.dart';

part 'user_dto.g.dart';

@JsonSerializable()
class UserDto {
  final String id;
  final String name;
  final String? chatId;
  final String? icon;

  factory UserDto.fromJson(Map<String, dynamic> json) =>
      _$UserDtoFromJson(json);

  UserDto(
      {required this.id,
      required this.name,
      required this.chatId,
      required this.icon});

  Map<String, dynamic> toJson() => _$UserDtoToJson(this);
}
