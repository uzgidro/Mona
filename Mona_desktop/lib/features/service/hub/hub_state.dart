part of 'hub_bloc.dart';

@immutable
sealed class HubState {}

final class HubInitial extends HubState {}

final class HubStarted extends HubState {
  final List<ChatDto> chatList;

  HubStarted({required this.chatList});
}

final class ChatLoaded extends HubState {
  final List<MessageDto> messages;

  ChatLoaded({required this.messages});
}
