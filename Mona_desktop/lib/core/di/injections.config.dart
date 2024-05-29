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
import 'package:mona_desktop/core/di/Injectable_module.dart' as _i15;
import 'package:mona_desktop/core/guard/auth_guard.dart' as _i7;
import 'package:mona_desktop/core/middleware/jwt_service.dart' as _i5;
import 'package:mona_desktop/core/middleware/middleware.dart' as _i8;
import 'package:mona_desktop/features/auth/bloc/auth_bloc.dart' as _i12;
import 'package:mona_desktop/features/hub/bloc/hub_bloc.dart' as _i14;
import 'package:mona_desktop/repository/auth/abstract_auth_repository.dart'
    as _i10;
import 'package:mona_desktop/repository/auth/auth_repository.dart' as _i11;
import 'package:mona_desktop/repository/repository_export.dart' as _i13;
import 'package:signalr_netcore/signalr_client.dart' as _i9;
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
    gh.lazySingleton<_i9.HubConnection>(
        () => injectableModule.hubConnection(gh<_i5.JwtService>()));
    gh.factory<_i10.AbstractAuthRepository>(
        () => _i11.AuthRepository(dio: gh<_i6.Dio>()));
    gh.factory<_i12.AuthBloc>(() => _i12.AuthBloc(
          gh<_i13.AbstractAuthRepository>(),
          gh<_i8.JwtService>(),
          gh<_i3.Talker>(),
        ));
    gh.factory<_i14.HubBloc>(() => _i14.HubBloc(
          gh<_i13.AbstractAuthRepository>(),
          gh<_i5.JwtService>(),
          gh<_i3.Talker>(),
          gh<_i9.HubConnection>(),
        ));
    return this;
  }
}

class _$InjectableModule extends _i15.InjectableModule {}
