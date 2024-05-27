import 'package:injectable/injectable.dart';
import 'package:mona_desktop/core/middleware/middleware.dart';

@Injectable()
class AuthGuard {
  final JwtService jwtService;

  AuthGuard({required this.jwtService});


  Future<bool> isAuthorized() async{
    return await jwtService.isAuthenticated();
  }
}
