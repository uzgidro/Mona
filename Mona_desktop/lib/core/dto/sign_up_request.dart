import 'package:equatable/equatable.dart';
import 'package:json_annotation/json_annotation.dart';

part 'sign_up_request.g.dart';

@JsonSerializable()
class SignUpRequest extends Equatable {
  final String username;
  final String firstName;
  final String lastName;
  final String password;


  factory SignUpRequest.fromJson(Map<String, dynamic> json) =>
      _$SignUpRequestFromJson(json);

  SignUpRequest({required this.username, required this.firstName, required this.lastName, required this.password});

  Map<String, dynamic> toJson() => _$SignUpRequestToJson(this);

  @override
  List<Object> get props => [username, firstName, lastName, password];
}