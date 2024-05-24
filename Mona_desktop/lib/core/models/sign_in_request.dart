import 'package:equatable/equatable.dart';
import 'package:json_annotation/json_annotation.dart';

part 'sign_in_request.g.dart';

@JsonSerializable()
class SignInRequest extends Equatable {
  final String username;
  final String password;


  factory SignInRequest.fromJson(Map<String, dynamic> json) =>
      _$SignInRequestFromJson(json);

  SignInRequest({required this.username, required this.password});

  Map<String, dynamic> toJson() => _$SignInRequestToJson(this);

  @override
  List<Object> get props => [username, password];
}