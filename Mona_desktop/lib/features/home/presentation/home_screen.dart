import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:go_router/go_router.dart';
import 'package:mona_desktop/core/di/injections.dart';
import 'package:mona_desktop/features/auth/bloc/auth_bloc.dart';

class HomeScreen extends StatelessWidget {
  final AuthBloc bloc = getIt<AuthBloc>();


  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: BlocListener<AuthBloc, AuthState>(
        bloc: bloc,
        listener: (context, state) {
          if (state is SignOutSuccess) {
            context.go('/');
          }
        },
        child:   ElevatedButton(
          onPressed: () async {
            bloc.add(SignOutEvent());
          },
          child: Text('Выход'),
        ),
      )
    );
  }
}
