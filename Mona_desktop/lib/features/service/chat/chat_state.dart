part of 'chat_bloc.dart';

@immutable
sealed class ChatState extends Equatable {}

final class ChatInitial extends ChatState {
  @override
  List<Object?> get props => [];
}

final class ChatOpened extends ChatState {
  final String chatName;
  final String receiverId;
  final String? chatId;

  ChatOpened(
      {required this.chatName, required this.receiverId, required this.chatId});

  @override
  List<Object?> get props => [chatName, chatId, receiverId];
}

final class ChatLoaded extends ChatState {
  final List<MessageDto> messages;

  ChatLoaded({required this.messages});

  @override
  List<Object?> get props => messages;
}

final class MessageReceived extends ChatState {
  final MessageDto message;

  MessageReceived({required this.message});

  @override
  List<Object?> get props => [message];
}

final class ChatUpdated extends ChatState {
  final ChatDto chat;

  ChatUpdated({required this.chat});
  @override
  List<Object?> get props => [chat];
  
}