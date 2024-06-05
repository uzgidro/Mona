part of 'auth_bloc.dart';

@immutable
sealed class AuthEvent {}

final class SignInEvent extends AuthEvent {
  final SignInRequest signInRequest;

  SignInEvent({required this.signInRequest});
}

final class SignUpEvent extends AuthEvent {
  final SignUpRequest signUpRequest;

  SignUpEvent({required this.signUpRequest});
}

final class SignOutEvent extends AuthEvent {}

final class RefreshTokenEvent extends AuthEvent{}