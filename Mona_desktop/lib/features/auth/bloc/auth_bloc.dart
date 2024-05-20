import 'package:equatable/equatable.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:mona_desktop/core/models/models_export.dart';
import 'package:mona_desktop/repository/repository_export.dart';

part 'auth_event.dart';
part 'auth_state.dart';

class AuthBloc extends Bloc<AuthEvent, AuthState> {
  AuthBloc(this._authRepository) : super(AuthInitial()) {
    on<LoginEvent>((event, emit) async {
      try {
        var login = await _authRepository.login();
        emit(LoginSuccess(response: login));
      } catch (e) {
        emit(LoginFail(exception: e));
      }
    });
  }

  final AbstractAuthRepository _authRepository;
}
