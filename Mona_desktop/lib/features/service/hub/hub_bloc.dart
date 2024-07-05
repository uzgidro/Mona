import 'package:bloc/bloc.dart';
import 'package:equatable/equatable.dart';
import 'package:injectable/injectable.dart';
import 'package:meta/meta.dart';
import 'package:mona_desktop/core/di/scope_names.dart';
import 'package:mona_desktop/core/dto/dto_export.dart';
import 'package:mona_desktop/features/service/service_export.dart';
import 'package:talker_flutter/talker_flutter.dart';

part 'hub_event.dart';
part 'hub_state.dart';

@LazySingleton(scope: ScopeNames.message)
class HubBloc extends Bloc<HubEvent, HubState> {
  HubBloc(this.talker, this.hubService) : super(HubInitial()) {
    on<StartConnection>((event, emit) async {
      try {
        await hubService.startConnection();
        List<ChatDto> chatList = await hubService.fetchChats();

        // TODO(): Add loading until emitted
        emit(HubStarted(chatList: chatList));

        launchListeners();
      } catch (e, st) {
        talker.handle(e, st);
      }
    });

    on<LoadContacts>((event, emit) async {
      try {
        List<UserDto> users = await hubService.fetchContacts();

        // TODO(): Add loading until emitted
        emit(ContactsLoaded(contacts: users));
      } catch (e, st) {
        talker.handle(e, st);
      }
    });

    on<UpdateChat>((event, emit) {
      emit(ChatUpdated(chat: event.chat));
    });

    on<ReceiveMessage>((event, emit) {
      emit(MessageReceived(message: event.message));
    });
  }

  final HubService hubService;
  final Talker talker;

  void launchListeners() {
    hubService.launchUpdateChatListener((chat) {
      add(UpdateChat(chat: chat));
    });
    hubService.launchReceiveMessageListener((message) {
      add(ReceiveMessage(message: message));
    });
  }
}
