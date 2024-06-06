part of 'hub_bloc.dart';

@immutable
sealed class HubState extends Equatable {}

final class HubInitial extends HubState {
  @override
  List<Object?> get props => [];
}

final class HubStarted extends HubState {
  final List<ChatDto> chatList;

  HubStarted({required this.chatList});

  @override
  List<Object?> get props => chatList;
}

final class ContactsLoaded extends HubState {
  final List<UserDto> contacts;

  ContactsLoaded({required this.contacts});

  @override
  List<Object?> get props => [contacts];
}