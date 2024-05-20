part of 'auth_bloc.dart';

@immutable
sealed class AuthEvent extends Equatable {}

final class LoginEvent extends AuthEvent {
  @override
  List<Object?> get props => [];
}
