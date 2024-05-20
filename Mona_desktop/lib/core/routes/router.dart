import 'package:go_router/go_router.dart';
import 'package:mona_desktop/core/injections.dart';
import 'package:mona_desktop/features/auth/presentation/presentation_export.dart';
import 'package:talker_flutter/talker_flutter.dart';

final GoRouter router = GoRouter(routes: [
  GoRoute(
    path: '/',
    redirect: (context, state) => '/auth',
  ),
  GoRoute(
      path: '/auth',
      redirect: (context, state) => '/auth/sign-in',
      routes: [
        GoRoute(
          path: 'sign-in',
          builder: (context, state) => LoginScreen(),
        )
      ])
], observers: [
  TalkerRouteObserver(getIt<Talker>())
]);
