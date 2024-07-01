// GENERATED CODE - DO NOT MODIFY BY HAND

// **************************************************************************
// InjectableConfigGenerator
// **************************************************************************

// ignore_for_file: type=lint
// coverage:ignore-file

// ignore_for_file: no_leading_underscores_for_library_prefixes
import 'package:dio/dio.dart' as _i6;
import 'package:flutter_secure_storage/flutter_secure_storage.dart' as _i4;
import 'package:get_it/get_it.dart' as _i1;
import 'package:injectable/injectable.dart' as _i2;
import 'package:mona_desktop/core/di/Injectable_module.dart' as _i14;
import 'package:mona_desktop/core/guard/auth_guard.dart' as _i7;
import 'package:mona_desktop/core/middleware/jwt_service.dart' as _i5;
import 'package:mona_desktop/core/middleware/middleware.dart' as _i8;
import 'package:mona_desktop/features/auth/bloc/auth_bloc.dart' as _i11;
import 'package:mona_desktop/features/service/chat/chat_bloc.dart' as _i18;
import 'package:mona_desktop/features/service/chat/chat_service.dart' as _i16;
import 'package:mona_desktop/features/service/hub/hub_bloc.dart' as _i19;
import 'package:mona_desktop/features/service/hub/hub_service.dart' as _i17;
import 'package:mona_desktop/repository/auth/abstract_auth_repository.dart'
    as _i9;
import 'package:mona_desktop/repository/auth/auth_repository.dart' as _i10;
import 'package:mona_desktop/repository/repository_export.dart' as _i12;
import 'package:mona_desktop/repository/signalr_repository.dart' as _i15;
import 'package:signalr_netcore/signalr_client.dart' as _i13;
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
    gh.factory<_i5.JwtService>(
        () => _i5.JwtService(storage: gh<_i4.FlutterSecureStorage>()));
    gh.lazySingleton<_i6.Dio>(() => injectableModule.provideDio(
          gh<_i3.Talker>(),
          gh<_i5.JwtService>(),
        ));
    gh.factory<_i7.AuthGuard>(
        () => _i7.AuthGuard(jwtService: gh<_i8.JwtService>()));
    gh.factory<_i9.AbstractAuthRepository>(
        () => _i10.AuthRepository(dio: gh<_i6.Dio>()));
    gh.lazySingleton<_i11.AuthBloc>(() => _i11.AuthBloc(
          gh<_i12.AbstractAuthRepository>(),
          gh<_i8.JwtService>(),
          gh<_i3.Talker>(),
        ));
    return this;
  }

// initializes the registration of Message-scope dependencies inside of GetIt
  _i1.GetIt initMessageScope({_i1.ScopeDisposeFunc? dispose}) {
    return _i2.GetItHelper(this).initScope(
      'Message',
      dispose: dispose,
      init: (_i2.GetItHelper gh) {
        final injectableModule = _$InjectableModule();
        gh.singleton<_i13.HubConnection>(
          () => injectableModule.hubConnection(gh<_i5.JwtService>()),
          dispose: _i14.disposeHubConnection,
        );
        gh.factory<_i15.SignalRRepository>(() =>
            _i15.SignalRRepository(hubConnection: gh<_i13.HubConnection>()));
        gh.factory<_i16.ChatService>(
            () => _i16.ChatService(repository: gh<_i15.SignalRRepository>()));
        gh.factory<_i17.HubService>(
            () => _i17.HubService(repository: gh<_i15.SignalRRepository>()));
        gh.lazySingleton<_i18.ChatBloc>(() => _i18.ChatBloc(
              gh<_i16.ChatService>(),
              gh<_i3.Talker>(),
            ));
        gh.lazySingleton<_i19.HubBloc>(() => _i19.HubBloc(
              gh<_i3.Talker>(),
              gh<_i17.HubService>(),
            ));
      },
    );
  }
}

class _$InjectableModule extends _i14.InjectableModule {}
