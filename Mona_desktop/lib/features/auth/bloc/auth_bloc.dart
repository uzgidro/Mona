import 'package:equatable/equatable.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:injectable/injectable.dart';
import 'package:mona_desktop/core/constants.dart';
import 'package:mona_desktop/core/di/injections.dart';
import 'package:mona_desktop/core/models/models_export.dart';
import 'package:mona_desktop/repository/repository_export.dart';
import 'package:talker_flutter/talker_flutter.dart';

part 'auth_event.dart';

part 'auth_state.dart';

@Injectable()
class AuthBloc extends Bloc<AuthEvent, AuthState> {
  AuthBloc(this._authRepository) : super(AuthInitial()) {
    on<SignInEvent>((event, emit) async {
      try {
        var login = await _authRepository.login(event.username, event.password);
        await getIt<FlutterSecureStorage>().write(key: accessToken, value: login.accessToken);
        await getIt<FlutterSecureStorage>().write(key: refreshToken, value: login.refreshToken);
        emit(LoginSuccess(response: login));
      } catch (e, st) {
        emit(LoginFail(exception: e));
        getIt<Talker>().handle(e, st);
      }
    });
  }

  final AbstractAuthRepository _authRepository;
}
