import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:go_router/go_router.dart';
import 'package:mona_desktop/core/di/injections.dart';
import 'package:mona_desktop/core/dto/auth_fail.dart';
import 'package:mona_desktop/features/auth/bloc/auth_bloc.dart';

import '../../../core/models/models_export.dart';

class SignUpScreen extends StatefulWidget {
  @override
  State<SignUpScreen> createState() => _SignUpScreenState();
}

class _SignUpScreenState extends State<SignUpScreen> {
  final _bloc = getIt<AuthBloc>();
  final _usernameController = TextEditingController();
  final _firstNameController = TextEditingController();
  final _lastNameController = TextEditingController();
  final _passwordController = TextEditingController();
  final _repeatPasswordController = TextEditingController();

  String? _usernameError;

  final GlobalKey<FormState> _formKey =
      GlobalKey<FormState>(); // A key for managing the form

  void _submitForm() {
    // Check if the form is valid
    if (_formKey.currentState!.validate()) {
      _formKey.currentState!.save(); // Save the form data
      setState(() {
        _usernameError = null;
      });
      // You can perform actions with the form data here and extract the details
      _bloc.add(SignUpEvent(
          signUpRequest: SignUpRequest(
        username: _usernameController.text,
        password: _passwordController.text,
        firstName: _firstNameController.text,
        lastName: _lastNameController.text,
      )));
    }
  }

  void _signIn() {
    _bloc.add(SignInEvent(
        signInRequest: SignInRequest(
            username: _usernameController.text,
            password: _passwordController.text)));
  }

  @override
  void dispose() {
    _usernameController.dispose();
    _passwordController.dispose();
    _firstNameController.dispose();
    _lastNameController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: BlocListener<AuthBloc, AuthState>(
        bloc: _bloc,
        listener: (context, state) {
          if (state is SignUpSuccess) {
            ScaffoldMessenger.of(context).showSnackBar(
              SnackBar(content: Text('Регистрация прошла успешно')),
            );
            _signIn();
          }
          if (state is SignInSuccess) {
            context.go('/home');
          }
          if (state is SignInFail) {
            ScaffoldMessenger.of(context).showSnackBar(SnackBar(
              content: Text('Запрос не удался'),
              action: SnackBarAction(
                label: 'Назад',
                onPressed: () {
                  context.go('/auth/sign-in');
                },
              ),
            ));
          }
          if (state is SignUpFail) {
            switch (state.authFail) {
              case AuthFail.conflict:
                setState(() {
                  _usernameError = 'Данный логин уже зарегистрирован';
                });
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
              key: _formKey, // Associate the form key with this Form widget
              child: Padding(
                padding: EdgeInsets.all(16.0),
                child: Column(
                  children: <Widget>[
                    TextFormField(
                      decoration: InputDecoration(
                          labelText: 'Логин', errorText: _usernameError),
                      // Label for the name field
                      validator: (value) {
                        // Validation function for the name field
                        if (value!.isEmpty) {
                          return 'Поле не может быть пустым'; // Return an error message if the name is empty
                        }
                        return null; // Return null if the name is valid
                      },
                      controller: _usernameController,
                    ),
                    TextFormField(
                      decoration: InputDecoration(labelText: 'Имя'),
                      // Label for the name field
                      validator: (value) {
                        // Validation function for the name field
                        if (value!.isEmpty) {
                          return 'Поле не может быть пустым'; // Return an error message if the name is empty
                        }
                        return null; // Return null if the name is valid
                      },
                      controller: _firstNameController,
                    ),
                    TextFormField(
                      decoration: InputDecoration(labelText: 'Фамилия'),
                      // Label for the name field
                      validator: (value) {
                        // Validation function for the name field
                        if (value!.isEmpty) {
                          return 'Поле не может быть пустым'; // Return an error message if the name is empty
                        }
                        return null; // Return null if the name is valid
                      },
                      controller: _lastNameController,
                    ),
                    TextFormField(
                      decoration: InputDecoration(labelText: 'Пароль'),
                      // Label for the name field
                      validator: (value) {
                        // Validation function for the name field
                        if (value!.isEmpty) {
                          return 'Поле не может быть пустым'; // Return an error message if the name is empty
                        }
                        if (_passwordController.text !=
                            _repeatPasswordController.text) {
                          return 'Пароли не совпадают';
                        }
                        return null; // Return null if the name is valid
                      },
                      controller: _passwordController,
                    ),
                    TextFormField(
                      decoration:
                          InputDecoration(labelText: 'Повторите пароль'),
                      // Label for the name field
                      validator: (value) {
                        // Validation function for the name field
                        if (value!.isEmpty) {
                          return 'Поле не может быть пустым'; // Return an error message if the name is empty
                        }
                        if (_passwordController.text !=
                            _repeatPasswordController.text) {
                          return 'Пароли не совпадают';
                        }
                        return null; // Return null if the name is valid
                      },
                      controller: _repeatPasswordController,
                    ),
                    SizedBox(height: 20.0),
                    ElevatedButton(
                        onPressed: _submitForm,
                        child: Text('Зарегистрироваться')),
                    TextButton(
                      onPressed: () {
                        context.go('/auth/sign-in');
                      },
                      // Call the _submitForm function when the button is pressed
                      child: Text(
                          'Уже есть аккаунт, войти!'), // Text on the button
                    ),
                  ],
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }
}
