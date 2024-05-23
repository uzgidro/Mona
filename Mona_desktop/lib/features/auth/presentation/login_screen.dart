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
  final usernameController = TextEditingController();
  final passwordController = TextEditingController();

  final GlobalKey<FormState> _formKey =
      GlobalKey<FormState>(); // A key for managing the form

  void _submitForm() {
    // Check if the form is valid
    if (_formKey.currentState!.validate()) {
      _formKey.currentState!.save(); // Save the form data
      // You can perform actions with the form data here and extract the details
      bloc.add(SignInEvent(
          username: usernameController.text,
          password: passwordController.text));
    }
  }

  @override
  void dispose() {
    usernameController.dispose();
    passwordController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Column(
        children: [
          Form(
            key: _formKey, // Associate the form key with this Form widget
            child: Padding(
              padding: EdgeInsets.all(16.0),
              child: Column(
                children: <Widget>[
                  TextFormField(
                    decoration: InputDecoration(labelText: 'Username'),
                    // Label for the name field
                    validator: (value) {
                      // Validation function for the name field
                      if (value!.isEmpty) {
                        return 'Please enter your name.'; // Return an error message if the name is empty
                      }
                      return null; // Return null if the name is valid
                    },
                    controller: usernameController,
                  ),
                  TextFormField(
                    decoration: InputDecoration(labelText: 'Password'),
                    // Label for the email field
                    validator: (value) {
                      // Validation function for the email field
                      if (value!.isEmpty) {
                        return 'Please enter your email.'; // Return an error message if the email is empty
                      }
                      // You can add more complex validation logic here
                      return null; // Return null if the email is valid
                    },
                    controller: passwordController,
                  ),
                  SizedBox(height: 20.0),
                  ElevatedButton(
                    onPressed: _submitForm,
                    // Call the _submitForm function when the button is pressed
                    child: Text('Submit'), // Text on the button
                  ),
                ],
              ),
            ),
          ),
          BlocBuilder<AuthBloc, AuthState>(
            bloc: bloc,
            builder: (context, state) {
              if (state is LoginSuccess) {
                return Text(state.response.accessToken);
              }
              if (state is LoginFail) {
                return Text('Username or password invalid');
              }
              return Container();
            },
          )
        ],
      ),
    );
  }
}
