import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:go_router/go_router.dart';
import 'package:mona_desktop/core/di/injections.config.dart';
import 'package:mona_desktop/core/di/injections.dart';
import 'package:mona_desktop/core/di/scope_names.dart';
import 'package:mona_desktop/features/auth/bloc/auth_bloc.dart';
import 'package:mona_desktop/features/home/presentation/chat_list.dart';
import 'package:mona_desktop/features/home/presentation/drawer.dart';
import 'package:mona_desktop/features/home/presentation/side_menu.dart';
import 'package:mona_desktop/features/service/service_export.dart';

import 'chat.dart';

class HomeScreen extends StatefulWidget {
  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  late final AuthBloc authBloc;
  late final HubBloc hubBloc;

  @override
  void initState() {
    super.initState();
    getIt.initMessageScope();
    authBloc = getIt<AuthBloc>();
    hubBloc = getIt<HubBloc>();
    hubBloc.add(StartConnection());
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        drawer: DrawerComponent(),
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
                listener: (context, state) async {
                  if (state is SignOutSuccess) {
                    context.go('/');
                    await getIt.dropScope(ScopeNames.message);
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
