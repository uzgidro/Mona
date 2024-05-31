import 'package:json_annotation/json_annotation.dart';

part 'generated/file_dto.g.dart';

@JsonSerializable()
class FileDto {
  final String? id;
  final String name;
  final num size;
  final String path;

  FileDto(
      {required this.id,
      required this.name,
      required this.size,
      required this.path});

  factory FileDto.fromJson(Map<String, dynamic> json) =>
      _$FileDtoFromJson(json);

  Map<String, dynamic> toJson() => _$FileDtoToJson(this);
}
