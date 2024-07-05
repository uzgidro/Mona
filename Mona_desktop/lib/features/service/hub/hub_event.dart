part of 'hub_bloc.dart';

@immutable
sealed class HubEvent {}

final class StartConnection extends HubEvent {}

final class LoadContacts extends HubEvent {}

final class UpdateChat extends HubEvent {
  final ChatDto chat;

  UpdateChat({required this.chat});
}

final class ReceiveMessage extends HubEvent {
  final MessageDto message;

  ReceiveMessage({required this.message});
}