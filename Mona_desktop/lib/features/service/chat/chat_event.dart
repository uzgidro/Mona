part of 'chat_bloc.dart';

@immutable
sealed class ChatEvent extends Equatable {}

final class OpenChat extends ChatEvent {
  final String chatName;
  final String receiverId;
  final String? chatId;

  OpenChat(
      {required this.chatName, required this.receiverId, required this.chatId});

  @override
  List<Object?> get props => [receiverId, chatId, chatName];
}

final class SendMessage extends ChatEvent {
  final MessageRequest messageRequest;

  SendMessage({required this.messageRequest});

  @override
  List<Object?> get props => [messageRequest];
}

final class ReceiveMessage extends ChatEvent {
  final MessageDto message;

  ReceiveMessage({required this.message});

  @override
  List<Object?> get props => [message];
}

final class UpdateChat extends ChatEvent {
  final ChatDto chat;

  UpdateChat({required this.chat});
  @override
  List<Object?> get props => [chat];
  
}