part of 'auth_bloc.dart';

@immutable
sealed class AuthState extends Equatable {}

final class AuthInitial extends AuthState {
  @override
  List<Object?> get props => [];
}

final class LoginSuccess extends AuthState {
  LoginSuccess({required this.response});

  final LoginResponse response;

  @override
  List<Object?> get props => [response];
}

final class LoginFail extends AuthState {
  LoginFail({required this.exception});

  final Object exception;

  @override
  List<Object?> get props => [exception];
}