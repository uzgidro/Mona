part of 'auth_bloc.dart';

@immutable
sealed class AuthEvent extends Equatable {}

final class SignInEvent extends AuthEvent {
  final SignInRequest signInRequest;

  SignInEvent({required this.signInRequest});


  @override
  List<Object?> get props => [signInRequest];
}

final class SignUpEvent extends AuthEvent {
  final SignUpRequest signUpRequest;

  SignUpEvent({required this.signUpRequest});


  @override
  List<Object?> get props => [signUpRequest];
}
