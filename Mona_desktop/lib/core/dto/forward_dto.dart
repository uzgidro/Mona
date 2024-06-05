import 'package:json_annotation/json_annotation.dart';

part 'generated/forward_dto.g.dart';

@JsonSerializable()
class ForwardDto {
  final String creatorId;
  final String creatorName;

  ForwardDto({required this.creatorId, required this.creatorName});

  factory ForwardDto.fromJson(Map<String, dynamic> json) =>
      _$ForwardDtoFromJson(json);

  Map<String, dynamic> toJson() => _$ForwardDtoToJson(this);
}
