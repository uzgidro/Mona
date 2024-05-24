part of 'auth_bloc.dart';

@immutable
sealed class AuthState {}

final class AuthInitial extends AuthState {}

final class SignInSuccess extends AuthState {
  SignInSuccess({required this.response});

  final SignInResponse response;
}

final class SignInFail extends AuthState {
  SignInFail({required this.exception});

  final Object exception;
}

final class SignUpSuccess extends AuthState {}

final class SignUpFail extends AuthState {
  final AuthFail authFail;

  SignUpFail({required this.authFail});
}
