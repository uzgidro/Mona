// GENERATED CODE - DO NOT MODIFY BY HAND

// **************************************************************************
// InjectableConfigGenerator
// **************************************************************************

// ignore_for_file: type=lint
// coverage:ignore-file

// ignore_for_file: no_leading_underscores_for_library_prefixes
import 'package:dio/dio.dart' as _i7;
import 'package:flutter_secure_storage/flutter_secure_storage.dart' as _i4;
import 'package:get_it/get_it.dart' as _i1;
import 'package:injectable/injectable.dart' as _i2;
import 'package:mona_desktop/core/di/Injectable_module.dart' as _i16;
import 'package:mona_desktop/core/guard/auth_guard.dart' as _i8;
import 'package:mona_desktop/core/middleware/jwt_service.dart' as _i6;
import 'package:mona_desktop/core/middleware/middleware.dart' as _i9;
import 'package:mona_desktop/features/auth/bloc/auth_bloc.dart' as _i13;
import 'package:mona_desktop/features/service/chat/chat_bloc.dart' as _i5;
import 'package:mona_desktop/features/service/hub/hub_bloc.dart' as _i15;
import 'package:mona_desktop/repository/auth/abstract_auth_repository.dart'
    as _i11;
import 'package:mona_desktop/repository/auth/auth_repository.dart' as _i12;
import 'package:mona_desktop/repository/repository_export.dart' as _i14;
import 'package:signalr_netcore/signalr_client.dart' as _i10;
import 'package:talker_flutter/talker_flutter.dart' as _i3;

extension GetItInjectableX on _i1.GetIt {
// initializes the registration of main-scope dependencies inside of GetIt
  _i1.GetIt init({
    String? environment,
    _i2.EnvironmentFilter? environmentFilter,
  }) {
    final gh = _i2.GetItHelper(
      this,
      environment,
      environmentFilter,
    );
    final injectableModule = _$InjectableModule();
    gh.lazySingleton<_i3.Talker>(() => injectableModule.talker);
    gh.lazySingleton<_i4.FlutterSecureStorage>(
        () => injectableModule.secureStorage);
    gh.lazySingleton<_i5.ChatBloc>(() => _i5.ChatBloc());
    gh.factory<_i6.JwtService>(
        () => _i6.JwtService(storage: gh<_i4.FlutterSecureStorage>()));
    gh.lazySingleton<_i7.Dio>(() => injectableModule.provideDio(
          gh<_i3.Talker>(),
          gh<_i6.JwtService>(),
        ));
    gh.factory<_i8.AuthGuard>(
        () => _i8.AuthGuard(jwtService: gh<_i9.JwtService>()));
    gh.lazySingleton<_i10.HubConnection>(
        () => injectableModule.hubConnection(gh<_i6.JwtService>()));
    gh.factory<_i11.AbstractAuthRepository>(
        () => _i12.AuthRepository(dio: gh<_i7.Dio>()));
    gh.lazySingleton<_i13.AuthBloc>(() => _i13.AuthBloc(
          gh<_i14.AbstractAuthRepository>(),
          gh<_i9.JwtService>(),
          gh<_i3.Talker>(),
        ));
    gh.lazySingleton<_i15.HubBloc>(() => _i15.HubBloc(
          gh<_i14.AbstractAuthRepository>(),
          gh<_i6.JwtService>(),
          gh<_i3.Talker>(),
          gh<_i10.HubConnection>(),
        ));
    return this;
  }
}

class _$InjectableModule extends _i16.InjectableModule {}
