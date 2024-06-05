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

final class ChatLoaded extends HubState {
  final List<MessageDto> messages;

  ChatLoaded({required this.messages});

  @override
  List<Object?> get props => messages;
}
