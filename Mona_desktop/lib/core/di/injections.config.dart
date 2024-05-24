// GENERATED CODE - DO NOT MODIFY BY HAND

// **************************************************************************
// InjectableConfigGenerator
// **************************************************************************

// ignore_for_file: type=lint
// coverage:ignore-file

// ignore_for_file: no_leading_underscores_for_library_prefixes
import 'package:dio/dio.dart' as _i5;
import 'package:flutter_secure_storage/flutter_secure_storage.dart' as _i4;
import 'package:get_it/get_it.dart' as _i1;
import 'package:injectable/injectable.dart' as _i2;
import 'package:mona_desktop/core/di/Injectable_module.dart' as _i11;
import 'package:mona_desktop/core/guard/auth_guard.dart' as _i6;
import 'package:mona_desktop/features/auth/bloc/auth_bloc.dart' as _i9;
import 'package:mona_desktop/repository/auth/abstract_auth_repository.dart'
    as _i7;
import 'package:mona_desktop/repository/auth/auth_repository.dart' as _i8;
import 'package:mona_desktop/repository/repository_export.dart' as _i10;
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
    gh.singleton<_i5.Dio>(() => injectableModule.provideDio(gh<_i3.Talker>()));
    gh.factory<_i6.AuthGuard>(
        () => _i6.AuthGuard(storage: gh<_i4.FlutterSecureStorage>()));
    gh.factory<_i7.AbstractAuthRepository>(
        () => _i8.AuthRepository(dio: gh<_i5.Dio>()));
    gh.factory<_i9.AuthBloc>(
        () => _i9.AuthBloc(gh<_i10.AbstractAuthRepository>()));
    return this;
  }
}

class _$InjectableModule extends _i11.InjectableModule {}
