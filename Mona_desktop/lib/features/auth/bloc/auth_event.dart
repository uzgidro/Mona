part of 'auth_bloc.dart';

@immutable
sealed class AuthEvent extends Equatable {}

final class SignInEvent extends AuthEvent {
  final String username;
  final String password;

  SignInEvent({required this.username, required this.password});

  @override
  List<Object?> get props => [];
}
