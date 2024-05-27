import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:go_router/go_router.dart';
import 'package:mona_desktop/core/di/injections.dart';
import 'package:mona_desktop/core/dto/auth_fail.dart';
import 'package:mona_desktop/core/dto/sign_in_request.dart';
import 'package:mona_desktop/features/auth/bloc/auth_bloc.dart';

class SignInScreen extends StatefulWidget {
  @override
  State<SignInScreen> createState() => _SignInScreenState();
}

class _SignInScreenState extends State<SignInScreen> {
  final bloc = getIt<AuthBloc>();
  final _usernameController = TextEditingController();
  final _passwordController = TextEditingController();

  String? _usernameError;
  String? _passwordError;

  final GlobalKey<FormState> _formKey = GlobalKey<FormState>();

  void _submitForm() {
    if (_formKey.currentState!.validate()) {
      _formKey.currentState!.save(); // Save the form data
      setState(() {
        _usernameError = null;
        _passwordError = null;
      });
      bloc.add(SignInEvent(
          signInRequest: SignInRequest(
              username: _usernameController.text,
              password: _passwordController.text)));
    }
  }

  @override
  void dispose() {
    _usernameController.dispose();
    _passwordController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        body: BlocListener<AuthBloc, AuthState>(
      bloc: bloc,
      listener: (context, state) {
        if (state is SignInSuccess) {
          context.go('/home');
        }
        if (state is SignInFail) {
          switch (state.authFail) {
            case AuthFail.connectionError:
              ScaffoldMessenger.of(context).showSnackBar(SnackBar(
                content: Text('Проблемы с интернетом'),
                action: SnackBarAction(
                  label: 'Повторить',
                  onPressed: () {
                    _submitForm();
                  },
                ),
              ));
            case AuthFail.badRequest:
              setState(() {
                _usernameError = 'Что-то не то с логином';
                _passwordError = 'Что-то не то с паролем';
              });
            default:
              ScaffoldMessenger.of(context).showSnackBar(SnackBar(
                content: Text('Запрос не удался'),
                action: SnackBarAction(
                  label: 'Повторить',
                  onPressed: () {
                    _submitForm();
                  },
                ),
              ));
          }
        }
      },
      child: Column(
        children: [
          Form(
            key: _formKey,
            child: Padding(
              padding: EdgeInsets.all(16.0),
              child: Column(
                children: <Widget>[
                  TextFormField(
                    decoration: InputDecoration(
                        labelText: 'Логин', errorText: _usernameError),
                    validator: (value) {
                      if (value!.isEmpty) {
                        return 'Поле не может быть пустым';
                      }
                      return null;
                    },
                    controller: _usernameController,
                  ),
                  TextFormField(
                    decoration: InputDecoration(
                        labelText: 'Пароль', errorText: _passwordError),
                    validator: (value) {
                      if (value!.isEmpty) {
                        return 'Поле не может быть пустым';
                      }
                      return null;
                    },
                    controller: _passwordController,
                  ),
                  SizedBox(height: 20.0),
                  ElevatedButton(
                    onPressed: _submitForm,
                    child: Text('Войти'),
                  ),
                  TextButton(
                    onPressed: () {
                      context.go('/auth/sign-up');
                    },
                    child: Text('Создать аккаунт'),
                  ),
                ],
              ),
            ),
          ),
        ],
      ),
    ));
  }
}
