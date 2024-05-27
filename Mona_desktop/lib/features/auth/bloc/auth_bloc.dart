import 'package:dio/dio.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:injectable/injectable.dart';
import 'package:mona_desktop/core/di/injections.dart';
import 'package:mona_desktop/core/dto/auth_fail.dart';
import 'package:mona_desktop/core/dto/dto_export.dart';
import 'package:mona_desktop/core/middleware/middleware.dart';
import 'package:mona_desktop/repository/repository_export.dart';
import 'package:talker_flutter/talker_flutter.dart';

part 'auth_event.dart';
part 'auth_state.dart';

@Injectable()
class AuthBloc extends Bloc<AuthEvent, AuthState> {
  AuthBloc(this.authRepository, this.jwtService) : super(AuthInitial()) {
    on<SignInEvent>((event, emit) async {
      try {
        var login = await authRepository.signIn(event.signInRequest);
        await jwtService.saveTokens(login);
        emit(SignInSuccess(response: login));
      } on DioException catch (e) {
        // on connection error
        if (e.type.index == 6) {
          emit(SignInFail(authFail: AuthFail.connectionError));
        } else if (e.response?.statusCode == 400) {
          emit(SignInFail(authFail: AuthFail.badRequest));
        }
      } catch (e, st) {
        emit(SignInFail(authFail: AuthFail.unexpected));
        getIt<Talker>().handle(e, st);
      }
    });

    on<SignUpEvent>((event, emit) async {
      try {
        var statusCode = await authRepository.signUp(event.signUpRequest);
        if (statusCode == 201) {
          emit(SignUpSuccess());
        }
      } on DioException catch (e) {
        if (e.response?.statusCode == 409) {
          emit(SignUpFail(authFail: AuthFail.conflict));
        } else if (e.response?.statusCode == 400) {
          emit(SignUpFail(authFail: AuthFail.badRequest));
        }
        // on connection error
        if (e.type.index == 6) {
          emit(SignUpFail(authFail: AuthFail.connectionError));
        }
      } catch (e, st) {
        emit(SignUpFail(authFail: AuthFail.unexpected));
        getIt<Talker>().handle(e, st);
      }
    });

    on<SignOutEvent>((event, emit) async {
      try {
        await jwtService.removeTokens();
        emit(SignOutSuccess());
      } catch (e, st) {
        getIt<Talker>().handle(e, st);
      }
    });
  }

  final AbstractAuthRepository authRepository;
  final JwtService jwtService;
}
