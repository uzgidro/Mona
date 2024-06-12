import 'package:json_annotation/json_annotation.dart';

part 'reply_dto.g.dart';

@JsonSerializable()
class ReplyDto {
  final String id;
  final String replyTo;
  final String repliedMessage;

  ReplyDto(
      {required this.id, required this.replyTo, required this.repliedMessage});

  factory ReplyDto.fromJson(Map<String, dynamic> json) =>
      _$ReplyDtoFromJson(json);

  Map<String, dynamic> toJson() => _$ReplyDtoToJson(this);
}
