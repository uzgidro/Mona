import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:go_router/go_router.dart';
import 'package:mona_desktop/core/di/injections.dart';
import 'package:mona_desktop/core/middleware/jwt_service.dart';
import 'package:mona_desktop/features/auth/bloc/auth_bloc.dart';
import 'package:signalr_netcore/signalr_client.dart';
import 'package:talker_flutter/talker_flutter.dart';

class HomeScreen extends StatefulWidget {
  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  final AuthBloc bloc = getIt<AuthBloc>();

  @override
  void initState() {
    super.initState();
    const serverUrl = "http://127.0.0.1:5031/chat";
    final hubConnection = HubConnectionBuilder().withUrl(serverUrl, options: HttpConnectionOptions(
      accessTokenFactory: () async => await getIt<JwtService>().getAccessToken()
    )).build();

    hubConnection.start()?.catchError((e, st) {
      getIt<Talker>().handle(e);
    }).then((value) {
      hubConnection.invoke('getUsers').then((value) => print(value));
    });
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
                ElevatedButton(
                  onPressed: () async {
                    bloc.add(SignOutEvent());
                  },
                  child: Text('Выход'),
                ),
              ],
            ),
          ),
        ),
        body: BlocListener<AuthBloc, AuthState>(
          bloc: bloc,
          listener: (context, state) {
            if (state is SignOutSuccess) {
              context.go('/');
            }
          },
          child: Row(
            children: [
              Padding(
                padding: const EdgeInsets.all(8.0),
                child: Column(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    Padding(
                      padding: const EdgeInsets.all(8.0),
                      child: Builder(builder: (context) {
                        return SizedBox.square(
                          child: IconButton(
                            onPressed: () => Scaffold.of(context).openDrawer(),
                            icon: Icon(Icons.menu),
                          ),
                        );
                      }),
                    ),
                    Padding(
                      padding: const EdgeInsets.all(8.0),
                      child: Icon(Icons.forum_rounded),
                    ),
                    Padding(
                      padding: const EdgeInsets.all(8.0),
                      child: Icon(Icons.group_rounded),
                    ),
                  ],
                ),
              )
            ],
          ),
        ));
  }
}
