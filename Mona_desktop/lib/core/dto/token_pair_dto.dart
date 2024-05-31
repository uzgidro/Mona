import 'package:equatable/equatable.dart';
import 'package:json_annotation/json_annotation.dart';

part 'generated/token_pair_dto.g.dart';

@JsonSerializable()
class TokenPairDto extends Equatable {
  final String accessToken;
  final String refreshToken;

  TokenPairDto({required this.accessToken, required this.refreshToken});

  factory TokenPairDto.fromJson(Map<String, dynamic> json) =>
      _$TokenPairDtoFromJson(json);

  Map<String, dynamic> toJson() => _$TokenPairDtoToJson(this);

  @override
  List<Object> get props => [accessToken, refreshToken];
}