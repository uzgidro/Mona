import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:mona_desktop/core/injections.dart';
import 'package:mona_desktop/features/auth/bloc/auth_bloc.dart';

class LoginScreen extends StatefulWidget {
  @override
  State<LoginScreen> createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen> {
  final bloc = getIt<AuthBloc>();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Column(
        children: [
          ElevatedButton(
            child: Text('login'),
            onPressed: () {
              bloc.add(LoginEvent());
            },
          ),
          BlocBuilder<AuthBloc, AuthState>(
            bloc: bloc,
            builder: (context, state) {
              if (state is LoginSuccess) {
                return Text('login Success');
              }
              if (state is LoginFail) {
                return Text('login fail');
              }
              return Container();
            },
          )
        ],
      ),
    );
  }
}