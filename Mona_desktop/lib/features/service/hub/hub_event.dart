part of 'hub_bloc.dart';

@immutable
sealed class HubEvent {}

final class StartConnection extends HubEvent {}

final class GetChatMessages extends HubEvent {
  final String chatId;

  GetChatMessages({required this.chatId});
}
