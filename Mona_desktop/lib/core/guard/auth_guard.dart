import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:injectable/injectable.dart';
import 'package:mona_desktop/core/constants.dart';

@Injectable()
class AuthGuard {
  final FlutterSecureStorage storage;

  AuthGuard({required this.storage});

  Future<bool> isAuthorized() async{
    return await storage.read(key: accessToken) != null;
  }
}
