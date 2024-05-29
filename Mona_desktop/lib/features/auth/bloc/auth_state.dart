part of 'auth_bloc.dart';

@immutable
sealed class AuthState {}

final class AuthInitial extends AuthState {}

final class SignInSuccess extends AuthState {
  SignInSuccess({required this.response});

  final TokenPairDto response;
}

final class SignInFail extends AuthState {
  final AuthFail authFail;

  SignInFail({required this.authFail});

}

final class SignUpSuccess extends AuthState {}

final class SignUpFail extends AuthState {
  final AuthFail authFail;

  SignUpFail({required this.authFail});
}

final class SignOutSuccess extends AuthState {}

final class RefreshSuccess extends AuthState {}

final class RefreshFail extends AuthState {
  final AuthFail authFail;

  RefreshFail({required this.authFail});
}