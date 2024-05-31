import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:go_router/go_router.dart';
import 'package:mona_desktop/core/di/injections.dart';
import 'package:mona_desktop/features/auth/bloc/auth_bloc.dart';
import 'package:mona_desktop/features/home/presentation/chat_list.dart';
import 'package:mona_desktop/features/home/presentation/side_menu.dart';
import 'package:mona_desktop/features/service/service_export.dart';
import 'package:signalr_netcore/signalr_client.dart';

import 'chat.dart';

class HomeScreen extends StatefulWidget {
  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  final authBloc = getIt<AuthBloc>();
  final hubBloc = getIt<HubBloc>();
  final hubConnection = getIt<HubConnection>();

  double _width = 240;
  double _minWidth = 140;

  @override
  void initState() {
    super.initState();
    hubBloc.add(StartConnection());
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        drawer: Drawer(
          child: Padding(
            padding: const EdgeInsets.all(12.0),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Padding(
                  padding: const EdgeInsets.all(8.0),
                  child: ElevatedButton(
                    onPressed: () async {
                      await hubConnection
                          .invoke('getChats')
                          .then((value) => print(value));
                    },
                    child: Text('Тест'),
                  ),
                ),
                Padding(
                  padding: const EdgeInsets.all(8.0),
                  child: ElevatedButton(
                    onPressed: () async {
                      authBloc.add(SignOutEvent());
                    },
                    child: Text('Выход'),
                  ),
                ),
              ],
            ),
          ),
        ),
        body: MultiBlocListener(
          listeners: [
            BlocListener<HubBloc, HubState>(
              bloc: hubBloc,
              listener: (context, state) {
                if (state is HubStarted) {
                  ScaffoldMessenger.of(context).showSnackBar(SnackBar(
                    content: Text('Welcum'),
                  ));
                }
              },
            ),
            BlocListener<AuthBloc, AuthState>(
                bloc: authBloc,
                listener: (context, state) {
                  if (state is SignOutSuccess) {
                    context.go('/');
                  }
                }),
          ],
          child: Row(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              SideMenu(),
              ChatList(),
              Chat(),
            ],
          ),
        ));
  }
}
