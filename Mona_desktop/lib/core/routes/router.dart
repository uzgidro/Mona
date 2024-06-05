import 'package:go_router/go_router.dart';
import 'package:mona_desktop/core/di/injections.dart';
import 'package:mona_desktop/core/guard/auth_guard.dart';
import 'package:mona_desktop/features/auth/presentation/presentation_export.dart';
import 'package:mona_desktop/features/home/presentation/home_screen.dart';
import 'package:talker_flutter/talker_flutter.dart';

final GoRouter router = GoRouter(routes: <RouteBase>[
  GoRoute(
    path: '/',
    redirect: (context, state) async {
      if (await getIt<AuthGuard>().isAuthorized()) {
        return '/home';
      } else {
        return '/auth/sign-in';
      }
    },
  ),
  GoRoute(
    path: '/auth/sign-in',
    builder: (context, state) => SignInScreen(),
  ),
  GoRoute(
    path: '/auth/sign-up',
    builder: (context, state) => SignUpScreen(),
  ),
  GoRoute(
    path: '/home',
    builder: (context, state) => HomeScreen(),
  )
], observers: [
  TalkerRouteObserver(getIt<Talker>())
]);
